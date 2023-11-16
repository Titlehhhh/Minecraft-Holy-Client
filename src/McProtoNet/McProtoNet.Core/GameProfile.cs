using System.Runtime.Serialization;

namespace McProtoNet.Core
{

	[DataContract]
	public class GameProfile
	{
		[DataMember(Name = "id")]
		public string UUID { get; set; }
		[DataMember(Name = "name")]
		public string? Username { get; set; }

		public GameProfile(Guid uUID, string? username)
		{
			UUID = uUID.ToString();
			Username = username;
		}
	}
}
