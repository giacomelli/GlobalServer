//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using Skahal.GlobalServer.Domain.Multiplayer;
using Vici.Core.Json;

namespace Skahal.GlobalServer
{
	public class EnqueueMessage : ServiceMethodBase<object>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase[System.Void*]
		protected override object Execute (ServiceMethodContext context)
		{
			MultiplayerService.EnqueueMessage(context.Message);
			
			return null;
		}
		#endregion
		
		
	}
}

