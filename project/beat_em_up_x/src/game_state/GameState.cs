using System;
using Godot;

[GlobalClass, Icon("res://addons/at-icons/node/joystick.svg")]
public partial class GameState : Node
{
    [Signal] public delegate void GameOverEventHandler();
    
    public State CurrentState { get; set;} = State.Inactive;

    public override void _Ready()
    {
        return;
    }

    public override void _Process(double delta)
    {
        return;
    }

    private void OnPlayerDied(Node player)
    {
        CurrentState = State.GameOver;
        EmitSignalGameOver();
    }
    
    public enum State
    {
        Inactive,
        Intro,
        Active,
        Outro,
        GameOver,
    }
}
