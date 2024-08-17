using Godot;
using System;

public partial class EndScene : Control
{
    public void _on_button_pressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/MainMenu/MainMenu.tscn");
    }
}
