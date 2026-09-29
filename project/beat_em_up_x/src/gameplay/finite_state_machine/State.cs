using Godot;

[GlobalClass, Icon("res://addons/at-icons/node/cog.svg")]
public partial class State : Node
{
    [Export] public bool Enabled { get; set; } = true;
    
    public FiniteStateMachine StateMachine;
    public bool IsActive;

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update(double delta) { }

    public virtual void PhysicsUpdate(double delta) { }

    public virtual void ReceiveEvent(string event_name) { }
}
