using Godot;
using System;

public partial class GameOverUi : Control
{
    private ScoreSync _scoreSync;
    private Label _recordLabel;
    private Label _movesLabel;
	
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _scoreSync = GetNode<ScoreSync>("/root/ScoreSync");
        _recordLabel = GetNode<Label>("MC/NP/MC/VBoxContainer/RecordLabel");
        _movesLabel = GetNode<Label>("MC/NP/MC/VBoxContainer/MovesLabel");
    }

    public void NewGame()
    {
        Hide();
        _recordLabel.Hide();
    }

    public void GameOver(string level, int moves)
    {
        _movesLabel.Text = $"Moves made: {moves}";
        Show();
        if (_scoreSync.ScoreIsNewBest(level, moves))
        {
            _recordLabel.Show();
        }
    }
}