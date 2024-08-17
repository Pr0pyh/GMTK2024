using Godot;
using System;

public partial class Beer : Area3D
{
    public PickUpSpawner spawner;
    public override void _Ready()
    {
        spawner = GetParent<PickUpSpawner>();
    }
    public void _on_body_entered(Node3D body)
    {
        if(body is Player player)
        {
            if(player.addBeer())
            {
                spawner.decrease();
                QueueFree();
            }
        }
    }
}
