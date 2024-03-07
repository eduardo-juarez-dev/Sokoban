using Godot;
using System;

public partial class Hud : Control
{
	private ScoreSync _scoreSync;
	private Label _levelLabel;
	private Label _movesLabel;
	private Label _bestLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_scoreSync = GetNode<ScoreSync>("/root/ScoreSync");
		_levelLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/LevelLabel");
		_movesLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer2/MovesLabel");
		_bestLabel = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer3/BestLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetLevelLabel(string level)
	{
		_levelLabel.Text = level;
	}
	
	public void SetMovesLabel(int moves)
	{
		_movesLabel.Text = moves.ToString();
	}

	public void SetBestLabel(int best)
	{
		_bestLabel.Text = best.ToString();
	}

	public void NewGame(string level)
	{
		SetBestLabel(_scoreSync.GetLevelBestScore(level));
		SetMovesLabel(0);
		SetLevelLabel(level);
		Show();
	}

	// private void GameOver()
	// {
	// 	Hide();
	// }
}
