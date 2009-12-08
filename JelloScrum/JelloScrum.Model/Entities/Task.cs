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
    using Helpers;
    using Interfaces;

    /// <summary>
    /// Represents a Task
    /// </summary>
    [ActiveRecord]
    public class Task : ModelBase, ILoggable
    {
        #region Fields

        private string title = string.Empty;
        private Story story;
        private string description = string.Empty;
        private State state = State.Open;
        private SprintUser assignedUser;
        private DateTime? dateClosed;
        private TimeSpan estimation;

        private IList<TimeRegistration> timeRegistrations = new List<TimeRegistration>();
        private IList<TaskLogMessage> logMessages = new List<TaskLogMessage>();
        private IList<TaskComment> comments = new List<TaskComment>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class.
        /// </summary>
        public Task()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class for the given story.
        /// </summary>
        /// <param name="story">The story.</param>
        public Task(Story story)
        {
            story.AddTask(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class with the given description.
        /// </summary>
        /// <param name="description">The description.</param>
        public Task(string description)
        {
            description = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Title of the task
        /// </summary>
        [Property]
        public virtual string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// The story this task belongs to.
        /// </summary>
        /// <value>The story.</value>
        [BelongsTo]
        public virtual Story Story
        {
            get { return story; }
            set { story = value; }
        }

        /// <summary>
        /// Description of this task
        /// </summary>
        /// <value>The description.</value>
        [Property(SqlType = "ntext")]
        public virtual string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// The status of this task.
        /// </summary>
        /// <value>The status.</value>
        [Property]
        public virtual State State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// The user this task is assigned to.
        /// </summary>
        /// <value>The assigned user.</value>
        [BelongsTo]
        public virtual SprintUser AssignedUser
        {
            get { return assignedUser; }
            set { assignedUser = value; }
        }

        /// <summary>
        /// Estimated time for this task.
        /// </summary>
        /// <value>Estimated time.</value>
        [Property(ColumnType = "TimeSpan"), ValidateNonEmpty("Estimate the time.")]
        public virtual TimeSpan Estimation
        {
            get { return estimation; }
            set { estimation = value; }
        }

        ///// <summary>
        ///// Hulp property voor schatting
        ///// </summary>
        //public virtual string SchattingString
        //{
        //    get { return schattingString; }
        //    set { schattingString = value; }
        //}

        /// <summary>
        /// Gets a readonly list of all timeregistrations belonging to this task.
        /// To add a timeregistration, use <see cref="RegisterTime(Gebruiker, DateTime, Sprint, TimeSpan)"/>.
        /// </summary>
        /// <value>The time registrations.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<TimeRegistration> TimeRegistrations
        {
            get { return new ReadOnlyCollection<TimeRegistration>(timeRegistrations); }
        }

        /// <summary>
        /// The logmessages
        /// </summary>
        /// <value>the logmessages.</value>
        [HasMany(Table = "LogMessage", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<TaskLogMessage> LogMessages
        {
            get { return logMessages; }
            set { logMessages = value; }
        }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        [HasMany(Table = "Comments", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<TaskComment> Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        /// <summary>
        /// The date this task was closed.
        /// </summary>
        [Property(Access = PropertyAccess.NosetterCamelcase)]
        public virtual DateTime? DateClosed
        {
            get { return dateClosed; }
        }
        #endregion

        #region derived properties

        /// <summary>
        /// Gets the name of the user this task is assigned to.
        /// </summary>
        public virtual string AssignedUserName
        {
            get
            {
                return AssignedUser != null ? AssignedUser.User.Name : string.Empty;
            }
        }

        /// <summary>
        /// Gets the available time left for this task.
        /// </summary>
        public virtual TimeSpan RemainingTime
        {
            get
            {
                return (Estimation - TotalTimeSpent());
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register time spent on this task.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="date">The date.</param>
        /// <param name="sprint">De sprint.</param>
        /// <param name="time">The time.</param>
        public virtual void RegisterTime(User user, DateTime date, Sprint sprint, TimeSpan time)
        {
            if (!story.Project.Sprints.Contains(sprint))
            {
                throw new ArgumentException("The given sprint does not belong to this project.", "sprint");
            }

            foreach (TimeRegistration registratie in GetTimeRegistrationsFor(user, sprint, date))
            {
                RemoveTimeRegistration(registratie);
            }

            //only add timeregistrations that actually contain time
            if (time.TotalSeconds == 0)
                return;
            
            TimeRegistration timeRegistration = new TimeRegistration(user, date, sprint, this, time);
            AddTimeRegistration(timeRegistration);
        }

        /// <summary>
        /// Add a comment
        /// </summary>
        /// <param name="comment">The comment.</param>
        public virtual void AddComment(TaskComment comment)
        {
            if (!comments.Contains(comment))
            {
                comments.Add(comment);
            }
        }

        /// <summary>
        /// Remove a comment
        /// </summary>
        /// <param name="comment">The comment.</param>
        public virtual void RemoveComment(TaskComment comment)
        {
            if (comments.Contains(comment))
            {
                comments.Remove(comment);
            }
        }

        /// <summary>
        /// Calculates the total time spent on this task.
        /// </summary>
        /// <returns></returns>
        public virtual TimeSpan TotalTimeSpent()
        {
            TimeSpan total = new TimeSpan(0);
            foreach (TimeRegistration timeRegistration in timeRegistrations)
            {
                total = total.Add(timeRegistration.Time);
            }
            return total;
        }

        /// <summary>
        /// Calculates the total time spent on this task by the given user in the given daterange.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public virtual TimeSpan TotalTimeSpent(User user, DateRange? dateRange)
        {
            TimeSpan total = new TimeSpan(0);
            foreach (TimeRegistration timeRegistration in timeRegistrations)
            {
                if ((user != null || timeRegistration.User == user) && (dateRange == null || dateRange.Value.Overlap(timeRegistration.Date.Date)))
                {
                    total = total.Add(timeRegistration.Time);
                }
            }
            return total;
        }

        /// <summary>
        /// Calculates the total time spent on this task on the given date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public virtual TimeSpan TotalTimeSpent(DateTime date)
        {
            TimeSpan total = new TimeSpan(0);
            foreach (TimeRegistration timeRegistration in timeRegistrations)
            {
                if (timeRegistration.Date.Date == date.Date)
                {
                    total = total.Add(timeRegistration.Time);
                }
            }
            return total;
        }

        /// <summary>
        /// Calculates the total time spent on this task between the given start and end date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public virtual TimeSpan TotalTimeSpent(DateTime startDate, DateTime endDate)
        {
            TimeSpan total = new TimeSpan(0);
            foreach (TimeRegistration timeRegistration in timeRegistrations)
            {
                if (timeRegistration.Date.Date >= startDate.Date && timeRegistration.Date.Date <= endDate.Date)
                {
                    total = total.Add(timeRegistration.Time);
                }
            }
            return total;
        }

        /// <summary>
        /// Calculates the total time spent on this task by the given user on the given date.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public virtual TimeSpan TotaalBestedeTijd(User user, DateTime date)
        {
            TimeSpan total = new TimeSpan(0);
            foreach (TimeRegistration timeRegistration in timeRegistrations)
            {
                if (timeRegistration.User == user && timeRegistration.Date.Date == date.Date)
                {
                    total = total.Add(timeRegistration.Time);
                }
            }
            return total;
        }

        /// <summary>
        /// Gets all timeregistrations of the given user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public virtual IList<TimeRegistration> GetTimeRegistrationsFor(User user)
        {
            IList<TimeRegistration> userTimeRegistrations = new List<TimeRegistration>();
            foreach (TimeRegistration timeRegistration in timeRegistrations)
            {
                if (timeRegistration.User == user)
                {
                    userTimeRegistrations.Add(timeRegistration);
                }
            }
            return userTimeRegistrations;
        }

        /// <summary>
        /// Gets all timeregistrations of the given user for the given sprint and date.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="sprint">The sprint.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public virtual IList<TimeRegistration> GetTimeRegistrationsFor(User user, Sprint sprint, DateTime date)
        {
            IList<TimeRegistration> userTimeRegistrations = new List<TimeRegistration>();
            foreach (TimeRegistration timeRegistration in timeRegistrations)
            {
                if (timeRegistration.User == user && timeRegistration.Sprint == sprint && timeRegistration.Date.ToShortDateString() == date.ToShortDateString())
                {
                    userTimeRegistrations.Add(timeRegistration);
                }
            }
            return userTimeRegistrations;
        }

        /// <summary>
        /// Close this task.
        /// </summary>
        public virtual void Close()
        {
            state = State.Closed;
            dateClosed = DateTime.Now;
        }

        /// <summary>
        /// Sets this task as not-taken.
        /// </summary>
        public virtual void SetAsNotTaken()
        {
            if (assignedUser != null)
            {
                assignedUser.UnAssignTask(this);
            }
        }

        //todo: refactoren / combine with above and create logmessage?
        /// <summary>
        /// Decouple this task from the user it was assigned to, set status as open and create a logmessage.
        /// </summary>
        public virtual void UnassignTaskAndSetSatusAsOpen(string logTitle, string logText)
        {
            if (assignedUser != null)
            {
                logText = logText + " \nWas assigned to: " + assignedUser.User.FullName;
                assignedUser.UnAssignTask(this);
            }

            CreateLogmessage(logTitle, logText);
        }

        /// <summary>
        /// Create a logmessage
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        private void CreateLogmessage(string title, string text)
        {
            TaskLogMessage logMessage = new TaskLogMessage(this, title, text);
            if (!logMessages.Contains(logMessage))
            {
                logMessages.Add(logMessage);
            }
        }

        /// <summary>
        /// Adds a timeregistration.
        /// </summary>
        /// <param name="timeRegistration">The time registration.</param>
        private void AddTimeRegistration(TimeRegistration timeRegistration)
        {
            if (!timeRegistrations.Contains(timeRegistration))
            {
                timeRegistrations.Add(timeRegistration);
            }
            timeRegistration.Task = this;
        }

        /// <summary>
        /// Removes a timeregistration.
        /// </summary>
        /// <param name="timeRegistration">The time registration.</param>
        public virtual void RemoveTimeRegistration(TimeRegistration timeRegistration)
        {
            if (timeRegistrations.Contains(timeRegistration))
            {
                timeRegistrations.Remove(timeRegistration);
            }
        }

        #endregion
    }
}