using Godot;
using System.Collections.Generic;
using Sokoban.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

public partial class GameData : Node
{
	private const string LEVEL_DATA_PATH = "res://data/level_data.json";
	public const int TILE_SIZE = 32;

	// private Dictionary _levelData = new Dictionary();
	private List<LevelData> _levelData;
	
	public override void _Ready()
	{
		LoadLevelData();
	}

	private void LoadLevelData()
	{
		var file = FileAccess.Open(LEVEL_DATA_PATH, FileAccess.ModeFlags.Read);
		_levelData = JsonSerializer.Deserialize<List<LevelData>>(file.GetAsText());
	}

	public LevelData GetDataForLabel(int levelNum)
	{
		return _levelData[levelNum-1]; // indexes for JSON levels start from zero, indexes from List start from 0
	}
}
