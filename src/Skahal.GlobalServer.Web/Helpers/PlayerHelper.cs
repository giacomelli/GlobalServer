//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using Skahal.GlobalServer.Domain.Players;
using System.Web;

namespace Skahal.GlobalServer.Helpers
{
	public static class PlayerHelper
	{
		public static Player Parse(HttpRequest request)
		{
			var id = request["id"];
			var ip = request["IP"];
			var name = request["name"];
			var device = request["device"];
			var gameVersion = request["gameVersion"];
		
			if(id != null && ip != null && name != null && device != null && gameVersion != null)
			{
				var player = new Player(id, ip);
				player.Name = name;
				player.Device = (PlayerDevice) Enum.Parse(typeof(PlayerDevice), device);
				player.GameVersion = gameVersion;

				return player;
			}

			return null;
		}
	}
}

