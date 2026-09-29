using Godot;
using System;

public partial class VictoryScreen : Control
{
    [Signal] public delegate void MainMenuRequestedEventHandler();
    [Signal] public delegate void QuitGameRequestedEventHandler();
    
    private void OnMainMenuButtonPressed() => EmitSignalMainMenuRequested();
    private void OnQuitGameButtonPressed() => EmitSignalQuitGameRequested();

    private void OnVisibilityChanged()
    {
        if (Visible)
        {
            var button = GetNode<Button>("Panel/VBoxContainer/MainMenu");
            if (button.IsInsideTree()) button.GrabFocus();
        }
    }
}
