using Godot;
using System;

public partial class GameManager : Node
{
	private static SignalManager _signalManager;
	private static PackedScene _levelScene = GD.Load<PackedScene>("res://Level/Level.tscn");
	private static PackedScene _mainScene = GD.Load<PackedScene>("res://Main/Main.tscn");

	private int _levelSelected;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signalManager = GetNode<SignalManager>("/root/SignalManager");
		_signalManager.OnLevelSelected += OnLevelSelected;
	}

	public int GetLevelSelected()
	{
		return _levelSelected;
	}

	private void OnLevelSelected(int levelnumber)
	{
		_levelSelected = levelnumber;
		GetTree().ChangeSceneToPacked(_levelScene);
	}

	public void LoadMainScene()
	{
		GetTree().ChangeSceneToPacked(_mainScene);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		_signalManager.OnLevelSelected -= OnLevelSelected;
	}
}
