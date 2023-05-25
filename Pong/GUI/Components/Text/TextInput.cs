using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Pong.Engine;
using Pong.Engine.Defaults;
using Pong.Engine.Models;
using static SDL2.SDL;

namespace Pong.GUI.Components.Text
{
    public class TextInput
    {
        private Renderer _renderer;
        private string _text;
        private Color _clour;
        private SDL_Rect _textArea;

        private TextDisplay _textDisplay;
        public string Text => _text;
        public SDL_Rect Frame;

        public event EventHandler Click;

        public TextInput(Renderer renderer, Color colour, SDL_Rect frame, string text = "")
        {
            _renderer = renderer;
            _clour = colour;
            _text = text;
            Frame = frame;
            _textArea = new SDL_Rect
            {
                w = Frame.w,
                h = Frame.h,
                y = Frame.y,
                x = frame.x
            };

            _textDisplay = new TextDisplay(renderer, text, colour, _textArea, isCenteredInParent: false);
        }
        
        public void OnClick()
        {
            var handler = Click;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public void Write(SDL_Event sdlEvent)
        {
            var pressedKey = sdlEvent.key.keysym.sym;
            SDL_Log($"{pressedKey}");
            switch (pressedKey)
            {
                case SDL_Keycode.SDLK_BACKSPACE when _text.Length > 0:
                    _text = _text.Remove(_text.Length - 1);
                    if (_text.Length == 0)
                    {
                        _text = " ";
                    }
                    break;
                case SDL_Keycode.SDLK_c when SDL_GetModState() == SDL_Keymod.KMOD_CTRL:
                    SDL_SetClipboardText(_text);
                    break;
                case SDL_Keycode.SDLK_v when SDL_GetModState() == SDL_Keymod.KMOD_CTRL:
                    _text = SDL_GetClipboardText();
                    break;
                default:
                {
                    if (sdlEvent.type == SDL_EventType.SDL_TEXTINPUT && SDL_GetModState() != SDL_Keymod.KMOD_CTRL)
                    {
                        unsafe
                        {
                            if (_text == " ")
                            {
                                _text = string.Empty;
                            }
                            var text = Marshal.PtrToStringUTF8((IntPtr)sdlEvent.text.text);
                            _text += text;
                        }
                    }
                    break;
                }
            }
        }

        public void Draw()
        {
            if (string.IsNullOrEmpty(_text))
                return;

            _textDisplay.UpdateText(_text);
            _textDisplay.Draw();

            var frameObject = new RenderObject
                {
                    Type = RenderObjectTypes.Rect,
                    ZIndex = 1,
                    RectObjects = new[]
                    {
                        new RectObject()
                        {
                            SdlRect = Frame,
                            Colour = Color.Red
                        }
                    }
                };

            var textAreaObject = new RenderObject
            {
                Type = RenderObjectTypes.Rect,
                Fill = true,
                ZIndex = 1,
                RectObjects = new[]
                {
                    new RectObject
                    {
                        SdlRect = _textArea,
                        Colour = Color.Black
                    }
                }
            };

            var renderObjects = new []
            {
                textAreaObject,
                frameObject
            };
                
            _renderer.AddMultipleToRenderQueue(renderObjects);
        }
    }
}