//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using Skahal.GlobalServer.Domain;
using System.Web;
using Skahal.GlobalServer.Domain.Multiplayer;

namespace Skahal.GlobalServer
{
	public static class MultiplayerMessageHelper
	{
		public static MultiplayerMessage Parse(HttpRequest request)
		{
			var message = new MultiplayerMessage();
			message.PlayerId = request["PlayerId"];
			message.Name = request["MessageName"];
			message.Value = request["Messagevalue"];
			
			return message;
		}
	}
}

