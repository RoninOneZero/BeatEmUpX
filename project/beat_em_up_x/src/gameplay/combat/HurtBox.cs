using System;
using Godot;

[GlobalClass, Icon("res://addons/at-icons/node2d/heart_broken.svg")]
public partial class HurtBox : Area2D
{
    [Signal] public delegate void DamageTakenEventHandler(DamageData data);

    [Export] public bool Enabled = true;

    public void TakeDamage(DamageData data)
    {
        if (Enabled) EmitSignalDamageTaken(data);
    }
}
