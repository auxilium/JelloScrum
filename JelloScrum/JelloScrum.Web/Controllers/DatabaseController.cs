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

using JelloScrum.Model;

namespace JelloScrum.Web.Controllers
{
    using System;
    using System.Text;
    using Castle.ActiveRecord;
    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using JelloScrum.Model.IRepositories;
    using JelloScrum.Repositories.Exceptions;
    using Model.Entities;
    using Model.Enumerations;
    using NHibernate;
    using NHibernate.Criterion;

    ///<summary>
    /// 
    ///</summary>
    [Rescue("generalerror")]
    [Layout("databaselayout")]
    public class DatabaseController : JelloScrumControllerBase
    {
        /// <summary>
        /// Creates the database.
        /// </summary>
        public void CreateDatabase()
        {
            StringBuilder log = new StringBuilder();
            bool error = false;
            try
            {
               
                    ActiveRecordStarter.DropSchema();
               
                log.AppendLine("Schema has been dropped<br />");
            }
            catch (Exception e)
            {
                log.AppendLine("Schema couldn't be dropped<br /><b>Exception:</b>" + e +"<br />");
                error = true;
            }
            try
            {
               
                    ActiveRecordStarter.CreateSchema();
               
                log.AppendLine("Done creating Schema<br />");
            }
            catch (Exception e)
            {
                log.AppendLine("Schema couldn't be created<br /><b>Exception:</b>" + e + "<br />");
                error = true;
            }
            PropertyBag.Add("log", log.ToString());
            PropertyBag.Add("hasError", error);
            RenderView("index");
        }

