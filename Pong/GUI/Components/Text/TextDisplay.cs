using System;
using System.Drawing;
using Pong.Engine;
using Pong.Engine.Defaults;
using Pong.Engine.Models;
using Pong.Game.Models;
using SDL2;
using static SDL2.SDL_ttf;
using static SDL2.SDL;

namespace Pong.GUI.Components.Text
{
    public class TextDisplay
    {
        private IntPtr _lato;
        private SDL_Color _sdlColor;
        private Renderer _renderer;
        private SDL_Rect _baseRect;
        private SDL_Rect _rect;
        private RenderObject _renderObject;

        public RenderObject RenderObject => _renderObject;

        public string Text { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="text"></param>
        /// <param name="colour"></param>
        /// <param name="sdlRect"></param>
        /// <param name="size"></param>
        /// <param name="isCenteredInParent"></param>
        public TextDisplay(Renderer renderer, string text, Color colour, SDL_Rect sdlRect, int size = 72, bool isCenteredInParent = false)
        {
            _baseRect = sdlRect;
            _lato = TTF_OpenFont("GUI\\Components\\Fonts\\Lato-Regular.ttf", size);
            _renderer = renderer;
            _sdlColor = new SDL_Color
            {
                r = colour.R,
                g = colour.G,
                b = colour.B,
                a = colour.A
            };
            if (isCenteredInParent)
            {
                _rect.w = text.Length * 24;
                _rect.h = sdlRect.h / 2;
                
                _rect.x = sdlRect.x + (sdlRect.w - _rect.w) / 2;
                _rect.y = sdlRect.y + _rect.h / 2;
            }
            else
            {
                _rect = sdlRect;
            }
            
            UpdateText(text);
        }

        public void UpdateText(string text, int size = 20)
        {
            Text = text;
            _rect.w = text.Length * size;
            _rect.x = _baseRect.x + (_baseRect.w - _rect.w) / 2;
            var messageSurface = TTF_RenderUTF8_Blended(_lato, text, _sdlColor);
            var message = SDL_CreateTextureFromSurface(_renderer.SdlRenderer, messageSurface);
            _renderObject = new RenderObject
            {
                Type = RenderObjectTypes.Texture,
                ZIndex = 2,
                RectObjects = new RectObject[]
                {
                    new ()
                    {
                        SdlRect = _rect,
                    }
                },
                Texture = message,
            };
        }

        public void Draw()
        {
           _renderer.AddToRenderQueue(_renderObject);
        }
    }
}