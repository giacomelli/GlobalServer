//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using Skahal.GlobalServer.Domain.Multiplayer;
using System.Collections.Generic;

namespace Skahal.GlobalServer
{
	public class GetGames : ServiceMethodBase<IList<MultiplayerGame>>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase
		protected override IList<MultiplayerGame> Execute (ServiceMethodContext context)
		{
			return MultiplayerService.GetAllGames();
		}
		#endregion
		
	}
}

