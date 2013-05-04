//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using Skahal.GlobalServer.Domain.Players;
using Skahal.GlobalServer.Domain.Multiplayer;

namespace Skahal.GlobalServer.Web
{
	public class GetAllPlayers : ServiceMethodBase<IList<Player>>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase
		protected override IList<Player> Execute (ServiceMethodContext context)
		{
			return MultiplayerService.GetAllPlayers();
		}
		#endregion
		
	}
}

