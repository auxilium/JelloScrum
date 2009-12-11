namespace JelloScrum.Login.Wachtwoord
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Enum;

    /// <summary>
    /// Helper class for password handling.
    /// Use to encrypt passwords and generate salts.
    /// Can also be used to generate a phonetic password.
    /// </summary>
    public class PassWordHelper
    {
        #region Salt
        /// <summary>
        /// Generates a 6 byte salt 
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt()
        {
            return GenerateSalt(6);
        }

        /// <summary>
        /// Generates a salt of a random byte size
        /// The minimum and maximum byte sizes are specified.
        /// </summary>
        /// <param name="minSaltSize">minimum salt byte size</param>
        /// <param name="maxSaltSize">maximum salt byte size</param>
        /// <returns></returns>
        public static string GenerateSalt(int minSaltSize, int maxSaltSize)
        {
            Random random = new Random();
            return GenerateSalt(random.Next(minSaltSize, maxSaltSize));
        }

        /// <summary>
        /// Generates a salt of specified byte size
        /// </summary>
        /// <param name="size">the salt byte size</param>
        /// <returns></returns>
        public static string GenerateSalt(int size)
        {
            byte[] data = new byte[size];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }
        #endregion

        #region Encrypt
        /// <summary>
        /// Encrypts the specified password using the MD5 hash algoritm and no salt
        /// </summary>
        /// <returns></returns>
        public static string EncryptPassWord(string passWord)
        {
            return EncryptPassWord(passWord, string.Empty, HashAlgoritm.MD5);
        }

        /// <summary>
        /// Encrypts the specified password using the MD5 hash algoritm
        /// </summary>
        /// <param name="passWord"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncryptPassWord(string passWord, string salt)
        {
            return EncryptPassWord(passWord, salt, HashAlgoritm.MD5);
        }

        /// <summary>
        /// Encrypts the specified password using the specified hash algoritm
        /// </summary>
        /// <param name="passWord"></param>
        /// <param name="hashAlgoritm"></param>
        /// <returns></returns>
        public static string EncryptPassWord(string passWord, HashAlgoritm hashAlgoritm)
        {
            return EncryptPassWord(passWord, string.Empty, hashAlgoritm);
        }

        /// <summary>
        /// Encrypts the specified password using the specified hash algoritm
        /// </summary>
        /// <param name="passWord"></param>
        /// <param name="salt"></param>
        /// <param name="hashAlgoritm"></param>
        /// <returns></returns>
        public static string EncryptPassWord(string passWord, string salt, HashAlgoritm hashAlgoritm)
        {
            UTF8Encoding textConverter = new UTF8Encoding();
            HashAlgorithm hash;

            switch (hashAlgoritm)
            {
                case HashAlgoritm.SHA1:
                    hash = new SHA1Managed();
                    break;
                case HashAlgoritm.SHA256:
                    hash = new SHA256Managed();
                    break;
                case HashAlgoritm.SHA384:
                    hash = new SHA384Managed();
                    break;
                case HashAlgoritm.SHA512:
                    hash = new SHA512Managed();
                    break;
                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            string tmpPassword = string.Format("{0}_{1}", passWord, salt);
            byte[] passBytes = textConverter.GetBytes(tmpPassword);

            return Convert.ToBase64String(hash.ComputeHash(passBytes));
        }
        #endregion

        #region Alphanumeric password
        /// <summary>
        /// Generate an alphanumeric password
        /// </summary>
        /// <returns></returns>
        public static string GenerateAlphanumericPassWord()
        {
            return GenerateAlphanumericPassWord(5);
        }

        /// <summary>
        /// Generate an alphanumeric password
        /// </summary>
        /// <returns></returns>
        public static string GenerateAlphanumericPassWord(int lengte)
        {
            Random random = new Random();

            if (lengte < 5)
            {
                lengte = 5;
            }

            string password = MD5Hash(random.Next().ToString()).Substring(0, lengte);
            string newPass = "";

            random = new Random();
            for (int i = 0; i < password.Length; i++)
            {
                if (random.Next(0, 2) == 1)
                {
                    newPass += password.Substring(i, 1).ToUpper();
                }
                else
                {
                    newPass += password.Substring(i, 1);
                }
            }
            return newPass;
        }

        private static string MD5Hash(string data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
        #endregion

        #region Phonetic password
        /// <summary>
        /// Generates a phonetic password of 6 characters
        /// </summary>
        /// <returns>het wachtwoord</returns>
        public static string GeneratePhoneticPassWord()
        {
            return GeneratePhoneticPassWord(6);
        }

        /// <summary>
        /// Generates a phonetic password of specified number of characters
        /// </summary>
        /// <param name="passwordLength"></param>
        /// <returns></returns>
        public static string GeneratePhoneticPassWord(int passwordLength)
        {
            char[] vowels = "aeiou".ToCharArray();
            char[] consonants = "bcdfghjklmnprstv".ToCharArray();
            char[] doubleConsonants = "cdfglmnprst".ToCharArray();

            bool wroteConsonant = false;

            StringBuilder password = new StringBuilder();

            for (int counter = 0; counter < passwordLength; counter++)
            {
                if (password.Length > 0 && !wroteConsonant && GetRandomNumber(100) < 10)
                {
                    password.Append(doubleConsonants[GetRandomNumber(doubleConsonants.Length)], 2);
                    wroteConsonant = true;
                    counter++;
                }
                else
                {
                    if (!wroteConsonant && GetRandomNumber(100) < 90)
                    {
                        password.Append(consonants[GetRandomNumber(consonants.Length)]);
                        wroteConsonant = true;
                    }
                    else
                    {
                        password.Append(vowels[GetRandomNumber(vowels.Length)]);
                        wroteConsonant = false;
                    }
                }
            }

            return password.ToString();
        }

        /// <summary>
        /// Gets a random number within a zero based specfied bandwith
        /// </summary>
        /// <param name="bandwith"></param>
        /// <returns></returns>
        private static int GetRandomNumber(int bandwith)
        {
            RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider();
            int randomNumber;

            do
            {
                // Create a byte array to hold the random value.
                byte[] randomByteArray = new byte[4];

                // Fill the array with a random value.
                generator.GetBytes(randomByteArray);

                // Convert the byte to an integer value to make the modulus operation easier.
                int rand = BitConverter.ToInt32(randomByteArray, 0);

                randomNumber = rand % bandwith;
            } while (randomNumber < 0);
            return randomNumber;
        }

        #endregion
    }
}