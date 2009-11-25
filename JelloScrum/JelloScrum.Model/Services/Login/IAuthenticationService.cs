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
    using Castle.MonoRail.Framework;
    using Entities;

    /// <summary>
    /// Interface van de Authentication Service
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Zet het koekje naar het verleden, en verlaat de sessie.
        /// </summary>
        /// <param name="context">Gebruiker context</param>
        void SignOut(IEngineContext context);

        /// <summary>
        /// Zet het authenticatie koekje
        /// </summary>
        /// <param name="user">gebruiker die inlogt.</param>
        /// <param name="context">Gebruikers context</param>
        void SetAuthCookie(Gebruiker user, IEngineContext context);

        /// <summary>
        /// Veriveerd de gebruikers recht om in te loggen
        /// </summary>
        /// <param name="context">De gebruiker context</param>
        /// <returns>een gebruiker</returns>
        Gebruiker Authenticatie(IEngineContext context);
    }
}