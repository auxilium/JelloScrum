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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using Castle.ActiveRecord;
    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using Container;
    using Helpers;
    using Model;
    using Model.Entities;
    using Model.Enumerations;
    using Model.Services;
    using QueryObjects;
    using Repositories.Exceptions;

    /// <summary>
    /// Controller voor alles met projecten
    /// </summary>
    public class ProjectController : SecureController
    {
        private readonly IExcelExportService excelExportService = IoC.Resolve<IExcelExportService>();

        /// <summary>
        /// Index
        /// </summary>
        public void Index()
        {
            ICollection<Project> projects = ProjectRepository.FindAll();

            PropertyBag.Add("projecten", projects);
            if (projects.Count == 0)
                AddErrorMessageToPropertyBag("Er zijn nog geen projecten gedefini&euml;erd.");
        }

        /// <summary>
        /// Index met zoek opties
        /// </summary>
        public void Index(string zoektekst)
        {
            ProjectQuery projecten = new ProjectQuery();
            projecten.SearchTerm = zoektekst;

            IList<Project> projects = projecten.GetQuery(ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(typeof(ModelBase))).List<Project>();

            //IList<Project> projects = projecten.GetQuery(UnitOfWork.CurrentSession).List<Project>();

            PropertyBag.Add("projecten", projects);
            if (projects.Count == 0)
                AddErrorMessageToPropertyBag("Er zijn nog geen projecten gedefini&euml;erd.");
        }

        /// <summary>
        /// Index
        /// </summary>
        [Layout("projectbeheer")]
        public void Beheer()
        {
            ICollection<Project> projects = ProjectRepository.FindAll();
            PropertyBag.Add("projecten", projects);
            if (projects.Count == 0)
                AddErrorMessageToPropertyBag("Er zijn nog geen projecten gedefini&euml;erd.");
        }

        ///<summary>
        /// Geeft een overzicht met alle projecten
        ///</summary>
        public void Project([ARFetch("projectId")] Project project)
        {
            Titel = project.Name;
            PropertyBag.Add("project", project);
        }

        ///<summary>
        /// Maak een nieuw project
        ///</summary>
        public void Nieuw()
        {
            PropertyBag.Add("itemtype", typeof (Project));
            Titel = "Nieuw project";
            RenderView("bewerk");
        }

        ///<summary>
        /// Bewerk een bestaand project.
        ///</summary>
        public void Bewerk([ARFetch("id")] Project project)
        {
            PropertyBag.Add("item", project);
            Titel = "<a href='/project/project.rails?projectId=" + project.Id + "'>" + project.Name + "</a> > Bewerk project";
        }

        #region ProjectBackLog
        /// <summary>
        /// Laat de productbacklog van het project zien.
        /// </summary>
        /// <param name="project">The project.</param>
        public void ProductBacklog([ARFetch("projectId")] Project project)
        {
            Titel = "<a href='/project/project.rails?projectId=" + project.Id + "'>" + project.Name + "</a> > ProductBacklog";

            PropertyBag.Add("project", project);
        }

        /// <summary>
        /// Geeft de lijst met Prioriteiten als JSON object
        /// </summary>
        public void OphalenPrioriteiten()
        {
            string js = ConvertEnumToJavascript(typeof (Priority));
            Response.ContentType = "application/json";
            RenderText(js);
        }

        /// <summary>
        /// Geeft de lijst met Storypunten als JSON object
        /// </summary>
        public void OphalenStoryPunten()
        {
            string js = ConvertEnumToJavascript(typeof (StoryPoint));
            Response.ContentType = "application/json";
            RenderText(js);
        }

        /// <summary>
        /// Zet een enum om in een JSON object
        /// </summary>
        /// <param name="t">Object type</param>
        /// <returns>JSON object</returns>
        private string ConvertEnumToJavascript(Type t)
        {
            if (!t.IsEnum)
                return string.Empty;

            Array values = Enum.GetValues(t);
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (object o in values)
            {
                string name = Enum.GetName(t, o);
                dict.Add(Enum.Format(t, o, "D"), name);
            }

            return Context.Services.JSONSerializer.Serialize(dict);
        }

        /// <summary>
        /// Opslaan van story prioriteit
        /// </summary>
        /// <param name="id">Story Id</param>
        /// <param name="value">Prioriteit</param>
        public void OpslaanPrioriteiten(int id, string value)
        {
            Story story = StoryRepository.Load(id);

            try
            {
                story.ProductBacklogPriority = (Priority) Enum.ToObject(typeof (Priority), Convert.ToInt32(value));
                StoryRepository.Save(story);
            }
            catch (JelloScrumRepositoryException e)
            {
                Hashtable args = new Hashtable();
                args.Add("projectId", story.Project.Id);
                AddErrorMessageToFlashBag(e.LogMessage);
                RedirectToAction("ProductBacklog", args);
            }

            RenderText(Enum.GetName(typeof (Priority), Convert.ToInt32(value)));
        }

        /// <summary>
        /// Opslaan van story pionts
        /// </summary>
        /// <param name="id">Story id</param>
        /// <param name="value">Aantal story pionts</param>
        public void OpslaanPunten(int id, string value)
        {
            Story story = StoryRepository.Load(id);

            try
            {
                story.StoryPoints = (StoryPoint) Enum.ToObject(typeof (StoryPoint), Convert.ToInt32(value));
                StoryRepository.Save(story);
            }
            catch (JelloScrumRepositoryException e)
            {
                Hashtable args = new Hashtable();
                args.Add("projectId", story.Project.Id);
                AddErrorMessageToFlashBag(e.LogMessage);
                RedirectToAction("ProductBacklog", args);
            }

            RenderText(Enum.GetName(typeof (StoryPoint), Convert.ToInt32(value)));
        }

        /// <summary>
        /// Opslaan van story schatting
        /// </summary>
        /// <param name="id">Story id</param>
        /// <param name="value">Aantal uren schatting</param>
        public void OpslaanSchatting(int id, string value)
        {
            Story story = StoryRepository.Load(id);

            try
            {
                if (!string.IsNullOrEmpty(value))
                    story.Estimation = TimeSpanHelper.Parse(value);
                StoryRepository.Save(story);
            }
            catch (JelloScrumRepositoryException e)
            {
                Hashtable args = new Hashtable();
                args.Add("projectId", story.Project.Id);
                AddErrorMessageToFlashBag(e.LogMessage);
                RedirectToAction("ProductBacklog", args);
            }

            RenderText(TimeSpanHelper.TimeSpanInMinuten(story.Estimation).ToString());
        }

        /// <summary>
        /// Exporteer Productbacklog naar excel
        /// </summary>
        /// <param name="project"></param>
        public void ExportProductBackLog([ARFetch("projectId")] Project project)
        {
            string fileName = this.excelExportService.ExportProjectBacklog(project);

            Response.ContentType = "application/x-excel";
            Response.AppendHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
            Response.WriteFile(ConfigurationManager.AppSettings["exportLocation"] + fileName);

            CancelView();
        }
        #endregion

        /// <summary>
        /// Informatie inzien van een project.
        /// </summary>
        /// <param name="project">Het project.</param>
        public void Informatie([ARFetch("projectId")] Project project)
        {
            PropertyBag.Add("project", project);
            CancelLayout();
        }

        /// <summary>
        /// Sla het project op.
        /// </summary>
        /// <param name="project">Het project.</param>
        [Layout("jellobeheer")]
        public void Opslaan([ARDataBind("item", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey)] Project project)
        {
            try
            {
                ProjectRepository.Save(project);
            }
            catch (JelloScrumRepositoryException e)
            {
                AddErrorMessageToFlashBag(e.LogMessage);
                PropertyBag.Add("item", project);
                RenderView("Bewerk");
            }

            AddPositiveMessageToPropertyBag("Het project " + project.Name + " is opgeslagen.");
            Project(project);
            RenderView("project");
        }

        /// <summary>
        /// Verwijderen van het project. Dit is enkel toegestaan als er geen stories aan gekoppeld zijn.
        /// </summary>
        /// <param name="project">Het project.</param>
        public void Verwijder([ARFetch("id")] Project project)
        {
            try
            {
                if (project.Stories.Count == 0)
                    ProjectRepository.Delete(project);
                else
                    AddErrorMessageToFlashBag("Kan project " + project.Name + " niet verwijderen. Dit project bevat nog stories.");
                RenderText("true");
            }
            catch (JelloScrumRepositoryException e)
            {
                AddErrorMessageToFlashBag(e.LogMessage);
                RenderText("false");
                return;
            }
            CancelView();
            CancelLayout();
        }

        /// <summary>
        /// Toond alle stories van het gegeven project die nog geen MoSCoW prioriteit hebben.
        /// </summary>
        /// <param name="p">Het project.</param>
        public void MoSCoW([ARFetch("id")] Project project)
        {
            PropertyBag.Add("project", project);
            if (project != null)
                PropertyBag.Add("item", project.GetAllStoriesWithUndefinedPriorities());
        }

        /// <summary>
        /// Geeft een lijst met sprints van dit project.
        /// </summary>
        /// <param name="project">Het project.</param>
        public void SprintList([ARFetch("id")] Project project)
        {
            PropertyBag.Add("project", project);
            CancelLayout();
        }

        /// <summary>
        /// Redirect the user to the dashboard of its active project
        /// </summary>
        public void ActiveProject()
        {
            if (CurrentUser.ActiveSprint == null)
            {
                Redirect("home", "index");
                return;
            }

            NameValueCollection args = new NameValueCollection();
            args.Add("projectId", CurrentUser.ActiveSprint.Project.Id.ToString());
            Redirect("project", "project", args);
        }

        /// <summary>
        /// Redirect the user to the projectbacklog of its active project
        /// </summary>
        public void ActiveProjectBacklog()
        {
            if (CurrentUser.ActiveSprint == null)
            {
                Redirect("home", "index");
                return;
            }

            NameValueCollection args = new NameValueCollection();
            args.Add("projectId", CurrentUser.ActiveSprint.Project.Id.ToString());
            Redirect("project", "productbacklog", args);
        }
    }
}