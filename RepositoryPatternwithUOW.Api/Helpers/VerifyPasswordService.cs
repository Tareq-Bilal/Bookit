using System.Security.Cryptography;

namespace RepositoryPatternwithUOW.Api.Helpers
{
    public class VerifyPasswordService
    {
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // Extract the bytes
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);

                // Get the salt
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);

                // Compute the hash on the password the user entered
                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
                byte[] hash = pbkdf2.GetBytes(32);

                // Compare the results
                for (int i = 0; i < 32; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
