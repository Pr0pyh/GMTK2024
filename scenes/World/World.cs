using Godot;
using System;
using System.Diagnostics;

public partial class World : Node3D
{
    AnimationPlayer animPlayer;
    Label label;
    int score;
    [Export]
    public Resource scoreResource;
    public override void _Ready()
    {
        score = 0;
        animPlayer = GetNode<CanvasLayer>("HighScore").GetNode<AnimationPlayer>("AnimationPlayer");
        label = GetNode<CanvasLayer>("HighScore").GetNode<Label>("Label");
    }
    public void scorePoints(int amount)
    {
        score += amount;
        if(scoreResource is Score resultScore)
        {
            resultScore.score = score;
            GD.Print(resultScore.score);
        }
        animPlayer.Play("highscore");
        label.Text = "Score: \n" + score.ToString();
    }
}
