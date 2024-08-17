using Godot;
using System;

public partial class EndScene : Control
{  
    [Export]
    public Resource scoreResource;
    int score;
    Label scoreBoard;
    public override void _Ready()
    {
        scoreBoard = GetNode<Label>("Label");
        if(scoreResource is Score resultScore)
        {
            GD.Print(resultScore.score);
            score = resultScore.score;
        }
        scoreBoard.Text = "Score: " + score.ToString();
    }
    public void _on_button_pressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/MainMenu/MainMenu.tscn");
    }
}
