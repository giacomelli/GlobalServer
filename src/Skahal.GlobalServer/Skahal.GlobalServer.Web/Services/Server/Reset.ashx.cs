//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using Skahal.GlobalServer.Domain.Multiplayer;
using Skahal.GlobalServer.Domain.Servers;

namespace Skahal.GlobalServer
{
	public class Reset : ServiceMethodBase<object>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase[System.Object]
		protected override object Execute (ServiceMethodContext context)
		{
			MultiplayerService.Reset();
			ServerService.Reset();
			
			return null;
		}
		#endregion		
	}
}