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

namespace Pong.GUI;

public class GameStartMenu
{
    private Renderer _renderer;
    private List<Button> _buttons;
    private Mouse _mouse;
    private TextInput _selectedTextInput;
    private TextInput _playerOneName;
    private TextInput _playerTwoName;
    private TextInput _scoreLimit;

    public List<Button> Buttons => _buttons;
    public TextInput SelectedTextInout => _selectedTextInput;
    public State LocalState;
    public string PlayerOneName => _playerOneName.Text;
    public string PlayerTwoName => _playerTwoName.Text;
    public TextInput ScoreLimit => _scoreLimit;

    public GameStartMenu(Renderer renderer, Mouse mouse)
    {
        LocalState = Program.State;

        _buttons = new List<Button>();
        _renderer = renderer;
        _mouse = mouse;


        CreateTextInputs();
        CreateButtons();
    }

    public void CreateTextInputs()
    {
        var inputRect = new SDL_Rect
        {
            w = 600, 
            h = 75, 
            x = 100, 
            y = (int)(_renderer.LogicalSize.LogicalHeight * 0.25)
        };
        
        _playerOneName = new TextInput(_renderer, Color.White, inputRect, "Player One");
        
        inputRect.y += UISizes.ButtonHeight + 10;
        _playerTwoName = new TextInput(_renderer, Color.White, inputRect, "Player Two");
        
        inputRect.y = (int)(_renderer.LogicalSize.LogicalHeight * 0.25);
        inputRect.x = _renderer.LogicalSize.LogicalWidth - inputRect.w - 100;
        _scoreLimit = new TextInput(_renderer, Color.White, inputRect, "10");
    }

    private void CreateButtons()
    {
        var centerPos = new Position
        {
            // Center buttons in the middle of the screen
            X = _renderer.LogicalSize.LogicalWidth / 2 - UISizes.ButtonWidth / 2,
                
            // Give buttons x% padding up top
            Y = (int)(_renderer.LogicalSize.LogicalHeight * 0.25)
        };
        
        var singlePlayerButton = new Button(_renderer, "Singleplayer", centerPos, UISizes.ButtonWidth,
            UISizes.ButtonHeight, Color.Black, Color.Wheat);
        singlePlayerButton.Click += ButtonOnClick;
        _buttons.Add(singlePlayerButton);
        
        centerPos.Y += UISizes.ButtonHeight + 20;
        var twoPlayerButton = new Button(_renderer, "Multiplayer", centerPos, UISizes.ButtonWidth, UISizes.ButtonHeight,
            Color.Black, Color.Wheat);
        twoPlayerButton.Click += ButtonOnClick;
        _buttons.Add(twoPlayerButton);
        
        centerPos.Y += UISizes.ButtonHeight + 20;
        var backButton = new Button(_renderer, "Back", centerPos, UISizes.ButtonWidth, UISizes.ButtonHeight, Color.Black,
            Color.Gold);
        backButton.Click += ButtonOnClick;
        _buttons.Add(backButton);
        
        centerPos.Y = singlePlayerButton.Rect.y;
        var playButton = new Button(_renderer, "Play", centerPos, UISizes.ButtonWidth, UISizes.ButtonHeight,
            Color.Black, Color.Wheat);
        playButton.Click += ButtonOnClick;
        _buttons.Add(playButton);
    }

    private void ButtonOnClick(object sender, EventArgs e)
    {
        var button = (Button) sender;
        switch (button.Text)
        {
            case "Back":
                if (LocalState is State.Singleplayer or State.TwoPlayer)
                {
                    LocalState = State.InGameMenu;
                }
                else
                {
                    Program.ChangeState(State.InMainMenu);
                }
                break;
            case "Multiplayer":
                LocalState = State.TwoPlayer;
                break;
            case "Singleplayer":
                LocalState = State.Singleplayer;
                break;
            case "Play":
                Program.ChangeState(State.CreateGame);
                break;
        }
    }

    public void CheckButtons(SDL_MouseButtonEvent buttonEvent)
    {
        _buttons.FirstOrDefault(button => _mouse.Intersects(button.Rect) && button.IsClickable)?.OnClick();
    }

    public TextInput CheckTextInputs()
    {
        if (_mouse.Intersects(_playerOneName.Frame))
        {
            _selectedTextInput = _playerOneName;
        }
        else if (_mouse.Intersects(_playerTwoName.Frame) && LocalState == State.TwoPlayer) 
        {
            _selectedTextInput = _playerTwoName;
        }
        else if (_mouse.Intersects(_scoreLimit.Frame))
        {
            _selectedTextInput = _scoreLimit;
        }
        else
        {
            _selectedTextInput = null;
        }

        return _selectedTextInput;
    }

    public void DrawButtons(int startIndex, int endIndex)
    {
        for (var i = 0; i < _buttons.Count; i++)
        {
            if (i < startIndex || i > endIndex)
            {
                _buttons[i].IsClickable = false;
            }
            else
            {
                _buttons[i].IsClickable = true;
                _buttons[i].Draw(_mouse);
            }
        }
    }
    
    public void Draw()
    {
        switch (LocalState)
        {
            case State.Singleplayer:
                _playerOneName.Draw();
                _scoreLimit.Draw();
                DrawButtons(2, _buttons.Count);
                break;
            case State.TwoPlayer:
                _playerOneName.Draw();
                _playerTwoName.Draw();
                _scoreLimit.Draw();
                DrawButtons(2, _buttons.Count);
                break;
            default:
                DrawButtons(0, 2);
                break;
        }
    }
}