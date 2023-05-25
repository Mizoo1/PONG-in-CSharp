using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Pong.Engine;
using Pong.Engine.Defaults;
using Pong.Game.Models;
using Pong.GUI.Components;
using static SDL2.SDL;

namespace Pong.GUI
{
    public class MainMenu
    {
        private readonly Mouse _mouse;

        private List<Button> _buttons { get; }

        public MainMenu(Renderer renderer, Mouse mouse)
        {
            _mouse = mouse;
            _buttons = new List<Button>();

            var windowSize = renderer.LogicalSize;
            var centerPos = new Position
            {
                // Center buttons in the middle of the screen
                X = windowSize.LogicalWidth / 2 - UISizes.ButtonWidth / 2,
                
                // Give buttons x% padding up top
                Y = (int)(windowSize.LogicalHeight * 0.25)
            };
            
            var playButton = new Button(renderer, "Play",centerPos, UISizes.ButtonWidth, UISizes.ButtonHeight, Color.Black, Color.LightGray);
            playButton.Click += ButtonOnClick;
            _buttons.Add(playButton);

            centerPos.Y += UISizes.ButtonHeight + 20;
            var settingsButton = new Button(renderer, "Settings",centerPos, UISizes.ButtonWidth, UISizes.ButtonHeight, Color.Black, Color.LightGray);
            settingsButton.Click += ButtonOnClick;
            _buttons.Add(settingsButton);
            
            centerPos.Y += UISizes.ButtonHeight + 20;
            var quitButton = new Button(renderer, "Quit",centerPos, UISizes.ButtonWidth, UISizes.ButtonHeight, Color.Black, Color.LightGray);
            quitButton.Click += ButtonOnClick;
            _buttons.Add(quitButton);
        }

        private void ButtonOnClick(object sender, EventArgs e)
        {
            var button = (Button) sender;
            SDL_Log($"{button.Text}");
            switch (button.Text)
            {
                case "Play":
                    Program.ChangeState(State.InGameMenu);
                    break;
                case "Resume":
                    Program.Paused = false;
                    Program.ChangeState(State.InGame);
                    break;
                case "Settings":
                    Program.ChangeState(State.InSettingsMenu);
                    break;
                case "Quit":
                    Program.RequestClose();
                    break;
                case "Back":
                    Program.Paused = false;
                    break;
            }
            
        }

        public void CheckButtons(SDL_MouseButtonEvent buttonEvent)
        {
            _buttons.FirstOrDefault(button => buttonEvent.x >= button.Rect.x &&
                                              buttonEvent.x <= button.Rect.x + button.Rect.w &&
                                              buttonEvent.y >= button.Rect.y &&
                                              buttonEvent.y <= button.Rect.y + button.Rect.h)?.OnClick();
        }
        
        public void Draw()
        {
            _buttons[0].UpdateText(Program.Paused ? "Resume" : "Play");
            _buttons[_buttons.Count - 1].UpdateText(Program.Paused ? "Back" : "Quit");
            foreach (var button in _buttons)
            {
                button.Draw(_mouse);
            }
        }
    }
}