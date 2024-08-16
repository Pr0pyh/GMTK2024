using Godot;
using System;

public partial class EnemyDead : RigidBody3D
{
    MeshInstance3D cube;
    public override void _Ready()
    {
        cube = GetNode<MeshInstance3D>("MeshInstance3D");
    }
    public void scale(Vector3 newScale)
    {
        cube.Scale = newScale;
    }
    public void _on_timer_timeout()
    {
        QueueFree();
    }
}
