//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using Skahal.GlobalServer.Domain.Multiplayer;
using Skahal.GlobalServer.Helpers;
using Skahal.GlobalServer.Domain.Players;
using Skahal.Web.Services;
using Skahal.GlobalServer.Domain.Servers;

namespace Skahal.GlobalServer
{
	public sealed class ServiceMethodContext : ServiceMethodContextBase
	{
		#region Properties
		public string PlayerId 
		{
			get
			{
				return Request["playerId"];
			}
		}
		
		public string GameId 
		{
			get
			{
				return Request["gameId"];
			}
		}
		
		public int LastDequeuedMessageId 
		{
			get
			{
				return Convert.ToInt32(Request["lastDequeuedMessageId"]);
			}
		}
		
		public MultiplayerMessage Message 
		{
			get
			{
				return MultiplayerMessageHelper.Parse(Request);
			}
		}
		
		public Player Player 
		{
			get
			{
				return PlayerHelper.Parse(Request);
			}
		}

		public string StorageKey 
		{
			get
			{
				return Request["key"];
			}
		}

		public string StorageValue
		{
			get
			{
				return Request["value"];
			}
		}

		public ServerState ServerState 
		{
			get
			{
				return ServerHelper.ParseServerState(Request);
			}
		}
		#endregion
	}
}