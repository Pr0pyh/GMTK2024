using Godot;
using System;

public partial class MainMenu : Control
{
    [Export]
    public Resource optionsResource;
    LineEdit lineEdit2;
    Label label;
    Label label2;
    float mouseSens;
    int targetFps;
    public override void _Ready()
    {
        mouseSens = 0.0f;
        label = GetNode<Label>("Label3");
        label2 = GetNode<Label>("Label2");
        // if(optionsResource is OptionsResource options)
        // {
        //     options.mouseSens = lineEdit1.Text.ToFloat();
        //     options.targetFps = lineEdit2.Text.ToFloat();
        // }
    }
    public void _on_button_pressed()
    {
        if(optionsResource is OptionsResource options)
        {
            options.mouseSens = mouseSens;
            options.targetFps = targetFps;
        }
        GetTree().ChangeSceneToFile("res://scenes/World/World.tscn");
    }
    public void _on_button_2_pressed()
    {
        GetTree().Quit();
    }
    public void _on_button_3_pressed()
    {
        if(mouseSens <= 2.9f) mouseSens += 0.05f;
        label.Text = "Mouse sens:    " + mouseSens.ToString();
    }
    public void _on_button_4_pressed()
    {
        if(mouseSens >= 0.1f) mouseSens -= 0.05f;
        label.Text = "Mouse sens:    " + mouseSens.ToString();
    }

    public void _on_button_5_pressed()
    {
        if(targetFps <= 360) targetFps += 30;
        label2.Text = "Target FPS:    " + targetFps.ToString();
    }
    public void _on_button_6_pressed()
    {
        if(targetFps >= 0) targetFps -= 30;
        label2.Text = "Target FPS:    " + targetFps.ToString();
    }
    public void _on_check_button_toggled(bool toggled)
    {
        if(toggled)
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        else
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
    }
}
