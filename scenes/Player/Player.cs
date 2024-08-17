using Godot;
using System;

public partial class Player : CharacterBody3D
{
    enum STATE
    {
        MOVING
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
    float trauma;
    //beer and cigarrete count
    int beer = 0;
    int cig = 0;
    int maxBeer = 6;
    int maxCig = 6;
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
        beerCountLabel.Text = beer.ToString();
        cigCountLabel.Text = cig.ToString();
        fists.player = this;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouseMotion)
        {
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
                exitInput();
                scaleInput();
                updateBar();
                viewportUpdate();
                shakeState(delta);
                attackInput();
                move(delta);
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
    public void damage(int value)
    {
        animPlayer.Play("screen");
        addTrauma(0.3f);
        if(health <= 0)
            GetTree().ReloadCurrentScene();
        else
            health -= value;
    }
    //input
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
            animPlayer.Play("beer");
            beer--;
            beerCountLabel.Text = beer.ToString();
            scale(0.5f);
        }
        else if(Input.IsActionJustPressed("e") && cig>0)
        {
            animPlayer.Play("cig");
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
}
