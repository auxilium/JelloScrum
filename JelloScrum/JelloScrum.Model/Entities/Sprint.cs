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

namespace JelloScrum.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Castle.ActiveRecord;
    using Enumerations;

    /// <summary>
    /// Represents a Sprint
    /// </summary>
    [ActiveRecord]
    public class Sprint : ModelBase
    {
        #region fields

        private string doel = string.Empty;
        private string omschrijving = string.Empty;
        private DateTime startDatum = DateTime.Now;
        private DateTime eindDatum = DateTime.Now;
        private int werkDagen;
        private bool afgesloten;
        private TimeSpan beschikbareUren;

        private Project project;

        private IList<SprintStory> sprintStories = new List<SprintStory>();
        private IList<SprintGebruiker> sprintGebruikers = new List<SprintGebruiker>();

        #endregion

        #region constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Sprint"/> class.
        /// </summary>
        public Sprint()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Sprint"/> class for the given project.
        /// </summary>
        /// <param name="project">The project.</param>
        public Sprint(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            project.AddSprint(this);
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the goal of this sprint.
        /// </summary>
        /// <value>The goal</value>
        [Property]
        public virtual string Doel
        {
            get { return doel; }
            set { doel = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Property(SqlType = "ntext")]
        public virtual string Omschrijving
        {
            get { return omschrijving; }
            set { omschrijving = value; }
        }

        /// <summary>
        /// Gets or sets the start date on which this sprint will start.
        /// </summary>
        /// <value>The start date.</value>
        [Property]
        public virtual DateTime StartDatum
        {
            get { return startDatum; }
            set { startDatum = value; }
        }

        /// <summary>
        /// Gets or sets the end date on which this sprint will end.
        /// </summary>
        /// <value>The end date.</value>
        [Property]
        public virtual DateTime EindDatum
        {
            get { return eindDatum; }
            set { eindDatum = value; }
        }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        [BelongsTo(NotNull = true)]
        public virtual Project Project
        {
            get { return project; }
            set { project = value; }
        }

        /// <summary>
        /// Gets a readonly collection of sprintstories.
        /// To make a new sprintstory and add it to this list, use <see cref="CreateSprintStoryFor(Story)"/>
        /// </summary>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase, OrderBy = "SprintBacklogPrioriteit")]
        public virtual IList<SprintStory> SprintStories
        {
            get { return new ReadOnlyCollection<SprintStory>(sprintStories); }
        }

        /// <summary>
        /// Gets the sprintusers
        /// </summary>
        /// <value>The sprintusers.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<SprintGebruiker> SprintGebruikers
        {
            get { return new ReadOnlyCollection<SprintGebruiker>(sprintGebruikers); }
        }

        /// <summary>
        /// Gets or sets the amount of available time.
        /// </summary>
        /// <value>The available time.</value>
        [Property(ColumnType = "TimeSpan")]
        public virtual TimeSpan BeschikbareUren
        {
            get { return beschikbareUren; }
            set { beschikbareUren = value; }
        }

        /// <summary>
        /// Gets or sets the amount of workdays in this sprint.
        /// </summary>
        /// <value>The amount of workdays.</value>
        [Property]
        public virtual int WerkDagen
        {
            get { return werkDagen; }
            set { werkDagen = value; }
        }

        /// <summary>
        /// Is this sprint closed?
        /// </summary>
        /// <value><c>true</c> if closed; else, <c>false</c>.</value>
        [Property]
        public virtual bool IsAfgesloten
        {
            get { return afgesloten; }
            set { afgesloten = value; }
        }

        #endregion

        #region methods

        /// <summary>
        /// Add a sprintstory to this sprint
        /// </summary>
        /// <param name="sprintStory">The sprintstory.</param>
        protected internal virtual void AddSprintStory(SprintStory sprintStory)
        {
            sprintStory.Sprint = this;
            if (!sprintStories.Contains(sprintStory))
                sprintStories.Add(sprintStory);
        }

        /// <summary>
        /// Searches for the sprintstory for the given story in this sprint 
        /// and removes it from this sprint
        /// </summary>
        /// <param name="story">The story.</param>
        public virtual void RemoveStory(Story story)
        {
            SprintStory sprintStory = GetSprintStoryFor(story);

            if (sprintStory == null)
                return;

            sprintStories.Remove(sprintStory);
            sprintStory.Sprint = null;
        }

        /// <summary>
        /// Add a user with a specified role to this sprint, hereby creating a sprintuser for 
        /// the given user and this sprint. If the sprintuser already exists then 
        /// the existing sprintuser is returned.
        /// </summary>
        /// <param name="gebruiker">The user.</param>
        /// <param name="sprintRol">The role.</param>
        /// <returns>The (new) sprintuser.</returns>
        public virtual SprintGebruiker AddUser(Gebruiker gebruiker, SprintRol sprintRol)
        {
            SprintGebruiker sprintGebruiker = GetSprintUserFor(gebruiker);

            return sprintGebruiker ?? new SprintGebruiker(gebruiker, this, sprintRol);
        }

        /// <summary>
        /// Remove the given user from this sprint. This removes the sprintuser
        /// belonging to the given user and this sprint and removes it from this sprint
        /// </summary>
        /// <param name="user">The user.</param>
        public virtual void RemoveUser(Gebruiker user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            SprintGebruiker sprintUser = user.GeefSprintGebruikerVoor(this);
            if (sprintUser == null)
                return;

            sprintUser.KoppelSprintGebruikerLos();
        }

        /// <summary>
        /// todo: how / why is this differen from above?
        /// Removes the given sprintuser from this sprint.
        /// </summary>
        /// <param name="sprintUser">The sprintuser.</param>
        protected internal virtual void RemoveSprintUser(SprintGebruiker sprintUser)
        {
            sprintGebruikers.Remove(sprintUser);
            sprintUser.Sprint = null;
        }

        /// <summary>
        /// Add the given sprintuser to this sprints collection of sprintusers
        /// </summary>
        /// <param name="sprintUser">The sprintuser.</param>
        protected internal virtual void AddSprintUser(SprintGebruiker sprintUser)
        {
            if (!sprintGebruikers.Contains(sprintUser))
            {
                sprintGebruikers.Add(sprintUser);
            }
            sprintUser.Sprint = this;
        }

        /// <summary>
        /// Create a sprintstory for the given story and this sprint.
        /// </summary>
        /// <param name="story">The story.</param>
        /// <returns></returns>
        public virtual SprintStory CreateSprintStoryFor(Story story)
        {
            foreach (SprintStory ss in sprintStories)
            {
                if (ss.Story == story)
                    return ss;
            }

            return new SprintStory(this, story, story.Schatting);
        }

        /// <summary>
        /// Adds the given workday
        /// </summary>
        /// <param name="workday">The workday.</param>
        public virtual void AddWorkday(WerkDag workday)
        {
            if (!HasWorkday(workday))
                werkDagen += (int) workday;
        }

        /// <summary>
        /// Does this sprint have the given workday?
        /// </summary>
        /// <param name="workday">The workday.</param>
        /// <returns>
        /// 	<c>true</c> if this sprint has the specified workday; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool HasWorkday(WerkDag workday)
        {
            return (int) workday == ((int) workday & werkDagen);
        }

        /// <summary>
        /// Calculates the total amount of time spent in this sprint
        /// </summary>
        /// <returns>The total time spent</returns>
        public virtual TimeSpan TotalTimeSpent()
        {
            TimeSpan total = new TimeSpan(0);
            foreach (SprintStory sprintStory in sprintStories)
            {
                total = total.Add(sprintStory.Story.TotalTimeSpent());
            }
            return total;
        }

        /// <summary>
        /// Gets all open sprintstories
        /// </summary>
        /// <returns>all open sprintstories</returns>
        public virtual IList<SprintStory> GetAllOpenSprintStories()
        {
            IList<SprintStory> stories = new List<SprintStory>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                if (sprintStory.Status != Status.Afgesloten)
                    stories.Add(sprintStory);
            }
            return stories;
        }

        /// <summary>
        /// Gets all open sprintstories with the given SprintBacklog priority
        /// </summary>
        /// <param name="prioriteit">The priority.</param>
        /// <returns></returns>
        public virtual IList<SprintStory> GetAllOpenSprintStories(Prioriteit prioriteit)
        {
            IList<SprintStory> stories = new List<SprintStory>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                if (sprintStory.IsVolledigeOpgepakt == false && sprintStory.SprintBacklogPrioriteit == prioriteit)
                    stories.Add(sprintStory);
            }
            return stories;
        }

        /// <summary>
        /// Get all sprintstories with the given priority that have one or more closed tasks.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <returns></returns>
        public virtual IList<SprintStory> GetSprintStoriesWithClosedTasks(Prioriteit priority)
        {
            IList<SprintStory> stories = new List<SprintStory>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                if (sprintStory.SprintBacklogPrioriteit == priority)
                {
                    foreach (Task task in sprintStory.Story.Tasks)
                    {
                        if (task.Status == Status.Afgesloten)
                        {
                            stories.Add(sprintStory);
                            break;
                        }
                    }
                }
            }
            return stories;
        }

        /// <summary>
        /// Gets all stories that have a sprintstory in this sprint.
        /// </summary>
        /// <returns></returns>
        public virtual IList<Story> GetAllStories()
        {
            IList<Story> stories = new List<Story>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                stories.Add(sprintStory.Story);
            }
            return stories;
        }

        /// <summary>
        /// Gets the sprintstory for the given story in this sprint.
        /// </summary>
        /// <param name="story">The story.</param>
        /// <returns></returns>
        public virtual SprintStory GetSprintStoryFor(Story story)
        {
            foreach (SprintStory sprintStory in sprintStories)
            {
                if (sprintStory.Story == story)
                    return sprintStory;
            }
            return null;
        }

        ///// <summary>
        ///// Deze method zorgt voor de synchronisatie van de huidige sprintgebruikers met de gegevenlijst
        ///// </summary>
        ///// <param name="gebruikersLijst"></param>
        //public virtual void VerwerkNieuweIngedeeldeGebruikersLijst(IList<Gebruiker> gebruikersLijst)
        //{
        //    // de huidige lijst doorlopen en alles dat niet in de nieuwe lijst voorkomt verwijderen.
        //    foreach (SprintGebruiker sprintGebruiker in new List<SprintGebruiker>(sprintGebruikers))
        //    {
        //        if (gebruikersLijst.Contains(sprintGebruiker.Gebruiker))
        //            continue;

        //        sprintGebruiker.KoppelSprintGebruikerLos();
        //    }

        //    foreach (Gebruiker gebruiker in gebruikersLijst)
        //    {
        //        if (GeefSprintGebruikerVoor(gebruiker) == null)
        //        {
        //            AddUser(gebruiker, 0);
        //        }
        //    }
        //}

        /// <summary>
        /// Gets all users that have a sprintuser in this sprint.
        /// </summary>
        /// <returns></returns>
        public virtual IList<Gebruiker> GetAllUsers()
        {
            IList<Gebruiker> users = new List<Gebruiker>();
            foreach (SprintGebruiker sprintUser in sprintGebruikers)
            {
                users.Add(sprintUser.Gebruiker);
            }
            return users;
        }

        /// <summary>
        /// Closes this sprint.
        /// This means that all tasks in this sprint that are 'taken' get their status changed to open. 
        /// Each task gets a logmessage describing why the status changed and who was working on that task.
        /// </summary>
        public virtual IList<Task> Close()
        {
            IList<Task> tasks = new List<Task>();

            foreach (SprintStory sprintStory in sprintStories)
            {
                foreach (Task task in sprintStory.Story.Tasks)
                {
                    if (task.Status == Status.Opgepakt)
                    {
                        tasks.Add(task);
                        task.UnassignTaskAndSetSatusAsOpen("Sprint closed", "This sprint is closed. ");
                    }
                }
            }

            afgesloten = true;

            return tasks;
        }

        /// <summary>
        /// Gets the sprintuser for the given user in this sprint.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public virtual SprintGebruiker GetSprintUserFor(Gebruiker user)
        {
            foreach (SprintGebruiker sprintUser in sprintGebruikers)
            {
                if (sprintUser.Gebruiker == user)
                    return sprintUser;
            }
            return null;
        }

        /// <summary>
        /// Gets all tasks that belong to a story that has a sprintstory in this sprint.
        /// </summary>
        /// <returns></returns>
        public virtual IList<Task> GetAllTasks()
        {
            IList<Task> sprintTasks = new List<Task>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                foreach (Task task in sprintStory.Story.Tasks)
                {
                    sprintTasks.Add(task);
                }
            }
            return sprintTasks;
        }

        ///// <summary>
        ///// Gets all tasks taken by sprintusers other than the given sprintuser.
        ///// </summary>
        ///// <param name="sprintUser">The sprintuser.</param>
        ///// <returns></returns>
        //public virtual IList<Task> GetAllTasksTakenBySprintUsersOtherThan(SprintGebruiker sprintUser)
        //{
        //    IList<Task> sprintTasks = new List<Task>();
        //    foreach (SprintStory sprintStory in sprintStories)
        //    {
        //        foreach (Task task in sprintStory.Story.Tasks)
        //        {
        //            if (task.Status != Status.Opgepakt && task.Behandelaar != sprintUser)
        //                sprintTasks.Add(task);
        //        }
        //    }
        //    return sprintTasks;
        //}

        /// <summary>
        /// Gets all tasks with the given status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public virtual IList<Task> GetAllTasksWith(Status status)
        {
            IList<Task> sprintTasks = new List<Task>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                foreach (Task task in sprintStory.Story.Tasks)
                {
                    if(task.Status == status)
                        sprintTasks.Add(task);
                }
            }
            return sprintTasks;
        }

        ///// <summary>
        ///// Geeft alle taken terug die niet door de gespecificeerde sprintgebruiker zijn opgepakt.
        ///// </summary>
        ///// <returns>Lijst met taken</returns>
        //public virtual IList<Task> GeefAndermansOfNietOpgepakteTaken(SprintGebruiker gebruiker)
        //{
        //    IList<Task> sprintTasks = new List<Task>();
        //    foreach (SprintStory sprintStory in sprintStories)
        //    {
        //        foreach (Task task in sprintStory.Story.Tasks)
        //        {
        //            if (task.Behandelaar != gebruiker)
        //                sprintTasks.Add(task);
        //        }
        //    }
        //    return sprintTasks;
        //}

        /// <summary>
        /// Gets the total time estimated for all stories in this sprint.
        /// </summary>
        /// <returns></returns>
        public virtual TimeSpan GetTotalTimeEstimatedForAllStories()
        {
            TimeSpan totalTime = new TimeSpan();

            foreach (SprintStory sprintStory in sprintStories)
            {
                totalTime += sprintStory.Story.Schatting;
            }

            return totalTime;
        }

        /// <summary>
        /// Gets the remaining amount of time available.
        /// </summary>
        /// <returns></returns>
        public virtual TimeSpan RemainingTimeAvailable()
        {
            TimeSpan remainingTime = BeschikbareUren - GetTotalTimeEstimatedForAllStories();

            return remainingTime;
        }
        
        /// <summary>
        /// Gets the total estimated time for stories that are not closed til the given date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public virtual TimeSpan GetTotalEstimatedTimeForNotClosedStoriesTil(DateTime date)
        {
            TimeSpan totalTime = new TimeSpan();

            foreach (SprintStory sprintStory in sprintStories)
            {
                if (sprintStory.Story.Status != Status.Afgesloten || (sprintStory.Story.Status == Status.Afgesloten && sprintStory.Story.ClosedDate.Value.Date > date.Date))
                    totalTime += sprintStory.Story.Schatting;
            }

            return totalTime;
        }

        #endregion
    }
}