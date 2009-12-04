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
    using Castle.Components.Validator;
    using Enumerations;
    using Interfaces;

    /// <summary>
    /// Represents a Story
    /// </summary>
    [ActiveRecord]
    public class Story : ModelBase, ILogable
    {
        #region fields

        private string titel = string.Empty;
        private string omschrijving = string.Empty;
        private string howtoDemo = string.Empty;
        private string notitie = string.Empty;
        private TimeSpan schatting;

        private Project project;
        private Gebruiker aangemaaktDoor;
        private Impact? impact;
        private Prioriteit productBacklogPrioriteit = Prioriteit.Onbekend;
        private StoryType storyType = StoryType.UserStory;

        private IList<StoryLogBericht> logBerichten = new List<StoryLogBericht>();
        private IList<StoryCommentaarBericht> commentaarBerichten = new List<StoryCommentaarBericht>();

        private IList<SprintStory> sprintStories = new List<SprintStory>();
        private IList<Task> tasks = new List<Task>();

        private StoryPoint storyPoints = StoryPoint.Onbekend;
        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Story"/> class.
        /// </summary>
        protected Story()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Story"/> class.
        /// </summary>
        /// <param name="project">The project.</param>  
        /// <param name="createdBy">The user that created this story.</param>
        /// <param name="impact">The impact.</param>
        /// <param name="storyType">Type of the story.</param>
        public Story(Project project, Gebruiker createdBy, Impact? impact, StoryType storyType)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project", "Project can't be null.");
            }

            if (createdBy == null)
            {
                throw new ArgumentNullException("createdBy", "The user creating this story can't be null.");
            }

            project.AddStory(this);
            this.aangemaaktDoor = createdBy;
            this.impact = impact;
            this.storyType = storyType;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the titel.
        /// </summary>
        /// <value>The titel.</value>
        [Property, ValidateNonEmpty("Vul een titel in.")]
        public virtual string Titel
        {
            get { return titel; }
            set { titel = value; }
        }

        /// <summary>
        /// Gets or sets the omschrijving.
        /// </summary>
        /// <value>The omschrijving.</value>
        [Property(SqlType = "ntext"), ValidateNonEmpty("Vul een omschrijving in.")]
        public virtual string Omschrijving
        {
            get { return omschrijving; }
            set { omschrijving = value; }
        }

        /// <summary>
        /// Gets or sets the howto demo.
        /// </summary>
        /// <value>The howto demo.</value>
        [Property(SqlType = "ntext")]
        public virtual string HowtoDemo
        {
            get { return howtoDemo; }
            set { howtoDemo = value; }
        }

        /// <summary>
        /// Gets or sets the notitie.
        /// </summary>
        /// <value>The notitie.</value>
        [Property(SqlType = "ntext")]
        public virtual string Notitie
        {
            get { return notitie; }
            set { notitie = value; }
        }

        /// <summary>
        /// De tijd die geschat is voor deze story
        /// </summary>
        /// <value>De schatting.</value>
        [Property, ValidateNonEmpty("Vul een schatting in.")]
        public virtual TimeSpan Schatting
        {
            get { return schatting; }
            set { schatting = value; }
        }

        /// <summary>
        /// Gets or sets the product backlog.
        /// </summary>
        /// <value>The product backlog.</value>
        [BelongsTo(NotNull = true)]
        public virtual Project Project
        {
            get { return project; }
            set { project = value; }
        }

        /// <summary>
        /// De gebruiker die deze story aangemaakt heeft
        /// </summary>
        [BelongsTo(NotNull = true)]
        public virtual Gebruiker AangemaaktDoor
        {
            get { return aangemaaktDoor; }
            set { aangemaaktDoor = value; }
        }

        /// <summary>
        /// Dee impact.
        /// </summary>
        /// <value>De impact.</value>
        [Property]
        public virtual Impact? Impact
        {
            get { return impact; }
            set { impact = value; }
        }

        /// <summary>
        /// Gets or sets the product backlog prioriteit.
        /// </summary>
        /// <value>The product backlog prioriteit.</value>
        [Property]
        public virtual Prioriteit ProductBacklogPrioriteit
        {
            get { return productBacklogPrioriteit; }
            set { productBacklogPrioriteit = value; }
        }

        /// <summary>
        /// Gets or sets the type of the story.
        /// </summary>
        /// <value>The type of the story.</value>
        [Property]
        public virtual StoryType StoryType
        {
            get { return storyType; }
            set { storyType = value; }
        }

        /// <summary>
        /// Gets or sets the log berichten.
        /// </summary>
        /// <value>The log berichten.</value>
        [HasMany(Table = "LogBericht", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<StoryLogBericht> LogBerichten
        {
            get { return logBerichten; }
            set { logBerichten = value; }
        }

        /// <summary>
        /// Gets or sets the commentaren.
        /// </summary>
        /// <value>The commentaren.</value>
        [HasMany(Table = "CommentaarBericht", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<StoryCommentaarBericht> CommentaarBerichten
        {
            get { return commentaarBerichten; }
            set { commentaarBerichten = value; }
        }

        /// <summary>
        /// Geeft een readonly collectie met de sprintstories van deze story
        /// Let Op: om een nieuwe sprintstory te maken en toe te voegen gebruik je <see cref="Sprint.MaakSprintStoryVoor(Story)"/>
        /// </summary>
        /// <value>The sprints.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<SprintStory> SprintStories
        {
            get { return new ReadOnlyCollection<SprintStory>(sprintStories); }
        }

        /// <summary>
        /// De tasks.
        /// </summary>
        /// <value>De tasks.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<Task> Tasks
        {
            get { return new ReadOnlyCollection<Task>(tasks); }
        }

        /// <summary>
        /// Het aantal storypunten dat door middel van een planning game gerealiseerd is.
        /// </summary>
        [Property]
        public virtual StoryPoint StoryPoints
        {
            get { return storyPoints; }
            set { storyPoints = value; }
        }

        #endregion

        #region derived properties

        /// <summary>
        /// Gets the state of this story based on the states of its tasks
        /// - if all tasks are closed, the story is closed
        /// - if a task is taken, the story is taken
        /// - if no tasks are taken, the story is not taken
        /// todo: different states for stories and tasks?
        /// </summary>
        /// <value>The state.</value>
        public virtual Status Status
        {
            get
            {
                int taken = 0;
                int closed = 0;
                
                foreach (Task task in tasks)
                {
                    if (task.Status == Status.Opgepakt)
                        taken++;

                    if (task.Status == Status.Afgesloten)
                        closed++;
                }

                if (closed == tasks.Count && tasks.Count > 0)
                    return Status.Afgesloten;

                if (taken == 0 || tasks.Count == 0)
                    return Status.NietOpgepakt;

                return Status.Opgepakt;
            }
        }

        /// <summary>
        /// Determines if a story is plannable
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is plannable; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsPlannable
        {
            get
            {
                if (Status == Status.Afgesloten)
                    return false;

                foreach (SprintStory sprintStory in SprintStories)
                {
                    // story has an active sprintstory / sprint
                    if (!sprintStory.Sprint.IsAfgesloten)
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets the value of the storypoints
        /// </summary>
        /// <value>The story points value.</value>
        public virtual int StoryPointsValue
        {
            get
            {
                return (int) Enum.Parse(typeof (StoryPoint), Enum.GetName(typeof (StoryPoint), StoryPoints));
            }
        }

        /// <summary>
        /// Gets the date this story is closed.
        /// A story is closed when the last task is closed, the cosed date
        /// is therefore the same as the date of the last closed task.
        /// </summary>
        /// <value>The closed date, or null if there are still open tasks.</value>
        public virtual DateTime? ClosedDate
        {
            get
            {
                if (Status != Status.Afgesloten)
                    return null;

                DateTime? lastDate = null;

                foreach (Task task in tasks)
                {
                    if (lastDate.HasValue == false || task.DatumAfgesloten > lastDate.Value)
                    {
                        lastDate = task.DatumAfgesloten;
                    }
                }

                return lastDate;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Adds the given task
        /// </summary>
        /// <param name="task">The task.</param>
        public virtual void AddTask(Task task)
        {
            if (!tasks.Contains(task))
                tasks.Add(task);
            task.Story = this;
        }

        /// <summary>
        /// Removes the given task
        /// </summary>
        /// <param name="task">The task.</param>
        public virtual void RemoveTask(Task task)
        {
            if (tasks.Contains(task))
                tasks.Remove(task);
            task.Story = null;
        }

        /// <summary>
        /// Adds the given sprintstory
        /// </summary>
        /// <param name="sprintStory">The sprintstory.</param>
        protected internal virtual void AddSprintStory(SprintStory sprintStory)
        {
            if (!sprintStories.Contains(sprintStory))
                sprintStories.Add(sprintStory);
            sprintStory.Story = this;
        }

        /// <summary>
        /// Adds the given text as comment.
        /// </summary>
        /// <param name="text">The tekst.</param>
        public virtual void AddComment(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                commentaarBerichten.Add(new StoryCommentaarBericht(this, text));
            }
        }

        /// <summary>
        /// Gets the total amount of time spent on this story.
        /// </summary>
        /// <returns></returns>
        public virtual TimeSpan TotalTimeSpent()
        {
            TimeSpan total = new TimeSpan(0);
            foreach (Task task in tasks)
            {
                total = total.Add(task.TotalTimeSpent());
            }
            return total;
        }

        /// <summary>
        /// Gets the total amount of time spent on this story between the given startdate and enddate.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public virtual TimeSpan TotalTimeSpent(DateTime startDate, DateTime endDate)
        {
            TimeSpan total = new TimeSpan(0);
            foreach (Task task in tasks)
            {
                total = total.Add(task.TotalTimeSpent(startDate, endDate));
            }
            return total;
        }

        /// <summary>
        /// Gets all tasks with the given state
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public virtual IList<Task> GetTasksWith(Status state)
        {
            IList<Task> tasksWithState = new List<Task>();
            foreach (Task task in tasks)
            {
                if (task.Status == state)
                    tasksWithState.Add(task);
            }
            return tasksWithState;
        }

        /// <summary>
        /// Gets all timeregistrations for all tasks belonging to this story.
        /// </summary>
        /// <returns></returns>
        public virtual IList<TijdRegistratie> GetTimeRegistrations()
        {
            List<TijdRegistratie> timeRegistrations = new List<TijdRegistratie>();
            foreach (Task task in tasks)
            {
                timeRegistrations.AddRange(task.TijdRegistraties);
            }
            timeRegistrations.Sort(delegate(TijdRegistratie t1, TijdRegistratie t2)
                                      {
                                          return t1.Datum.CompareTo(t2.Datum);
                                      });

            return timeRegistrations;
        }
        
        /// <summary>
        /// Determines whether the estimated time of all tasks is less then the estimated time of this story.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is estimated time of tasks less then estimated time of story]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsEstimatedTimeOfTasksLessThenEstimatedTimeOfStory()
        {
            double timeTasks = 0; 
            foreach (Task task in Tasks)
            {
                timeTasks += task.Schatting.TotalMinutes;
            }

            return timeTasks <= Schatting.TotalMinutes;
        }

        #endregion

    }
}