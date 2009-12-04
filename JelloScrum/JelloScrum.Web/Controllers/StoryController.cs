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
    using Castle.MonoRail.ActiveRecordSupport;
    using Helpers;
    using Model.Entities;
    using Model.Enumerations;
    using Repositories.Exceptions;

    ///<summary>
    /// Controller die dingen doet met stories.
    ///</summary>
    public class StoryController : SecureController
    {
        ///<summary>
        /// Geeft een lijst van alle stories van het opgegeven project.
        ///</summary>
        ///<param name="projectId">Het project</param>
        public void Overzicht(int projectId)
        {
            Project project = ProjectRepository.Load(projectId);

            PropertyBag.Add("story", project.Stories);
            if (project.Stories.Count == 0)
            {
                AddErrorMessageToFlashBag("Geen stories gevonden");
            }
        }

        /// <summary>
        /// Ellende....
        /// </summary>
        public ISprintControllerParameters SprintControllerParameters
        {
            get { return AdapterFactory.GetAdapter<ISprintControllerParameters>(Session); }
        }

        ///<summary>
        /// Maak een nieuwe story voor het gegeven project.
        ///</summary>
        public void Nieuw([ARFetch("projectId")] Project project)
        {
            Story story = new Story(project, CurrentUser, null, StoryType.UserStory);

            Titel = "<a href='/project/project.rails?projectId=" + project.Id + "'>" + project.Naam + "</a> > <a href='/project/productBacklog.rails?projectId=" + project.Id + "'>ProductBacklog</a> > Story toevoegen";
            PropertyBag.Add("storyTypes", Enum.GetNames(typeof (StoryType)));
            PropertyBag.Add("prioriteit", Enum.GetNames(typeof (Prioriteit)));
            PropertyBag.Add("punten", Enum.GetNames(typeof (StoryPoint)));
            PropertyBag.Add("story", story);
            PropertyBag.Add("Schatting", new TimeSpan(0, 0, 0));

            RenderView("bewerk");
        }

        ///<summary>
        /// Bewerk een bestaande story.
        ///</summary>
        public void Bewerk([ARFetch("storyId")] Story story, [ARFetch("sprintId")] Sprint sprint)
        {
            if (sprint != null)
            {
                Titel = "<a href='/project/project.rails?projectId=" + story.Project.Id + "'>" + story.Project.Naam + "</a> > <a href='/sprint/sprint.rails?sprintId=" + sprint.Id + "'>" + sprint.Doel + "</a> > <a href='/sprint/sprintBacklog.rails?sprintId=" + sprint.Id + "'>SprintBacklog</a> > Story (" + story.Id + ") bewerken";
                LayoutName = "sprint";
                PropertyBag.Add("sprint", sprint);
            }
            else
            {
                Titel = "<a href='/project/project.rails?projectId=" + story.Project.Id + "'>" + story.Project.Naam + "</a> > <a href='/project/productBacklog.rails?projectId=" + story.Project.Id + "'>ProductBacklog</a> > Story (" + story.Id + ") bewerken";
            }

            PropertyBag.Add("storyTypes", Enum.GetNames(typeof (StoryType)));
            PropertyBag.Add("prioriteit", Enum.GetNames(typeof (Prioriteit)));
            PropertyBag.Add("punten", Enum.GetNames(typeof (StoryPoint)));
            PropertyBag.Add("Schatting", story.Schatting);
            PropertyBag.Add("story", story);
        }

        /// <summary>
        /// Sla de story op.
        /// </summary>
        /// <param name="story">De story die moet worden opgeslagen.</param>
        /// <param name="schatting"></param>
        /// <param name="sprint">Als de story wordt bewerkt vanuit sprintbacklog keren we daar weer terug</param>
        /// <param name="tasks">De taken.</param>
        /// <param name="schatting">Urenschatting.</param>
        /// <param name="taskSchatting"></param>
        /// <param name="opslaanVolgendeHidden"></param>
        public void Opslaan([ARDataBind("story", AutoLoad = AutoLoadBehavior.NewRootInstanceIfInvalidKey)] Story story,
                            string schatting,
                            [ARDataBind("sprint", AutoLoad = AutoLoadBehavior.NullIfInvalidKey)] Sprint sprint,
                            [ARDataBind("task", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey)] Task[] tasks, bool opslaanVolgendeHidden)
        {
            story.AangemaaktDoor = CurrentUser;
            story.Schatting = TimeSpanHelper.Parse(schatting);

            IList<Task> taken = new List<Task>(tasks);

            //verwijder gedelete taken
            foreach (Task task in new List<Task>(story.Tasks))
            {
                if (!taken.Contains(task))
                    story.RemoveTask(task);
            }

            //voeg alle nieuwe taken toe
            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i].Id == 0)
                {
                    story.AddTask(tasks[i]);
                }
            }

            try
            {
                StoryRepository.Save(story);
            }
            catch (JelloScrumRepositoryException jre)
            {
                AddErrorMessageToFlashBag("Het opslaan van story" + story.Titel + " is niet gelukt." + jre.LogMessage);
                RedirectToAction("Bewerk");
            }

            Hashtable args = new Hashtable();

            if (sprint != null)
                args.Add("sprintId", sprint.Id);
            else
                args.Add("projectId", story.Project.Id);

            if (story.IsEstimatedTimeOfTasksLessThenEstimatedTimeOfStory())
            {
                AddPositiveMessageToFlashBag("Story is opgeslagen.");
                if (opslaanVolgendeHidden)
                {
                    Redirect("Story", "nieuw", args);
                }
                else
                {
                    if (sprint != null)
                        Redirect("Sprint", "SprintBacklog", args);
                    else
                        Redirect("Project", "ProductBacklog", args);
                }
            }
            else
            {
                args.Add("storyId", story.Id);
                AddErrorMessageToFlashBag("De uren schatting van alle taken samen is groter dan de uren schatting van de story");
                Redirect("Story", "Bewerk", args);
            }
        }

        /// <summary>
        /// Verander de prioriteit van de story
        /// </summary>
        /// <param name="story">De story.</param>
        /// <param name="prioriteit">De prioriteit.</param>
        public void VeranderPrioriteit([ARFetch("id")] Story story, Prioriteit prioriteit)
        {
            story.ProductBacklogPrioriteit = prioriteit;
            StoryRepository.Save(story);

            RenderText("done");
            CancelLayout();
            CancelView();
        }

        ///<summary>
        /// Bewerk een bestaande story.
        ///</summary>
        public void Story([ARFetch("storyId")] Story story)
        {
            Titel = "<a href='/project/project.rails?projectId=" + story.Project.Id + "'>" + story.Project.Naam + "</a> > <a href='/project/productBacklog.rails?projectId=" + story.Project.Id + "'>ProductBacklog</a> > Story (" + story.Id + ") bekijken";

            PropertyBag.Add("storyTypes", Enum.GetNames(typeof (StoryType)));
            PropertyBag.Add("prioriteit", Enum.GetNames(typeof (Prioriteit)));
            PropertyBag.Add("punten", Enum.GetNames(typeof (StoryPoint)));
            PropertyBag.Add("story", story);
        }
    }
}