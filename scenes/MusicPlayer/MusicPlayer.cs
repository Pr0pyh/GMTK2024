using Godot;
using System;

public partial class MusicPlayer : AudioStreamPlayer
{
	[Export]
	public AudioStream[] musicArray;
	public int i = 0;
	public override void _Ready()
	{
		if(musicArray[i] != null)
		{
			Stream = musicArray[i];
		}
		Play();
	}
	public void changeSong()
	{
		i++;
		if(i >= musicArray.Length) 
			i = 0;
		Stop();
		Stream = musicArray[i];
		Play(); 
	}
}
