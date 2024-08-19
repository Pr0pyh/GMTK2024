using Godot;
using System;

public partial class Tutorial : Control
{
	public void _on_button_pressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/MainMenu/MainMenu.tscn");
    }
}
