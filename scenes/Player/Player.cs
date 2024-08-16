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
    STATE state;
    Camera3D camera;
    Fists fists;
    RayCast3D ray;

    public override void _Ready()
    {
        camera = GetNode<Camera3D>("Camera3D");
        ray = camera.GetNode<RayCast3D>("RayCast3D");
        fists = camera.GetNode<Fists>("Fists");
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
                attackInput();
                move(delta);
                break;
        }
    }
    //input
    private void exitInput()
    {
        if(Input.IsActionPressed("quit"))
            GetTree().Quit();
    }
    private void scaleInput()
    {
        if(Input.IsActionJustPressed("q"))
            scale(0.5f);
        else if(Input.IsActionJustPressed("e"))
            scale(-0.5f);
    }
    private void attackInput()
    {
        if(Input.IsActionJustPressed("attack"))
            fists.attack(ray);
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
        speed -= amount;
        Scale += new Vector3(amount, amount, amount);
        GlobalPosition += new Vector3(0.0f, amount/2.0f, 0.0f);
    }
}
