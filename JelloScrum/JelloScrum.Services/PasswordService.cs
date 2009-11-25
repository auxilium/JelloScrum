// Copyright 2009 Auxilium B.V. - http://www.auxilium.nl/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace JelloScrum.Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using JelloScrum.Model.Services;

    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Generates an alphanumeric password of a random length
        /// </summary>
        /// <returns>the alphanumeric password</returns>
        public string GenerateAlphanumericPassword()
        {
            return GenerateAlphanumericPassword(GenerateRandomNumber(5, 10));
        }

        /// <summary>
        /// Generates an alphanumeric password with the given length
        /// </summary>
        /// <param name="length">length of the password being generated</param>
        /// <returns>the alphanumeric password</returns>
        public string GenerateAlphanumericPassword(int length)
        {
            if (length < 5)
            {
                length = 5;
            }

            string password = MD5Hash(GenerateRandomNumber(length).ToString()).Substring(0, length);
            string newPass = string.Empty;

            for (int i = 0; i < password.Length; i++)
            {
                if (GenerateRandomNumber(1) == 1)
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

        /// <summary>
        /// Generates an phonetic password with a random length
        /// </summary>
        /// <returns>the phonetic password</returns>
        public string GeneratePhoneticPassword()
        {
            return GeneratePhoneticPassword(GenerateRandomNumber(5, 10));
        }

        /// <summary>
        /// Generates an phonetic password with a given length
        /// </summary>
        /// <param name="passwordLength">length of the password being generated</param>
        /// <returns>the phonetic password</returns>
        public string GeneratePhoneticPassword(int passwordLength)
        {
            char[] vowels = "aeiou".ToCharArray();
            char[] consonants = "bcdfghjklmnprstv".ToCharArray();
            char[] doubleConsonants = "cdfglmnprst".ToCharArray();

            bool wroteConsonant = false;

            StringBuilder password = new StringBuilder();

            for (int counter = 0; counter < passwordLength; counter++)
            {
                if (password.Length > 0 && !wroteConsonant && GenerateRandomNumber(100) < 10)
                {
                    password.Append(doubleConsonants[GenerateRandomNumber(doubleConsonants.Length)], 2);
                    wroteConsonant = true;
                    counter++;
                }
                else
                {
                    if (!wroteConsonant && GenerateRandomNumber(100) < 90)
                    {
                        password.Append(consonants[GenerateRandomNumber(consonants.Length)]);
                        wroteConsonant = true;
                    }
                    else
                    {
                        password.Append(vowels[GenerateRandomNumber(vowels.Length)]);
                        wroteConsonant = false;
                    }
                }
            }

            return password.ToString();
        }

        /// <summary>
        /// Generates a random length of random characters
        /// </summary>
        /// <returns>the random characters</returns>
        public string GenerateSALT()
        {
            return GenerateSALT(GenerateRandomNumber(5, 10));
        }

        /// <summary>
        /// Generates a random length within a given range of random characters
        /// </summary>
        /// <param name="minSaltSize">Minimum length</param>
        /// <param name="maxSaltSize">Maximum length</param>
        /// <returns>the random characters</returns>
        public string GenerateSALT(int minSaltSize, int maxSaltSize)
        {
            return GenerateSALT(GenerateRandomNumber(minSaltSize, maxSaltSize));
        }

        /// <summary>
        /// Generates a random length of random characters
        /// </summary>
        /// <param name="length">length of text</param>
        /// <returns>the random characters</returns>
        public string GenerateSALT(int length)
        {
            byte[] data = new byte[length];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// MD5 Encrypt password 
        /// </summary>
        /// <param name="password">the plain text password</param>
        /// <returns>the MD5 encrypted password</returns>
        public string EncryptPassword(string password)
        {
            return EncryptPassword(password, GenerateSALT(GenerateRandomNumber(5, 10)));
        }

        /// <summary>
        /// Encrypt password in combination with a salt
        /// </summary>
        /// <param name="password">the plain text password</param>
        /// <param name="salt">the salt</param>
        /// <returns>the MD5 encrypted password</returns>
        public string EncryptPassword(string password, string salt)
        {
            UTF8Encoding textConverter = new UTF8Encoding();
            HashAlgorithm hash = new MD5CryptoServiceProvider();

            string tmpPassword = string.Format("{0}_{1}", password, salt);
            byte[] passBytes = textConverter.GetBytes(tmpPassword);

            return Convert.ToBase64String(hash.ComputeHash(passBytes));
        }

        /// <summary>
        /// Generates a random positive int inside the given range default starting from 0
        /// </summary>
        /// <param name="range">The maximum number</param>
        /// <returns>the random int</returns>
        public int GenerateRandomNumber(int range)
        {
            return GenerateRandomNumber(0, range);
        }

        public int GenerateRandomNumber(int min, int max)
        {
            if (min < 0)
            {
                min = 0;
            }

            RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider();
            int randomNumber;

            do
            {
                // Create a byte array to hold the random value.
                byte[] randomByteArray = new byte[4];

                // Fill the array with a random value.
                generator.GetNonZeroBytes(randomByteArray);

                // Convert the byte to an integer value to make the modulus operation easier.
                int rand = Math.Abs(BitConverter.ToInt32(randomByteArray, 0));

                randomNumber = rand%max + min;
            }
            while (randomNumber <= min || randomNumber >= max);

            return randomNumber;
        }

        private static string MD5Hash(string Data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
    }
}