        /// <summary>
        /// Creates the data.
        /// </summary>
        public void CreateData()
        {
            #region Projecten
            Project project_JelloScrum = new Project();
            project_JelloScrum.Naam = "Jello Scrum";
            project_JelloScrum.Omschrijving = "Scrum project managment tool";
            
            ProjectRepository.Save(project_JelloScrum);

            Project project_James = new Project();
            project_James.Naam = "James";
            project_James.Omschrijving = "Inklok systeem voor Auxilium BV";
            ProjectRepository.Save(project_James);
            #endregion

            #region Gebruikers
            Gebruiker user1 = new Gebruiker();
            user1.Naam = "user1";
            user1.GebruikersNaam = "ProductOwner";
            user1.VolledigeNaam = "Gebruiker 1";
            GebruikerRepository.Save(user1);
            
            Gebruiker user2 = new Gebruiker();
            user2.Naam = "user2";
            user2.GebruikersNaam = "ScrumMaster";
            user2.SysteemRol = SysteemRol.Administrator;
            user2.VolledigeNaam = "Gebruiker 2";
            GebruikerRepository.Save(user2);

            Gebruiker user3 = new Gebruiker();
            user3.Naam = "user3";
            user3.GebruikersNaam = "Developer";
            user3.SysteemRol = SysteemRol.Gebruiker;
            user3.VolledigeNaam = "Gebruiker 3";
            GebruikerRepository.Save(user3);
            #endregion

            #region Stories
            #region JelloScrum stories
            for (int i = 0; i < 20; i++)
            {
                Story story = new Story(project_JelloScrum, user1, null, StoryType.UserStory);
                story.HowtoDemo = "uitleg voor de demo " + i.ToString();
                story.Notitie = "notitie " + i.ToString();
                story.Omschrijving = "omschrijving " + i.ToString();
                story.ProductBacklogPrioriteit = (Prioriteit)RandomNumber(0, 3);
                story.Schatting = new TimeSpan(4, 30, 0);
                story.Titel = "JelloScrum story number: " + i.ToString();
                
                ProjectRepository.Save(project_JelloScrum);
            }
            #endregion

            #region James stories
            for (int i = 0; i < 20; i++)
            {
                Story story = new Story(project_James, user1, null, StoryType.UserStory);
                story.HowtoDemo = "uitleg voor de demo " + i.ToString();
                story.Notitie = "notitie " + i.ToString();
                story.Omschrijving = "omschrijving " + i.ToString();
                story.ProductBacklogPrioriteit = (Prioriteit)RandomNumber(0, 3);
                story.Schatting = new TimeSpan(6, 0, 0);
                story.Titel = "James story number: " + i.ToString();

                ProjectRepository.Save(project_James);
            }
            #endregion
            #endregion
           
            #region Tasks
                #region JelloScrum
            foreach (Story story in project_JelloScrum.Stories)
            {
                for (int i = RandomNumber(0, 4); i < 6; i++)
                {
                    Task task = new Task();
                    task.Omschrijving = "Omschrijving voor JelloScrum story " + story.Titel + " taak nummer " + i.ToString();
                    task.Story = story;
                    story.AddTask(task);
                    TaskCommentaarBericht bericht = new TaskCommentaarBericht(task, "blabla comment teskt " + i.ToString());
                    task.AddComment(bericht);
                    StoryRepository.SaveOrUpdate(story);
                }
                story.AddComment("Storycomment James teskt");
                StoryRepository.Save(story);
            }
            #endregion

                #region James
            foreach (Story story in project_James.Stories)
            {
                for (int i = RandomNumber(0, 4); i < 5; i++)
                {
                    Task task = new Task();
                    task.Omschrijving = "Omschrijving voor James story " + story.Titel + " taak nummer " + i.ToString();
                    task.Story = story;
                    story.AddTask(task);
                    TaskCommentaarBericht bericht = new TaskCommentaarBericht(task,"blabla comment teskt " + i.ToString());
                    task.AddComment(bericht);
                    StoryRepository.Save(story);
                }
                story.AddComment("Storycomment James teskt");
                StoryRepository.Save(story);
            }
            #endregion
            #endregion

            #region Sprints
            #region JelloScrum Sprints
            int num = 0;
            for (int i = 0; i < 6; i++)
            {
                Sprint sprint = new Sprint();
                sprint.Doel = "JelloScrum SprintDoel #" + i.ToString();
                sprint.StartDatum = DateTime.Now.AddDays((5 * i));
                sprint.EindDatum = DateTime.Now.AddDays((5 * i) + 20);
                sprint.Project = project_JelloScrum;
                SprintRepository.Save(sprint);
                
                SprintGebruiker sprintGebruiker = new SprintGebruiker(user1, sprint, SprintRol.ProductOwner);
                SprintGebruiker sprintGebruiker2 = new SprintGebruiker(user3, sprint, SprintRol.Developer);
                SprintGebruiker sprintGebruiker3 = new SprintGebruiker(user2, sprint, SprintRol.ScrumMaster);
                SprintGebruikerRepository.Save(sprintGebruiker);
                SprintGebruikerRepository.Save(sprintGebruiker2);
                SprintGebruikerRepository.Save(sprintGebruiker3);

                SprintRepository.Save(sprint);
                GebruikerRepository.Save(user1);
                GebruikerRepository.Save(user3);
                /*for (int y = 0; y < 3; y++)
                {
                    SprintStory sprintStory = sprint.MaakSprintStoryVoor(project_JelloScrum.Stories[num]);
                    
                    // retrieve a story
                    
                    sprintStory.SprintBacklogPrioriteit = (Prioriteit)RandomNumber(0, 3);
                    SprintStoryRepository.Save(sprintStory);

                    if (sprintStory.Status != Status.NietOpgepakt)
                    {
                        // ook even wat werkuren toevoegen
                        sprintStory.Story.Tasks[RandomNumber(0, sprintStory.Story.Tasks.Count - 1)].MaakTijdRegistratie(user1, DateTime.Today, sprintStory.Sprint, new TimeSpan(0, 0, RandomNumber(1, 60)));
                        sprintStory.Story.Tasks[RandomNumber(0, sprintStory.Story.Tasks.Count - 1)].MaakTijdRegistratie(user2, DateTime.Today, sprintStory.Sprint, new TimeSpan(0, 0, RandomNumber(1, 60)));

                        SprintStoryRepository.Save(sprintStory);
                    }
                    
                    num++;
                }*/
                SprintRepository.Save(sprint);
            }
            #endregion

            #region James Sprints
            num = 0;
            for (int i = 0; i < 4; i++)
            {
                Sprint sprint = new Sprint();
                sprint.Doel = "James SprintDoel #" + i.ToString();
                sprint.StartDatum = DateTime.Now.AddDays((4 * i));
                sprint.EindDatum = DateTime.Now.AddDays((4 * i) + 16);
                sprint.Project = project_James;
                SprintRepository.Save(sprint);

                SprintGebruiker sprintGebruiker = new SprintGebruiker(user1, sprint, SprintRol.ScrumMaster);
                SprintGebruiker sprintGebruiker2 = new SprintGebruiker(user3, sprint, SprintRol.Developer);
                SprintGebruikerRepository.Save(sprintGebruiker);
                SprintGebruikerRepository.Save(sprintGebruiker2);

                SprintRepository.Save(sprint);
                GebruikerRepository.Save(user2);
                GebruikerRepository.Save(user3);

                for (int y = 0; y < 3; y++)
                {
                    SprintStory sprintStory = sprint.CreateSprintStoryFor(project_James.Stories[num]);
                    
                    sprintStory.SprintBacklogPrioriteit = (Prioriteit)RandomNumber(0, 3);
                    SprintStoryRepository.Save(sprintStory);

                    if (sprintStory.Status != Status.NietOpgepakt)
                    {
                        // ook even wat werkuren toevoegen
                        sprintStory.Story.Tasks[RandomNumber(0, sprintStory.Story.Tasks.Count - 1)].RegisterTime(user2, DateTime.Today, sprintStory.Sprint, new TimeSpan(0, 0, RandomNumber(1, 60)));
                        sprintStory.Story.Tasks[RandomNumber(0, sprintStory.Story.Tasks.Count - 1)].RegisterTime(user3, DateTime.Today, sprintStory.Sprint, new TimeSpan(0, 0, RandomNumber(1, 60)));

                        SprintStoryRepository.Save(sprintStory);
                    }
                    num++;
                }
                SprintRepository.Save(sprint);
            }
            #endregion
            #endregion
            
            #region Impediments
            #region JelloScrum
            for (int i = 0; i < 10; i++)
            {
                Story impediment = new Story(project_JelloScrum, user3, null, StoryType.Impediment);
                impediment.Titel = "Impediment JelloScrum #" + i.ToString();
                impediment.Omschrijving = "JelloScrum Impediment omschrijving";
                impediment.AddComment("Ahum comment voor....." + i);
                StoryRepository.Save(impediment);
            }
            #endregion

            #region James
            for (int i = 0; i < 10; i++)
            {
                Story impediment = new Story(project_James, user2, null, StoryType.Impediment);
                impediment.Titel = "Impediment James #" + i.ToString();
                impediment.Omschrijving = "James Impediment omschrijving";
                impediment.AddComment("Ahum comment voor....." + i);
                StoryRepository.Save(impediment);
            }
            #endregion
            #endregion

            #region Bugs
            #region JelloScrum
            for (int i = 0; i < 10; i++)
            {
                Story bug = new Story(project_JelloScrum, user1, (Impact)RandomNumber(0, 4), StoryType.Bug);
                bug.HowtoDemo = "Bug demo description for JelloScrum #" + i.ToString();
                bug.Notitie = "Note " + i.ToString();
                bug.Omschrijving = "Description " + i.ToString();
                bug.Schatting = new TimeSpan(0);
                bug.Titel = "BUG JelloScrum #" + i.ToString();
                StoryRepository.Save(bug);
            }
            #endregion

            #region James
            for (int i = 0; i < 10; i++)
            {
                Story bug = new Story(project_James, user1, (Impact)RandomNumber(0, 4), StoryType.Bug);
                bug.HowtoDemo = "Bug demo description for James #" + i.ToString();
                bug.Notitie = "Note " + i.ToString();
                bug.Omschrijving = "Description " + i.ToString();
                bug.Schatting = new TimeSpan(0);
                bug.Titel = "BUG James #" + i.ToString();
                StoryRepository.Save(bug);
            }
            #endregion
            #endregion
            PropertyBag.Add("ready", true);
            RenderView("index");
        }
        /// <summary>
        /// Returns a random number.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public void Index()
        {
        }

        public void Toeter()
        {

            //            SprintStoriesQuery bla = new SprintStoriesQuery();
            //            bla.Sprint = SprintRepository.Load(6);      
            //            ICriteria crit = bla.GetQuery(UnitOfWork.CurrentSession);
            //            crit.AddOrder(new Order("SprintBacklogPrioriteit", false));
            //            crit.CreateCriteria("SprintStory").Add(Restrictions.Eq("SprintBacklogPrioriteit", Prioriteit.MustHave));
            //            IList lijst = crit.List();

            //ICriteria crit =
            //    UnitOfWork.CurrentSession.CreateCriteria(typeof(SprintStory)).SetProjection(Projections.ProjectionList().Add(Projections.Property("SprintBacklogPrioriteit").As("TinkyWinky"), "TinkyWinky2").Add(Projections.Property("Id").As("LaLa"), "Lala2"));

            //IList lijtmetprioshoopikdanmaar = crit.List();

        }
    }
}
