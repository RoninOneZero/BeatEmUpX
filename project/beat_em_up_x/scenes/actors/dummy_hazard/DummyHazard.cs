using Godot;
using System;

public partial class DummyHazard : HitBox
{
    public override void _Ready() => Trigger();

    public async override void Disable()
    {
        base.Disable();
        
        await ToSignal(GetTree().CreateTimer(0.1), SceneTreeTimer.SignalName.Timeout);
        Trigger();
    }
}
