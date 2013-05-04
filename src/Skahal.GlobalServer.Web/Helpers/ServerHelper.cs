//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2013 Skahal Studios
using System;
using Skahal.GlobalServer.Domain.Servers;
using System.Globalization;
using System.Web;

namespace Skahal.GlobalServer.Helpers
{
	public static class ServerHelper
	{
		public static ServerState ParseServerState(HttpRequest request)
		{
			var state = request["serverState"];
			
			if(state != null)
			{
				try
				{
					return (ServerState) Enum.Parse(typeof(ServerState), state);
				}
				catch(InvalidCastException ex)
				{
					var msg = String.Format(CultureInfo.InvariantCulture, "Was not possible cast the serverState argument with value '{0}' to any ServerState enum value.", state);
					throw new InvalidOperationException(msg, ex);
				}
			}

			throw new InvalidOperationException("serverState argument was not found on request.");
		}
	}
}

