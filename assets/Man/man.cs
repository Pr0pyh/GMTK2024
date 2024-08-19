using Godot;
using System;

public partial class man : Node3D
{
	[Export]
	public PackedScene[] manVariants;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int number = GD.RandRange(0, manVariants.Length-1);
		Node3D man = manVariants[number].Instantiate<Node3D>();
		AddChild(man);
		man.Position = Position;
	}
}
