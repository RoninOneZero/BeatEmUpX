using System;
using Godot;

[GlobalClass]
public partial class AttackState : State
{
    const float FRAME_TIME = 1f / 60f;

    [Export] public int StartupFrames = 5;
    [Export] public int ActiveFrames = 3;
    [Export] public int RecoveryFrames = 7;
    [Export] public int CancelFrame = -1;

    [Export] public string AnimationName = "attack1";
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] public AudioStreamPlayer2D SoundNode;
    [Export] public HitBox HitBox;
    [Export] public DamageData DamageData = new();

    [Export] public string NextAttack;

    private Pawn2D _pawn;

    private string _nextState = "idle";
    private bool _exitState = false;
    private bool _isComboReady = false;
    private bool _isActionStored = false;
    private double _time = 0f;

    public override void _Ready()
    {
        _pawn = Owner as Pawn2D;
        
        HitBox.HitLanded += OnHitLanded;
        
        if (CancelFrame < 0) CancelFrame = StartupFrames + ActiveFrames + RecoveryFrames;
    }

    public override async void Enter()
    {
        AnimatedSprite.Play(AnimationName);
        SoundNode?.Play();

        _nextState = "idle";
        _exitState = false;
        _isComboReady = false;
        _isActionStored = false;
        _time = 0.0f;

        _pawn.MovementEnabled = false;

        // await startup, if active, trigger hitbox
        await ToSignal(GetTree().CreateTimer(StartupFrames * FRAME_TIME), SceneTreeTimer.SignalName.Timeout);
        if (IsActive)
        {
            HitBox.Trigger(DamageData, ActiveFrames * FRAME_TIME);
        }

    }

    public override void Exit()
    {
        // disable HitBox
        return;
    }

    public override void Update(double delta)
    {
        _time += delta;

        if (_isActionStored && (_time >= CancelFrame * FRAME_TIME)) _exitState = true;
        else if (_time >= (StartupFrames + ActiveFrames + RecoveryFrames) * FRAME_TIME) _exitState = true;

        if (_exitState)
        {
            StateMachine.TransitionTo(_nextState);
            return;
        }
    }

    public override void ReceiveEvent(string event_name)
    {
        if (event_name == "attack" && _isComboReady && NextAttack != null)
        {
            _nextState = NextAttack;
            _isActionStored = true;
        }
    }

    private void OnHitLanded()
    {
        if (IsActive) _isComboReady = true;
    }

    // enum AttackState
    // {
    //     STARTUP,
    //     ACTIVE,
    //     RECOVERY
    // }
}
