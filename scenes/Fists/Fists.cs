using Godot;
using System;

public partial class Fists : Node3D
{
    public AnimationPlayer animPlayer;
    public AnimationPlayer animPlayer2;
    RayCast3D raycast;
    TextureRect textureRect;
    Player player;
    AudioStreamPlayer audioPlayer;
    [Export]
    PackedScene impactScene;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer2 = GetNode<AnimationPlayer>("AnimationPlayer2");
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        textureRect = GetNode<CanvasLayer>("CanvasLayer").GetNode<TextureRect>("TextureRect");
        player = (Player)GetParent().GetParent();
        textureRect.Visible = false;
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
            player.addTrauma(0.2f);
            animPlayer2.Play("screen");
            GD.Print("colliding");
            audioPlayer.Play();
        }
    }

    private void addImpact(Vector3 position)
    {
        Impact impact = impactScene.Instantiate<Impact>();
        GetParent().GetParent().GetParent().AddChild(impact);
        impact.GlobalPosition = position;
    }
}
