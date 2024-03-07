using Godot;

public partial class SignalManager : Node
{
    [Signal]
    public delegate void OnLevelSelectedEventHandler(int levelNumber);
}
