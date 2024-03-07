using Godot;
using System;

public partial class Main : CanvasLayer
{
	private static PackedScene _buttonScene = GD.Load<PackedScene>("res://LevelButton/LevelButton.tscn");
	private GridContainer _gridContainer;
	private const int LEVEL_COLS = 6;
	private const int LEVEL_ROWS = 5;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gridContainer = GetNode<GridContainer>("MC/VBoxContainer/GridContainer");
		SetupGrid();
	}

	private void SetupGrid()
	{
		for (int i = 0; i < (LEVEL_COLS * LEVEL_ROWS); i++)
		{
			var levelButtonScene = (LevelButton)_buttonScene.Instantiate();		
			levelButtonScene.SetLevelNumber($"{i+1}");
			_gridContainer.AddChild(levelButtonScene);
		}
	}
}
