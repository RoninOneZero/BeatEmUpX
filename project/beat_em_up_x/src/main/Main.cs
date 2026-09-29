using System;
using Godot;

[GlobalClass, Icon("res://addons/at-icons/node/gobot.svg")]

public partial class Main : Node
{
    [Export(PropertyHint.File)] public string TitleScreenFile;
    [Export] public Control GameOverControl { get; set; }
    [Export] public Control VictoryScreen { get; set; }
    
    
    public static Main Instance { get; private set; }
    public Level CurrentLevel { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        GameOverControl?.Hide();
        VictoryScreen?.Hide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            CurrentLevel.ProcessMode = ProcessModeEnum.Disabled;
            GameOverControl?.Show();
        }
    }
    
    public void AddLevel(Level level)
    {
        if (level == CurrentLevel) return;

        level.CallDeferred("reparent", this);
        CurrentLevel = level;
        CurrentLevel.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted()
    {
        CurrentLevel.SetDeferred("process_mode", (int)ProcessModeEnum.Disabled);
        var gameState = GetNode<GameState>("GameState");
        gameState.CurrentState = GameState.State.Outro;
        
        VictoryScreen?.Show();
    }
    

    private void OnGameOver()
    {
        GameOverControl?.Show();
    }

    private void OnContinueRequested()
    {
        GameOverControl?.Hide();
        CurrentLevel.ProcessMode = ProcessModeEnum.Inherit;
    }
    
    private void OnMainMenuRequested()
    {
        var combatManager =  GetNode<CombatManager>("GameState/CombatManager");
        var battleSlotManager = combatManager.Player.GetNode<BattleSlotManager>("BattleSlotManager");
        battleSlotManager.Reparent(combatManager, false);
        
        GameOverControl?.Hide();
        VictoryScreen?.Hide();
        CurrentLevel.QueueFree();
        GetTree().ChangeSceneToFile(TitleScreenFile);
    }

    private void OnQuitGameRequested()
    {
        GetTree().Quit();
    }
}
