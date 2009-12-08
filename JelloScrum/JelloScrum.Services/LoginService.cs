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
    
    public class LoginService : ILoginService
    {
        private readonly IPasswordService passwordService = IoC.Resolve<IPasswordService>();
        private readonly IUserRepository userService = IoC.Resolve<IUserRepository>();

        private string lDAPPath = string.Empty;
        private string domain = string.Empty;

        public string LDAPPath
        {
            get { return lDAPPath; }
            set { lDAPPath = value; }
        }

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        /// <summary>
        /// Geeft een gebruiker terug als deze inglogd kan worden, anders gooit hij een exception
        /// </summary>
        /// <param name="password"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public User Login(string password, string username)
        {
            User user;
            try
            {
                user = userService.ZoekOpGebruikersNaam(username);
                if (!user.IsActive)
                {
                    throw new Exception("Deze gebruiker is niet actief.");
                }
                CheckPassword(password, user);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "|Gebruikersnaam en / of wachtwoord zijn niet geldig.");
            }
            return user;
        }

        /// <summary>
        /// Controleerd het wachtwoord
        /// </summary>
        /// <param name="password"></param>
        /// <param name="user"></param>
        public void CheckPassword(string password, User user)
        {
            if (passwordService.EncryptPassword(password, user.Salt) != user.Password)
            {
                throw new Exception("Gebruikersnaam en / of wachtwoord zijn niet geldig.");
            }
        }
        private static String SearchResultProperty(SearchResult sr, string field)
        {
            if (sr.Properties[field] != null)
            {
                return (String)sr.Properties[field][0];
            }
            return null;
        }
        public User LDapGebruikerCheck(string username, string password)
        {
            string domainAndUsername = domain + @"\" + username;
            string fullName = string.Empty;

            DirectoryEntry entry = new DirectoryEntry(LDAPPath, domainAndUsername, password);
            try
            {
                // Bind to the native AdsObject to force authentication.
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                // aangeven, dat we CN willen inladen wat de fullname is van de user
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    throw new Exception("geen resultaten gevonden");
                }
                fullName = SearchResultProperty(result, "cn");
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

            User gebruiker = userService.ZoekOpGebruikersNaam(username);
            if(gebruiker == null)
            {
                gebruiker = new User(username);
                gebruiker.Name = username;
                gebruiker.FullName = fullName;
                userService.Save(gebruiker);
                return gebruiker;
            }
            return gebruiker;
        }
    }
}