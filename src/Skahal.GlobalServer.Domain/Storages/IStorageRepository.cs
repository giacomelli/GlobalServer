//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;

namespace Skahal.GlobalServer.Domain.Storages
{
	/// <summary>
	/// Defines a interface for a storage repository.
	/// </summary>
	public interface IStorageRepository
	{
		/// <summary>
		/// Inserts the storage.
		/// </summary>
		/// <param name='storage'>
		/// Storage.
		/// </param>
		void InsertStorage(Storage storage);

		/// <summary>
		/// Updates the storage.
		/// </summary>
		/// <param name='storage'>
		/// Storage.
		/// </param>
		void UpdateStorage(Storage storage);

		/// <summary>
		/// Finds the storage.
		/// </summary>
		/// <returns>
		/// The storage.
		/// </returns>
		/// <param name='playerId'>
		/// Player identifier.
		/// </param>
		/// <param name='key'>
		/// Key.
		/// </param>
		Storage FindStorage(string playerId, string key);
	}
}

