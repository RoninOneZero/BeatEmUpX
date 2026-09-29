using Godot;
using Godot.Collections;

[GlobalClass, Icon("res://addons/at-icons/node/swords.svg")]
public partial class CombatManager : Node
{
    [Signal] public delegate void PlayerDiedEventHandler(Node player);
    [Signal] public delegate void EnemiesClearedEventHandler();
    
    [Export] public BattleSlotManager BattleSlotManager;
    [Export] public PackedScene BattleSlotPrefab;
    
    public int ComboCount { get; private set; } = 0;
    public int KillCount { get; private set; } = 0;
    public bool ArenaLocked = false;

    public Dictionary<Node2D, Node> BattleSlots { get; private set; } = [];
    public Node2D Player { get; private set; }
    public Array<Pawn2D> Enemies { get; private set; } = [];

    public void InitializeCombat()
    {
        ComboCount = 0;
        KillCount = 0;
        ArenaLocked = false;

        if (BattleSlotManager == null)
        {
            BattleSlotManager = BattleSlotPrefab.Instantiate() as BattleSlotManager;
            AddChild(BattleSlotManager);
        }
        
        
        Player = null;
        Enemies.Clear();
    }
    
    public void AddPlayerToTracker(Node2D node)
    {
        var nodeHealth = node.GetNodeOrNull<Health>("Health");

        if (nodeHealth != null) nodeHealth.HealthDepleted += () => OnPlayerDeath(node);
        
        Player = node;
        
        BattleSlotManager.Reparent(Player, false);

        foreach (var enemy in Enemies)
        {
            var controller = enemy.GetNode<AIController>("AIController");
            controller.Target ??= Player;
        }
        
    }
    

    public void AddEnemyToTracker(Pawn2D node)
    {
        var nodeHealth = node.GetNodeOrNull<Health>("Health");

        if (nodeHealth != null) nodeHealth.HealthDepleted += () => RemoveEnemyFromTracker(node);

        Enemies.Add(node);
    }

    public void RemoveEnemyFromTracker(Pawn2D node)
    {
        Enemies.Remove(node);
        if (Enemies.Count == 0) EmitSignalEnemiesCleared();
    }
    
    public void RemovePlayerFromTracker(Node2D node)
    {
        GD.Print("Player defeated!: ", node.Name);
        
        BattleSlotManager.Reparent(this);
        
        Player = null;
    }

    private void OnPlayerDeath(Node2D player)
    {
        RemovePlayerFromTracker(player);
        EmitSignalPlayerDied(player);

        foreach (var enemy in Enemies)
        {
            var controller = enemy.GetNode<AIController>("AIController");
            if (controller != null) controller.Target = null;
        }
    }
}
