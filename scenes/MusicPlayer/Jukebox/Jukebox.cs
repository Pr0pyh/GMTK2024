using Godot;
using System;

public partial class Jukebox : StaticBody3D
{
	MusicPlayer musicPlayer;
    public override void _Ready()
    {
        musicPlayer = GetNode<MusicPlayer>("/root/MusicPlayer");
    }

	public void changeSong()
	{
		GD.Print(musicPlayer.musicArray.Length);
		musicPlayer.changeSong();
	}
}
