using System;
using Godot;

public partial class FallState : State
{
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] public Node2D BodyPivot;
    
    private Pawn2D _pawn;
    
    public override void _Ready()
    {
        _pawn = Owner as Pawn2D;
    }

    public async override void Enter()
    {
        _pawn.MovementEnabled = true;
        
        if (AnimatedSprite.Animation == "jump_apex")
            await ToSignal(AnimatedSprite, "animation_finished");

        AnimatedSprite.Play("fall");
    }

    public override void PhysicsUpdate(double delta)
    {
        if (!_pawn.IsAirborn())
        {
            StateMachine.TransitionTo("idle");
            return;
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

    private void OnAnimationFinished()
    {
        AnimatedSprite.Play("fall");
    }
}