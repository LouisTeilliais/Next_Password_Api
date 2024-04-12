namespace NextPassswordAPI.Services.Interfaces
{
    public interface IHashPasswordService
    {
        public string EncryptPassword(string password, string token);

        public string GenerateToken(int length = 16);

        public string DecryptPassword(string encryptedPassword, string token);
    }
}
