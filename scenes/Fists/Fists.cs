using Godot;
using System;

public partial class Fists : Node3D
{
    public AnimationPlayer animPlayer;
    public AnimationPlayer animPlayer2;
    RayCast3D raycast;
    TextureRect textureRect;
    public Player player;
    AudioStreamPlayer audioPlayer;
    [Export]
    PackedScene impactScene;
    bool canAttack = true;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer2 = GetNode<AnimationPlayer>("AnimationPlayer2");
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        textureRect = GetNode<CanvasLayer>("CanvasLayer").GetNode<TextureRect>("TextureRect");
        // player = (Player)GetParent().GetParent();
        textureRect.Visible = false;
    }
    public void attack(ref RayCast3D raycast)
    {
        if(!canAttack) return;
        this.raycast = raycast;
        canAttack = false;
        animPlayer.Stop();
        animPlayer.Play("attack");
    }

    public void attack2(ref RayCast3D raycast)
    {
        if(!canAttack) return;
        this.raycast = raycast;
        canAttack = false;
        animPlayer.Stop();
        animPlayer.Play("attack_2");
    }

    public void attackRay()
    {
        canAttack = true;
        raycast.ForceRaycastUpdate();
        if(raycast.IsColliding())
        {
            if(raycast.GetCollider() is Enemy enemy)
            {
                enemy.damage(player, 10);
            }
            addImpact(raycast.GetCollisionPoint());
            player.addTrauma(0.2f);
            animPlayer2.Play("screen");
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
