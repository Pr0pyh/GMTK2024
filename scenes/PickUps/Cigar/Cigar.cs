using Godot;
using System;

public partial class Cigar : Area3D
{
    public void _on_body_entered(Node3D body)
    {
        if(body is Player player)
        {
            if(player.addCigarete()) QueueFree();
        }
    }
}
