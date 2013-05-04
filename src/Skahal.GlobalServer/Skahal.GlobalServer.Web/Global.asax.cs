//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Net.Sockets;
using System.Web;
using System.Web.SessionState;
using Skahal.GlobalServer.Domain.Servers;
using Skahal.GlobalServer.Domain.Storages;
using Skahal.GlobalServer.Infrastructure.Repositories;
using Skahal.GlobalServer.Infrastructure.Repositories.Memory;
using Skahal.GlobalServer.Infrastructure.Repositories.MySql;

namespace Skahal.GlobalServer.Web
{
	public class Global : System.Web.HttpApplication
	{
		
		protected virtual void Application_Start (Object sender, EventArgs e)
		{
		//	ServerService.Initialize(new MySqlServerRepository());
		//	StorageService.Initialize(new MySqlStorageRepository());
			ServerService.PlayerInactivityTimeoutSeconds = Convert.ToInt32( ConfigurationManager.AppSettings["PlayerInactivityTimeoutSeconds"]);
			ServerService.Initialize(new MemoryServerRepository());
			StorageService.Initialize(new MemoryStorageRepository());

		}
		
		protected virtual void Session_Start (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_BeginRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_EndRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_AuthenticateRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_Error (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Session_End (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_End (Object sender, EventArgs e)
		{
		}
	}
}

