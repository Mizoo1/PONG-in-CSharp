using static SDL2.SDL;

namespace Pong.GUI.Components
{
    public class Mouse
    {
        public SDL_Rect Position;

        public Mouse()
        {
            Position = new SDL_Rect
            {
                w = 1,
                h = 1
            };
        }

        public void Update(SDL_MouseMotionEvent mouseMotionEvent)
        {
            Position.x = mouseMotionEvent.x;
            Position.y = mouseMotionEvent.y;
        }

        public bool Intersects(SDL_Rect target)
        {
            return SDL_HasIntersection(ref Position, ref target) == SDL_bool.SDL_TRUE;
        }
    }
}