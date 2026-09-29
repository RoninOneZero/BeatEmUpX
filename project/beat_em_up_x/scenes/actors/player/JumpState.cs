using System;
using Godot;

public partial class JumpState : State
{
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] public Node2D BodyPivot;

    
    private Pawn2D _pawn;
    private SubState _subState;
    private Vector2 _originPosition;
    
    public override void _Ready()
    {
        _pawn = Owner as Pawn2D;
    }

    public override void Enter()
    {
        AnimatedSprite.Play("jump_up");
        _pawn.MovementEnabled = true;
        
        _subState = SubState.Ascending;
        _originPosition = BodyPivot.Position;
        
        _pawn.Jump();
    }

    public override void PhysicsUpdate(double delta)
    {
        if (!_pawn.IsAirborn())
        {
            StateMachine.TransitionTo("idle");
            return;
        }
        
        if (_pawn.BodyVelocity.Y >= -2f) _subState = SubState.Apex;
        if (_pawn.BodyVelocity.Y >= 0f)
        {
            StateMachine.TransitionTo("fall");
            return;
        }

        switch (_subState)
        {
            case SubState.Ascending:
                break;
            case SubState.Apex:
                AnimatedSprite.Play("jump_apex");
                break;
        }

    }

    public override void ReceiveEvent(string eventName)
    {
        switch (eventName)
        {
            case "attack":
                StateMachine.TransitionTo("attack1");
                break;
        }
    }

    public enum SubState
    {
        Start,
        Ascending,
        Apex,
    }
}