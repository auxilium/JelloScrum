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
    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using JelloScrum.Model.Entities;

    /// <summary>
    /// Controller voor alles mbt taken
    /// </summary>
    public class TaskController : SecureController
    {
        ///<summary>
        /// Geeft een overzicht van alle taken van de gegeven story.
        ///</summary>
        public void Overzicht([ARFetch("storyId")] Story story)
        {
            PropertyBag.Add("tasks", story.Tasks);
            CancelLayout();
        }

        ///<summary>
        /// Geeft een overzicht van alle taken van de gegeven story.
        ///</summary>
        public void OverzichtBekijken([ARFetch("storyId")] Story story)
        {
            PropertyBag.Add("tasks", story.Tasks);
            CancelLayout();
        }


        /// <summary>
        /// Nieuws this instance.
        /// </summary>
        public void Nieuw()
        {
            CancelLayout();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        public void Task(int count)
        {
            PropertyBag.Add("count", count + 1);
            PropertyBag.Add("task", new Task());
            CancelLayout();
        }


        /////<summary>
        ///// Maak een nieuwe taak voor de gegeven story.
        /////</summary>
        //public void Nieuw([ARFetch("storyId")] Story story)
        //{
        //    Task task = new Task();
        //    story.VoegTaskToe(task);

        //    PropertyBag.Add("item", task);
        //    RenderView("bewerk");
        //    CancelLayout();
        //}

        /// <summary>
        /// Bewerk een bestaande taak.
        /// </summary>
        /// <param name="task">De taak.</param>
        public void Bewerk([ARFetch("id")] Task task)
        {
            PropertyBag.Add("item", task);
            CancelLayout();
        }

        /// <summary>
        /// Opslaan van de taak in de repository.
        /// </summary>
        /// <param name="task">Taak instantie die moet worden toegevoegd.</param>
        public void Opslaan([ARDataBind("item", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey)] Task task)
        {
            try
            {
                TaskRepository.Save(task);
                RedirectToAction("Overzicht", "id=" + task.Story.Id);
            }
            catch (Exception)
            {
                AddErrorMessageToFlashBag("Het opslaan van taak " + task.Omschrijving + " is niet gelukt.");
                RedirectToAction("Overzicht", "id=" + task.Story.Id);
                return;
            }
            CancelView();
            CancelLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="omschrijving"></param>
        /// <param name="story"></param>
        public void Opslaan(int taskId, string omschrijving, [ARFetch("storyId")] Story story)
        {
            Task task;
            if (taskId == 0)
            {
                task = new Task(story);
            }
            else
            {
                task = TaskRepository.Load(taskId);
                if (task == null)
                {
                    task = new Task(story);
                }
            }
            task.Omschrijving = omschrijving;
            TaskRepository.Save(task);
            CancelView();
        }

        /// <summary>
        /// Verwijderen van de taak. Is enkel toegestaan als er geen commentaren aan gekoppeld zijn.
        /// </summary>
        /// <param name="task">Taak instantie die moet worden toegevoegd.</param>
        public void Verwijder([ARFetch("id")] Task task)
        {
            TaskRepository.Delete(task);
            CancelView();
        }

        /// <summary>
        /// Slaat de Task op als niet opgepakt en ontkoppeld de behandelaar van de Task
        /// </summary>
        /// <param name="task">De task.</param>
        public void TaskOpslaanAlsNietOpgepakt([ARFetch("id")] Task task)
        {
            task.SetAsNotTaken();
            TaskRepository.Save(task);
            CancelView();
            CancelLayout();
        }

        /// <summary>
        /// Toon het tijdboekingscherm
        /// </summary>
        /// <param name="task">De task.</param>
        public void BoekTijd([ARFetch("taskId")] Task task)
        {
            TijdRegistratie tijdRegistratie = new TijdRegistratie();
            tijdRegistratie.Task = task;
            PropertyBag.Add("tijdRegistratie", tijdRegistratie);
            CancelLayout();
        }

        /// <summary>
        /// Sluit de task met het gegeven taakid.
        /// </summary>
        /// <param name="taak">De taak.</param>
        public void SluitTaak([ARFetch("taakId")] Task taak)
        {
            taak.Close();
            TaskRepository.Save(taak);

            CancelView();
            CancelLayout();
        }
                
        /// <summary>
        /// Maak een nieuwe tijdregistratie aan de hand van het temp tijdregistratie object.
        /// </summary>
        /// <param name="tmpTijdRegistratie">Een tijdelijk tijdregistratie object.</param>
        public void TijdBoekingOpslaan([ARDataBind("tijdRegistratie", AutoLoadBehavior.NewInstanceIfInvalidKey)] TijdRegistratie tmpTijdRegistratie)
        {
            if (tmpTijdRegistratie.Task == null)
                return; //todo: foutmelding tonen?

            Gebruiker gebruiker = CurrentUser;

            tmpTijdRegistratie.Task.RegisterTime(gebruiker, DateTime.Now, gebruiker.ActieveSprint, tmpTijdRegistratie.Tijd);

            TaskRepository.Save(tmpTijdRegistratie.Task);

            RenderText("succes");
        }

        /// <summary>
        /// Logs the berichten.
        /// </summary>
        /// <param name="task">The task.</param>
        public void LogBerichten([ARFetch("taskId", false, true)] Task task)
        {
            PropertyBag.Add("logBerichten", task.LogBerichten);
            CancelLayout();
        }

        /// <summary>
        /// Commentaars the berichten.
        /// </summary>
        /// <param name="task">The task.</param>
        public void CommentaarBerichten([ARFetch("taskId", false, true)] Task task)
        {
            PropertyBag.Add("taskCommentaarBerichten", task.CommentaarBerichten);
            PropertyBag.Add("taak", task);
            CancelLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="story"></param>
        public void TaskRow(int count)
        {
            PropertyBag.Add("count", count);
            Task task = new Task();
            PropertyBag.Add("task", task);

            CancelLayout();
        }




 }
}