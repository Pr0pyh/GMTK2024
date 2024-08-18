using Godot;
using System;
using System.Diagnostics;

public partial class World : Node3D
{
    AnimationPlayer animPlayer;
    Label label;
    Label fpsLabel;
    int score;
    [Export]
    public Resource scoreResource;
    public override void _Ready()
    {
        score = 0;
        animPlayer = GetNode<CanvasLayer>("HighScore").GetNode<AnimationPlayer>("AnimationPlayer");
        label = GetNode<CanvasLayer>("HighScore").GetNode<Label>("Label");
        fpsLabel = GetNode<CanvasLayer>("HighScore").GetNode<Label>("Label2");
        Engine.MaxFps = 240;
    }
    public override void _PhysicsProcess(double delta)
    {
        fpsLabel.Text = "FPS: " + Engine.GetFramesPerSecond();
        if(scoreResource is Score resultScore)
        {
            if(resultScore.highscore < score)
                resultScore.highscore = score;
            label.Text = "Score: \n" + resultScore.score.ToString();
        }
    }
    public void scorePoints(int amount)
    {
        if(scoreResource is Score resultScore)
        {
            resultScore.score += amount;
            if(resultScore.highscore < score)
                resultScore.highscore = resultScore.score;
            GD.Print(resultScore.score);
        }
        animPlayer.Play("highscore");
    }
}
