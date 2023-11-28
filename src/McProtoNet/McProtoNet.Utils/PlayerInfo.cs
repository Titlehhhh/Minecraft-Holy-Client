using McProtoNet.Core;
using System.Runtime.Serialization;

namespace McProtoNet.Utils
{
	[DataContract]
	public class PlayerInfo
	{

		[DataMember(Name = "online")]
		public int OnlinePlayers { get; set; }

		[DataMember(Name = "max")]
		public int MaxPlayers { get; set; }


		[DataMember(Name = "sample")]
		public GameProfile[] PlayerList { get; set; }
		public PlayerInfo()
		{
			PlayerList = new GameProfile[0];
		}
	}
}