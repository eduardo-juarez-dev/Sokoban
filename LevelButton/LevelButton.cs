using Godot;
using System;

public partial class LevelButton : NinePatchRect
{
	private static Resource _greenTexture = GD.Load("res://assets/green_panel.png");
	private ScoreSync _scoreSync;
	private static SignalManager _signalManager;
	private Label _levelLabel;
	private TextureRect _checkMark;
	
	private string _levelNumber = "22";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_scoreSync = GetNode<ScoreSync>("/root/ScoreSync");
		_signalManager = GetNode<SignalManager>("/root/SignalManager");
		_levelLabel = GetNode<Label>("LevelLabel");
		_checkMark = GetNode<TextureRect>("Checkmark");

		_levelLabel.Text = _levelNumber;
		if(_scoreSync.HasLevelScore(_levelNumber))
			_checkMark.Show();
	}

	public void SetLevelNumber(string levelNumber)
	{
		_levelNumber = levelNumber;
	}

	private void OnGuiInput(InputEvent ie)
	{
		if (ie.IsActionPressed("select"))
		{
			Texture = (Texture2D)_greenTexture;
			_signalManager.EmitSignal(SignalManager.SignalName.OnLevelSelected, _levelNumber);
			GD.Print($"Selected: {_levelNumber}");
		}
	}
}
