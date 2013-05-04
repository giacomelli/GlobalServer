//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using Skahal.GlobalServer.Domain.Servers;
using System.Net.Sockets;

namespace Skahal.GlobalServer.Web
{
	public class ChangeState : ServiceMethodBase<bool>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase
		protected override bool Execute (ServiceMethodContext context)
		{		
			return ServerService.ChangeServerState(context.ServerState);
		}
		#endregion
	}	
}