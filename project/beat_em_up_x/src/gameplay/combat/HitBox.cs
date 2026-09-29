using System;
using Godot;

[GlobalClass, Icon("res://addons/at-icons/node2d/sword.svg")]
public partial class HitBox : Area2D
{
    [Signal] public delegate void HitLandedEventHandler();

    const string MonitoringString = "monitoring";
    
    [Export] public DamageData DamageData { get; set; } = new();

    public override void _Ready()
    {
        Monitorable = false;
        Monitoring = false;

        AreaEntered += OnAreaEntered;
    }

    public async void Trigger(DamageData damageData = null, double time = -1.0f)
    {
        SetDeferred(MonitoringString, true);
        DamageData = damageData ?? new();

        if (time > 0f)
        {
            await ToSignal(GetTree().CreateTimer(time), SceneTreeTimer.SignalName.Timeout);
            Disable();
        }
    }

    public virtual void Disable()
    {
        SetDeferred(MonitoringString, false);
        DamageData = new();
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not HurtBox hurtBox) return;

        hurtBox.TakeDamage(DamageData);
        EmitSignalHitLanded();
        Disable();
    }
}
