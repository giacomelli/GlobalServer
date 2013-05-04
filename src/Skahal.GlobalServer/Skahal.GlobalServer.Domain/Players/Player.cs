//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;

namespace Skahal.GlobalServer.Domain.Players
{
	public enum PlayerDevice
	{
		iPod,
		iPhone,
		iPad,
		Mac,
		Windows,
		Android,
		Editor,
		Web
	}
	
	public enum PlayerState
	{
		Lobby,
		WaitingOpponent,
		Playing,
		ConnectionLost,
		Quit
	}
	
	public class Player
	{
		public Player (string id, string IP)
		{
			Id = id;
			this.IP = IP;
			State = PlayerState.Lobby;
			GameVersion = "1.0";
			LastActivityDate = DateTime.Now;
		}
		
		public string Id { get; private set; }
		public string Name { get; set; }
		public string IP { get; set; }
		public PlayerDevice Device { get; set; }
		public string GameVersion { get; set; }
		public PlayerState State { get; set; }
		public DateTime LastActivityDate { get; set; }
	}
}

