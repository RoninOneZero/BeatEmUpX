using System;
using Godot;

public partial class TitleScreen : Control
{
    [Export] public Button InitialButton { get; set; }

    public override void _Ready()
    {
        InitialButton?.GrabFocus();
    }
    
    private void OnStartGameButtonPressed()
    {
        var combat = Main.Instance.GetNode<CombatManager>("GameState/CombatManager");
        combat.InitializeCombat();
        
        var startingLevelPath = Global.Instance.Levels[0];
        GetTree().ChangeSceneToFile(startingLevelPath);
    }

    private void OnQuitGameButtonPressed()
    {
        GetTree().Quit();
    }
}
