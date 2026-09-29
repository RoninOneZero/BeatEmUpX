using Godot;
using System;

public partial class DeadState : State
{
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] public HurtBox HurtBox;
    
    private Pawn2D _pawn ;
    
    public override void _Ready()
    {
        _pawn = Owner as Pawn2D;
    }

    public override void Enter()
    {   
        AnimatedSprite.Play("death");
        _pawn.MovementEnabled = false;
        HurtBox.Enabled = false;
        _pawn.CollisionLayer = 0;
        _pawn.CollisionMask = 0;
    }

    public override void Exit()
    {
        _pawn.MovementEnabled = true;
        HurtBox.Enabled = true;
    }
}