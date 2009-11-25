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
    using Model.Entities;

    /// <summary>
    /// Interface van de loginService
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Controlleerd het wachtwoord
        /// </summary>
        /// <param name="password"></param>
        /// <param name="user"></param>
        void CheckPassword(string password, Gebruiker user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Gebruiker Login(string password, string username);

        /// <summary>
        /// Controlleerd of de gebruiker bestaat aan de hand van de LDAP
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Gebruiker LDapGebruikerCheck(string username, string password);
    }
}