namespace McProtoNet.Utils
{
    public class LoginResponse
    {
        public string UUID { get; private set; }
        public string SessionId { get; private set; }
        public LoginResult Result { get; private set; }
        public string Nick { get; private set; }

        public LoginResponse(string uUID, LoginResult result, string nick, string sessionId)
        {
            UUID = uUID;
            Result = result;
            Nick = nick;
            SessionId = sessionId;
        }
    }
}