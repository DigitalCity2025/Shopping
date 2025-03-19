namespace Shopping.API.Security
{
    public interface ITokenManager
    {
        string CreateToken(int id, string email, string role);
        int ValidateTokenWithoutLifeTime(string token);
    }
}