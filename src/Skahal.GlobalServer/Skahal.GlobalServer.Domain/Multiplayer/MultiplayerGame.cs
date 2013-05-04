//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios

#region Usings
using System;
using System.Collections.Generic;
using Skahal.GlobalServer.Domain.Players;
using System.Linq;
#endregion

namespace Skahal.GlobalServer.Domain.Multiplayer
{
	#region Enum
	/// <summary>
	/// Game communication.
	/// </summary>
	public enum GameCommunicationType
	{
		/// <summary>
		/// A peer-to-peer communication.
		/// </summary>
		P2P,
		
		/// <summary>
		/// The clients does not know the other clientes, all the communication is done by the server.
		/// </summary>
		AuthoritativeServer
	}
	#endregion
	
	/// <summary>
	/// A multiplayer game between two players.
	/// </summary>
	public class MultiplayerGame
	{
		#region Events
		/// <summary>
		/// Occurs when game ended.
		/// </summary>
		public event EventHandler Ended;
		#endregion

		#region Fields
		private Dictionary<string, List<MultiplayerMessage>> m_playersMessages = new Dictionary<string, List<MultiplayerMessage>>();
		private bool m_gameEndedAlreadyRaised;
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		public string Id { get; private set; }
		
		/// <summary>
		/// Gets the start date.
		/// </summary>
		public DateTime StartDate { get; private set; }
		
		/// <summary>
		/// Gets the host player.
		/// </summary>
		public Player HostPlayer { get; private set; }
		
		/// <summary>
		/// Gets the guess player.
		/// </summary>
		public Player GuessPlayer{ get; private set; }
		
		/// <summary>
		/// Gets the type of the communication.
		/// </summary>
		public GameCommunicationType CommunicationType { get; private set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Skahal.GlobalServer.Domain.Multiplayer.MultiplayerGame"/> class.
		/// </summary>
		/// <param name='hostPlayer'>
		/// Host player.
		/// </param>
		/// <param name='guessPlayer'>
		/// Guess player.
		/// </param>
		public MultiplayerGame (Player hostPlayer, Player guessPlayer)
		{
			Id = Guid.NewGuid().ToString();
			StartDate = DateTime.Now;
			
			HostPlayer = hostPlayer;
			GuessPlayer = guessPlayer;
			
			HostPlayer.State = PlayerState.Playing;
			GuessPlayer.State = PlayerState.Playing;
			
			m_playersMessages.Add(hostPlayer.Id, new List<MultiplayerMessage>());
			m_playersMessages.Add(guessPlayer.Id, new List<MultiplayerMessage>());
			
			EnqueueEnemyConnectedMessage(HostPlayer, GuessPlayer);
			EnqueueEnemyConnectedMessage(GuessPlayer, HostPlayer);
			
			CommunicationType = 
				!String.IsNullOrEmpty(HostPlayer.IP) && !String.IsNullOrEmpty(GuessPlayer.IP)
					? GameCommunicationType.P2P
					: GameCommunicationType.AuthoritativeServer;
			
		}
		#endregion
		
		#region Methods
		public Player GetPlayer(string playerId)
		{
			Player player = null;

			if(HostPlayer.Id.Equals(playerId, StringComparison.OrdinalIgnoreCase))
			{
				player = HostPlayer;
			}
			else if(GuessPlayer.Id.Equals(playerId, StringComparison.OrdinalIgnoreCase))
			{
				player = GuessPlayer;
			}

			return player;
		}

		public Player GetOtherPlayer(string playerId)
		{
			Player player = null;
			
			if(HostPlayer.Id.Equals(playerId, StringComparison.OrdinalIgnoreCase))
			{
				player = GuessPlayer;
			}
			else if(GuessPlayer.Id.Equals(playerId, StringComparison.OrdinalIgnoreCase))
			{
				player = HostPlayer;
			}
			
			return player;
		}

		private void EnqueueEnemyConnectedMessage(Player player, Player enemyPlayer)
		{
			var msg = new MultiplayerMessage();
			msg.GameId = Id;
			msg.PlayerId = player.Id;
			msg.Name = "GameCreated";
			msg.Value = enemyPlayer;
			
			EnqueueMessage(msg);
		}
		
		public void EnqueueMessage(MultiplayerMessage message)
		{
			var playerId = message.PlayerId;
			var player = m_playersMessages[playerId];	
			player.Add(message);

			if(!m_gameEndedAlreadyRaised && message.Name.Equals("OnEnemyGameOver", StringComparison.InvariantCultureIgnoreCase))
			{
				m_gameEndedAlreadyRaised = true;

				if(Ended != null)
				{
					Ended(this, EventArgs.Empty);
				}
			}
		}
	
		public IList<MultiplayerMessage> DequeueMessages(string playerId)
		{
			UpdateLastActivityDate (playerId);
			
			return m_playersMessages[playerId];
		}
		
		private void UpdateLastActivityDate (string playerId)
		{
			if (HostPlayer.Id.Equals (playerId, StringComparison.OrdinalIgnoreCase))
			{
				HostPlayer.LastActivityDate = DateTime.Now;
			}
			else
			{
				GuessPlayer.LastActivityDate = DateTime.Now;
			}
		}	
		
		public bool MarkMessageAsReceived(string playerId, int messageId)
		{
			var queue = m_playersMessages[playerId];
			
			return queue.RemoveAll((m) => {
				return m.Id <= messageId;
			}) > 0;
		}
		
		public IList<MultiplayerMessage> GetPlayerMessages(string playerId)
		{
			return m_playersMessages[playerId].ToList();
		}
		
		public IList<MultiplayerMessage> GetAllMessages()
		{
			return m_playersMessages[HostPlayer.Id].Concat(m_playersMessages[GuessPlayer.Id]).ToList();
		}
		
		public override int GetHashCode ()
		{
			return Id.GetHashCode();
		}
		
		public override bool Equals (object obj)
		{
			var g = obj as MultiplayerGame;
			
			if(g != null)
			{
				return Id.Equals(g.Id);
			}
			
			return false;
		}
		#endregion
	}
}