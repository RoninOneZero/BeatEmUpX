using Godot;


[GlobalClass, Tool]
public partial class Pawn2D : CharacterBody2D
{

    [Signal] public delegate void FacingChangedEventHandler(Vector2 facing);
    
    [Export] public float WalkSpeed { get; set; } = 100f;
    [Export] public bool MovementEnabled { get; set; } = true;
    [Export] public bool AutoFacingEnabled { get; set; } = true;
    [Export] public float JumpVelocity { get; set; } = 30f;
    [Export] public bool GravityEnabled { get; set; } = true;
    [Export] public Node2D BodyPivot { get; private set; }
    
    public Vector2 MovementDirection { get; private set; } = Vector2.Zero;
    public Vector2 Facing { get; private set; } = Vector2.Right;
    public int StunCounter = 0;
    public Vector2 BodyVelocity { get; private set; } = Vector2.Zero;
    
    private Vector2 _bodyOrigin;

    public override void _Ready()
    {
        _bodyOrigin = BodyPivot.Position;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint()) return;
        
        if (!MovementEnabled) MovementDirection = Vector2.Zero;
        
        var yAdjustedMovementDirection = MovementDirection;
        yAdjustedMovementDirection.Y *= 0.7f;
        var newVelocity = yAdjustedMovementDirection * WalkSpeed;

        Velocity = newVelocity;
        if (AutoFacingEnabled) SetFacing(MovementDirection);

        MoveAndSlide();

        // Apply 'gravity' to body pivot
        if (BodyPivot == null) GD.PrintErr("Body pivot is null");
        
        if (GravityEnabled && IsAirborn())
        {
            BodyVelocity += GetGravity();
        }
        
        BodyPivot.Position += BodyVelocity;
        
        if (BodyPivot.Position.Y > _bodyOrigin.Y) BodyPivot.Position = _bodyOrigin;
    }
    
    public bool IsAirborn() => BodyPivot.Position.Y < _bodyOrigin.Y;

    public void SetMovementDirection(Vector2 direction)
    {
        MovementDirection = direction;
    }
    
    public void SetFacing(Vector2 direction)
    {
        // if facing.Locked return;
        if (direction.X == 0f) return;

        Vector2 newFacing = Vector2.Zero;

        if (direction.X < 0f) newFacing = Vector2.Left;
        if (direction.X > 0f) newFacing = Vector2.Right;

        if (newFacing == Facing) return;

        Facing = newFacing;
        EmitSignalFacingChanged(Facing);
    }

    public void FlipFacing()
    {
        Vector2 newFacing = Facing;
        newFacing.X *= -1f;
        SetFacing(newFacing);
    }

    public void Jump()
    {
        BodyVelocity = Vector2.Up * JumpVelocity;
    }    
        

}
