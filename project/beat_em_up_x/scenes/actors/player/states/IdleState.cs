using System;
using Godot;

public partial class IdleState : State
{
    [Export] public AnimatedSprite2D AnimatedSprite;
    
    private Pawn2D _pawn;

    public override void _Ready()
    {
        _pawn = Owner as Pawn2D;
    }

    public override void Enter()
    {
        AnimatedSprite.Play("idle");
        _pawn.MovementEnabled = false;
    }

    public override void Update(double delta)
    {
        if (_pawn.MovementDirection != Vector2.Zero)
        {
            StateMachine.TransitionTo("walk");
        }
    }

    public override void PhysicsUpdate(double delta)
    {
        if (_pawn.IsAirborn())
        {
            StateMachine.TransitionTo("fall");
        }
    }
    
    
    public override void ReceiveEvent(string eventName)
    {
        switch (eventName)
        {
            case "attack":
                StateMachine.TransitionTo("attack1");
                break;
            case "jump":
                StateMachine.TransitionTo("jump");
                break;
        }
    }
}
