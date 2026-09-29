using Godot;
using System;

public partial class BlockState : State
{
    [Export] public AnimatedSprite2D AnimatedSprite;
    
    private Pawn2D _pawn;

    public override void _Ready()
    {
        _pawn = Owner as Pawn2D;
    }
    
    public async override void Enter()
    {
        AnimatedSprite.Play("block");
        _pawn.MovementEnabled = false;
        
        await ToSignal(AnimatedSprite, "animation_finished");
        if (IsActive) StateMachine.TransitionTo("idle");
    }

    public override void Exit()
    {
        _pawn.MovementEnabled = true;
    }
}
