using System.Drawing;
using static SDL2.SDL;

namespace Pong.Engine.Models
{
    public class RectObject
    {
        public SDL_Rect SdlRect;

        public Color Colour { get; set; }
    }
}