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

namespace JelloScrum.Model.IRepositories
{
    using System.Collections.Generic;
    using Entities;
    using Enumerations;

    /// <summary>
    /// Interface for the user repository
    /// </summary>
    public interface IGebruikerRepository : IJelloScrumRepository<Gebruiker>
    {
        /// <summary>
        /// Find the user belonging to the given username
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <returns>The found user, else null</returns>
        Gebruiker ZoekOpGebruikersNaam(string userName);

        /// <summary>
        /// Find all users with the given systenrole
        /// </summary>
        /// <param name="role">The systemrole</param>
        /// <returns>Users that have the given systemrole.</returns>
        IList<Gebruiker> ZoekOpSysteemRol(SystemRole role);

        /// <summary>
        /// Find all users currently not in the given sprint
        /// </summary>
        /// <param name="sprint">The sprint</param>
        /// <returns>The users that are currently not in the given sprint.</returns>
        IList<Gebruiker> ZoekOpNietInSprint(Sprint sprint);
    }
}