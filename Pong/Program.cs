using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Pong.Engine;
using Pong.Engine.Defaults;
using Pong.Game;
using Pong.Game.Models;
using Pong.Game.TechnicalLogic;
using Pong.GUI;
using Pong.GUI.Components;
using Pong.GUI.Components.Text;
using static SDL2.SDL;
using static SDL2.SDL_ttf;
using static SDL2.SDL_image;

namespace Pong
{
    class Program
    {
        private static Window _window;
        private static Renderer _renderer;
        private static MainMenu _mainMenu;
        private static SettingsMenu _settingsMenu;
        private static MainGame _mainGame;
        private static Mouse _mouse;
        private static Music _backroundMusic = new Music(Constants.BackroundMusic);
        private static TextInput _selectedTextInput;

        public static State State { get; private set; }
        public static bool Paused { get; set; }

        private static bool _running = true;
        static void Main(string[] args)
        {
            SDL_SetHint(SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
            if (SDL_Init(SDL_INIT_VIDEO) < 0 || TTF_Init() < 0 || IMG_Init(IMG_InitFlags.IMG_INIT_PNG) < 0)
            {
                Console.WriteLine($"There was an issue initilizing SDL. {SDL_GetError()}");
                return;
            }

            Paused = false;
            State = State.InMainMenu;
            _window = new Window("Pong", fullscreen: false);
            _renderer = new Renderer(_window);
            _mouse = new Mouse();
            _mainMenu = new MainMenu(_renderer, _mouse);
            _settingsMenu = new SettingsMenu(_renderer, _mouse);
            _backroundMusic.playMusik();
            _mainGame = new MainGame(_renderer, _mouse);
            while (_running)
            {
                switch (State)
                {
                    case State.CreateGame:
                        _mainGame.CreateLogic();
                        break;
                    case State.InGame:
                        _mainGame.Logic.Update();
                        break;
                    case State.GameOver:
                        break;
                }

                while (SDL_PollEvent(out var sdlEvent) == 1)
                {
                    switch (sdlEvent.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            _running = false;
                            break;
                        //case SDL_EventType.SDL_KEYDOWN:
                        //case SDL_EventType.SDL_TEXTINPUT:
                            switch (State)
                            {
                                case State.InGame:
                                    _mainGame.Logic.UpdatePlayer(sdlEvent);
                                    break;
                                case State.InGameMenu:
                                    _selectedTextInput?.Write(sdlEvent);
                                    break;
                            }
                            break;
                        case SDL_EventType.SDL_MOUSEMOTION:
                            _mouse.Update(sdlEvent.motion);
                            break;
                        case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                            //Break if its not a left click
                            if (sdlEvent.button.button != SDL_BUTTON_LEFT)
                                break;
                            //Find the button that is being clicked and fire the OnClick
                            switch (State)
                            {
                                case State.InMainMenu:
                                    _mainMenu.CheckButtons(sdlEvent.button);
                                    _backroundMusic.close();
                                    break;
                                case State.InSettingsMenu:
                                    _settingsMenu.CheckButtons(sdlEvent.button);
                                    _settingsMenu.CheckCheckboxes(sdlEvent.button);
                                    break;
                                case State.InGameMenu:
                                    _mainGame.UpdateNames();
                                    _mainGame.GameStartMenu.CheckButtons(sdlEvent.button);
                                    _selectedTextInput = _mainGame.GameStartMenu.CheckTextInputs();
                                    break;
                                case State.GameOver:
                                    _mainGame.GameOver.Checkbuttons(_mouse);
                                    break;
                            }
                            break;
                        default:
                            if (State == State.InGame)
                            {
                                _mainGame.Logic.UpdatePlayer(sdlEvent);
                            }
                            break;
                    }
                }
                
                #region rendering
                _renderer.Clear();
                
                switch (State)
                {
                    case State.InMainMenu:
                        _mainMenu.Draw();
                        break;
                    case State.InSettingsMenu:
                        _settingsMenu.Draw();
                        break;
                    case State.InGameMenu:
                        _mainGame.Draw();
                        break;
                    case State.InGame:
                        _mainGame.Logic.Draw();
                        break;
                    case State.GameOver:
                        _mainGame.Draw();
                        break;
                }
                _renderer.Render();
                
                #endregion
            }
            Close();
        }

        public static void RequestClose()
        {
            _running = false;
        }

        public static void ChangeState(State newState)
        {
            State = newState; 
        }
        
        private static void Close()
        {
            _window.Destroy();
            _renderer.Destroy();
            TTF_Quit();
            IMG_Quit();
            SDL_Quit();
        }
    }
}