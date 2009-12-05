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
    using System.Collections.Generic;

    using Castle.MonoRail.ActiveRecordSupport;
    using JelloScrum.Model.Entities;

    using Repositories.Exceptions;

    /// <summary>
    /// 
    /// </summary>
    public class CommentaarBerichtController : SecureController
    {
        /// <summary>
        /// Sla een nieuwe of bewerk een bestaant commentaarbericht
        /// </summary>
        /// <param name="beschrijving">de (nieuwe) text van het commentaar bericht</param>
        /// <param name="bericht">het eventueele bestaande bericht</param>
        /// <param name="task"></param>
        public void TaakOpslaan(string beschrijving, [ARFetch("commentId", true, false)] TaskCommentaarBericht bericht, [ARFetch("taskId", false, true)] Task task)
        {
            bericht.Tekst = beschrijving;

            if(bericht.Id == 0)
            {
                bericht.Gebruiker = CurrentUser;
                bericht.LogObject = task;
                task.CommentaarBerichten.Add(bericht);
            }
            TaskRepository.SaveOrUpdate(task);
            RedirectToReferrer();
        }
        
        /// <summary>
        /// Verwijder de comment.
        /// </summary>
        /// <param name="bericht"></param>
        public void Verwijder([ARFetch("commentId")] TaskCommentaarBericht bericht)
        {
            Task task = bericht.LogObject;
            task.RemoveComment(bericht);
            TaskRepository.SaveOrUpdate(task);
            RedirectToReferrer();
        }

        /// <summary>
        /// 
        /// </summary>
        public void TaakCommentaar([ARFetch("Id")] Task taak)
        {
            PropertyBag.Add("taak", taak);
            PropertyBag.Add("taskCommentaarBerichten", taak.CommentaarBerichten);

            CancelLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OpslaanTaakCommentaar([ARDataBind("taak", AutoLoad = AutoLoadBehavior.NewRootInstanceIfInvalidKey)] Task taak,
            [ARDataBind("comment", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey)] TaskCommentaarBericht[] comments)
        {
            IList<TaskCommentaarBericht> taakCommentaren = new List<TaskCommentaarBericht>(comments);

            //verwijder gedelete commentaren
            foreach (TaskCommentaarBericht taakCommentaar in new List<TaskCommentaarBericht>(taak.CommentaarBerichten))
            {
                if (!taakCommentaren.Contains(taakCommentaar))
                    taak.RemoveComment(taakCommentaar);
            }

            //voeg alle nieuwe commentaren toe
            foreach (TaskCommentaarBericht taakCommentaar in comments)
            {
                if (taakCommentaar.Id == 0)
                {
                    taakCommentaar.LogObject = taak;
                    taakCommentaar.Gebruiker = CurrentUser;
                    taakCommentaar.Datum = DateTime.Now;
                    taak.AddComment(taakCommentaar);
                }
            }

            try
            {
                TaskRepository.Save(taak);
            }
            catch 
            {
                AddErrorMessageToFlashBag("Het opslaan van de taak" + taak.Id + " is niet gelukt.");
            }
            
            RedirectToReferrer();
        }
    }
}