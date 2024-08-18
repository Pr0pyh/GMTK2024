using Godot;
using System;

public partial class UpgradeVendor : StaticBody3D
{
	[Export]
	int upgradeValue;
	[Export]
	int cost;
	Label label;
    public override void _Ready()
    {
        label = GetNode<CanvasLayer>("CanvasLayer").GetNode<Label>("Label");
		label.Text = "Punch to upgrade.\nCost: " + cost.ToString();
		label.Visible = false;
    }
    
	public void upgrade(Fists fists, int pay)
	{
		if(cost > pay) return;
		fists.upgrade(upgradeValue, cost);
		cost += 20;
		label.Text = "Punch to upgrade.\nCost: " + cost.ToString();
	}
	public void _on_trigger_area_body_entered(Node3D body)
	{
		if(body is Player player) label.Visible = true;
	}
	public void _on_trigger_area_body_exited(Node3D body)
	{
		if(body is Player player) label.Visible = false;
	}
}
