using Pong.Engine;
using Pong.Engine.Defaults;
using Pong.Game.TechnicalLogic;
using Pong.GUI;
using Pong.GUI.Components;

namespace Pong.Game;

public class MainGame
{
    private Renderer _renderer;
    private Mouse _mouse;
    private Logic _logic;
    private GameStartMenu _startMenu;
    private bool _isSinglePlayer;
    private string _playerOneName;
    private string _playerTwoName;
    private GameOver _gameOver;

    public Logic Logic => _logic;
    public GameStartMenu GameStartMenu => _startMenu;
    public GameOver GameOver => _gameOver;

    public MainGame(Renderer renderer, Mouse mouse)
    {
        _gameOver = null;
        _renderer = renderer;
        _mouse = mouse;
        _startMenu = new GameStartMenu(renderer, mouse);
    }

    public void Draw()
    {
        switch (Program.State)
        {
            case State.InGameMenu:
                _startMenu.Draw();
                break;
            case State.InGame:
                _logic.Draw();
                break;
            case State.GameOver:
                if (_gameOver == null)
                {
                    _gameOver = new GameOver(_renderer, _logic.Winner, _logic.LeftPlayer, _logic.RightPlayer);
                }
                _gameOver.Draw(_mouse);
                break;
        }
    }

    public void UpdateNames()
    {
        _playerOneName = _startMenu.PlayerOneName;
        _playerTwoName = _startMenu.PlayerTwoName;
    }

    public void CreateLogic()
    {
        if (_startMenu.LocalState == State.Singleplayer)
            _playerTwoName = null;
        var hasScoreLimit = int.TryParse(_startMenu.ScoreLimit.Text, out var scoreLimit);
        _logic = new Logic(_renderer, hasScoreLimit ? scoreLimit : 0, _playerOneName, _playerTwoName);
        _startMenu.LocalState = State.CreateGame;
        _gameOver = null;
        Program.ChangeState(State.InGame);
    }
}