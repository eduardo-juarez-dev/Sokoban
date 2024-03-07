using System.Collections.Generic;

namespace Sokoban.Models;

public class RootLevelData
{
    public List<LevelData> LevelDataEntries { get; set; }
}

public class LevelData
{
    public Tiles tiles { get; set; }
    public PlayerStart player_start { get; set; }
}

public class Box
{
    public Coord coord { get; set; }
}

public class Coord
{
    public int x { get; set; }
    public int y { get; set; }
}

public class Floor
{
    public Coord coord { get; set; }
}

public class PlayerStart
{
    public int x { get; set; }
    public int y { get; set; }
}

public class Target
{
    public Coord coord { get; set; }
}

public class Tiles
{
    public List<Floor> Floor { get; set; }
    public List<Wall> Walls { get; set; }
    public List<Target> Targets { get; set; }
    public List<Target> TargetBoxes { get; set; }
    public List<Box> Boxes { get; set; }
}

public class Wall
{
    public Coord coord { get; set; }
}