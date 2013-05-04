//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;

namespace Skahal.GlobalServer.Domain.Storages
{
	/// <summary>
	/// Represents a storage.
	/// </summary>
	public class Storage
	{
		#region Properties
		/// <summary>
		/// Gets or sets the player identifier.
		/// </summary>
		/// <value>
		/// The player identifier.
		/// </value>
		public string PlayerId { get; set; }

		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>
		/// The key.
		/// </value>
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public string Value { get; set; }
		#endregion
	}
}

