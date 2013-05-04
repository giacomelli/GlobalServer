//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System.Collections.Generic;
using Skahal.GlobalServer.Domain.Multiplayer;
using Skahal.GlobalServer.Domain.Players;

namespace Skahal.GlobalServer
{
	public class GetAvailablePlayers : ServiceMethodBase<IList<Player>>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase[IList[Player]]
		protected override IList<Player> Execute (ServiceMethodContext context)
		{
			return MultiplayerService.GetPlayersInLobby();
		}
		#endregion
		
	}
}

