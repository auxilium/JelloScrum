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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Castle.MonoRail.ActiveRecordSupport;
    using Model.Entities;
    using Model.Enumerations;

    /// <summary>
    /// De controller voor het taken overzicht
    /// </summary>
    public class TakenOverzichtController : SecureController
    {
        #region Mijn taken
        /// <summary>
        /// Overzicht van eigen opgepakte taken uit de actieve sprint
        /// </summary>
        public void Mijntaken([ARFetch("sprintId")] Sprint sprint)
        {
            SprintGebruiker sprintGebruiker = sprint.GeefSprintGebruikerVoor(CurrentUser);

            PropertyBag.Add("taken", sprintGebruiker.GeefOpgepakteTaken());
            PropertyBag.Add("sprint", sprint);
            CancelLayout();
        }

        /// <summary>
        /// Sprintgebruiker actieve sprint geeft een taak af
        /// </summary>
        /// <param name="taak"></param>
        /// <param name="sprint"></param>
        public void TaakAfgeven([ARFetch("id")] Task taak, [ARFetch("sprintId")] Sprint sprint)
        {
            try
            {
                SprintGebruiker sprintGebruiker = sprint.GeefSprintGebruikerVoor(CurrentUser);
                sprintGebruiker.GeefTaakAf(taak);
                SprintGebruikerRepository.Save(sprintGebruiker);
                PropertyBag.Add("taken", sprintGebruiker.GeefOpgepakteTaken());
            }
            catch
            {
                AddErrorMessageToPropertyBag("Het afgeven van taak nr: " + taak.Id + " is niet gelukt.");
            }
            PropertyBag.Add("sprint", sprint);
        }

        /// <summary>
        /// Sprintgebruiker actieve sprint sluit een taak af
        /// </summary>
        /// <param name="taak"></param>
        /// <param name="sprint"></param>
        public void TaakAfsluiten([ARFetch("id")] Task taak, [ARFetch("sprintId")] Sprint sprint)
        {
            try
            {
                SprintGebruiker sprintGebruiker = sprint.GeefSprintGebruikerVoor(CurrentUser);
                taak.SluitTaak();
                TaskRepository.Save(taak);
                PropertyBag.Add("taken", sprintGebruiker.GeefOpgepakteTaken());
            }
            catch
            {
                AddErrorMessageToPropertyBag("Het afsluiten van taak nr: " + taak.Id + " is niet gelukt.");
            }
            PropertyBag.Add("sprint", sprint);
        }
        #endregion

        #region Open taken
        /// <summary>
        /// Alle nog niet opgepakte taken van de actieve sprint
        /// </summary>
        public void Nietopgepaktetaken([ARFetch("sprintId")] Sprint sprint)
        {
            IList<Task> taken = sprint.GeefAlleTakenVanSprint(Status.NietOpgepakt);

            PropertyBag.Add("sprint", sprint);
            PropertyBag.Add("taken", taken);

            CancelLayout();
        }

        /// <summary>
        /// Sprintgebruiker actieve sprint pakt een taak op
        /// </summary>
        /// <param name="taak"></param>
        /// <param name="sprint"></param>
        public void TaakOppakken([ARFetch("id")] Task taak, [ARFetch("sprintId")] Sprint sprint)
        {
            SprintGebruiker sprintGebruiker = sprint.GeefSprintGebruikerVoor(CurrentUser);

            if (sprintGebruiker != null)
            {
                try
                {
                    sprintGebruiker.PakTaakOp(taak);
                    SprintGebruikerRepository.Save(sprintGebruiker);
                }
                catch
                {
                    AddErrorMessageToPropertyBag("Het oppakken van taak nr: " + taak.Id + " is niet gelukt.");
                }
            }
            else
            {
                //Hier proberen nieuwe sprint gebruiker te maken.
                sprintGebruiker = sprint.VoegGebruikerToe(CurrentUser, SprintRol.Developer);
                try
                {
                    sprintGebruiker.PakTaakOp(taak);
                    SprintGebruikerRepository.Save(sprintGebruiker);
                }
                catch
                {
                    AddErrorMessageToPropertyBag("Het oppakken van taak nr: " + taak.Id + " is niet gelukt.");
                }
            }
            NameValueCollection args = new NameValueCollection();
            args.Add("sprintId", sprint.Id.ToString());
            RedirectToAction("Nietopgepaktetaken", args);
        }
        #endregion

        #region Opgepakte taken
        /// <summary>
        /// Alle door andere opgepakte taken uit de actieve sprint
        /// </summary>
        public void Andermanstaken([ARFetch("sprintId")] Sprint sprint)
        {
            IList<Task> taken = sprint.GeefAlleTakenVanSprint(Status.Opgepakt);

            PropertyBag.Add("sprint", sprint);
            PropertyBag.Add("taken", taken);

            CancelLayout();
        }
        #endregion

        #region Afgeronde taken
        /// <summary>
        /// Alle afgeronde taken uit de actieve sprint
        /// </summary>
        public void Afgerondetaken([ARFetch("sprintId")] Sprint sprint)
        {
            IList<Task> taken = sprint.GeefAlleTakenVanSprint(Status.Afgesloten);

            PropertyBag.Add("sprint", sprint);
            PropertyBag.Add("taken", taken);

            CancelLayout();
        }

        /// <summary>
        /// Sprintgebruiker actieve sprint pakt een reeds gesloten taak weer op
        /// </summary>
        /// <param name="taak"></param>
        public void TaakHeropenen([ARFetch("id")] Task taak, [ARFetch("sprintId")] Sprint sprint)
        {
            try
            {
                SprintGebruiker sprintGebruiker = sprint.GeefSprintGebruikerVoor(CurrentUser);
                sprintGebruiker.PakTaakOp(taak);
                SprintGebruikerRepository.Save(sprintGebruiker);
            }
            catch
            {
                AddErrorMessageToPropertyBag("Het heropenen van taak nr: " + taak.Id + " is niet gelukt.");
            }
            PropertyBag.Add("sprint", sprint);
            RedirectToAction("Afgerondetaken");
        }
        #endregion
    }
}