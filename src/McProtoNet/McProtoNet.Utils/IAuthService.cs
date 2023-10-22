namespace McProtoNet.Utils
{
    public interface IAuthService
    {
        Task<LoginResponse> AuthAsync(AuthInfo authInfo);
    }
}