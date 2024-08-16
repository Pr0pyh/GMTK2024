using Godot;
using System;

public partial class Fists : Node3D
{
    public AnimationPlayer animPlayer;
    RayCast3D raycast;
    Player player;
    [Export]
    PackedScene impactScene;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        player = (Player)GetParent().GetParent();
    }
    public void attack(ref RayCast3D raycast)
    {
        this.raycast = raycast;
        animPlayer.Stop();
        animPlayer.Play("attack");
    }

    public void attack2(ref RayCast3D raycast)
    {
        this.raycast = raycast;
        animPlayer.Stop();
        animPlayer.Play("attack_2");
    }

    public void attackRay()
    {
        raycast.ForceRaycastUpdate();
        if(raycast.IsColliding())
        {
            if(raycast.GetCollider() is Enemy enemy)
            {
                GD.Print("colliding wall" + enemy);
                enemy.damage(player, 10);
            }
            addImpact(raycast.GetCollisionPoint());
            player.addTrauma(0.1f);
            GD.Print("colliding");
        }
    }

    private void addImpact(Vector3 position)
    {
        Impact impact = impactScene.Instantiate<Impact>();
        GetParent().GetParent().GetParent().AddChild(impact);
        impact.GlobalPosition = position;
    }
}
