namespace McProtoNet.Utils
{
    public struct AuthInfo
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public AuthInfo(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}