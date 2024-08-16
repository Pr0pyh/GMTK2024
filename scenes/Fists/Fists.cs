using Godot;
using System;

public partial class Fists : Node3D
{
    AnimationPlayer animPlayer;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public void attack(RayCast3D raycast)
    {
        raycast.ForceRaycastUpdate();
        if(raycast.IsColliding())
        {
            if(raycast.GetCollider() is Enemy enemy)
            {
                GD.Print("colliding wall" + enemy);
                enemy.damage((Player)GetParent().GetParent(), 10);
            }
            GD.Print("colliding");
        }
        animPlayer.Stop();
        animPlayer.Play("attack");
    }

    public void attackRay(RayCast3D raycast)
    {
        raycast.ForceRaycastUpdate();
        if(raycast.IsColliding())
        {
            GD.Print("colliding");
        }
    }
}
