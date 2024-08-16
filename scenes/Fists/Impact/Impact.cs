using Godot;
using System;

public partial class Impact : Sprite3D
{
    AnimationPlayer animPlayer;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("impact");
    }

    public void _on_animation_player_animation_finished(String animName)
    {
        if(animName == "impact")
            QueueFree();
    }
}
