using System;
using System.Drawing;
using Pong.Engine;
using Pong.Engine.Defaults;
using Pong.Engine.Models;
using Pong.Game.Models;
using Pong.GUI.Components.Text;
using Pong.Utilities;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace Pong.GUI.Components
{
    public class Button
    {
        private IntPtr _defaultTexture;
        private IntPtr _hoverTexture;
        private Renderer _renderer;

        private RenderObject _buttonBackground;
        private TextDisplay _textDisplay;
        private Color _colour;
        
        public bool IsHovered;
        public SDL_Rect Rect;
        public string Text => _textDisplay.Text;
        public event EventHandler Click;
        public bool IsClickable;
        

        public Button(Renderer renderer, string text, Position position, int width, int height, Color colour, Color backgroundColour)
        {
            _renderer = renderer;
            
            _colour = backgroundColour;
            Rect = new SDL_Rect
            {
                w = (int)(width * 0.75),
                h = (int) (height * 0.75),
                x = position.X,
                y = position.Y
            };
            
            _textDisplay = new TextDisplay(_renderer, text, colour, Rect, isCenteredInParent: true);

            _buttonBackground = new RenderObject
            {
                Type = RenderObjectTypes.Rect,
                ZIndex = 1,
                RectObjects = new RectObject[]
                {
                    new()
                    {
                        SdlRect = Rect,
                        Colour =  colour
                    }
                },
                Fill = true
            };
            
            IsClickable = false;
        }

        public void OnClick()
        {
            var handler = Click;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public void Draw(Mouse mouse)
        {
            IsHovered = SDL_IntersectRect(ref Rect, ref mouse.Position, out _) == SDL_bool.SDL_TRUE;

            if (_defaultTexture != IntPtr.Zero && _hoverTexture != IntPtr.Zero)
            {
                var renderObject = new RenderObject
                {
                    Type = RenderObjectTypes.Texture,
                    Texture = IsHovered ? _hoverTexture : _defaultTexture,
                    RectObjects = new RectObject[]
                    {
                        new()
                        {
                            SdlRect = Rect
                        }
                    }
                };
                _renderer.AddToRenderQueue(renderObject);
            }
            else
            {
                _buttonBackground.RectObjects[0].Colour = IsHovered ? ColorExtensions.ChangeColorBrightness(_colour, 0.7f) : _colour;
                _renderer.AddToRenderQueue(_buttonBackground);
                _textDisplay.Draw();
            }
        }

        public void UpdateText(string text)
        {
            _textDisplay.UpdateText(text);
        }
    }
}