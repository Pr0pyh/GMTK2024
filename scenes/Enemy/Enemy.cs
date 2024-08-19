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
    enum MOVING_STATE
    {
        MOVING,
        FLANK,
    };
    STATE state;
    MOVING_STATE movingState;
    [Export]
    public int health;
    [Export]
    public PackedScene enemyDeadScene;
    [Export]
    public float speed;
    [Export]
    public AudioStream damageSound;
    [Export]
    public AudioStream[] attackSoundArray;
    [Export]
    public AudioStream[] yappinSoundArray;
    AnimationPlayer animPlayer;
    AnimationPlayer animPlayer2;
    GpuParticles3D particles;
    RayCast3D raycast;
    Player player;
    Enemy flankEnemy;
    EnemySpawner spawner;
    Area3D enemyDetect;
    AudioStreamPlayer3D audioPlayer;
    AudioStreamPlayer3D audioPlayer2;
    bool canAttack = true;
    public override void _Ready()
    {
        player = GetParent().GetParent().GetNode<Player>("Player");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer2 = GetNode<AnimationPlayer>("AnimationPlayer2");
        particles = GetNode<GpuParticles3D>("GPUParticles3D");
        raycast = GetNode<RayCast3D>("RayCast3D");
        enemyDetect = GetNode<Area3D>("EnemyDetect");
        audioPlayer = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
        audioPlayer2 = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D2");
        spawner = GetParent<EnemySpawner>();
        particles.Emitting = false;
        speed += 2.0f-Scale.Y;
        state = STATE.MOVING;
        GD.Randomize();
        chooseYappinRandom();
    }
    public override void _PhysicsProcess(double delta)
    {
        switch(state)
        {
            case STATE.MOVING:
                chooseMovingState();
                move(speed);
                break;
            // case STATE.FLANK:
            //     moveFlank(speed);
            //     break;
            case STATE.ATTACKING:
                // move(0.0f);
                break;
            case STATE.HURT:
                // move(0.0f);
                canAttack = false;
                break;
        }
    }
    public void chooseMovingState()
    {
        switch(movingState)
        {
            case MOVING_STATE.MOVING:
                move(speed);
                break;
            case MOVING_STATE.FLANK:
                moveFlank(speed);
                break;
        }
    }
    public void damage(Player player, int amount)
    {
        audioPlayer.Stream = damageSound;
        audioPlayer.Play();
        health -= amount;
        state = STATE.HURT;
        if(player.Scale.Y < Scale.Y)
        {
            animPlayer.Stop();
            animPlayer.Play("reset");
            animPlayer.Advance(0);
            animPlayer2.Stop();
            animPlayer2.Play("reset");
            animPlayer2.Advance(0);
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
            // scale(-0.2f);
            animPlayer.Stop();
            animPlayer.Play("reset");
            animPlayer.Advance(0);
            animPlayer2.Stop();
            animPlayer2.Play("reset");
            animPlayer2.Advance(0);
            animPlayer.Play("damage");
        }
    }
    public void attackPlayer()
    {
        raycast.ForceRaycastUpdate();
        if(raycast.IsColliding())
        {
            if(raycast.GetCollider() is Player player)
                player.damage(10, this);
        }
    }
    private void move(float speedValue)
    {
        animPlayer2.Play("reset");
        animPlayer2.Play("walk");
        LookAt(player.GlobalPosition, Vector3.Up, true);
        Rotation = new Vector3(0.0f, Rotation.Y, Rotation.Z);
        Vector3 moveVector = (player.GlobalPosition - GlobalPosition).Normalized();
        Velocity = moveVector*speedValue;
        MoveAndSlide();
    }
    private void moveFlank(float speedValue)
    {
        animPlayer2.Play("reset");
        animPlayer2.Play("walk");
        Vector3 moveVector = (GlobalPosition - flankEnemy.GlobalPosition).Normalized();
        moveVector.Y = 0.0f;
        Velocity = moveVector*speedValue;
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
    private void chooseAudioRandom()
    {
        audioPlayer.Stream = attackSoundArray[GD.RandRange(0, attackSoundArray.Length-1)];
        audioPlayer.Play();
    }
    private void chooseYappinRandom()
    {
        int number = GD.RandRange(0, yappinSoundArray.Length-1);
        audioPlayer2.Stream = yappinSoundArray[number];
        GD.Print(number);
        audioPlayer2.Play();
    }
    public void _on_enemy_detect_body_entered(Node3D body)
    {
        if(body is Player player)
        {
            if(!canAttack) return;
            chooseAudioRandom();
            state = STATE.ATTACKING;
            // animPlayer2.Stop();
            animPlayer2.Play("reset");
            animPlayer2.Play("attack");
        }
    }

    public void _on_enemy_detect_body_exited(Node3D body)
    {
        
    }
    public void _on_flank_detect_body_entered(Node3D body)
    {
        if(body is Enemy enemy && enemy != this)
        {
            flankEnemy = enemy;
            // state = STATE.MOVING;
            movingState = MOVING_STATE.FLANK;
        }
    }
    public void _on_flank_detect_body_exited(Node3D body)
    {
        if(body is Enemy enemy) 
        {
            // state = STATE.MOVING;
            movingState = MOVING_STATE.MOVING;
        }
    }
    public void _on_animation_player_animation_finished(String animName)
    {
        canAttack = true;
        enemyDetect.Monitoring = false;
        enemyDetect.Monitoring = true;
        if(animName == "damage" || animName == "hurt") 
        {
            state = STATE.MOVING;
            // movingState = MOVING_STATE.MOVING;
        }
    }
    public void _on_animation_player_2_animation_finished(String animName)
    {
        if(animName == "attack") 
        {
            state = STATE.MOVING;
            // movingState = MOVING_STATE.MOVING;
            enemyDetect.Monitoring = false;
            enemyDetect.Monitoring = true;
        }
    }
}
