using Godot;
using System;

public partial class HurtState : State
{
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] public double StateDuration { get; set; } = 0.5f;
    [Export] public HurtBox HurtBox;
    
    private Pawn2D _pawn ;
    
    private Timer _timer;
    
    public override void _Ready()
    {
        _pawn = Owner as Pawn2D;

        _timer = new Timer();
        AddChild(_timer);
        _timer.Timeout += OnTimerTimeout;
    }

    public override void Enter()
    {   
        AnimatedSprite.Stop();
        AnimatedSprite.Play("hurt");
        _pawn.MovementEnabled = false;
        
        _timer.Start(StateDuration);
    }

    public override void Exit()
    {
        _pawn.MovementEnabled = true;
        _timer.Stop();
    }

    private void OnTimerTimeout()
    {
        StateMachine.TransitionTo("idle");
    }
    
}
