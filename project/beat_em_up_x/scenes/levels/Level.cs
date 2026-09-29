using Godot;
using Godot.Collections;

[GlobalClass, Icon("res://addons/at-icons/node2d/building.svg")]

public partial class Level : Node
{
    [Signal] public delegate void LevelCompletedEventHandler();
    
    [Export] public FollowCam FollowCam { get; set; }
    
    public override void _Ready()
    {
        Main.Instance.AddLevel(this);
    }

    private void OnLevelComplete(Node2D player)
    {
        GD.Print("Level Complete");
        EmitSignalLevelCompleted();
    }

    private void OnPlayerSpawned(Node2D player)
    {
        FollowCam.Target = player;
        FollowCam.MakeCurrent();
    }
    
    
}
