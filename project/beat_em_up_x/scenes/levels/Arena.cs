using Godot;
using Godot.Collections;

public partial class Arena : Node2D
{
    [Signal] public delegate void TriggeredEventHandler();

    [Export] public int Waves { get; set; } = 3;
    
    [Export] public Timer SpawnDelay; // Time after screen is clear to spawn enemies
    [Export] public Timer RespawnDelay; // Time between enemy spawns
    [Export] public StaticBody2D Wall1;
    [Export] public StaticBody2D Wall2;
    [Export] public Node2D CameraFocusPoint;
    [Export] public FollowCam Camera;

    private bool _active = false;
    private Array<Spawner> _spawners = [];
    private Pawn2D _player;

    public override void _Ready()
    {
        UnlockArena();
        
        foreach (var child in GetChildren())
        {
            if (child is Spawner spawner)
            {
                _spawners.Add(spawner);
                spawner.CallDeferred("reparent", GetParent());
            }
            
        }
        
        var combat = Main.Instance.GetNode<CombatManager>("GameState/CombatManager");
        combat.EnemiesCleared += OnEnemiesCleared;
    }
    
    public void Trigger(Node2D body)
    {
        if (_active) return;
        GD.Print("Arena triggered");
        _active = true;
        _player = body as Pawn2D;
        LockArena();
        Camera.Target = CameraFocusPoint;
        Respawn();
    }

    public async void Respawn()
    {
        if (Waves <= 0) return;
        GD.Print("Respawning arena");
        Waves -= 1;
        
        SpawnDelay.Start();
        await ToSignal(SpawnDelay, "timeout");

        var shuffledSpawners = _spawners;
        shuffledSpawners.Shuffle();
        
        foreach (var spawner in shuffledSpawners)
        {
            spawner.SpawnActor();
            RespawnDelay.Start();
            await ToSignal(SpawnDelay, "timeout");
        }
    }

    public void LockArena()
    {
        Wall1.ProcessMode = ProcessModeEnum.Inherit;
        Wall2.ProcessMode = ProcessModeEnum.Inherit;
    }

    public void UnlockArena()
    {
        Wall1.ProcessMode = ProcessModeEnum.Disabled;
        Wall2.ProcessMode = ProcessModeEnum.Disabled;
    }
    

    public void OnEnemiesCleared()
    {
        if (!_active) return;
        
        if (Waves > 0)
        {
            Respawn();
            return;
        }
        
        _active = false;
        Camera.Target = _player;
        UnlockArena();
    }
    
}
