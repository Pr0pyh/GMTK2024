using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    [Export]
    public int health;
    [Export]
    public PackedScene enemyDeadScene;
    public void damage(Player player, int amount)
    {
        if(health <= 0)
            spawnRagdoll(player);
    }

    private void spawnRagdoll(Player player)
    {
        RigidBody3D enemyDead = enemyDeadScene.Instantiate<RigidBody3D>();
        GetParent().AddChild(enemyDead);
        enemyDead.GlobalTransform = GlobalTransform;
        enemyDead.LinearVelocity = (GlobalPosition - player.GlobalPosition).Normalized() * 8.0f;
        QueueFree();
    }
}
