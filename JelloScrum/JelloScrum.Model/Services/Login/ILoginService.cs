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
    using Entities;

    /// <summary>
    /// Interface for the loginService
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Check the given password
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="user">The user.</param>
        void CheckPassword(string password, User user);

        /// <summary>
        /// Log the specified user in with the given password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="username">The username.</param>
        /// <returns>The logged in user</returns>
        User Login(string password, string username);

        /// <summary>
        /// Check LDAP if the given user / password combo exists.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The user</returns>
        User LDapGebruikerCheck(string username, string password);
    }
}