using Godot;
using System;

public partial class Beer : Area3D
{
    public void _on_body_entered(Node3D body)
    {
        if(body is Player player)
        {
            if(player.addBeer()) QueueFree();
        }
    }
}
