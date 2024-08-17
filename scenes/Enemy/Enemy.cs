using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    [Export]
    public int health;
    [Export]
    public PackedScene enemyDeadScene;
    [Export]
    float speed;
    AnimationPlayer animPlayer;
    AnimationPlayer animPlayer2;
    GpuParticles3D particles;
    RayCast3D raycast;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer2 = GetNode<AnimationPlayer>("AnimationPlayer2");
        particles = GetNode<GpuParticles3D>("GPUParticles3D");
        raycast = GetNode<RayCast3D>("RayCast3D");
        particles.Emitting = false;
        speed = 5.0f;
    }
    public void damage(Player player, int amount)
    {
        if(player.Scale.Y < Scale.Y)
            return;
        if(health <= 0)
        {
            spawnRagdoll(player);
        }
        else
        {
            particles.Emitting = true;
            health -= 10;
            scale(-0.2f);
            animPlayer.Play("damage");
        }
    }
    public void attackPlayer()
    {
        raycast.ForceRaycastUpdate();
        if(raycast.IsColliding())
        {
            if(raycast.GetCollider() is Player player)
                player.damage(5);
        }
    }
    private void scale(float amount)
    {
        if((Scale.Y+amount) <= 0.8f || (Scale.Y+amount) >= 2.0f)
            return;
        Scale += new Vector3(amount, amount, amount);
        GlobalPosition += new Vector3(0.0f, amount/2.0f, 0.0f);
    }

    private void spawnRagdoll(Player player)
    {
        EnemyDead enemyDead = (EnemyDead)enemyDeadScene.Instantiate<RigidBody3D>();
        GetParent().AddChild(enemyDead);
        enemyDead.GlobalTransform = GlobalTransform;
        enemyDead.scale(Scale);
        GD.Print(enemyDead.Scale);
        enemyDead.LinearVelocity = ((GlobalPosition - player.GlobalPosition).Normalized() + new Vector3(0.0f, 0.4f, 0.0f)) * 15.0f;
        QueueFree();
    }
    public void _on_enemy_detect_body_entered(Node3D body)
    {
        if(body is Player player)
        {
            animPlayer2.Stop();
            animPlayer2.Play("attack");
        }
    }
}
