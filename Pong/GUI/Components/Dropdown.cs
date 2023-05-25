using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Pong.Engine;
using Pong.Game.Models;
using static SDL2.SDL;

namespace Pong.GUI.Components
{
    public class Dropdown
    {
        private List<Button> _options;
        private Dictionary<string, object> _optionDictionary;
        private Mouse _mouse;

        public object _selectedOption { get; private set; }
        public event EventHandler Click;

        public Dropdown(Renderer renderer, Mouse mouse, Dictionary<string, object> inputDictionary, SDL_Rect sdlRect)
        {
            _mouse = mouse;
            _options = new List<Button>();
            _optionDictionary = new Dictionary<string, object>();
            foreach (var (name, option) in inputDictionary)
            {
                sdlRect.y += sdlRect.h - 20;
                var pos = new Position
                {
                    X = sdlRect.x,
                    Y = sdlRect.y
                };
                var newOption = new Button(renderer, name, pos, 400, 100, Color.Black, Color.LightGray );
                newOption.Click += OptionOnClick;
                _optionDictionary.Add(name, option);
                _options.Add(newOption);
            }
        }
        
        public void OnClick()
        {
            var handler = Click;
            handler?.Invoke(_selectedOption, EventArgs.Empty);
        }

        private void OptionOnClick(object sender, EventArgs e)
        {
            var button = (Button) sender;
            _selectedOption = _optionDictionary[button.Text];
            OnClick();
        }

        public void Draw()
        {
            foreach (var option in _options)
            {
                option.Draw(_mouse);
            }
        }

        public void CheckOptions(SDL_MouseButtonEvent buttonEvent)
        {
            _options.FirstOrDefault(button => buttonEvent.x >= button.Rect.x &&
                                               buttonEvent.x <= button.Rect.x + button.Rect.w &&
                                               buttonEvent.y >= button.Rect.y &&
                                               buttonEvent.y <= button.Rect.y + button.Rect.h)?.OnClick();
        }
    }
}