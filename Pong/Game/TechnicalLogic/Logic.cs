/*
 * @file Logic.cs
 * @author Muaaz Bdear
 * @brief
 * @version 0.1
 * @date 2022-11-14
 *
 * @copyright Copyright (c) 2022
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Timers;
using Pong.Game.Models;
using Microsoft.VisualBasic;
using Pong.Engine;
using Pong.Engine.Defaults;
using Pong.Engine.Models;
using Pong.GUI.Components.Text;
using static SDL2.SDL;

namespace Pong.Game.TechnicalLogic
{
    public class Logic : Constants
    {
        private Renderer _renderer;
        private TextDisplay _resetTimerDisplay;
        
        //Reset timer needs to be 1 second more than you think it should be
        //not quite sure why
        private int _resetTimeSeconds;

        //Ball
        private Ball _ball;

        //paddle
        private Paddle _paddleLeft;
        private Paddle _paddleRight;
        private bool _aiEnabled;

        //User
        private User _leftPlayer;
        private User _rightPlayer;
        private Music _losMusic = new Music(Constants.LosMusic);
        private Music _hitMusic = new Music(Constants.HitMusic);
        private TextDisplay _scoreLimitDisplay;
        private int _scoreLimit;

        public User LeftPlayer => _leftPlayer;
        public User RightPlayer => _rightPlayer;
        public string Winner { get; private set; }

        public Logic(Renderer renderer, int scoreLimit, string player1Name, string player2Name = null)
        {
            _renderer = renderer;
            _scoreLimit = scoreLimit;
            _resetTimeSeconds = 0;

            var centerPos = new SDL_Rect()
            {
                w = 500,
                h = 100,
                x = _renderer.LogicalSize.LogicalWidth / 2 - 500 / 2,
                y = _renderer.LogicalSize.LogicalHeight / 2 - 100 / 2
            };
            _resetTimerDisplay = new TextDisplay(renderer, $"{_resetTimeSeconds}", Color.White, centerPos);
            
            if (_scoreLimit > 0)
            {
                centerPos = new SDL_Rect()
                {
                    w = 800,
                    h = UISizes.ButtonHeight,
                    x = _renderer.LogicalSize.LogicalWidth / 2 - 800 / 2,
                    y = 30
                };
                _scoreLimitDisplay = new TextDisplay(renderer, $"First to {_scoreLimit}", Color.Wheat, centerPos, isCenteredInParent: true);
            }
            _paddleLeft = new Paddle(_renderer.LogicalSize);
            _paddleRight = new Paddle(_renderer.LogicalSize, true);
            _ball = new Ball(_renderer.LogicalSize);
            _ball.RestBallPostion(_ball.Position.X, _ball.Position.Y);
            _leftPlayer = new User(renderer, player1Name, true);
            _rightPlayer = player2Name != null ?  new User(renderer, player2Name) : new User(renderer,"Ai");
            _aiEnabled = player2Name == null;
        }

        private void ResetTimer()
        {
            SDL_Log($"{_resetTimeSeconds}");
            
            //No, moving this down does not remove the need for the added second
            //I tried
            _resetTimeSeconds--;
            _resetTimerDisplay.UpdateText($"{_resetTimeSeconds}");
            _resetTimerDisplay.Draw();
            SDL_Delay(1000);
        }

        public void Update()
        {
            if(_aiEnabled)
                AI();
            Collision();
            
            if(_resetTimeSeconds > 0)
                return;
            
            _ball.Move();
        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="sdlEvent"></param>
        public void UpdatePlayer(SDL_Event sdlEvent)
        {
            PollEvents(sdlEvent);
            Draw();
        }


// Controlls if any event is in the backgound or pocessing

        void PollEvents(SDL_Event sdlEvent)
        {
            //If its not a keydown event, we can just return
            if (sdlEvent.type != SDL_EventType.SDL_KEYDOWN) 
                return;
            
            switch (sdlEvent.key.keysym.sym)
            {
                case SDL_Keycode.SDLK_UP:
                    if (_rightPlayer.Name == "Ai")
                        break;
                    _paddleRight.Position.Y -= 20;
                    _paddleRight.Position.Y -= Constants.PADDLE_MOVE;
                    _paddleRight.SdlRect.y = _paddleRight.Position.Y;
                    SDL_Log($"{_paddleRight.Position.Y}");
                    if (_paddleRight.Position.Y == -15)
                    {
                        _paddleRight.Position.Y +=Constants.PADDLE_MOVE;
                    }
                    //   y_p -= 20;
                    break;
                case SDL_Keycode.SDLK_DOWN:
                    if (_rightPlayer.Name == "Ai")
                        break;
                    _paddleRight.Position.Y += 20;
                    _paddleRight.Position.Y += Constants.PADDLE_MOVE;
                    _paddleRight.SdlRect.y = _paddleRight.Position.Y;
                    SDL_Log($"{_paddleRight.Position.Y}");
                    if (_paddleRight.Position.Y == 945)
                    {
                        _paddleRight.Position.Y -=Constants.PADDLE_MOVE;
                    }
                    //   y_p += 20;
                    break;
                case SDL_Keycode.SDLK_w:
                    _paddleLeft.Position.Y -= Constants.PADDLE_MOVE;
                    _paddleLeft.SdlRect.y = _paddleLeft.Position.Y ;
                    if (_paddleLeft.Position.Y == -15)
                    {
                        _paddleLeft.Position.Y +=Constants.PADDLE_MOVE;
                    }
                    
                    //   y_p -= 20;
                    break;
                case SDL_Keycode.SDLK_s:
                    _paddleLeft.Position.Y += Constants.PADDLE_MOVE;
                    _paddleLeft.SdlRect.y = _paddleLeft.Position.Y;
                    if (_paddleLeft.Position.Y == 945)
                    {
                        _paddleLeft.Position.Y-=Constants.PADDLE_MOVE;
                    }
                    //   y_p += 20;
                    break;
                case SDL_Keycode.SDLK_ESCAPE:
                    Program.Paused = true;
                    Program.ChangeState(State.InMainMenu);
                    break;
            }
        }
        
        public void Draw()
        {
            var paddleLeftRenderObject = new RenderObject
            {
                Type = RenderObjectTypes.Rect,
                Fill = true,
                ZIndex = 1,
                RectObjects = new []
                {
                    new RectObject
                    {
                        Colour = Color.Red,
                        SdlRect = _paddleLeft.SdlRect
                    }
                }
            };
            
            var paddleRightRenderObject = new RenderObject
            {
                Type = RenderObjectTypes.Rect,
                Fill = true,
                ZIndex = 1,
                RectObjects = new []
                {
                    new RectObject
                    {
                        Colour = Color.Yellow,
                        SdlRect = _paddleRight.SdlRect
                    }
                }
            };

            var ballRenderObject = new RenderObject
            {
                Type = RenderObjectTypes.Rect,
                Fill = true,
                ZIndex = 1,
                RectObjects = new []
                {
                    new RectObject
                    {
                        Colour = Color.White,
                        SdlRect = _ball.SdlRect
                    }
                }
            };

            var tableRect = new RenderObject
            {
                Type = RenderObjectTypes.Rect,
                Fill = true,
                RectObjects = new[]
                {
                    new RectObject
                    {
                        Colour = Color.DarkOliveGreen,
                        SdlRect = new SDL_Rect
                        {
                            x = 0,
                            y = 0,
                            h = _renderer.LogicalSize.LogicalHeight,
                            w = _renderer.LogicalSize.LogicalWidth
                        }
                    }
                }
            };
            
            var halfLogicalWidth = _renderer.LogicalSize.LogicalWidth / 2;
            var lineRenderObject = new RenderObject
            {
                Type = RenderObjectTypes.Line,
                ZIndex = 1,
                LineObjects = new []
                {
                    new LineObject
                    {
                        Colour = Color.White,
                        X1 = halfLogicalWidth,
                        X2 = halfLogicalWidth,
                        Y1 = 0,
                        Y2 = _renderer.LogicalSize.LogicalHeight
                    }
                }
            };


            var renderObjects = new []
            {
                paddleLeftRenderObject,
                paddleRightRenderObject,
                ballRenderObject,
                lineRenderObject,
                tableRect,
                _leftPlayer.UserTextDisplayRenderObject,
                _rightPlayer.UserTextDisplayRenderObject
            };

            if (_scoreLimit > 0)
            {
                _scoreLimitDisplay.Draw();    
            }

            if (_resetTimeSeconds > 0)
                ResetTimer();
            
            _renderer.AddMultipleToRenderQueue(renderObjects);
        }
        public void AI()
        {
            if (_ball.Position.X < _renderer.LogicalSize.LogicalWidth - _renderer.LogicalSize.LogicalWidth / 14)
            {
                _paddleRight.Position.Y = _ball.Position.Y - Constants.PADDLE_HEIGHT / 2;
                _paddleRight.SdlRect.y = _paddleRight.Position.Y;
            }
            else
            {
                _paddleRight.Position.Y = _ball.Position.Y >= _paddleRight.Position.Y + Constants.PADDLE_HEIGHT / 2 ? _paddleRight.Position.Y += 1 : _paddleRight.Position.Y -1;
                _paddleRight.SdlRect.y = _paddleRight.Position.Y;
            }
        }

        private void EndGame(string winnerName)
        {
            Winner = winnerName;
            Program.ChangeState(State.GameOver);
        }

        private void ResetGame()
        {
            _paddleLeft = new Paddle(_renderer.LogicalSize);
            _paddleRight = new Paddle(_renderer.LogicalSize, true);
            // _resetTimerDisplay.UpdateText($"{_resetTimeSeconds}");
            _resetTimeSeconds = 4;
            _ball.RestBallPostion(_renderer.LogicalSize.LogicalWidth / 2, _renderer.LogicalSize.LogicalHeight / 2);
        }

        void Collision()
        {
            if (_ball.Position.X >= _renderer.LogicalSize.LogicalWidth - Constants.BALLRADIUS)
            {
                _losMusic.playMusik();
                _leftPlayer.Score += 1;
                if (_scoreLimit > 0 && _leftPlayer.Score >= _scoreLimit)
                {
                    EndGame(_leftPlayer.Name);
                }
                _leftPlayer.UpdateTextDisplay();
                
                SDL_Log($"{_leftPlayer.Name}: {_leftPlayer.Score}");
                ResetGame();
            }

            if (_ball.Position.X <= 0)
            {
                _losMusic.playMusik();
                _rightPlayer.Score += 1;
                if (_scoreLimit > 0 && _rightPlayer.Score >= _scoreLimit)
                {
                    EndGame(_rightPlayer.Name);
                }
                _rightPlayer.UpdateTextDisplay();
                SDL_Log($"{_rightPlayer.Name}: {_rightPlayer.Score}");
                ResetGame();
            }

            if (_ball.Position.Y >= _renderer.LogicalSize.LogicalHeight- Constants.BALLRADIUS || _ball.Position.Y <= 0)
                _ball.DirectionY *= -1;
            
            // add music must be done
            if (SDL_HasIntersection(ref _ball.SdlRect, ref _paddleLeft.SdlRect) == SDL_bool.SDL_TRUE ||
                SDL_HasIntersection(ref _ball.SdlRect, ref _paddleRight.SdlRect) == SDL_bool.SDL_TRUE)
            {
                _hitMusic.playMusik();
                _ball.DirectionX *= -1;
                _ball.BallSpeed_X+=2;
                _ball.BallSpeed_Y++;
                SDL_Log("Collision");
                
            }
        }
    }
}