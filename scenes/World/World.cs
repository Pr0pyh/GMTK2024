using Godot;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class World : Node3D
{
    AnimationPlayer animPlayer;
    Label label;
    Label fpsLabel;
    Timer timer;
    int score;
    float multiplier;
    [Export]
    public Resource scoreResource;
    public override void _Ready()
    {
        score = 0;
        multiplier = 1.0f;
        animPlayer = GetNode<CanvasLayer>("HighScore").GetNode<AnimationPlayer>("AnimationPlayer");
        label = GetNode<CanvasLayer>("HighScore").GetNode<Label>("Label");
        fpsLabel = GetNode<CanvasLayer>("HighScore").GetNode<Label>("Label2");
        timer = GetNode<Timer>("Timer");
    }
    public override void _PhysicsProcess(double delta)
    {
        fpsLabel.Text = "FPS: " + Engine.GetFramesPerSecond();
        if(scoreResource is Score resultScore)
        {
            if(resultScore.highscore < score)
                resultScore.highscore = score;
            if(multiplier > 1.0f)
                label.Text = "Score: \n" + resultScore.score.ToString() + " X " + multiplier + "\n" + (int)timer.TimeLeft;
            else
                label.Text = "Score: \n" + resultScore.score.ToString();
        }
    }
    public void scorePoints(int amount)
    {
        if(scoreResource is Score resultScore)
        {
            resultScore.score += (int)((float)amount*multiplier);
            if(resultScore.highscore < score)
                resultScore.highscore = resultScore.score;
            GD.Print(resultScore.score);
        }
        timer.Start();
        multiplier += 0.5f;
        animPlayer.Play("highscore");
    }
    public void _on_timer_timeout()
    {
        multiplier = 1.0f;
    }
}
