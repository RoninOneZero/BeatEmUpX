using System;
using Godot;

[GlobalClass, Tool, Icon("res://addons/at-icons/node2d/chess_pawn.svg")]
public partial class Spawner : Node2D
{
    [Signal] public delegate void SpawnedEventHandler(Node2D node);

    [Export] public SpawnData SpawnData
    {
        get { return _spawnData; }
        set
        {
            _spawnData = value;
            if (!Engine.IsEditorHint()) return;

            var sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
            if (sprite == null)  return;
            
            sprite.Texture = _spawnData.PreviewImage;
        }
    }
    [Export] public bool Flip
    {
        get { return _flip; }
        set
        {
            _flip = value;
            
            if (!Engine.IsEditorHint()) return;
            
            var sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
            if (sprite == null)  return;
            sprite.FlipH = _flip;
        }
    }
    [Export] public Team NodeTeam = Team.Enemy;
    [Export] public SpawnMethod SelectSpawnMethod = SpawnMethod.OnReady;

    public Node SpawnedEntity;
    
    private SpawnData _spawnData;
    private bool _flip = false;
    private bool _triggered = false;
    
    public CombatManager CombatManager { get; set; }

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;
        
        CombatManager = Main.Instance.GetNode<CombatManager>("GameState/CombatManager");
        
        var sprite = GetNode<Sprite2D>("Sprite2D");
        sprite.Hide();
        
        if (SelectSpawnMethod == SpawnMethod.OnReady) SpawnActor();
    }

    public void SpawnActor(bool respawn = false)
    {
        // if (_triggered)
        // {
        //     if (respawn == false) return;
        // }
        
        GD.Print("Poof!");
        SpawnedEntity?.QueueFree();

        _triggered = true;
        var newActor = SpawnData.SceneToSpawn.Instantiate() as Node2D;
        newActor.GlobalPosition = GlobalPosition;
        CallDeferred("add_sibling", newActor);
    
        if (Flip)
        {
            var pawn = newActor as Pawn2D;
            pawn?.FlipFacing();
        }
        
        switch (NodeTeam)
        {
            case Team.Player:
                CombatManager.AddPlayerToTracker(newActor);
                break;
            case Team.Enemy:
                CombatManager.AddEnemyToTracker(newActor as Pawn2D);
                break;
        }


        SpawnedEntity = newActor;
        
        var health = newActor.GetNodeOrNull<Health>("Health");
        if (health != null) health.HealthDepleted += OnEntityDefeated;
        
        EmitSignalSpawned(newActor);
    }

    private void OnScreenEntered()
    {
        if (SelectSpawnMethod != SpawnMethod.OnVisible) return;
        
        SpawnActor();
    }

    private void OnEntityDefeated()
    {
        SpawnedEntity = null;
    }
    
    public enum SpawnMethod
    {
        Manual,
        OnReady,
        OnVisible,
    }

    public enum Team
    {
        Player,
        Ally,
        Enemy,
    }
    
}
