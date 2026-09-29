using System;
using Godot;
using Godot.Collections;

[GlobalClass, Icon("res://addons/at-icons/node/globe.svg")]

public partial class Global : Node
{
    public static Global Instance { get; private set; }

    [Export]
    public Array<string> Levels { get; set; } = [];


    public override void _Ready()
    {
        Instance = this;
    }
}
