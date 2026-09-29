using Godot;
using System;

public partial class GameOverControl : Control
{
    [Signal] public delegate void ContinueRequestedEventHandler();
    [Signal] public delegate void MainMenuRequestedEventHandler();
    [Signal] public delegate void QuitGameRequestedEventHandler();
    
    [Export] public Label ContinueLabel { get; private set; }
    [Export] public Button ContinueButton { get; private set; }
    [Export] public Label GameOverLabel { get; private set; }
    [Export] public Button MainMenuButton { get; private set; }

    private void OnMainMenuButtonPressed() => EmitSignalMainMenuRequested();
    private void OnQuitGameButtonPressed() => EmitSignalQuitGameRequested();

    public override void _Ready()
    {
        VisibilityChanged += OnVisibilityChanged;
    }
    
    
    private void OnVisibilityChanged()
    {
        GameOverLabel?.Hide();
        ContinueLabel?.Hide();
        ContinueButton?.Hide();
        
        var gameState = Main.Instance.GetNode<GameState>("GameState");
        if (Visible && gameState.CurrentState != GameState.State.GameOver)
        {
            ContinueButton?.Show();
            ContinueLabel?.Show();
            ContinueButton?.GrabFocus();
        }
        else
        {
            GameOverLabel?.Show();
            MainMenuButton?.GrabFocus();
        }
    }
    
    private void OnContinueButtonPressed()
    {
        EmitSignalContinueRequested();
    }

}
