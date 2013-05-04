//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using Skahal.GlobalServer.Domain.Servers;
using System.Net.Sockets;

namespace Skahal.GlobalServer.Web
{
	public class GetServer : ServiceMethodBase<Server>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase
		protected override Server Execute (ServiceMethodContext context)
		{		
			return ServerService.GetServer(context.Player);
		}
		#endregion
	}	
}