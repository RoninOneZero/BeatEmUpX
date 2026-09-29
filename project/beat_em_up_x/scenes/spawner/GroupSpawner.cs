using Godot;
using Godot.Collections;

[GlobalClass]
public partial class GroupSpawner : Node2D
{
    private Array<Spawner> _spawners = [];

    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is Spawner spawner)
            {
                _spawners.Add(spawner);
                spawner.SelectSpawnMethod = Spawner.SpawnMethod.Manual;
                spawner.CallDeferred("reparent", GetParent());
            }
        }
    }

    public void Spawn()
    {
        foreach (var spawner in _spawners)
        {
            spawner.SpawnActor();
        }
    }
    
}
