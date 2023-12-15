using System.Security.Cryptography;
using System.Text;

namespace AuthenticationModels
{
    public class EncryptionService
    {

        private readonly string encryptionKey;

        // Constructor that takes an encryption key
        public EncryptionService(string key)
        {
            encryptionKey = key;
        }

        // เข้ารหัสรหัสผ่านโดยใช้วิธี AES encryption
        public string EncryptPassword(AddUserRequest data)
        {
            // แทนที่คีย์การเข้ารหัสด้วยคีย์จริงของคุณ
            string encryptionKey = "=EunN/CgBs_EUO9FNYiRR6c:QDr-AY9_";

            using var aesAlg = Aes.Create();
            using var encryptor = aesAlg.CreateEncryptor(Encoding.UTF8.GetBytes(encryptionKey), aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(data.Password);
            }

            return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
        }

        // ถอดรหัสรหัสผ่าน
        public string DecryptPassword(LoginRequest request)
        {
            // Convert the encryptedPassword from Base64 to byte array
            byte[] encryptedBytes = Convert.FromBase64String(request.Password);

            // Extract IV from the beginning of the encrypted data
            byte[] iv = encryptedBytes.Take(16).ToArray();
            byte[] encryptedData = encryptedBytes.Skip(16).ToArray();

            using var aesAlg = Aes.Create();
            aesAlg.IV = iv;

            using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(encryptedData);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            // Read the decrypted bytes from the decrypting stream and convert to string
            string decryptedPassword = srDecrypt.ReadToEnd();

            return decryptedPassword;
        }
    }
}
