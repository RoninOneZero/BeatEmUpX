using Godot;
using Godot.Collections;

[GlobalClass, Icon("res://addons/at-icons/node/head_with_gear.svg")]
public partial class FiniteStateMachine : Node
{
    [Signal] public delegate void StateChangedEventHandler(string newStateName);
    
    [Export] public State InitialState;

    public State CurrentState { get; private set; }
    public State PreviousState;

    private readonly Dictionary<string, State> _states = [];

    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is not State state) continue;
            _states[state.Name.ToString().ToLower()] = state;
            state.StateMachine = this;
        }

        if (InitialState != null)
        {
            CurrentState = InitialState;
            CurrentState.Enter();
            CurrentState.IsActive = true;
        }
        else if (GetChildCount() > 0 && GetChild(0) is State firstState)
        {
            CurrentState = firstState;
            CurrentState.Enter();
            CurrentState.IsActive = true;
        }
    }

    public override void _Process(double delta) => CurrentState?.Update(delta);

    public override void _PhysicsProcess(double delta) => CurrentState?.PhysicsUpdate(delta);


    public void TransitionTo(string newStateName, bool forceReEntry = false)
    {
        string key = newStateName.ToLower();

        if (!_states.ContainsKey(newStateName))
        {
            GD.Print($"State does not exist: '{newStateName}'.");
            return;
        }
        
        if (!_states[newStateName].Enabled) return;

        if (CurrentState.Name.ToString().ToLower() == key && !forceReEntry)
        {
            GD.Print($"Attempted invalid re-entry of state: {newStateName}");
            return;
        }

        CurrentState?.Exit();
        if (CurrentState != null) CurrentState.IsActive = false;
        
        CurrentState = _states[key];
        CurrentState.Enter();
        CurrentState.IsActive = true;
        
        EmitSignalStateChanged(newStateName);
    }

    public void SendEvent(string eventName)
    {
        CurrentState?.ReceiveEvent(eventName);
    }
}
