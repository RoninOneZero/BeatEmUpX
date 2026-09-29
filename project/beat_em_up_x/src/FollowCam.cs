using System;
using Godot;

[GlobalClass, Tool]
public partial class FollowCam : Camera2D
{
    [Export]
    public Node2D Target;

    public override void _Process(double delta)
    {
        if (Target != null) GlobalPosition = Target.GlobalPosition;
    }
}
