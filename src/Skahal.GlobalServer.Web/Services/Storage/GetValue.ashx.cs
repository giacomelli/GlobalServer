//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using System.Net.Sockets;
using Skahal.GlobalServer.Domain.Storages;

namespace Skahal.GlobalServer.Web
{
	public class GetValue : ServiceMethodBase<string>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase
		protected override string Execute (ServiceMethodContext context)
		{
			var storage = StorageService.GetStorage(context.PlayerId, context.StorageKey);

			return storage == null ? string.Empty : storage.Value;
		}
		#endregion
	}	
}