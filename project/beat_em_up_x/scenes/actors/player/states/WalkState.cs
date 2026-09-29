using System;
using Godot;


public partial class WalkState : State
{
    [Export] public AnimatedSprite2D AnimatedSprite;
    
    private Pawn2D _pawn;

    public override void _Ready()
    {
        _pawn = Owner as Pawn2D;
    }

    public override void Enter()
    {
        AnimatedSprite.Play("walk");
        _pawn.MovementEnabled = true;
    }

    public override void Update(double delta)
    {
        if (_pawn.MovementDirection == Vector2.Zero)
        {
            StateMachine.TransitionTo("idle");
        }
    }

        public override void ReceiveEvent(string event_name)
    {
        switch (event_name)
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
