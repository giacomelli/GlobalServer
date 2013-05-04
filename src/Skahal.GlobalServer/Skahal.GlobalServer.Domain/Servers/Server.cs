//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;

namespace Skahal.GlobalServer.Domain.Servers
{
	public class Server
	{
		#region Properties
		public int AvailablePlayersCount { get; set; }
		public int GamesCount { get; set; }
		public int RequestsCount { get; set; }
		public float MinDequeueMessagesWaitingSeconds { get; set; }
		public float MaxDequeueMessagesWaitingSeconds { get; set; }
		public float DecreaseDequeueMessagesWaitingSeconds { get; set; }
		public ServerState State { get; set; }
		#endregion
	}
}