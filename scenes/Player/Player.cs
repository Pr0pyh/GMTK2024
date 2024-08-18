using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Player : CharacterBody3D
{
    enum STATE
    {
        MOVING,
        HIT
    };

    [Export]
    public float speed;
    [Export]
    public float mouseSens;
    [Export]
    public int health;
    STATE state;
    Camera3D camera;
    Camera3D viewportCamera;
    Fists fists;
    RayCast3D ray;
    AnimationPlayer animPlayer;
    TextureProgressBar textureProgressBar;
    Label beerCountLabel;
    Label cigCountLabel;
    Timer hitTimer;
    Enemy attacker;
    float trauma;
    //beer and cigarrete count
    int beer = 0;
    int cig = 0;
    int maxBeer = 6;
    int maxCig = 6;
    //sway mouse
    float mouseMove;
    public override void _Ready()
    {
        camera = GetNode<Camera3D>("Camera3D");
        viewportCamera = camera.GetNode<SubViewportContainer>("SubViewportContainer").GetNode<SubViewport>("SubViewport").GetNode<Camera3D>("ViewportCamera");
        ray = camera.GetNode<RayCast3D>("RayCast3D");
        fists = viewportCamera.GetNode<Fists>("Fists");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        textureProgressBar = GetNode<CanvasLayer>("CanvasLayer").GetNode<TextureProgressBar>("TextureProgressBar");
        beerCountLabel = GetNode<CanvasLayer>("CanvasLayer").GetNode<AnimatedSprite2D>("AnimatedSprite2D").GetNode<Label>("Label");
        cigCountLabel = GetNode<CanvasLayer>("CanvasLayer").GetNode<AnimatedSprite2D>("AnimatedSprite2D3").GetNode<Label>("Label");
        hitTimer = GetNode<Timer>("Timer");
        animPlayer.Play("start");
        beerCountLabel.Text = beer.ToString();
        cigCountLabel.Text = cig.ToString();
        fists.player = this;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouseMotion)
        {
            mouseMove = -mouseMotion.Relative.X;
            camera.RotateX(Mathf.DegToRad(mouseMotion.Relative.Y * mouseSens * -1));
            camera.RotationDegrees = new Vector3(Mathf.Clamp(camera.RotationDegrees.X, -75.0f, 75.0f), 0.0f, 0.0f);
            this.RotateY(Mathf.DegToRad(mouseMotion.Relative.X * mouseSens * -1));
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        switch(state)
        {
            case STATE.MOVING:
                sway(delta);
                exitInput();
                scaleInput();
                updateBar();
                viewportUpdate();
                shakeState(delta);
                attackInput();
                move(delta);
                break;
            case STATE.HIT:
                sway(delta);
                exitInput();
                scaleInput();
                updateBar();
                viewportUpdate();
                shakeState(delta);
                hitMove();
                break;
        }
    }
    //public methods
    public bool addCigarete()
    {
        if(cig < maxCig)
        {
            animPlayer.Play("cig");
            cig++;
            cigCountLabel.Text = cig.ToString();
            return true;
        }
        return false;
    }
    public bool addBeer()
    {
        if(beer < maxBeer)
        {
            animPlayer.Play("beer");
            beer++;
            beerCountLabel.Text = beer.ToString();
            return true;
        }
        return false;
    }
    public void addTrauma(float value)
    {
        trauma += value;
    }
    public void damage(int value, Enemy enemy)
    {
        animPlayer.Play("screen");
        addTrauma(0.3f);
        if(health <= 0)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            GetTree().ChangeSceneToFile("res://scenes/EndScreen/EndScene.tscn");
        }
        else
        {
            attacker = enemy;
            state = STATE.HIT;
            health -= value;
            hitTimer.Start();
        }
    }
    //input
    private void sway(double delta)
    {
        if(mouseMove != null)
        {
            if(mouseMove > 3)
                fists.Position = fists.Position.Lerp(new Vector3(-0.24f, -0.784f, -0.925f), (float)(delta*5)); 
            else if(mouseMove < -3)
                fists.Position = fists.Position.Lerp(new Vector3(0.24f, -0.784f, -0.925f), (float)(delta*5));
            else
                fists.Position = fists.Position.Lerp(new Vector3(0.0f, -0.784f, -0.925f), (float)(delta*5));
        }
    }
    private void updateBar()
    {
        textureProgressBar.Value = health;
    }
    private void exitInput()
    {
        if(Input.IsActionPressed("quit"))
            GetTree().Quit();
    }
    private void scaleInput()
    {
        if(Input.IsActionJustPressed("q") && beer>0)
        {
            animPlayer.Play("beer_consumed");
            beer--;
            beerCountLabel.Text = beer.ToString();
            scale(0.5f);
        }
        else if(Input.IsActionJustPressed("e") && cig>0)
        {
            animPlayer.Play("cig_consumed");
            cig--;
            cigCountLabel.Text = cig.ToString();
            scale(-0.5f);
        }
    }
    private void attackInput()
    {
        if(Input.IsActionJustPressed("attack"))
            fists.attack(ref ray);

        if(Input.IsActionJustPressed("attack2"))
            fists.attack2(ref ray);
    }
    private Vector3 moveInput()
    {
        Vector3 moveVector = new Vector3(0.0f, 0.0f, 0.0f);
        if(Input.IsActionPressed("up"))
            moveVector -= camera.GlobalTransform.Basis.Z;
        if(Input.IsActionPressed("down"))
            moveVector += camera.GlobalTransform.Basis.Z;
        if(Input.IsActionPressed("left"))
            moveVector -= camera.GlobalTransform.Basis.X;
        if(Input.IsActionPressed("right"))
            moveVector += camera.GlobalTransform.Basis.X;
        moveVector.Y = 0.0f;
        moveVector = moveVector.Normalized();

        return moveVector;
    }
    private void hitMove()
    {
        Vector3 moveVector = new Vector3(0.0f, 0.0f, 0.0f);
        if(attacker != null) moveVector = (GlobalPosition - attacker.GlobalPosition).Normalized();
        Velocity = moveVector*speed;
        MoveAndSlide();
    }
    private void viewportUpdate()
    {
        viewportCamera.GlobalTransform = camera.GlobalTransform;
        viewportCamera.HOffset = camera.HOffset;
        viewportCamera.VOffset = camera.VOffset;
    }
    private void move(double delta)
    {
        Vector3 moveVector = moveInput();

        Velocity = moveVector*speed;
        MoveAndSlide();
    }
    private void scale(float amount)
    {
        if((Scale.Y+amount) <= 0.0f || (Scale.Y+amount) >= 2.0f)
            return;
        speed -= amount*5.0f;
        fists.animPlayer.SpeedScale -= amount;
        Scale += new Vector3(amount, amount, amount);
        GlobalPosition += new Vector3(0.0f, amount, 0.0f);
    }
    private void shake()
    {
        float amount = trauma;
        // camera.HOffset = (float)(amount*GD.RandRange(-1, 1));
        // camera.VOffset = (float)(amount*GD.RandRange(-1, 1));
        // viewportCamera.HOffset = (float)(amount*GD.RandRange(-1, 1));
        // viewportCamera.VOffset = (float)(amount*GD.RandRange(-1, 1));
        camera.HOffset = (float)(-1*-amount);
        camera.VOffset = (float)(-1*amount);
        viewportCamera.HOffset = (float)(-1*-amount);
        viewportCamera.VOffset = (float)(-1*amount);
    }
    private void shakeState(double delta)
    {
        if(!(trauma < 0.0f))
        {
            shake();
            trauma = Mathf.Max((float)(trauma - 0.8*delta), 0.0f);
        }
    }

    public void _on_timer_timeout()
    {
        attacker = null;
        state = STATE.MOVING;
    }
}
