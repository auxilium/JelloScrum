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

namespace JelloScrum.Model.Services
{
    ///<summary>
    /// Interface for the password service
    ///</summary>
    public interface IPasswordService
    {
        #region Generate Password

        #region alphanumeric

        /// <summary>
        /// Generates an alphanumeric password of a random length
        /// </summary>
        /// <returns>the alphanumeric password</returns>
        string GenerateAlphanumericPassword();

        /// <summary>
        /// Generates an alphanumeric password with the given length
        /// </summary>
        /// <param name="length">length of the password being generated</param>
        /// <returns>the alphanumeric password</returns>
        string GenerateAlphanumericPassword(int length);

        #endregion

        #region phonetic

        /// <summary>
        /// Generates an phonetic password with a random length
        /// </summary>
        /// <returns>the phonetic password</returns>
        string GeneratePhoneticPassword();

        /// <summary>
        /// Generates an phonetic password with a given length
        /// </summary>
        /// <param name="passwordLength">length of the password being generated</param>
        /// <returns>the phonetic password</returns>
        string GeneratePhoneticPassword(int passwordLength);

        #endregion

        #endregion

        #region Generate Salt

        /// <summary>
        /// Generates a random length of random characters
        /// </summary>
        /// <returns>the random characters</returns>
        string GenerateSalt();

        /// <summary>
        /// Generates a random length within a given range of random characters
        /// </summary>
        /// <param name="minSaltSize">Minimum length</param>
        /// <param name="maxSaltSize">Maximum length</param>
        /// <returns>the random characters</returns>
        string GenerateSalt(int minSaltSize, int maxSaltSize);

        /// <summary>
        /// Generates a random length of random characters
        /// </summary>
        /// <param name="length">length of text</param>
        /// <returns>the random characters</returns>
        string GenerateSalt(int length);

        #endregion

        #region Encrypt

        /// <summary>
        /// MD5 Encrypt password 
        /// </summary>
        /// <param name="password">the plain text password</param>
        /// <returns>the MD5 encrypted password</returns>
        string EncryptPassword(string password);

        /// <summary>
        /// Encrypt password in combination with a salt
        /// </summary>
        /// <param name="password">the plain text password</param>
        /// <param name="salt">the salt</param>
        /// <returns>the MD5 encrypted password</returns>
        string EncryptPassword(string password, string salt);

        #endregion

        /// <summary>
        /// Generates a random positive int inside the given range default starting from 0
        /// </summary>
        /// <param name="range">The maximum number</param>
        /// <returns>the random int</returns>
        int GenerateRandomNumber(int range);
    }
}