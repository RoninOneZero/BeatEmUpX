using System;
using Godot;

[GlobalClass, Icon("res://addons/at-icons/node/mirror_horizontally.svg")]
public partial class FacingHandler : Node
{
    [Export] public bool Enabled { get; set; } = true;
    [Export] private AnimatedSprite2D AnimatedSprite2D;
    [Export] private HitBox HitBox;
    [Export] private HurtBox HurtBox;

    public void SetFacing(Vector2 facing)
    {
        if (!Enabled) return;
        
        if (facing == Vector2.Left)
        {
            AnimatedSprite2D.FlipH = true;

            Vector2 newHitBoxPosition = HitBox.Position;
            newHitBoxPosition.X = MathF.Abs(newHitBoxPosition.X) * -1f;
            HitBox.Position = newHitBoxPosition;

            Vector2 newHurtBoxPosition = HurtBox.Position;
            newHurtBoxPosition.X = MathF.Abs(newHurtBoxPosition.X) * -1f;
            HurtBox.Position = newHurtBoxPosition;


        }
        else
        {
            AnimatedSprite2D.FlipH = false;

            Vector2 newHitBoxPosition = HitBox.Position;
            newHitBoxPosition.X = MathF.Abs(newHitBoxPosition.X);
            HitBox.Position = newHitBoxPosition;

            Vector2 newHurtBoxPosition = HurtBox.Position;
            newHurtBoxPosition.X = MathF.Abs(newHurtBoxPosition.X);
            HurtBox.Position = newHurtBoxPosition;
        }
    }
    
    public void OnFacingChanged(Vector2 facing)
    {
        SetFacing(facing);
    }
}
