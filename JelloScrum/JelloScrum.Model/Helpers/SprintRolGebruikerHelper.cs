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
    /// Deze helper helpt met gebruikers helpen aan sprintrollen. Of het verwijderen hiervan.
    /// Verder helpt dit ook met aan maken en verwijderen van sprintgebruikers als een gebruiker helemaal geen rollen meer heeft.
    /// </summary>
    public class SprintRolGebruikerHelper
    {
        private Gebruiker gebruiker;
        private SprintRol sprintRol;

        /// <summary>
        /// 
        /// </summary>
        public SprintRolGebruikerHelper()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Gebruiker Gebruiker
        {
            get { return gebruiker; }
            set { gebruiker = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual SprintRol SprintRol
        {
            get { return sprintRol; }
            set { sprintRol = value; }
        }

        /// <summary>
        /// Verwerk dit object met de gegeven sprint.
        /// </summary>
        /// <param name="sprint"></param>
        public virtual void Verwerk(Sprint sprint)
        {
            if (gebruiker == null || sprint == null)
                return;

            SprintGebruiker sprintGebruiker = sprint.GetSprintUserFor(gebruiker);

            if (sprintRol == 0)
            {
                if (sprintGebruiker == null)
                    return;
                
                sprintGebruiker.KoppelSprintGebruikerLos();
            }
            else
            {
                if (sprintGebruiker == null)
                    sprint.AddUser(gebruiker, sprintRol);
                else
                    sprintGebruiker.SprintRol = sprintRol;
            }
        }
    }
}