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

namespace JelloScrum.Web.Helpers
{
    using System.DirectoryServices;
    using System;

    public static class LDAPAuthenticationHelper
    {
        private static string Username;
        private static string _path = "LDAP://bert/DC=vlaming,DC=auxilium-sd,DC=nl";

        /// <summary>
        /// Determines whether the specified domain is authenticated.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="username">The username.</param>
        /// <param name="pwd">The PWD.</param>
        /// <returns>
        /// 	<c>true</c> if the specified domain is authenticated; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAuthenticated(string domain, string username, string pwd)
        {
            string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
            try
            {
                // Bind to the native AdsObject to force authentication.
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                // aangeven, dat we CN willen inladen wat de fullname is van de user
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                
                // Update the new path to the user in the directory
                //path = result.Path;

                // Set Username
                Username = SearchResultProperty(result, "cn");
            }
            catch 
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Searches the result property.
        /// </summary>
        /// <param name="sr">The sr.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        private static String SearchResultProperty(SearchResult sr, string field)
        {
            if (sr.Properties[field] != null)
            {
                return (String)sr.Properties[field][0];
            }
            return null;
        }
        public static string FullName
        {
            get { return Username; }
        }
    }
}
