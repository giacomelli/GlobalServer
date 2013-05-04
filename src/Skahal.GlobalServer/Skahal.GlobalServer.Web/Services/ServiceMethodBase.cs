//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using Skahal.GlobalServer.Domain.Servers;

namespace Skahal.GlobalServer
{
	public abstract class ServiceMethodBase<TResult> : 
		Skahal.Web.Services.ServiceMethodBase<ServiceMethodContext, TResult>
	{
		public override void ProcessRequest (System.Web.HttpContext context)
		{
			ServerService.RegisterRequest();
			base.ProcessRequest (context);
		}

		protected override TResult Execute(ServiceMethodContext context)
		{
			return default(TResult);
		}
	}
}

