using Godot;
using System;
using System.Linq;
using Godot.Collections;

public partial class ScoreSync : Node
{
	private const string SCORE_FILE = "user://scores.dat";
	private const int NO_BEST = 10000;

	private Dictionary _bestScores = new();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadScores();
	}

	private void LoadScores()
	{
		if (!FileAccess.FileExists(SCORE_FILE))
			return;

		var file = FileAccess.Open(SCORE_FILE, FileAccess.ModeFlags.Read);
		var something = file.GetAsText();
		_bestScores = (Dictionary)Json.ParseString(file.GetAsText());
		GD.Print($"load_scores(): {_bestScores}");
	}
	
	private void SaveScores()
	{
		var file = FileAccess.Open(SCORE_FILE, FileAccess.ModeFlags.Write);
		var value = Json.Stringify(_bestScores);
		file.StoreString(Json.Stringify(_bestScores));
	}

	public bool HasLevelScore(string level)
	{
		return _bestScores.ContainsKey(level);
	}

	public int GetLevelBestScore(string level)
	{
		if (HasLevelScore(level))
			return (int)_bestScores[level];

		return NO_BEST;
	}

	public bool ScoreIsNewBest(string level, int moves)
	{
		if (!HasLevelScore(level))
			return true;

		if (GetLevelBestScore(level) > moves)
			return true;

		return false;
	}

	public void LevelCompleted(string level, int moves)
	{
		if (ScoreIsNewBest(level, moves))
			_bestScores[level] = moves;
		
		SaveScores();
	}
}
