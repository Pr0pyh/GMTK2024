using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    enum STATE
    {
        MOVING,
        FLANK,
        ATTACKING,
        HURT
    };
    STATE state;
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
    Player player;
    Enemy flankEnemy;
    EnemySpawner spawner;
    bool canAttack = true;
    public override void _Ready()
    {
        player = GetParent().GetParent().GetNode<Player>("Player");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer2 = GetNode<AnimationPlayer>("AnimationPlayer2");
        particles = GetNode<GpuParticles3D>("GPUParticles3D");
        raycast = GetNode<RayCast3D>("RayCast3D");
        spawner = GetParent<EnemySpawner>();
        particles.Emitting = false;
        speed = 2.0f-Scale.Y;
        state = STATE.MOVING;
    }
    public override void _PhysicsProcess(double delta)
    {
        switch(state)
        {
            case STATE.MOVING:
                move();
                break;
            case STATE.FLANK:
                moveFlank();
                break;
            case STATE.ATTACKING:
                break;
            case STATE.HURT:
                canAttack = false;
                break;
        }
    }
    public void damage(Player player, int amount)
    {
        state = STATE.HURT;
        if(player.Scale.Y < Scale.Y)
        {
            animPlayer.Stop();
            animPlayer.Play("hurt");
            return;
        }
        if(health <= 0)
        {
            spawnRagdoll(player);
        }
        else
        {
            particles.Emitting = true;
            health -= 10;
            // scale(-0.2f);
            animPlayer.Stop();
            animPlayer.Play("damage");
        }
    }
    public void attackPlayer()
    {
        raycast.ForceRaycastUpdate();
        if(raycast.IsColliding())
        {
            if(raycast.GetCollider() is Player player)
                player.damage(10);
        }
    }
    private void move()
    {
        LookAt(player.GlobalPosition, Vector3.Up, true);
        Rotation = new Vector3(0.0f, Rotation.Y, Rotation.Z);
        Vector3 moveVector = (player.GlobalPosition - GlobalPosition).Normalized();
        Velocity = moveVector*speed;
        MoveAndSlide();
    }
    private void moveFlank()
    {
        Vector3 moveVector = (GlobalPosition - flankEnemy.GlobalPosition).Normalized();
        moveVector.Y = 0.0f;
        Velocity = moveVector*speed;
        MoveAndSlide();
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
        spawner.decrease();
        EnemyDead enemyDead = (EnemyDead)enemyDeadScene.Instantiate<RigidBody3D>();
        GetParent().AddChild(enemyDead);
        enemyDead.GlobalTransform = GlobalTransform;
        enemyDead.scale(Scale);
        enemyDead.LinearVelocity = ((GlobalPosition - player.GlobalPosition).Normalized() + new Vector3(0.0f, 0.4f, 0.0f)) * 15.0f;
        QueueFree();
    }
    public void _on_enemy_detect_body_entered(Node3D body)
    {
        if(body is Enemy enemy)
        {
            flankEnemy = enemy;
            state = STATE.FLANK;
        }
        if(body is Player player)
        {
            if(!canAttack) return;
            state = STATE.ATTACKING;
            animPlayer2.Play("attack");
        }
    }

    public void _on_enemy_detect_body_exited(Node3D body)
    {
        if(body is Enemy enemy) state = STATE.MOVING;
    }
    public void _on_animation_player_animation_finished(String animName)
    {
        canAttack = true;
        // if(animName == "damage") state = STATE.MOVING;
    }
    public void _on_animation_player_2_animation_finished(String animName)
    {
        if(animName == "attack") state = STATE.MOVING;
    }
}
