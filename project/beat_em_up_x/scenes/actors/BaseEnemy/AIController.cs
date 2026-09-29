using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/node/brain.svg")]
public partial class AIController : Controller
{
    [Export] public float AttackChance = 0.5f;
    [Export] public float MaxApproachDistance = 250f;
    [Export] public float MinApproachDistance = 100f;
    [Export] public RepositionStrategy SelectRepositionStrategy = RepositionStrategy.Distance;
    
    [Export] public FacingHandler FacingHandler;
    [Export] public Node2D Target { get; set; }

    public Mood CurrentMood { get; set; } = Mood.Idle;
    
    private CombatManager _combat;
    private Timer _actionTimer;
    private Timer _delayTimer;
    
    public override void _Ready()
    {
        _combat = Main.Instance.GetNode<CombatManager>("GameState/CombatManager");
        Target = _combat.Player;
        _actionTimer = GetNode<Timer>("ActionTimer");
        _actionTimer.Timeout += OnActionTimerTimeout;
        _delayTimer = GetNode<Timer>("DelayTimer");
        _delayTimer.Timeout += OnDelayTimerTimeout;
        var health = Pawn.GetNode<Health>("Health");
        health.HealthDepleted += OnHealthDepleted;

        var attackDelay = 0.5;
        attackDelay = GD.RandRange(0.1, attackDelay);
        
        _delayTimer.Start(attackDelay);
        
        DecideMood();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Enabled) return;
        if (Target == null) return;
        
        FaceTarget();
        
        switch (CurrentMood)
        {
            case Mood.Idle:
                IdleProcess();
                break;
            case Mood.Reposition:
                RepositionProcess();
                break;
            case Mood.Attack:
                AttackProcess();
                break;
        }
        
    }

    public override bool IsBlocking() => false;

    public bool TargetInRange()
    {
        var distanceToTarget = Pawn.GlobalPosition.DistanceTo(Target.GlobalPosition);
        if (distanceToTarget < MinApproachDistance) return false;
        if (distanceToTarget > MaxApproachDistance) return false;
        return true;
    }

    /// returns True if near destination
    public bool ApproachPosition(Vector2 destination)
    {
        Pawn.AutoFacingEnabled = false;
        
        var distanceToTarget = Pawn.GlobalPosition.DistanceTo(destination);
        var directionToTarget = Pawn.GlobalPosition.DirectionTo(destination);
        
        // if (directionToTarget.X < 0) Pawn.SetFacing(Vector2.Left);
        // else if (directionToTarget.X > 0) Pawn.SetFacing(Vector2.Right);
        
        if (distanceToTarget > 10f)
        {
            Pawn.SetMovementDirection(directionToTarget);
            return false;
        }
        else
        {
            FaceTarget();
            Pawn.SetMovementDirection(Vector2.Zero);
            CurrentMood = Mood.Idle;
            return true;
        }
    }
    
    public void ApproachPositionInRange(Vector2 destination)
    {
        Pawn.AutoFacingEnabled = false;
        
        var distanceToTarget = Pawn.GlobalPosition.DistanceTo(destination);
        var directionToTarget = Pawn.GlobalPosition.DirectionTo(destination);
        
        // if (directionToTarget.X < 0) Pawn.SetFacing(Vector2.Left);
        // else if (directionToTarget.X > 0) Pawn.SetFacing(Vector2.Right);
        
        if (distanceToTarget < MinApproachDistance)
        {
            Pawn.SetMovementDirection(-directionToTarget);
        }
        else if (distanceToTarget > MaxApproachDistance)
        {
            Pawn.SetMovementDirection(directionToTarget);
        }
        else
        {
            FaceTarget();
            Pawn.SetMovementDirection(Vector2.Zero);
            CurrentMood = Mood.Idle;
        }
    }

    public void FaceTarget()
    {
        var directionToTarget = Pawn.GlobalPosition.DirectionTo(Target.GlobalPosition);
        if (directionToTarget.X < 0) Pawn.SetFacing(Vector2.Left);
        else if (directionToTarget.X > 0) Pawn.SetFacing(Vector2.Right);

    }

    public Mood DecideMood()
    {
        if (Target == null)
        {
            return Mood.Idle;
        }

        var randomNumber = GD.Randf();
        
        if (randomNumber <= AttackChance)
        {
            return Mood.Attack;
        }
        else
        {
            return Mood.Reposition;
        }
    }

    public void IdleProcess()
    {
        Pawn.SetMovementDirection(Vector2.Zero);
    }

    public void AttackProcess()
    {
        var destination = Target.GlobalPosition;
        var yDistance = Pawn.GlobalPosition.Y - destination.Y;
        if (Math.Abs(yDistance) > 10) 
        {
            destination.X = Pawn.GlobalPosition.X;
            ApproachPosition(destination);
            return;
        }
        
        ApproachPositionInRange(destination);

        if (TargetInRange())
        {
            StateMachine.SendEvent("attack");
            CurrentMood = Mood.Idle;
        }
    }

    public void RepositionProcess()
    {
        switch (SelectRepositionStrategy)
        {
            case RepositionStrategy.Direct:
                ApproachPositionInRange(Target.GlobalPosition);
                break;
            case RepositionStrategy.Orbit:
                return;
            case RepositionStrategy.BattleSlot:
                ApproachBattleSlot();
                break;
            case RepositionStrategy.Distance:
                DistanceReposition();
                break;
        }
    }

    public void ApproachBattleSlot()
    {
        Vector2 destination;
        
        var battleSlotManager = _combat.BattleSlotManager;
        var battleSlot = battleSlotManager?.GetRandomOpenBattleSlot(Pawn);
        
        if (battleSlot != null)
        {
            destination = battleSlot.GlobalPosition;
            var result = ApproachPosition(destination);
            
            if (result) battleSlotManager.ClearAssignment(battleSlot);
        }
        else
        {
            CurrentMood = Mood.Idle;
        }
    }

    public void DistanceReposition()
    {
        const float screenRightX = 1600f * 0.75f;
        const float screenLeftX = 1600f * 0.25f;
        
        var destination = Pawn.GlobalPosition;
        var pawnScreenPosition = Pawn.GetGlobalTransformWithCanvas().Origin;
        
        destination.X = pawnScreenPosition.X >= 1600f * 0.5f ? screenRightX : screenLeftX;

        ApproachPosition(GetViewport().GetCanvasTransform().AffineInverse() * destination);

    }
    
    private void OnActionTimerTimeout()
    {
        if (Target == null) return;
        
        var randomNumber = GD.Randf();

        SelectRepositionStrategy = randomNumber >= 0.5f ? RepositionStrategy.BattleSlot : RepositionStrategy.Distance;
        
        CurrentMood = DecideMood();
    }

    private void OnDelayTimerTimeout()
    {
        _actionTimer.Start();
    }
    
    private void OnHealthDepleted()
    {
        Enabled = false;
    }

    public enum Mood
    {
        Idle,
        Attack,
        Reposition,
    }
    
    public enum RepositionStrategy
    {
        Direct,
        Orbit,
        BattleSlot,
        Distance,
    }
}

