using System;
using Godot;

[GlobalClass, Icon("res://addons/at-icons/node/joypad.svg")]

public partial class InputController : Controller
{
    // Instead of setting pawn directly, could be accessed externally.
    public Vector2 Direction = Vector2.Zero;

    public override void _Process(double delta)
    {
        Direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        Pawn.SetMovementDirection(Direction);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("attack")) StateMachine.SendEvent("attack");
        if (@event.IsActionPressed("jump")) StateMachine.SendEvent("jump");
    }

    public override bool IsBlocking() => Input.IsActionPressed("attack");
}
