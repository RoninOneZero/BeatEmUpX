using Godot;
using System;

[GlobalClass]
public partial class EnemyGate : StaticBody2D
{
    const int MaxLimit = 10000000;
    
    [Export] public Camera2D Camera;
    public bool Active = false;

    public override void _Ready()
    {
        var combat = Main.Instance.GetNode<CombatManager>("GameState/CombatManager");
        combat.EnemiesCleared += OnEnemiesCleared;
    }
    
    public void Activate()
    {
        Camera.LimitRight = (int)GlobalPosition.X;
        Active = true;
    }

    public void Deactivate()
    {
        if (!Active)  return;
        Active = false;
        Camera.LimitRight = MaxLimit;
        QueueFree();
    }
    
    public void OnVisibleOnScreen()
    {
        Activate();
    }

    public void OnEnemiesCleared()
    {
        Deactivate();
    }
}
