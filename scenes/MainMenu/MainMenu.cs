using Godot;
using System;

public partial class MainMenu : Control
{
    public void _on_button_pressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/World/World.tscn");
    }
    public void _on_button_2_pressed()
    {
        GetTree().Quit();
    }
}
