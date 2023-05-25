using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Pong.Engine;
using Pong.Engine.Defaults;
using Pong.Engine.Models;
using Pong.Game.Models;
using Pong.GUI.Components;
using static SDL2.SDL;

namespace Pong.GUI
{
    public class SettingsMenu
    {
        private Renderer _renderer;
        private Mouse _mouse;
        private List<Button> _buttons;
        private List<LabelledCheckbox> _checkboxes;
        private Position _centerPos;
        private Dropdown _resolutionDropdown;

        public SettingsMenu(Renderer renderer, Mouse mouse)
        {
            _renderer = renderer;
            _mouse = mouse;
            _checkboxes = new List<LabelledCheckbox>();
            _buttons = new List<Button>();

            var rect = new SDL_Rect
            {
                w = 200,
                h = 100,
                x = 200,
                y = 0
            };
            
            var optionDictionary = _renderer.Window.ValidResolutions.ToDictionary<SDL_DisplayMode, string, object>(mode => $"{mode.w}x{mode.h}", mode => mode);
            _resolutionDropdown = new Dropdown(renderer, mouse, optionDictionary, rect);
            _resolutionDropdown.Click += ResolutionDropdownOnClick;

            CreateButtons();
            CreateCheckboxes();
        }

        private void ResolutionDropdownOnClick(object sender, EventArgs e)
        {
            var option = (SDL_DisplayMode) sender;
            _renderer.Window.SetSize(option.w, option.h);
            _renderer.Window.SetPosition(SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);
        }

        private void CreateCheckboxes()
        {
            const int width = 230;
            const int height = 50;
            var windowSize = _renderer.Window.GetWindowSize();
            var position = new Position
            {
                X = windowSize.Item1 / 2 - width / 2,
                Y = (int)(windowSize.Item2 * 0.25)
            };
            var fullScreenCheckbox = new LabelledCheckbox(_renderer, "Fullscreen", Color.WhiteSmoke, position, width, height);
            fullScreenCheckbox.Click += FullScreenCheckboxOnClick;
            var flags = (SDL_WindowFlags)SDL_GetWindowFlags(_renderer.Window.SdlWindow);
            fullScreenCheckbox.Checked = flags.HasFlag(SDL_WindowFlags.SDL_WINDOW_FULLSCREEN);
            _checkboxes.Add(fullScreenCheckbox);
        }

        private void FullScreenCheckboxOnClick(object sender, EventArgs e)
        {
            var checkbox = (Checkbox) sender;
            _renderer.Window.SetFullscreen(checkbox.Toggle());
            _renderer.Window.SetPosition(SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);
        }

        private void CreateButtons()
        {
            var backButton = new Button(_renderer, "Back", Positions.TopLeft, 200, 75, Color.Black, Color.LightGray);
            backButton.Click += BackButtonOnClick;
            _buttons.Add(backButton);
        }

        private void BackButtonOnClick(object sender, EventArgs e)
        {
            Program.ChangeState(State.InMainMenu);
        }

        public void CheckButtons(SDL_MouseButtonEvent buttonEvent)
        {
            _buttons.FirstOrDefault(button => buttonEvent.x >= button.Rect.x &&
                                              buttonEvent.x <= button.Rect.x + button.Rect.w &&
                                              buttonEvent.y >= button.Rect.y &&
                                              buttonEvent.y <= button.Rect.y + button.Rect.h)?.OnClick();
            _resolutionDropdown.CheckOptions(buttonEvent);
        }

        public void CheckCheckboxes(SDL_MouseButtonEvent buttonEvent)
        {
            _checkboxes.FirstOrDefault(checkbox => buttonEvent.x >= checkbox.Rect.SdlRect.x &&
                                              buttonEvent.x <= checkbox.Rect.SdlRect.x + checkbox.Rect.SdlRect.w &&
                                              buttonEvent.y >= checkbox.Rect.SdlRect.y &&
                                              buttonEvent.y <= checkbox.Rect.SdlRect.y + checkbox.Rect.SdlRect.h)?.OnClick();
        }

        public void Draw()
        {
            var flags = (SDL_WindowFlags)SDL_GetWindowFlags(_renderer.Window.SdlWindow);
            _checkboxes[0].Checked = flags.HasFlag(SDL_WindowFlags.SDL_WINDOW_FULLSCREEN);
            
            foreach (var button in _buttons)
            {
                button.Draw(_mouse);
            }

            foreach (var checkbox in _checkboxes)
            {
                checkbox.Draw();
            }
            
            _resolutionDropdown.Draw();
        }
    }
}