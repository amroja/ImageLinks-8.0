using System.Security.Cryptography;

namespace ImageLinks_.Application.Common.Helpers
{
    public class Encryption
    {
        //Don't Edit this config plz
        private const string AES_KEY = "YNe8YwuIn1zxt3FPWTZFOr==";
        private const string AES_IV = "asxnWolsAyn7kCfWutrnqg==";
        /*For SQL CODE AND CONNECTION STRING  (EncryptString,DecryptString) Don't Edit this config plz */
        private const string AES_KEY_CON = "M7UDO9dF506xIQDJScbMxw==";
        private const string AES_IV_CON = "RBxzFN4wb0WpoL9A+pH80Q==";

        public static string EncryptAES(string plainText)
        {
            try
            {
                byte[] encrypted;

                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.Key = Convert.FromBase64String(AES_KEY);
                    aes.IV = Convert.FromBase64String(AES_IV);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }

                            encrypted = ms.ToArray();
                        }
                    }
                }

                return Convert.ToBase64String(encrypted);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public static string DecryptAES(string encryptedText)
        {
            try
            {
                string decrypted = null;
                byte[] cipher = Convert.FromBase64String(encryptedText);

                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.Key = Convert.FromBase64String(AES_KEY);
                    aes.IV = Convert.FromBase64String(AES_IV);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform dec = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(cipher))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, dec, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                decrypted = sr.ReadToEnd();
                            }
                        }
                    }
                }

                return decrypted;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public static string EncryptStringSQLCODE(string plainText)
        {

            byte[] encrypted;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = Convert.FromBase64String(AES_KEY_CON);
                aes.IV = Convert.FromBase64String(AES_IV_CON);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }

                        encrypted = ms.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);

        }
        public static string DecryptStringSQLCODE(string encryptedText)
        {
            string decrypted = null;
            byte[] cipher = Convert.FromBase64String(encryptedText);

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = Convert.FromBase64String(AES_KEY_CON);
                aes.IV = Convert.FromBase64String(AES_IV_CON);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform dec = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, dec, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
        }

    }
}
