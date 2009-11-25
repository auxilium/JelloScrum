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

namespace JelloScrum.Web.Controllers
{
    using Castle.MonoRail.ActiveRecordSupport;
    using JelloScrum.Model.Entities;

    /// <summary>
    /// Controller die dingen met gebruikers doet. 
    /// Voor Het beheer van gebruikers hebben we de gebruikerbeheercontroller. (waarom weet ik niet)
    /// </summary>
    public class GebruikerController : SecureController
    {
        /// <summary>
        /// Selecteer deze sprint als de active sprint van de in de sessie actieve gebruiker
        /// </summary>
        /// <param name="sprint">De sprint.</param>
        public void SelecteerSprint([ARFetch("sprintId")] Sprint sprint)
        {
            if (sprint == null)
            {
                AddErrorMessageToPropertyBag("Geen geldige sprint geselecteerd.");
                RedirectToReferrer();
            }

            CurrentUser.ActieveSprint = sprint;

            GebruikerRepository.Save(CurrentUser);
            
            RedirectToReferrer();
        }
    }
}