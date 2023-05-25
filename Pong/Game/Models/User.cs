using System;
using System.Drawing;
using Pong.Engine;
using Pong.Engine.Models;
using Pong.GUI.Components.Text;
using static SDL2.SDL;

/**
     * 
     * @author Nour AHmed
     */
    namespace Pong.Game.Models
    {
        public class User
        {
            private TextDisplay _textDisplay;
            private bool _isLeft;
            public string Name { get; set; }
            public int Score { get; set; }
            public RenderObject UserTextDisplayRenderObject => _textDisplay.RenderObject;

            public User(Renderer renderer, string name, bool isLeft = false)
            {
                _isLeft = isLeft;
                Name = name;
                Score = 0;

                var sdlRect = new SDL_Rect
                {
                    x = renderer.LogicalSize.LogicalWidth / 2,
                    y = 10,
                    w = name.Length * 24,
                    h = 50
                };

                var textDisplayText = "";
                if (isLeft)
                {
                    sdlRect.x -= sdlRect.w + 70;
                    textDisplayText = $"{Name} - {Score}";
                }
                else
                {
                    sdlRect.x += 70;
                    textDisplayText = $"{Score} - {Name}";
                }
                _textDisplay = new TextDisplay(renderer, textDisplayText, Color.White, sdlRect);
            }

            public void UpdateTextDisplay()
            {
                var textDisplayText = _isLeft ? $"{Name} - {Score}" : $"{Score} - {Name}";
                _textDisplay.UpdateText(textDisplayText);
            }
        }
    }
