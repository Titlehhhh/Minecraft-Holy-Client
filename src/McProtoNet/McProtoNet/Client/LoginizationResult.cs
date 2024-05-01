namespace McProtoNet.Client
{
	public readonly struct LoginizationResult
	{
		public readonly Stream TransportStream;
		public readonly int CompressionThreshold;

		public LoginizationResult(Stream transportStream, int compressionThreshold)
		{
			TransportStream = transportStream;
			CompressionThreshold = compressionThreshold;
		}
	}
}
