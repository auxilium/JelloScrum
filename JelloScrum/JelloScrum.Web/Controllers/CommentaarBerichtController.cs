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
        public void TaakOpslaan(string beschrijving, [ARFetch("commentId", true, false)] TaskComment bericht, [ARFetch("taskId", false, true)] Task task)
        {
            bericht.Text = beschrijving;

            if(bericht.Id == 0)
            {
                bericht.User = CurrentUser;
                bericht.LogObject = task;
                task.Comments.Add(bericht);
            }
            TaskRepository.SaveOrUpdate(task);
            RedirectToReferrer();
        }
        
        /// <summary>
        /// Verwijder de comment.
        /// </summary>
        /// <param name="bericht"></param>
        public void Verwijder([ARFetch("commentId")] TaskComment bericht)
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
            PropertyBag.Add("taskCommentaarBerichten", taak.Comments);

            CancelLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OpslaanTaakCommentaar([ARDataBind("taak", AutoLoad = AutoLoadBehavior.NewRootInstanceIfInvalidKey)] Task taak,
            [ARDataBind("comment", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey)] TaskComment[] comments)
        {
            IList<TaskComment> taakCommentaren = new List<TaskComment>(comments);

            //verwijder gedelete commentaren
            foreach (TaskComment taakCommentaar in new List<TaskComment>(taak.Comments))
            {
                if (!taakCommentaren.Contains(taakCommentaar))
                    taak.RemoveComment(taakCommentaar);
            }

            //voeg alle nieuwe commentaren toe
            foreach (TaskComment taakCommentaar in comments)
            {
                if (taakCommentaar.Id == 0)
                {
                    taakCommentaar.LogObject = taak;
                    taakCommentaar.User = CurrentUser;
                    taakCommentaar.Date = DateTime.Now;
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