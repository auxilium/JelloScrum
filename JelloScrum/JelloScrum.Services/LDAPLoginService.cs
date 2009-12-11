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
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.IRepositories;
    using JelloScrum.Model.Services;
    using Login.Model;
    using Login.Services;
    using Login.Wachtwoord;

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
            get { return ldapPath; }
            set { ldapPath = value; }
        }

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        /// <summary>
        /// check the password against the user
        /// </summary>
        /// <param name="password"></param>
        /// <param name="user"></param>
        private static bool CheckPassword(string password, IUser user)
        {
            return PassWordHelper.EncryptPassWord(password, user.Salt) == user.PassWord;
        }

        private static String SearchResultProperty(SearchResult sr, string field)
        {
            if (sr.Properties[field] != null)
            {
                return (String)sr.Properties[field][0];
            }
            return null;
        }
        
        private User LDapGebruikerCheck(string username, string password)
        {
            string domainAndUsername = domain + @"\" + username;
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
            catch(Exception)
            {
                return null;
            }

            User gebruiker = userService.ZoekOpGebruikersNaam(username);
            if(gebruiker == null)
            {
                gebruiker = new User();
                gebruiker.ChangeUserName(username);
                gebruiker.ChangePassWord(password);
                gebruiker.Name = username;
                gebruiker.FullName = fullName;
                userService.Save(gebruiker);
            }
            else
            {
                gebruiker.ChangePassWord(password);
                userService.Save(gebruiker);
            }
            return gebruiker;
        }

        /// <summary>
        /// Check if the username / password combination is valid for user in this application
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public bool CheckPassWord(string userName, string passWord)
        {
            return GetUser(userName, passWord) != null;
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
            User user = GetUser(userName, passWord) as User;
            return user != null && user.IsActive;
        }

        /// <summary>
        /// Retrieve the user object by it's Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetUser(long id)
        {
            return userService.Load(id) as T;
        }

        /// <summary>
        /// Retrieve the user object by the username / password combination
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public T GetUser(string userName, string passWord)
        {
            //first search the database, if null is returned, try the LDAP check
            User user = this.userService.ZoekOpGebruikersNaam(userName) ?? LDapGebruikerCheck(userName, passWord);

            if (user != null && CheckPassword(passWord, user))
                return user as T;
            return null;
        }

        /// <summary>
        /// Indicates if the username is valid and unique in this application
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsUserNameValid(string userName, T user)
        {
            User foundUser = userService.ZoekOpGebruikersNaam(userName);

            return foundUser == null || foundUser.Equals(user); 
        }
    }
}