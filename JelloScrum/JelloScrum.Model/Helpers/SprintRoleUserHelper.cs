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

namespace JelloScrum.Model.Helpers
{
    using Entities;
    using Enumerations;

    /// <summary>
    /// This helper is used to assign (or unassign) sprintroles to users
    /// </summary>
    public class SprintRoleUserHelper
    {
        private Gebruiker user;
        private SprintRole sprintRole;
        
        /// <summary>
        /// The user
        /// </summary>
        public virtual Gebruiker User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// The sprintrole
        /// </summary>
        public virtual SprintRole SprintRole
        {
            get { return sprintRole; }
            set { sprintRole = value; }
        }

        /// <summary>
        /// Process this object with the given sprint
        /// </summary>
        /// <param name="sprint"></param>
        public virtual void Process(Sprint sprint)
        {
            if (user == null || sprint == null)
                return;

            SprintGebruiker sprintGebruiker = sprint.GetSprintUserFor(user);

            if (sprintRole == 0)
            {
                if (sprintGebruiker == null)
                    return;
                
                sprintGebruiker.KoppelSprintGebruikerLos();
            }
            else
            {
                if (sprintGebruiker == null)
                    sprint.AddUser(user, sprintRole);
                else
                    sprintGebruiker.SprintRol = sprintRole;
            }
        }
    }
}