using Godot;
using System;

public abstract partial class Controller : Node
{
    [Export] public bool Enabled { get; set; } = true;
    [Export] public FiniteStateMachine StateMachine { get; private set; }
    [Export] public Pawn2D Pawn { get; private set; }
    
    public abstract bool IsBlocking();
}
