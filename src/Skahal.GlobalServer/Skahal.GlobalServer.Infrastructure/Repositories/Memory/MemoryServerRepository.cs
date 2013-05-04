using System;
using Skahal.GlobalServer.Domain.Servers;

namespace Skahal.GlobalServer.Infrastructure.Repositories.Memory
{
	public class MemoryServerRepository : IServerRepository
	{
		#region Fields
		private ServerStatistics m_statistics = new ServerStatistics();
		#endregion

		#region IServerRepository implementation

		public void SaveStatistics (ServerStatistics statistics)
		{
			m_statistics = statistics;
		}

		public ServerStatistics FindStatistics ()
		{
			return m_statistics;
		}
		#endregion
	}
}

