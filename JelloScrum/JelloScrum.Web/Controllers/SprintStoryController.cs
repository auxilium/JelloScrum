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
    using System;
    using System.Collections.Specialized;
    using Castle.MonoRail.ActiveRecordSupport;
    using JelloScrum.Model.Enumerations;
    using Model.Entities;

    /// <summary>
    /// De controller voor sprintstories
    /// </summary>
    public class SprintStoryController : SecureController
    {
        /// <summary>
        /// Laat informatie zien over de gegeven sprintstory.
        /// </summary>
        /// <param name="sprintstory">De sprintstory.</param>
        public void Informatie([ARFetch("id")] SprintStory sprintstory)
        {
            PropertyBag.Add("item", sprintstory);
            CancelLayout();
        }

        /// <summary>
        /// Toont de status van een sprint story.
        /// </summary>
        /// <param name="sprintstory">De sprintstory.</param>
        //public void Status([ARFetch("id")] SprintStory sprintstory)
        //{
        //    PropertyBag.Add("item", sprintstory);
        //    CancelLayout();
        //}

        /// <summary>
        /// Voeg de SprintStory toe aan een SprintGebruiker waar de taken nog niet afgerond van zijn
        /// </summary>
        /// <param name="sprintStory">De sprintstory.</param>
        /// <param name="sprintGebruiker">De sprintgebruiker.</param>
        public void SaveSprintStoryToSprintGebruiker([ARFetch("sprintstory")] SprintStory sprintStory,
                                                     [ARFetch("sprintgebruiker")] SprintGebruiker sprintGebruiker)
        {
            foreach (Task task in sprintStory.Story.GetTasksWith(State.Open))
            {
                sprintGebruiker.PakTaakOp(task);
            }
            
            SprintGebruikerRepository.Save(sprintGebruiker);

            NameValueCollection args = new NameValueCollection();
            args.Add("id", sprintGebruiker.Id.ToString());
            RedirectToAction("GetSprintStoriesBySprintGebruikerOrderedByPrioriteit", args);
        }

        /// <summary>
        /// Voeg de Task toe aan een SprintGebruiker
        /// </summary>
        /// <param name="task">De task.</param>
        /// <param name="sprintGebruiker">De sprintgebruiker.</param>
        public void SaveTaskToSprintGebruiker([ARFetch("task")] Task task,
                                              [ARFetch("sprintgebruiker")] SprintGebruiker sprintGebruiker)
        {
            sprintGebruiker.PakTaakOp(task);
            SprintGebruikerRepository.Save(sprintGebruiker);

            NameValueCollection args = new NameValueCollection();
            args.Add("id", sprintGebruiker.Id.ToString());
            RedirectToAction("GetSprintStoriesBySprintGebruikerOrderedByPrioriteit", args);
        }
                
        /// <summary>
        /// Zet alle Tasks in de Story van een SprintStory op afgesloten, waardoor de Story automatisch afgerond is.
        /// </summary>
        /// <param name="sprintStory">The sprint story.</param>
        public void SaveStoryAsDone([ARFetch("sprintStory")] SprintStory sprintStory)
        {
            foreach (Task task in sprintStory.Story.Tasks)
            {
                task.Close();
                TaskRepository.Save(task);
            }
            CancelView();
            CancelLayout();
        }

        /// <summary>
        /// Gets the sprint stories by sprint gebruiker ordered by prioriteit.
        /// </summary>
        /// <param name="sprintGebruiker">The sprint gebruiker.</param>
        public void GetSprintStoriesBySprintGebruikerOrderedByPrioriteit([ARFetch("id")] SprintGebruiker sprintGebruiker)
        {
            PropertyBag.Add("item", sprintGebruiker);
            CancelLayout();
        }

        public void OpslaanPrioriteiten([ARFetch("id")] SprintStory sprintStory, int value)
        {
            sprintStory.SprintBacklogPrioriteit = (Priority)value;

            SprintStoryRepository.Save(sprintStory);

            RenderText(Enum.GetName(typeof(Priority), value));
        }
    }
}