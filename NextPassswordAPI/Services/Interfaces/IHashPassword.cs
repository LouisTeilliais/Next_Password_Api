namespace NextPassswordAPI.Services.Interfaces
{
    public interface IHashPassword
    {
        public string HashPasswordWithUniqueSalt(string password, string securityStamp);
    }
}
