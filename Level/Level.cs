using Godot;
using System;
using System.Collections;
using Godot.Collections;
using Sokoban.Models;

public partial class Level : Node2D
{
	private GameData _gameData;
	private TileMap _tileMap;
	private AnimatedSprite2D _player;
	private Camera2D _camera;
	
	private const int FLOOR_LAYER = 0;
	private const int WALL_LAYER = 1;
	private const int TARGET_LAYER = 2;
	private const int BOX_LAYER = 3;

	private const int SOURCE_ID = 0;

	private const string LAYER_KEY_FLOOR = "Floor";
	private const string LAYER_KEY_WALLS = "Walls";
	private const string LAYER_KEY_TARGETS = "Targets";
	private const string LAYER_KEY_TARGET_BOXES = "TargetBoxes";
	private const string LAYER_KEY_BOXES = "Boxes";

	private static readonly Dictionary<string, int> LAYER_MAP = new()
	{
		{ LAYER_KEY_FLOOR, FLOOR_LAYER },
		{ LAYER_KEY_WALLS, WALL_LAYER },
		{ LAYER_KEY_TARGETS, TARGET_LAYER },
		{ LAYER_KEY_TARGET_BOXES, BOX_LAYER },
		{ LAYER_KEY_BOXES, BOX_LAYER }
	};

	private bool _moving = false;
	private int _totalMoves = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameData = GetNode<GameData>("/root/GameData");
		_tileMap = GetNode<TileMap>("TileMap");
		_player = GetNode<AnimatedSprite2D>("Player");
		_camera = GetNode<Camera2D>("Camera2D");
		
		SetupLevel();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_moving)
			return;

		var moveDirection = Vector2I.Zero;
		
		if (Input.IsActionJustPressed("right"))
		{
			_player.FlipH = false;
			moveDirection = Vector2I.Right;   
		}     
		if (Input.IsActionJustPressed("left"))
		{
			_player.FlipH = true;
			moveDirection = Vector2I.Left;   
		}     
		if (Input.IsActionJustPressed("up"))
		{
			_player.FlipH = false;
			moveDirection = Vector2I.Up;  
		}      
		if (Input.IsActionJustPressed("down"))
		{
			_player.FlipH = false;
			moveDirection = Vector2I.Down;
		}

		if(moveDirection != Vector2I.Zero)
			PlayerMove(moveDirection);
	}
	
	#region GAME LOGIC

	private Vector2I GetPlayerTile()
	{
		var playerOffset = _player.GlobalPosition - _tileMap.GlobalPosition;
		var vectorWithOffset = (Vector2I)(playerOffset / GameData.TILE_SIZE);
		return new Vector2I(vectorWithOffset.X, vectorWithOffset.Y);
	}

	private void PlayerMove(Vector2I direction)
	{
		_moving = true;

		var playerTile = GetPlayerTile();
		var newTile = playerTile + direction;

		GD.Print($"direction: {{ \"x\": {direction.X}, \"y\": {direction.Y} }} ");
		GD.Print($"playerTile: {{ \"x\": {playerTile.X}, \"y\": {playerTile.Y} }} ");
		GD.Print($"newTile: {{ \"x\": {newTile.X}, \"y\": {newTile.Y} }} ");
		
		_moving = false;
	}
	
	#endregion

	#region LEVEL SETUP
	
	private void PlacePlayerOnTile(Vector2I tileCoord)
	{
		var newPosition = new Vector2(tileCoord.X * GameData.TILE_SIZE, 
			                  tileCoord.Y * GameData.TILE_SIZE) + _tileMap.GlobalPosition;
		_player.GlobalPosition = newPosition;
		CenterCamera();
	}
	
	private Vector2I GetAtlasCoordForLayerName(string layerName)
	{
		switch (layerName)
		{
			case LAYER_KEY_FLOOR:
				
				var random = new Random();
				int randomRange = random.Next(3, 8);
				return new Vector2I(randomRange, 0);
			
			case LAYER_KEY_WALLS:
				return new Vector2I(2,0);
			
			case LAYER_KEY_TARGETS:
				return new Vector2I(9,0);
			
			case LAYER_KEY_TARGET_BOXES:
				return new Vector2I(0,0);	
			
			case LAYER_KEY_BOXES:
				return new Vector2I(1,0);	
			
			default:
				return Vector2I.Zero;
		}
	}	

	
	private void AddLayerTiles(object layerTiles, string layerName)
	{
		var layerType = layerTiles.GetType();
		var layerTilesObject = ConvertToType(layerTiles, layerType) as IEnumerable; // cast as IEnumerable to be able to cycle through values
		if (layerTilesObject != null)
		{
			foreach (var item in layerTilesObject)
			{
				var property = item.GetType().GetProperty("coord"); // uses Reflection to be castable as Coord
				if (property != null)
				{
					AddTile(property.GetValue(item) as Coord, layerName);
				}
			}
		}
	}
	
	private void SetupLevel()
	{
		_tileMap.Clear();
		var levelData = _gameData.GetDataForLabel(1);
		var levelTiles = levelData.tiles;
		var playerStart = levelData.player_start;
		GD.Print($"playerStart: {{ \"x\": {playerStart.x}, \"y\": {playerStart.y} }} ");
		
		foreach (var layerName in LAYER_MAP.Keys)
		{
			var propertyInfo = typeof(Tiles).GetProperty(layerName);
			if (propertyInfo != null)
			{
				// Get the value of the property
				var propertyValue = propertyInfo.GetValue(levelTiles);
				AddLayerTiles(propertyValue, layerName);
			}
		}
		
		PlacePlayerOnTile(new Vector2I(playerStart.x, playerStart.y));
	}

	// for some reason the tutorial's centering could not be possible without adding 12 to tileMapsEnd points
	// I believe it has to do with the pivot point of the TileMap. I was not able to change the pivot point
	private void CenterCamera()
	{
		var tmr = _tileMap.GetUsedRect();
		
		var tileMapStartX = tmr.Position.X * GameData.TILE_SIZE;
		var tileMapEndX = ((tmr.Size.X + 12) * GameData.TILE_SIZE) + tileMapStartX;
		
		var tileMapStartY = tmr.Position.Y * GameData.TILE_SIZE;
		var tileMapEndY = ((tmr.Size.Y + 12) * GameData.TILE_SIZE) + tileMapStartY;

		var midX = tileMapStartX + (tileMapEndX - tileMapStartX) / 2;
		var midY = tileMapStartY + (tileMapEndY - tileMapStartY) / 2;

		_camera.GlobalPosition = new Vector2(midX, midY);
	}

	private void AddTile(Coord tileCord, string layerName)
	{
		var layerNumber = LAYER_MAP[layerName];
		var coordVector = new Vector2I(tileCord.x, tileCord.y);
		var atlasVector = GetAtlasCoordForLayerName(layerName);

		_tileMap.SetCell(layerNumber, coordVector, SOURCE_ID, atlasVector);
	}
	
	// Converts value to Type so we can cast it later
	public static object ConvertToType(object value, Type targetType)
	{
		return Convert.ChangeType(value, targetType);
	}
	
	#endregion
}
