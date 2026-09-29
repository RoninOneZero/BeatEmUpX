using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/node/heart.svg")]
public partial class Health : Node
{
    [Signal] public delegate void HealthChangedEventHandler(int amount);
    [Signal] public delegate void HealthDepletedEventHandler();
    
    [Export] public int MaxHealth = 3;
    
    public int CurrentHealth = 3;

    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Math.Max(CurrentHealth - amount, 0);
        EmitSignalHealthChanged(CurrentHealth);
        
        if (CurrentHealth <= 0) EmitSignalHealthDepleted();
    }

    public void Heal(int amount)
    {
        CurrentHealth = Math.Min(CurrentHealth + amount, MaxHealth);
        EmitSignalHealthChanged(CurrentHealth);
    }
}
