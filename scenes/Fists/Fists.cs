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
    Node3D fist1;
    Node3D fist2;
    [Export]
    Resource scoreResource;
    bool canAttack = true;
    int damage = 10;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer2 = GetNode<AnimationPlayer>("AnimationPlayer2");
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        textureRect = GetNode<CanvasLayer>("CanvasLayer").GetNode<TextureRect>("TextureRect");
        fist1 = GetNode<Node3D>("fist");
        fist2 = GetNode<Node3D>("fist2");
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
    public void upgrade(int upgrade, int cost)
    {
        if(damage > 100) return;
        damage += upgrade;
        float scaleUpgrade = upgrade/100.0f;
        fist1.Scale += new Vector3(scaleUpgrade, scaleUpgrade, scaleUpgrade);
        fist2.Scale += new Vector3(scaleUpgrade, scaleUpgrade, scaleUpgrade);
        if(scoreResource is Score score) score.score -= cost;
    }
    public void attackRay()
    {
        canAttack = true;
        raycast.ForceRaycastUpdate();
        if(raycast.IsColliding())
        {
            if(raycast.GetCollider() is Enemy enemy)
            {
                enemy.damage(player, damage);
            }
            else if(raycast.GetCollider() is UpgradeVendor upgradeVendor)
            {
                if(scoreResource is Score resourceScore)
                {
                    upgradeVendor.upgrade(this, resourceScore.score);
                }
            }
            else if(raycast.GetCollider() is Jukebox jukebox)
            {
                jukebox.changeSong();
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
