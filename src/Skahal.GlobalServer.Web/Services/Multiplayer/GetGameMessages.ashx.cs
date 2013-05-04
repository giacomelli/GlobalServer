//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using Skahal.GlobalServer.Domain.Multiplayer;

namespace Skahal.GlobalServer
{
	public class GetGameMessages : ServiceMethodBase<IList<MultiplayerMessage>>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase[IList[MultiplayerMessage]]
		protected override IList<MultiplayerMessage> Execute (ServiceMethodContext context)
		{
			return MultiplayerService.GetGameById(context.GameId).GetAllMessages();
		}
		#endregion
		
	}
}

