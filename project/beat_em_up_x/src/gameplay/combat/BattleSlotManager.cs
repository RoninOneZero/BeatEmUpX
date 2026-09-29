using Godot;
using Godot.Collections;

public partial class BattleSlotManager : Node2D
{
    public Array<Node2D> BattleSlots { get; private set; } = [];
    public Dictionary<Node2D, Node2D> BattleSlotAssignments { get; private set; } = [];

    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is Node2D node2D)
            {
                BattleSlots.Add(node2D);
                BattleSlotAssignments[node2D] = null;
            }
        }
    }

    public Node2D GetRandomOpenBattleSlot(Node2D node)
    {
        Node2D openSlot = null;

        if (BattleSlotAssignments.Values.Contains(node))
        {
            foreach (var (slot, assignment) in BattleSlotAssignments)
            {
                if (assignment == node) return slot;
            }
            return null;
        }
        
        var shuffledBattleSlots = BattleSlots;
        shuffledBattleSlots.Shuffle();
        foreach (var slot in shuffledBattleSlots)
        {
            var assignment = BattleSlotAssignments[slot];

            if (assignment == null)
            {
                openSlot = slot;
                BattleSlotAssignments[slot] = node;
                break;
            }
        }
        
        return openSlot;
    }

    public void ClearAssignment(Node2D node)
    {
        if  (BattleSlotAssignments.ContainsKey(node))
        {
            BattleSlotAssignments[node] = null;
        }
    }
}
