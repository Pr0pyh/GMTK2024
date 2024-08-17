using Godot;
using System;

public partial class World : Node3D
{
    AnimationPlayer animPlayer;
    Label label;
    int score;
    public override void _Ready()
    {
        score = 0;
        animPlayer = GetNode<CanvasLayer>("HighScore").GetNode<AnimationPlayer>("AnimationPlayer");
        label = GetNode<CanvasLayer>("HighScore").GetNode<Label>("Label");
    }
    public void scorePoints(int amount)
    {
        score += amount;
        animPlayer.Play("highscore");
        label.Text = "Score: \n" + score.ToString();
    }
}
