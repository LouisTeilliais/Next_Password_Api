using NextPassswordAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace NextPassswordAPI.Services
{
    public class HashPasswordService : IHashPassword
    {

        /// <summary>
        /// Hash Password with Security Stamp
        /// </summary>
        /// <param name="password"></param>
        /// <param name="securityStamp"></param>
        /// <returns></returns>
        public string HashPasswordWithUniqueSalt(string password, string securityStamp)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Générer un sel unique
                string uniqueSalt = Guid.NewGuid().ToString();

                // Combinez le mot de passe avec le security stamp et le sel unique
                string combinedString = String.Concat(password, securityStamp, uniqueSalt);

                // Convertit la chaîne combinée en tableau de bytes
                byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combinedString));

                // Crée une chaîne hexadécimale à partir des bytes hachés
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Verify Password
        /// </summary>
        /// <param name="enteredPassword"></param>
        /// <param name="storedHash"></param>
        /// <param name="securityStamp"></param>
        /// <returns></returns>
        public bool VerifyPassword(string enteredPassword, string storedHash, string securityStamp)
        {
            // Récupérer le sel utilisé lors du hachage
            int saltLength = Guid.NewGuid().ToString().Length; // Longueur du sel généré par Guid.NewGuid()
            string salt = storedHash.Substring(storedHash.Length - saltLength);

            // Recalculer le hachage du mot de passe fourni avec le même sel et le même security stamp
            string hashedEnteredPassword = HashPasswordWithUniqueSalt(enteredPassword, securityStamp + salt);

            // Comparer le hachage calculé avec le hachage stocké
            return string.Equals(storedHash, hashedEnteredPassword);
        }


    }
}
