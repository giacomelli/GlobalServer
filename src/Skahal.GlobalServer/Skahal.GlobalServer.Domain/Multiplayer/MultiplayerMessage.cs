//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;

namespace Skahal.GlobalServer.Domain.Multiplayer
{
	public class MultiplayerMessage
	{
		#region Fields
		private static object s_lock = new object();
		private static int s_lastId;
		#endregion
		
		#region Constructors
		public MultiplayerMessage()
		{
			lock(s_lock)
			{
				Id = ++s_lastId;
			}
		}
		#endregion
		
		#region Properties
		public int Id { get; private set; }
		public string GameId { get; set; }
		public string PlayerId { get; set; }
		public string Name { get; set; }
		public object Value { get; set; }
		#endregion
	}
}