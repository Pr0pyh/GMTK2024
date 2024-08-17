using Godot;
using System;

public partial class EnemySpawner : Node3D
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
    PackedScene enemyScene;
    [Export]
    float enemyScale;
    [Export]
    int maxNumber;
    int number = 0;
    int score = 0;
    Timer timer;
    World world;
    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        world = GetParent<World>();
        GD.Randomize();
    }
    public void spawn(Vector3 position)
    {
        Enemy enemy = (Enemy)enemyScene.Instantiate();
        AddChild(enemy);
        enemy.Scale = new Vector3(enemyScale, enemyScale, enemyScale);
        enemy.GlobalPosition = new Vector3(position.X, enemyScale, position.Z);
        number++;
    }
    public void decrease()
    {
        if(number > 0)
        {
            world.scorePoints(20);
            number--;
        }
    }
    public void _on_timer_timeout()
    {
        Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        position.X = (float)GD.RandRange(minX, maxX);
        position.Z = (float)GD.RandRange(minZ, maxZ);
        if(number < maxNumber) spawn(position);
        if(timer.WaitTime > 2) timer.WaitTime -= 0.5;
        GD.Print(timer.WaitTime);
    }
}
