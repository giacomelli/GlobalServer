//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using Skahal.GlobalServer.Domain;
using Skahal.GlobalServer.Domain.Players;
using Skahal.GlobalServer.Domain.Multiplayer;
using Skahal.GlobalServer.Helpers;

namespace Skahal.GlobalServer
{
	/// <summary>
	/// Register player.
	/// <remarks>RegisterPlayer.ashx?id=<ID>&name=<Name>&device<Device></remarks>
	/// </summary>
	public class RegisterPlayer : ServiceMethodBase<bool>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase[System.Boolean]
		protected override bool Execute (ServiceMethodContext context)
		{
			return MultiplayerService.RegisterPlayer(context.Player);
		}
		#endregion
	
	}
}

