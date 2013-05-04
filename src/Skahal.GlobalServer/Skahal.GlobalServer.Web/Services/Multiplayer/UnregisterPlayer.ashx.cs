//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using Skahal.GlobalServer.Domain.Multiplayer;
using Skahal.GlobalServer.Helpers;
using Vici.Core.Json;

namespace Skahal.GlobalServer
{
	public class UnregisterPlayer : ServiceMethodBase<bool>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase[System.Boolean]
		protected override bool Execute (ServiceMethodContext context)
		{
			return MultiplayerService.UnregisterPlayer(context.PlayerId, true);
		}
		#endregion	
	}
}