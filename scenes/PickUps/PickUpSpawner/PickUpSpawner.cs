using Godot;
using System;

public partial class PickUpSpawner : Node3D
{
    [Export]
    public double minX;
    [Export]
    public double maxX;
    [Export]
    public double minZ;
    [Export]
    public double maxZ;
    [Export]
    PackedScene pickupScene;
    [Export]
    PackedScene pickupScene2;
    int maxNumber = 6;
    int number = 0;
    public override void _Ready()
    {
        GD.Randomize();
    }
    public void spawn(Vector3 position)
    {
        PackedScene chosenScene;
        if(GD.Randi()%2 == 0)  chosenScene = pickupScene;
        else chosenScene = pickupScene2; 
        Node3D pickup = (Node3D)chosenScene.Instantiate();
        AddChild(pickup);
        pickup.GlobalPosition = position;
        number++;
    }
    public void decrease()
    {
        if(number > 0) number--;
    }
    public void _on_timer_timeout()
    {
        Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        position.X = (float)GD.RandRange(minX, maxX);
        position.Z = (float)GD.RandRange(minZ, maxZ);
        if(number < maxNumber) spawn(position);
    }
}
