using Godot;
using System;

public partial class Player : Pawn2D
{
    [Export] public InputController InputController { get; set; }
    
    private FiniteStateMachine _finiteStateMachine;
    private Health _health;
    
    
    public override void _Ready()
    {
        _finiteStateMachine = GetNode<FiniteStateMachine>("FiniteStateMachine");
        _health = GetNode<Health>("Health");
    }

    private void OnDamageTaken(DamageData data)
    {
        if (_finiteStateMachine.CurrentState.Name == "Idle" && InputController.IsBlocking() ||
            _finiteStateMachine.CurrentState.Name == "Walk" && InputController.IsBlocking() ||
            _finiteStateMachine.CurrentState.Name == "Block" && InputController.IsBlocking())
        {
            _finiteStateMachine.TransitionTo("block", true);
            StunCounter += 1;
            return;
        }
        
        _health.TakeDamage(1);

        if (_health.CurrentHealth <= 0)
        {
            _finiteStateMachine.TransitionTo("dead");
            return;
        }
        
        _finiteStateMachine.TransitionTo("hurt", true);
        StunCounter += 1;
    }

}
