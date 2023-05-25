using System;
using Microsoft.VisualBasic;
using Pong.Engine.Models;
using Pong.Game.Models;
using static SDL2.SDL;

public class Ball
{
    public Position Position;
    public int DirectionX { get; set; }
    public int DirectionY { get; set; }
    public int BallSpeed_X ;
    public int BallSpeed_Y ;

    public SDL_Rect SdlRect;

    public Ball(LogicalSize logicalSize)
    {
        Position = new Position
        {
            X = logicalSize.LogicalWidth / 2,
            Y = logicalSize.LogicalHeight / 2
        };
        
        SdlRect = new SDL_Rect
        {
            h = Constants.BALLRADIUS,
            w = Constants.BALLRADIUS,
            y = Position.Y,
            x = Position.X
        };
    }
    
    public void RestBallPostion(int x, int y)
    {
        
        Position.X = x;
        Position.Y = y;
        Random r = new Random();
        // Random BAll dirction in X Postion 
        int[] arrayRandom_x = { -1, 1 };
        int newX = r.Next(2);
        DirectionX = arrayRandom_x[newX];
        // Random BAll dirction in Y Postion 
        int[] arrayRandom_y = { -1, 1 };
        int newY = r.Next(2);
        DirectionY = arrayRandom_y[newY];
        BallSpeed_X = 4;
        BallSpeed_Y = 1;
        
    }
    
    public void Move()
    {
        if (DirectionX == -1 && DirectionY == -1)
        {
            Position.X -= BallSpeed_X;
            Position.Y -= BallSpeed_Y;
        }
        else if (DirectionX == -1 && DirectionY == 1)
        {
            Position.X -= BallSpeed_X;
            Position.Y += BallSpeed_Y;
        }
        else if (DirectionX == 1 && DirectionY == -1)
        {
            Position.X += BallSpeed_X;
            Position.Y -= BallSpeed_Y;
        }
        else
        {
            Position.X += BallSpeed_X;
            Position.Y += BallSpeed_Y;
        }

        SdlRect.y = Position.Y;
        SdlRect.x = Position.X;
    }
    
}