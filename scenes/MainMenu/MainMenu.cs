using Godot;
using System;

public partial class MainMenu : Control
{
    [Export]
    public Resource optionsResource;
    LineEdit lineEdit1;
    LineEdit lineEdit2;
    public override void _Ready()
    {
        lineEdit1 = GetNode<LineEdit>("LineEdit");
        lineEdit1 = GetNode<LineEdit>("LineEdit2");
        GD.Print(lineEdit1.Text.ToFloat());
        GD.Print(lineEdit2.Text.ToFloat());
        // if(optionsResource is OptionsResource options)
        // {
        //     options.mouseSens = lineEdit1.Text.ToFloat();
        //     options.targetFps = lineEdit2.Text.ToFloat();
        // }
    }
    public void _on_button_pressed()
    {
        GD.Print(lineEdit1.Text.ToFloat());
        GD.Print(lineEdit2.Text.ToFloat());
        // if(optionsResource is OptionsResource options)
        // {
        //     options.mouseSens = lineEdit1.Text.ToInt();
        //     options.targetFps = lineEdit2.Text.ToInt();
        // }
        GetTree().ChangeSceneToFile("res://scenes/World/World.tscn");
    }
    public void _on_button_2_pressed()
    {
        GetTree().Quit();
    }
}
