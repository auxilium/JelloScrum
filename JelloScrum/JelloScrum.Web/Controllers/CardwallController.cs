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
    using System.Configuration;
    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using Container;
    using Model.Entities;
    using Model.Enumerations;
    using Model.Helpers;
    using Model.Services;
   
    /// <summary>
    /// De controller voor de card wall
    /// </summary>
    [Layout("sprint")]
    public class CardwallController: SecureController
    {
        private readonly IExcelExportService excelExportService = IoC.Resolve<IExcelExportService>();

        /// <summary>
        /// Toon de cardwall van deze sprint
        /// </summary>
        public void Cardwall([ARFetch("sprintId")]Sprint sprint)
        {
            Titel = "<a href='/project/project.rails?projectId=" + sprint.Project.Id + "'>" + sprint.Project.Name + "</a> > <a href='/sprint/sprint.rails?sprintId=" + sprint.Id + "'>" + sprint.Goal + "</a> > Cardwall";
   
            PropertyBag.Add("sprint", sprint);
            PropertyBag.Add("sprintStories", sprint.SprintStories);
        }

        /// <summary>
        /// Sla de nieuwe status van de versleepte taak op
        /// </summary>
        /// <param name="task">de taak</param>
        public void OpslaanTaak([ARDataBind("task", AutoLoad = AutoLoadBehavior.NullIfInvalidKey)] Task task, [ARFetch("sprintId")]Sprint sprint)
        {
            //de status wordt meegezonden, dus als de taak de status opgepakt krijgt, gaat de huidige gebruiker de taak oppakken
            if (task.State == State.Taken)
                task.AssignedUser = sprint.GetSprintUserFor(CurrentUser);
            try
            {
                TaskRepository.Save(task);
            }
            catch (Exception)
            {
                AddErrorMessageToFlashBag("Het opslaan van taak " + task.Title + " is niet gelukt.");
            }
            CancelView();
        }

        /// <summary>
        /// Exporteer SprinntCardwall naar excel
        /// </summary>
        /// <param name="sprint"></param>
        public void ExportCardwall([ARFetch("sprintId")] Sprint sprint)
        {
            string fileName = excelExportService.ExportSprintCardwall(sprint);

            Response.ContentType = "application/x-excel";
            Response.AppendHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
            Response.WriteFile(ConfigurationManager.AppSettings["exportLocation"] + fileName);

            CancelView();
        }

    }
}
