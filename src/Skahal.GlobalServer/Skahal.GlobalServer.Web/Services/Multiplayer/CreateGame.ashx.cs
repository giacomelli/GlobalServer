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
	public class CreateGame : ServiceMethodBase<MultiplayerGame>
	{	
		#region implemented abstract members of Skahal.GlobalServer.ServiceHandlerBase
		protected override MultiplayerGame Execute (ServiceMethodContext context)
		{
			var game = MultiplayerService.CreateGame(context.PlayerId);
			
			// FIX: remover quando for lancada o proximo update da versao MAC (remover o P2P).
			if(game != null){
				game.GuessPlayer.IP = string.Empty;
			}
			
			return game;
		}
		#endregion
	}
}