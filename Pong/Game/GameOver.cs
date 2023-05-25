using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Pong.Engine;
using Pong.Engine.Defaults;
using Pong.Game.Models;
using Pong.GUI.Components;
using Pong.GUI.Components.Text;
using static SDL2.SDL;

namespace Pong.Game;

public class GameOver
{
    private Renderer _renderer;
    private List<TextDisplay> _textDisplays;
    private List<Button> _buttons;

    public GameOver(Renderer renderer, string winnerName, User leftPlayer, User rightPlayer)
    {
        _renderer = renderer;
        _textDisplays = new List<TextDisplay>();
        _buttons = new List<Button>();
        var winText = $"{winnerName} Won!";
        var centerRect = new SDL_Rect
        {
            x = renderer.LogicalSize.LogicalWidth / 2 - UISizes.ButtonWidth / 2,
            y = (int)(renderer.LogicalSize.LogicalHeight * 0.10),
            h = 100,
            w = UISizes.ButtonWidth
        };
        var winnerNameDisplay = new TextDisplay(renderer, winText, Color.Gold, centerRect);
        _textDisplays.Add(winnerNameDisplay);

        var scoreText = $"{leftPlayer.Name} - {leftPlayer.Score} | {rightPlayer.Score} - {rightPlayer.Name}";
        centerRect.y += 100;
        var scoreDisplay = new TextDisplay(renderer, scoreText, Color.White, centerRect);
        _textDisplays.Add(scoreDisplay);
        
        var centerPos = new Position
        {
            X = renderer.LogicalSize.LogicalWidth / 2 - UISizes.ButtonWidth / 2,
            Y = renderer.LogicalSize.LogicalHeight - UISizes.ButtonHeight - 10
        };
        var playAgainButton = new Button(renderer, "Play Again", centerPos,
            UISizes.ButtonWidth, UISizes.ButtonHeight, Color.Black, Color.Wheat);
        playAgainButton.Click += PlayAgainButtonOnClick;
        _buttons.Add(playAgainButton);
    }

    private void PlayAgainButtonOnClick(object sender, EventArgs e)
    {
        Program.ChangeState(State.InGameMenu);
    }

    public void Checkbuttons(Mouse mouse)
    {
        _buttons.FirstOrDefault(button => mouse.Intersects(button.Rect))?.OnClick();
    }
    
    public void Draw(Mouse mouse)
    {
        foreach (var textDisplay in _textDisplays)
        {
            textDisplay.Draw();
        }

        foreach (var button in _buttons)
        {
            button.Draw(mouse);
        }
    }
}