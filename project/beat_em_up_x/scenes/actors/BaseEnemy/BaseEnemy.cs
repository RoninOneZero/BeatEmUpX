using System;
using Godot;


[GlobalClass]//, Tool]
public partial class BaseEnemy : Pawn2D
{
    public Health Health { get; private set; }

    private FiniteStateMachine _finiteStateMachine; 
    public override void _Ready()
    {
        Health = GetNode<Health>("Health");
        _finiteStateMachine = GetNode<FiniteStateMachine>("FiniteStateMachine");
    }
    
    private void OnDamageTaken(DamageData damageData)
    {
        _finiteStateMachine.TransitionTo("hurt", true);
        
        Health.TakeDamage(1);
        StunCounter += 1;
    }
}
