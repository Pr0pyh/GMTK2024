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
    GpuParticles3D particles;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        particles = GetNode<GpuParticles3D>("GPUParticles3D");
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
}
