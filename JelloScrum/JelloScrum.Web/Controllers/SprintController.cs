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
    using Model.Helpers;
    using Model.Services;
    using NHibernate.Criterion;
    using QueryObjects;

    /// <summary>
    /// Parameters benodigd voor een correcte werking van de sprintcontroller
    /// </summary>
    public interface ISprintControllerParameters
    {
        /// <summary>
        /// ProjectId uit de sessie.
        /// </summary>
        int? SprintControllerParametersProjectId { get; set; }
    }

    /// <summary>
    /// De controller voor de sprint
    /// </summary>
    [Helper(typeof (BurnDown2Helper))]
    [Layout("sprint")]
    public class SprintController : SecureController
    {
        private readonly IExcelExportService excelExportService = IoC.Resolve<IExcelExportService>();

        /// <summary>
        /// Toont een lijst met taken van de ingelogde gebruiker
        /// </summary>
        public void MijnTaken()
        {
            Gebruiker gb = CurrentUser;
            SprintGebruiker sg = gb.GeefActieveSprintGebruiker();

            //mijn taken
            if (sg != null)
                PropertyBag.Add("mijnTaken", sg.GeefOpgepakteTaken());
        }

        /// <summary>
        /// Toont een lijst met taken van andere gebruikers in deze sprint.
        /// </summary>
        public void TakenVanAnderen()
        {
            Gebruiker gb = CurrentUser;
            IList takenVanAnderen = GeefTakenVanAnderen(gb);

            if (takenVanAnderen.Count > 0)
                PropertyBag.Add("takenVanAnderen", takenVanAnderen);
        }

        /// <summary>
        /// Toont een lijst met onopgepakte taken
        /// </summary>
        public void OnOpgepakteTaken()
        {
            Gebruiker gb = CurrentUser;
            IList onopgepakteTaken = GeefOnopgepakteTaken(gb.ActieveSprint);

            if (onopgepakteTaken.Count > 0)
                PropertyBag.Add("onopgepakteTaken", onopgepakteTaken);
        }

        /// <summary>
        /// Toont een lijst met door de gebruiker afgesloten taken.
        /// </summary>
        public void MijnAfgeslotenTaken()
        {
            Gebruiker gb = CurrentUser;
            SprintGebruiker sg = gb.GeefActieveSprintGebruiker();

            //mijn taken
            if (sg != null)
                PropertyBag.Add("mijnTaken", sg.GeefAfgeslotenTaken());
        }

        /// <summary>
        /// Het dashboard van deze sprint.
        /// Hier kan de actieve gebruiker de volgende zaken zien:
        ///  - eigen taken
        ///  - taken van anderen
        ///  - nog niet opgepakte taken
        ///  - eigen afgesloten taken
        /// </summary>
        public void Dashboard()
        {
            RedirectToAction("mijntaken");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        public void Overzicht([ARFetch("projectId")] Project project)
        {
            if (project == null)
            {
                RedirectToAction("Index");
            }

            PropertyBag.Add("project", project);
            CancelLayout();
            if (project == null)
            {
                RedirectToAction("Index");
            }

            PropertyBag.Add("project", project);
            CancelLayout();
        }

        /// <summary>
        /// Nieuwe sprint toevoegen
        /// </summary>
        /// <param name="project"></param>
        [Layout("default")]
        public void Nieuw([ARFetch("projectId")] Project project)
        {
            Sprint sprint = new Sprint(project);
            PropertyBag.Add("sprint", sprint);
            PropertyBag.Add("project", project);

            PropertyBag.Add("availableUsers", GebruikerRepository.FindAll());
            Titel = "<a href='/project/project.rails?projectId=" + project.Id + "'>" + project.Naam + "</a> > Sprint toevoegen";
            RenderView("bewerk");
        }

        /// <summary>
        /// Bewerk een sprint
        /// </summary>
        /// <param name="sprint"></param>
        /// <param name="project"></param>
        [Layout("default")]
        public void Bewerk([ARFetch("sprintId")] Sprint sprint)
        {
            Project project = sprint.Project;
            PropertyBag.Add("sprint", sprint);
            PropertyBag.Add("availableUsers", GebruikerRepository.FindAll());
            PropertyBag.Add("project", project);

            Titel = "<a href='/project/project.rails?projectId=" + project.Id + "'>" + project.Naam + "</a>  > <a href='/sprint/sprint.rails?sprintId=" + sprint.Id + "'>" + sprint.Doel + "</a> > Sprint bewerken";
        }

        /// <summary>
        /// Bindt en sla de sprint op met de bijbehorende actieve gebruikers
        /// </summary>
        /// <param name="sprint"></param>
        /// <param name="sprintRolGebruikerHelpers"></param>
        /// <param name="project"></param>
        public void Opslaan([ARDataBind("sprint", AutoLoad = AutoLoadBehavior.NewInstanceIfInvalidKey)] Sprint sprint,
                            string BeschikbareUren,
                            [ARDataBind("rol", AutoLoadBehavior.NewRootInstanceIfInvalidKey)] SprintRolGebruikerHelper[]
                                sprintRolGebruikerHelpers,
                            [ARFetch("projectId")] Project project)
        {
            sprint.BeschikbareUren = TimeSpanHelper.Parse(BeschikbareUren);
            project.AddSprint(sprint);
            foreach (SprintRolGebruikerHelper sprintRolGebruikerHelper in sprintRolGebruikerHelpers)
            {
                sprintRolGebruikerHelper.Verwerk(sprint);
            }

            SprintRepository.Save(sprint);

            NameValueCollection args = new NameValueCollection();
            args.Add("projectId", project.Id.ToString());
            Redirect("Project", "project", args);
        }

        /// <summary>
        /// Maak een lijst van stories en taken die in de sprint nog gedaan moeten worden
        /// </summary>
        /// <param name="item">Sprint.</param>
        public void SprintStories([ARFetch("id")] Sprint item)
        {
            Dictionary<Prioriteit, IList<SprintStory>> sprintStories = new Dictionary<Prioriteit, IList<SprintStory>>();
            sprintStories.Add(Prioriteit.Must,
                              new List<SprintStory>(item.GetAllOpenSprintStories(Prioriteit.Must)));
            sprintStories.Add(Prioriteit.Should,
                              new List<SprintStory>(item.GetAllOpenSprintStories(Prioriteit.Should)));
            sprintStories.Add(Prioriteit.Could,
                              new List<SprintStory>(item.GetAllOpenSprintStories(Prioriteit.Could)));
            sprintStories.Add(Prioriteit.Would,
                              new List<SprintStory>(item.GetAllOpenSprintStories(Prioriteit.Would)));

            PropertyBag.Add("sprintStories", sprintStories);
            CancelLayout();
        }

        /// <summary>
        /// Maak een lijst van stories en taken die in de sprint gemarkeerd zijn als afgerond
        /// </summary>
        /// <param name="item">The item.</param>
        public void SprintStoriesAfgerond([ARFetch("id")] Sprint item)
        {
            Dictionary<Prioriteit, IList<SprintStory>> sprintStories = new Dictionary<Prioriteit, IList<SprintStory>>();
            sprintStories.Add(Prioriteit.Must,
                              new List<SprintStory>(item.GetSprintStoriesWithClosedTasks(Prioriteit.Must)));
            sprintStories.Add(Prioriteit.Should,
                              new List<SprintStory>(item.GetSprintStoriesWithClosedTasks(Prioriteit.Should)));
            sprintStories.Add(Prioriteit.Could,
                              new List<SprintStory>(item.GetSprintStoriesWithClosedTasks(Prioriteit.Could)));
            sprintStories.Add(Prioriteit.Would,
                              new List<SprintStory>(item.GetSprintStoriesWithClosedTasks(Prioriteit.Would)));

            PropertyBag.Add("sprintStories", sprintStories);

            CancelLayout();
        }

        /// <summary>
        /// De startpagina van iedere sprint
        /// </summary>
        /// <param name="sprint"></param>
        public void Sprint([ARFetch("sprintId")] Sprint sprint)
        {
            Titel = "<a href='/project/project.rails?projectId=" + sprint.Project.Id + "'>" + sprint.Project.Naam + "</a> > " + sprint.Doel;
            PropertyBag.Add("sprint", sprint);
        }

        /// <summary>
        /// Sprint Health
        /// </summary>
        /// <param name="sprint"></param>
        public void Health([ARFetch("sprintId")] Sprint sprint)
        {
            PropertyBag.Add("sprint", sprint);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Planning([ARFetch("sprintId")] Sprint sprint)
        {
            PropertyBag.Add("sprint", sprint);
        }

        public void KoppelTaskAanSprintGebruiker([ARFetch("sprintGebruiker")] SprintGebruiker sprintGebruiker, [ARFetch("task")] Task task)
        {
            sprintGebruiker.PakTaakOp(task);
            SprintGebruikerRepository.Save(sprintGebruiker);

            PropertyBag.Add("sprintGebruiker", sprintGebruiker);
            RenderView("planning_developer");
            CancelLayout();
        }

        /// <summary>
        /// Sprint informatie
        /// </summary>
        /// <param name="item"></param>
        public void SprintInformatie([ARFetch("id")] Sprint item)
        {
            PropertyBag.Add("item", item);
            CancelLayout();
        }

        /// <summary>
        /// Toont het scherm waarop de sprint ingericht kan worden met stories uit het productbacklog
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        public void SprintBacklog([ARFetch("sprintId")] Sprint sprint)
        {
            PropertyBag.Add("sprint", sprint);

            Titel = "<a href='/project/project.rails?projectId=" + sprint.Project.Id + "'>" + sprint.Project.Naam + "</a> > <a href='/sprint/sprint.rails?sprintId=" + sprint.Id + "'>" + sprint.Doel + "</a> > Sprintbacklog";
        }

        /// <summary>
        /// Exporteer sprintbacklog naar excel
        /// </summary>
        /// <param name="sprint">sprint</param>
        public void ExportSprintBackLog([ARFetch("sprintId")] Sprint sprint)
        {
            string fileName = this.excelExportService.ExportSprintBacklog(sprint);

            Response.ContentType = "application/ms-excel";
            Response.AppendHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
            Response.WriteFile(ConfigurationManager.AppSettings["exportLocation"] + fileName);

            CancelView();
        }

        /// <summary>
        /// Koppelt de story aan een sprint.en geeft vervolgens de korte sprintinfo terug aan de sprintplanning
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        /// <param name="story">The story.</param>
        public void KoppelStoryAanSprint([ARFetch("sprintId")] Sprint sprint, [ARFetch("storyId")] Story story)
        {
            SprintStory sprintStory = new SprintStory(sprint, story, story.Schatting);
            sprintStory.SprintBacklogPrioriteit = story.ProductBacklogPrioriteit;
            SprintStoryRepository.Save(sprintStory);

            PropertyBag.Add("gekozenSprint", sprint);
            PropertyBag.Add("story", story);

            RenderView("sprintlogitem", true);
        }

        /// <summary>
        /// Verwijder de story uit de sprint
        /// </summary>
        /// <param name="sprint"></param>
        /// <param name="story"></param>
        public void OntkoppelStoryVanSprint([ARFetch("sprintId")] Sprint sprint, [ARFetch("storyId")] Story story)
        {
            sprint.RemoveStory(story);
            SprintRepository.Save(sprint);

            PropertyBag.Add("gekozenSprint", sprint);
            PropertyBag.Add("story", story);

            RenderView("productbacklogitem", true);
        }

        /// <summary>
        /// Geeft de resterende tijd weer die er beschikbaar is in de sprint
        /// </summary>
        /// <param name="sprint"></param>
        public void sprintPlanningSprintTijd([ARFetch("sprintId")] Sprint sprint)
        {
            PropertyBag.Add("gekozenSprint", sprint);
            CancelLayout();
        }

        /// <summary>
        /// Koppelt de story aan een sprint.
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        /// <param name="story">The story.</param>
        /// <param name="prioriteit">The prioriteit.</param>
        public void KoppelStoryAanSprint([ARFetch("sprintId")] Sprint sprint, [ARFetch("storyId")] Story story,
                                         Prioriteit prioriteit)
        {
            SprintStory sprintStory = new SprintStory(sprint, story, story.Schatting);
            sprintStory.SprintBacklogPrioriteit = prioriteit;
            SprintStoryRepository.Save(sprintStory);

            NameValueCollection args = new NameValueCollection();
            args.Add("sprintId", sprint.Id.ToString());
            args.Add("prioriteit", prioriteit.ToString());

            RedirectToAction("RenderIngeplandeStorieList", args);
        }

        /// <summary>
        /// Maakt een lijst van sprintstories van een prioriteit van een sprint
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        /// <param name="prioriteit">The prioriteit.</param>
        public void RenderIngeplandeStorieList([ARFetch("sprintId")] Sprint sprint, Prioriteit prioriteit)
        {
            SprintStoriesQuery ingeplandeStories = new SprintStoriesQuery();
            ingeplandeStories.Sprint = sprint;
            IList list =
                ingeplandeStories.GetQuery(ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(typeof(ModelBase))).Add(
                    Restrictions.Eq("SprintBacklogPrioriteit", prioriteit)).List();

            PropertyBag.Add("sprintStories", list);
            CancelLayout();
        }

        /// <summary>
        /// Toont een knop waarmee een sprint kan worden afgesloten.
        /// </summary>
        [Layout("projectbeheer")]
        public void Sluit([ARFetch("sprintId")] Sprint sprint)
        {
            PropertyBag.Add("sprint", sprint);
        }

        /// <summary>
        /// Sluit de gegeven sprint af.
        /// </summary>
        [Layout("projectbeheer")]
        public void SprintAfsluiten([ARFetch("sprintId")] Sprint sprint)
        {
            //todo: security!
            IList<Task> taken = sprint.Close();

            SprintRepository.Save(sprint);

            //Het saven van een sprint spacecadet niet door naar de taken. Die moeten we dus apart saven.
            foreach (Task task in taken)
            {
                TaskRepository.Save(task);
            }

            NameValueCollection args = new NameValueCollection();
            args.Add("projectId", sprint.Project.Id.ToString());
            Redirect("project", "overzicht", args);
        }

        /// <summary>
        /// inplannen van de sprint, gekozen vanuit het project.
        /// Er is geen specifieke sprint gekozen.
        /// 
        /// We moeten dus achterhalen in welke sprint we gaan werken.
        /// </summary>
        /// <param name="project"></param>
        public void SprintPlanning([ARFetch("projectId")] Project project)
        {
            Titel = "<a href='/project/project.rails?projectId=" + project.Id + "'>" + project.Naam + "</a> > Sprintplanning";

            IList<Sprint> sprints = project.GetAllOpenSprints();
            PropertyBag.Add("sprints", sprints);

            PropertyBag.Add("project", project);

            /** We moeten altijd zorgen dat zowel de project stories als de sprint stories er zijn **/

            // Hier dus nog de gekozen sprint meesturen
            if (sprints.Count > 0)
            {
                PropertyBag.Add("gekozenSprint", sprints[0]);
            }
        }

        /// <summary>
        /// inplannen van de sprint
        /// </summary>
        /// <param name="sprint"></param>
        public void SprintPlanning([ARFetch("sprintId")] Sprint sprint)
        {
            Project project = sprint.Project;
            Titel = "<a href='/project/project.rails?projectId=" + project.Id + "'>" + project.Naam + "</a> > <a href='/sprint/sprint.rails?sprintId=" + sprint.Id + "'>" + sprint.Doel + "</a> > Sprintplanning";

            IList<Sprint> sprints = project.GetAllOpenSprints();
            PropertyBag.Add("sprints", sprints);
            PropertyBag.Add("sprint", sprint);
            PropertyBag.Add("project", project);

            /** We moeten altijd zorgen dat zowel de project stories als de sprint stories er zijn **/

            // Hier dus nog de gekozen sprint meesturen
            if (sprints.Count > 0)
            {
                PropertyBag.Add("gekozenSprint", sprint);
            }
        }

        /// <summary>
        /// Iets zowel met project en sprint
        /// Geen sprint dan proberen af tehandelen, door eerste sprint de kiezen.
        /// TODO: MARIJN    
        /// </summary>
        public void Stories()
        {
            // Dit moet dus zowel voor een project de stories geven als mede voor de sprint
            CancelLayout();
        }

        public void Stories([ARFetch("sprintId")] Sprint sprint)
        {
            // Dit moet dus zowel voor een project de stories geven als mede voor de sprint
            PropertyBag.Add("stories", sprint.GetAllStories());
            CancelLayout();
        }

        public void Stories([ARFetch("projectId")] Project project)
        {
            // Dit moet dus zowel voor een project de stories geven als mede voor de sprint
            PropertyBag.Add("stories", project.GetAllPlannableStories());
            CancelLayout();
        }

        /// <summary>
        /// Geeft een korte informatie beschrijving van de sprint
        /// </summary>
        public void ShortSprintInfo()
        {
            CancelLayout();
        }

        /// <summary>
        /// Geeft een korte informatie beschrijving van de sprint
        /// </summary>
        public void ShortSprintInfo([ARFetch("sprintId")] Sprint sprint)
        {
            PropertyBag.Add("gekozenSprint", sprint);
            CancelLayout();
        }

        /// <summary>
        /// Geeft een korte informatie beschrijving van de sprint
        /// </summary>
        public void ShortSprintInfo([ARFetch("projectId")] Project project)
        {
            PropertyBag.Add("project", project);
            RenderView("shortprojectinfo", true);
        }

        /// <summary>
        /// Invullen uren per taak
        /// </summary>
        public void Uren()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Burndown([ARFetch("sprintId")] Sprint sprint)
        {
            PropertyBag.Add("sprintstories", sprint.SprintStories);
            PropertyBag.Add("sprint", sprint);

            Titel = "<a href='/project/project.rails?projectId=" + sprint.Project.Id + "'>" + sprint.Project.Naam + "</a> > <a href='/sprint/sprint.rails?sprintId=" + sprint.Id + "'>" + sprint.Doel + "</a> > Burndown";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprint"></param>
        public void ActieveSprintZetten([ARFetch("sprintId")] Sprint sprint)
        {
            Gebruiker gb = CurrentUser;
            gb.ActieveSprint = sprint;

            try
            {
                GebruikerRepository.Save(gb);
                AddPositiveMessageToFlashBag(sprint.Doel + " is nu je actieve sprint.");
            }
            catch (Exception e)
            {
                AddErrorMessageToFlashBag(e.Message);
            }

            RedirectToReferrer();
        }

        /// <summary>
        /// Taken weergeven voor sprint, wat niet de actieve sprint hoeft te zijn.
        /// </summary>
        /// <param name="sprint"></param>
        public void Taken([ARFetch("sprintId")] Sprint sprint)
        {
            PropertyBag.Add("sprint", sprint);
           
            Titel = "<a href='/project/project.rails?projectId=" + sprint.Project.Id + "'>" + sprint.Project.Naam + "</a> > <a href='/sprint/sprint.rails?sprintId=" + sprint.Id + "'>" + sprint.Doel + "</a> > Taken";
            RenderText("Deze pagina wijkt af van het normale dashboard, dit moet dus even gefixed worden.");
        }

        /// <summary>
        /// Sprint voortgang
        /// </summary>
        public void Voortgang([ARFetch("sprintId")] Sprint sprint)
        {
            DateTime startdatum = sprint.StartDatum;
            DateTime einddatum = sprint.EindDatum;

            DateTime temp = startdatum;

            IList<DateTime> werkdagen = new List<DateTime>();

            while (temp <= einddatum)
            {
                if (temp.DayOfWeek != DayOfWeek.Saturday && temp.DayOfWeek != DayOfWeek.Sunday)
                {
                    werkdagen.Add(temp);
                }
                temp = temp.AddDays(1);
            }

            PropertyBag.Add("sprint", sprint);
            PropertyBag.Add("sprintstories", sprint.SprintStories);

            PropertyBag.Add("werkdagen", werkdagen);

            Titel = "<a href='/project/project.rails?projectId=" + sprint.Project.Id + "'>" + sprint.Project.Naam + "</a> > <a href='/sprint/sprint.rails?sprintId=" + sprint.Id + "'>" + sprint.Doel + "</a> > Voortgang";
        }

        #region Uren registeren
        /// <summary>
        /// Urenboek scherm
        /// </summary>
        public void UrenRegistreren([ARFetch("sprintId")] Sprint sprint, DateTime maandag)
        {
            SprintGebruiker sprintGebruiker = sprint.GetSprintUserFor(CurrentUser);
           

            Titel = "<a href='/project/project.rails?projectId=" + sprint.Project.Id + "'>" + sprint.Project.Naam + "</a> > <a href='/sprint/sprint.rails?sprintId=" + sprint.Id + "'>" + sprint.Doel + "</a> > Uren registreren";

            PropertyBag.Add("maandag", maandag);
            PropertyBag.Add("sprint", sprint);
            PropertyBag.Add("sprintGebruiker", sprintGebruiker);
        }

        /// <summary>
        /// Uren boeken op taken van actieve sprint
        /// </summary>
        public void UrenBoeken([ARDataBind("urenregistratie", AutoLoadBehavior.OnlyNested)]UrenRegistratieHelper[] urenRegistraties, [ARFetch("sprintId")] Sprint sprint, DateTime maandag)
        {
            try
            {
                foreach (UrenRegistratieHelper helper in urenRegistraties)
                {
                    helper.MaakUrenRegistratie();
                    TaskRepository.Save(helper.Task);
                }
            }
            catch
            {
                AddErrorMessageToFlashBag("Het opslaan van de urenregistraties is niet gelukt.");
            }
            NameValueCollection args = new NameValueCollection();
            args.Add("sprintId", sprint.Id.ToString());
            args.Add("maandag", maandag.ToString("dd-MM-yyyy"));
            RedirectToAction("urenregistreren", args);
        }
        #endregion

        #region methods
        /// <summary>
        /// Geeft de onopgepakte taken in de gegeven sprint.
        /// </summary>
        /// <param name="sprint">De sprint.</param>
        /// <returns>De onopgepakte taken</returns>
        private static IList GeefOnopgepakteTaken(Sprint sprint)
        {
            NietOpgepakteTakenQuery nietOpgepakteTakenQuery = new NietOpgepakteTakenQuery();
            nietOpgepakteTakenQuery.sprint = sprint;
            return nietOpgepakteTakenQuery.GetQuery(ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(typeof(ModelBase))).List();
        }

        /// <summary>
        /// Geeft de opgepakte taken van anderen dan de gegeven gebruiker.
        /// </summary>
        /// <param name="gebruiker">De gebruiker.</param>
        /// <returns>De taken van anderen.</returns>
        private static IList GeefTakenVanAnderen(Gebruiker gebruiker)
        {
            OpgepakteTakenQuery opgepakteTakenQuery = new OpgepakteTakenQuery();
            opgepakteTakenQuery.Sprint = gebruiker.ActieveSprint;
            opgepakteTakenQuery.BehalveVoorDezeSprintGebruiker = gebruiker.GeefActieveSprintGebruiker();
            return opgepakteTakenQuery.GetQuery(ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(typeof(ModelBase))).List();
        }
        #endregion
    }
}