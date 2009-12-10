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
    public class Story : ModelBase, ILoggable
    {
        #region fields

        private string title = string.Empty;
        private string description = string.Empty;
        private string howtoDemo = string.Empty;
        private string note = string.Empty;
        private TimeSpan schatting;

        private Project project;
        private User createdBy;
        private Impact? impact;
        private Priority productBacklogPriority = Priority.Unknown;
        private StoryType storyType = StoryType.UserStory;

        private IList<StoryLogMessage> logMessages = new List<StoryLogMessage>();
        private IList<StoryComment> comments = new List<StoryComment>();

        private IList<SprintStory> sprintStories = new List<SprintStory>();
        private IList<Task> tasks = new List<Task>();

        private StoryPoint storyPoints = StoryPoint.Unknown;
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
        public Story(Project project, User createdBy, Impact? impact, StoryType storyType)
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
            this.createdBy = createdBy;
            this.impact = impact;
            this.storyType = storyType;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Property, ValidateNonEmpty("Please provide a title.")]
        public virtual string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Property(SqlType = "ntext"), ValidateNonEmpty("Please provide a description.")]
        public virtual string Description
        {
            get { return description; }
            set { description = value; }
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
        /// Gets or sets the note.
        /// </summary>
        /// <value>The note.</value>
        [Property(SqlType = "ntext")]
        public virtual string Note
        {
            get { return note; }
            set { note = value; }
        }

        /// <summary>
        /// The estimated time this story should take
        /// </summary>
        /// <value>The estimation.</value>
        [Property, ValidateNonEmpty("Please provide an estimation.")]
        public virtual TimeSpan Estimation
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
        /// The user that created this story
        /// </summary>
        [BelongsTo(NotNull = true)]
        public virtual User CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// The impact.
        /// </summary>
        /// <value>The impact.</value>
        [Property]
        public virtual Impact? Impact
        {
            get { return impact; }
            set { impact = value; }
        }

        /// <summary>
        /// Gets or sets the product backlog priority.
        /// </summary>
        /// <value>The product backlog priority.</value>
        [Property]
        public virtual Priority ProductBacklogPriority
        {
            get { return productBacklogPriority; }
            set { productBacklogPriority = value; }
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
        /// Gets or sets the log messages.
        /// </summary>
        /// <value>The log messages.</value>
        [HasMany(Table = "LogMessage", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<StoryLogMessage> LogMessages
        {
            get { return logMessages; }
            set { logMessages = value; }
        }

        /// <summary>
        /// Gets or sets the commentaren.
        /// </summary>
        /// <value>The commentaren.</value>
        [HasMany(Table = "Comment", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<StoryComment> Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        /// <summary>
        /// Gets a readonly collection of sprintstories belonging to this story
        /// To create a new sprintstory, use <see cref="Sprint.CreateSprintStoryFor(Story)"/>
        /// </summary>
        /// <value>The sprintstories.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<SprintStory> SprintStories
        {
            get { return new ReadOnlyCollection<SprintStory>(sprintStories); }
        }

        /// <summary>
        /// Gets a readonly collection of tasks belonging to this story.
        /// </summary>
        /// <value>The tasks.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<Task> Tasks
        {
            get { return new ReadOnlyCollection<Task>(tasks); }
        }

        /// <summary>
        /// The amount of storypoints that has been estimated in a game of planning poker.
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
        public virtual State State
        {
            get
            {
                int taken = 0;
                int closed = 0;
                
                foreach (Task task in tasks)
                {
                    if (task.State == State.Taken)
                        taken++;

                    if (task.State == State.Closed)
                        closed++;
                }

                if (closed == tasks.Count && tasks.Count > 0)
                    return State.Closed;

                if (taken == 0 || tasks.Count == 0)
                    return State.Open;

                return State.Taken;
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
                if (State == State.Closed)
                    return false;

                foreach (SprintStory sprintStory in SprintStories)
                {
                    // story has an active sprintstory / sprint
                    if (!sprintStory.Sprint.IsClosed)
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
                if (State != State.Closed)
                    return null;

                DateTime? lastDate = null;

                foreach (Task task in tasks)
                {
                    if (lastDate.HasValue == false || task.DateClosed > lastDate.Value)
                    {
                        lastDate = task.DateClosed;
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
                comments.Add(new StoryComment(this, text));
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
        public virtual IList<Task> GetTasksWith(State state)
        {
            IList<Task> tasksWithState = new List<Task>();
            foreach (Task task in tasks)
            {
                if (task.State == state)
                    tasksWithState.Add(task);
            }
            return tasksWithState;
        }

        /// <summary>
        /// Gets all timeregistrations for all tasks belonging to this story.
        /// </summary>
        /// <returns></returns>
        public virtual IList<TimeRegistration> GetTimeRegistrations()
        {
            List<TimeRegistration> timeRegistrations = new List<TimeRegistration>();
            foreach (Task task in tasks)
            {
                timeRegistrations.AddRange(task.TimeRegistrations);
            }
            timeRegistrations.Sort(delegate(TimeRegistration t1, TimeRegistration t2)
                                      {
                                          return t1.Date.CompareTo(t2.Date);
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
                timeTasks += task.Estimation.TotalMinutes;
            }

            return timeTasks <= Estimation.TotalMinutes;
        }

        #endregion

    }
}