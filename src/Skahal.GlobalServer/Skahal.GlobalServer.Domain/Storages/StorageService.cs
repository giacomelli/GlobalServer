//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;

namespace Skahal.GlobalServer.Domain.Storages
{
	/// <summary>
	/// A domain layer storage service.
	/// </summary>
	public static class StorageService
	{
		#region Fields
		private static IStorageRepository s_repository;
		#endregion

		#region Methods
		public static void Initialize(IStorageRepository repository)
		{
			s_repository = repository;
		}

		public static Storage GetStorage(string playerId, string key)
		{
			return s_repository.FindStorage(playerId, key); 
		}

		public static void SaveStorage(Storage storage)
		{
			var oldStorage = s_repository.FindStorage(storage.PlayerId, storage.Key);

			if(oldStorage == null)
			{
				s_repository.InsertStorage(storage);
			}
			else
			{
				s_repository.UpdateStorage(storage);
			}
		}
		#endregion
	}
}

