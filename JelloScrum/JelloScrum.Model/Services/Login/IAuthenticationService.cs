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
    /// Interface for the Authentication Service
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Set cookie date to the past and exit the session.
        /// </summary>
        /// <param name="context">The context.</param>
        void SignOut(IEngineContext context);

        /// <summary>
        /// Set authentication cookie
        /// </summary>
        /// <param name="user">The logged in user.</param>
        /// <param name="context">The context</param>
        void SetAuthCookie(Gebruiker user, IEngineContext context);

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The authenticated user</returns>
        Gebruiker Authenticatie(IEngineContext context);
    }
}