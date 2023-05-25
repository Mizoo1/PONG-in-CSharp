using System.Drawing;
using static SDL2.SDL;

namespace Pong.Engine.Models
{
    public class PointObject
    {
        public SDL_Point SdlPoint { get; }

        public Color Colour { get; init; }
    }
}