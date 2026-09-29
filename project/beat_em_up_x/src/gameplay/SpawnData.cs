using Godot;
using System;

[GlobalClass, Tool, Icon("res://addons/at-icons/node/file_box.svg")]
public partial class SpawnData : Resource
{
    public SpawnData() { }
    
    [Export] public PackedScene SceneToSpawn { get; set; }
    [Export] public Texture2D PreviewImage;
}
