
using Pong.Engine.Models;
using Pong.Game.Models;
using static SDL2.SDL;

/*
 * @Author: Nour Ahmed
 */

public class Paddle
{
    public Position Position;
    public SDL_Rect SdlRect;

    public Paddle(LogicalSize logicalSize, bool isRight = false)
    {
        Position = new Position
        {
            X = isRight ? logicalSize.LogicalWidth - Constants.PADDLE_WIDTH * 2 : Constants.PADDLE_WIDTH,
            Y = logicalSize.LogicalHeight / 2 - Constants.PADDLE_HEIGHT / 2
        };
        SdlRect = new SDL_Rect
        {
            h = Constants.PADDLE_HEIGHT,
            w = Constants.PADDLE_WIDTH,
            y = Position.Y,
            x = Position.X
        };
    }
}