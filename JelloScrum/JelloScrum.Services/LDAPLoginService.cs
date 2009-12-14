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
    using System.DirectoryServices;
    using Container;
    using Login.Model;
    using Login.Services;
    using Login.Wachtwoord;
    using Model.Entities;
    using Model.IRepositories;

    /// <summary>
    /// Generic Service for logging on in a LDAP environment
    /// Persists user information to the database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LDAPLoginService<T> : ILoginService<T> where T : class, IUser
    {
        private readonly IUserRepository userService = IoC.Resolve<IUserRepository>();

        private string ldapPath = string.Empty;
        private string domain = string.Empty;

        public string LDAPPath
        {
            get { return this.ldapPath; }
            set { this.ldapPath = value; }
        }

        public string Domain
        {
            get { return this.domain; }
            set { this.domain = value; }
        }

        /// <summary>
        /// check if the encrypted version of password is equal to the user's password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="user"></param>
        private static bool CheckPasswordEncryption(string password, IUser user)
        {
            return PassWordHelper.EncryptPassWord(password, user.Salt) == user.PassWord;
        }

        private static String SearchResultProperty(SearchResult sr, string field)
        {
            if (sr.Properties[field] != null)
            {
                return (String) sr.Properties[field][0];
            }
            return null;
        }

        private User LDAPAccountCheck(string username, string password)
        {
            string domainAndUsername = this.domain + @"\" + username;
            string fullName;

            DirectoryEntry entry = new DirectoryEntry(LDAPPath, domainAndUsername, password);
            try
            {
                // Bind to the native AdsObject to force authentication.
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                // indicate we want to load CN and search for fullname
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    throw new Exception("geen resultaten gevonden");
                }
                fullName = SearchResultProperty(result, "cn");
            }
            catch (Exception)
            {
                return null;
            }

            User user = this.userService.ZoekOpGebruikersNaam(username);
            if (user == null)
            {
                user = new User();
                user.ChangeUserName(username);
                user.ChangePassWord(password);
                user.Name = username;
                user.FullName = fullName;
                this.userService.Save(user);
            }
            else if (!CheckPasswordEncryption(password, user))
            {
                user.ChangePassWord(password);
                this.userService.Save(user);
            }
            return user;
        }

        /// <summary>
        /// Check if the username / password combination is valid for user in this application
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public bool CheckPassWord(string userName, string passWord)
        {
            return LDAPAccountCheck(userName, passWord) != null;
        }

        /// <summary>
        /// Checks if the username / password combination is valid for a 
        /// user in this application and if the user is still allowed to login
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public bool IsAllowedToLogin(string userName, string passWord)
        {
            User user = LDAPAccountCheck(userName, passWord);
            return user != null && user.IsActive;
        }

        /// <summary>
        /// Retrieve the user object by it's Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetUser(long id)
        {
            return this.userService.Load(id) as T;
        }

        /// <summary>
        /// Retrieve the user object by the username / password combination
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public T GetUser(string userName, string passWord)
        {
            //first search the database, if a user with that password exists
            User user = this.userService.ZoekOpGebruikersNaam(userName);
            if (CheckPasswordEncryption(passWord, user))
            {
                return user as T;
            }
            //an invalid password was entered, let's check if the password has changed using LDAP check
            return LDAPAccountCheck(userName, passWord) as T;
        }

        /// <summary>
        /// Indicates if the username is valid and unique in this application
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsUserNameValid(string userName, T user)
        {
            User foundUser = this.userService.ZoekOpGebruikersNaam(userName);

            return foundUser == null || foundUser.Equals(user);
        }
    }
}