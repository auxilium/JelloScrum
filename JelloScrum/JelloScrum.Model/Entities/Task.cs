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
    public class Task : ModelBase, ILogable
    {
        #region Fields

        private string titel = string.Empty;
        private Story story;
        private string omschrijving = string.Empty;
        private Status status = Status.NietOpgepakt;
        private SprintGebruiker behandelaar;
        private DateTime? datumAfgesloten;
        private TimeSpan schatting;

        private IList<TijdRegistratie> tijdRegistraties = new List<TijdRegistratie>();
        private IList<TaskLogBericht> logBerichten = new List<TaskLogBericht>();
        private IList<TaskCommentaarBericht> commentaarBerichten = new List<TaskCommentaarBericht>();

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
            omschrijving = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Title of the task
        /// </summary>
        [Property]
        public virtual string Titel
        {
            get { return titel; }
            set { titel = value; }
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
        public virtual string Omschrijving
        {
            get { return omschrijving; }
            set { omschrijving = value; }
        }

        /// <summary>
        /// The status of this task.
        /// </summary>
        /// <value>The status.</value>
        [Property]
        public virtual Status Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// The user this task is assigned to.
        /// </summary>
        /// <value>The assigned user.</value>
        [BelongsTo]
        public virtual SprintGebruiker Behandelaar
        {
            get { return behandelaar; }
            set { behandelaar = value; }
        }

        /// <summary>
        /// Estimated time for this task.
        /// </summary>
        /// <value>Estimated time.</value>
        [Property(ColumnType = "TimeSpan"), ValidateNonEmpty("Estimate the time.")]
        public virtual TimeSpan Schatting
        {
            get { return schatting; }
            set { schatting = value; }
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
        public virtual IList<TijdRegistratie> TijdRegistraties
        {
            get { return new ReadOnlyCollection<TijdRegistratie>(tijdRegistraties); }
        }

        /// <summary>
        /// The logmessages
        /// </summary>
        /// <value>the logmessages.</value>
        [HasMany(Table = "LogBericht", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<TaskLogBericht> LogBerichten
        {
            get { return logBerichten; }
            set { logBerichten = value; }
        }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        [HasMany(Table = "LogBericht", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<TaskCommentaarBericht> CommentaarBerichten
        {
            get { return commentaarBerichten; }
            set { commentaarBerichten = value; }
        }

        /// <summary>
        /// The date this task was closed.
        /// </summary>
        [Property(Access = PropertyAccess.NosetterCamelcase)]
        public virtual DateTime? DatumAfgesloten
        {
            get { return datumAfgesloten; }
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
                return Behandelaar != null ? Behandelaar.Gebruiker.Naam : string.Empty;
            }
        }

        /// <summary>
        /// Gets the available time left for this task.
        /// </summary>
        public virtual TimeSpan RemainingTime
        {
            get
            {
                return (Schatting - TotalTimeSpent());
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
        public virtual void RegisterTime(Gebruiker user, DateTime date, Sprint sprint, TimeSpan time)
        {
            if (!story.Project.Sprints.Contains(sprint))
            {
                throw new ArgumentException("The given sprint does not belong to this project.", "sprint");
            }

            foreach (TijdRegistratie registratie in GetTimeRegistrationsFor(user, sprint, date))
            {
                RemoveTimeRegistration(registratie);
            }

            //only add timeregistrations that actually contain time
            if (time.TotalSeconds == 0)
                return;
            
            TijdRegistratie timeRegistration = new TijdRegistratie(user, date, sprint, this, time);
            AddTimeRegistration(timeRegistration);
        }

        /// <summary>
        /// Add a comment
        /// </summary>
        /// <param name="comment">The comment.</param>
        public virtual void AddComment(TaskCommentaarBericht comment)
        {
            if (!commentaarBerichten.Contains(comment))
            {
                commentaarBerichten.Add(comment);
            }
        }

        /// <summary>
        /// Remove a comment
        /// </summary>
        /// <param name="comment">The comment.</param>
        public virtual void RemoveComment(TaskCommentaarBericht comment)
        {
            if (commentaarBerichten.Contains(comment))
            {
                commentaarBerichten.Remove(comment);
            }
        }

        /// <summary>
        /// Calculates the total time spent on this task.
        /// </summary>
        /// <returns></returns>
        public virtual TimeSpan TotalTimeSpent()
        {
            TimeSpan total = new TimeSpan(0);
            foreach (TijdRegistratie timeRegistration in tijdRegistraties)
            {
                total = total.Add(timeRegistration.Tijd);
            }
            return total;
        }

        /// <summary>
        /// Calculates the total time spent on this task by the given user in the given daterange.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public virtual TimeSpan TotalTimeSpent(Gebruiker user, DateRange? dateRange)
        {
            TimeSpan total = new TimeSpan(0);
            foreach (TijdRegistratie timeRegistration in tijdRegistraties)
            {
                if ((user != null || timeRegistration.Gebruiker == user) && (dateRange == null || dateRange.Value.Overlap(timeRegistration.Datum.Date)))
                {
                    total = total.Add(timeRegistration.Tijd);
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
            foreach (TijdRegistratie timeRegistration in tijdRegistraties)
            {
                if (timeRegistration.Datum.Date == date.Date)
                {
                    total = total.Add(timeRegistration.Tijd);
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
            foreach (TijdRegistratie timeRegistration in tijdRegistraties)
            {
                if (timeRegistration.Datum.Date >= startDate.Date && timeRegistration.Datum.Date <= endDate.Date)
                {
                    total = total.Add(timeRegistration.Tijd);
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
        public virtual TimeSpan TotaalBestedeTijd(Gebruiker user, DateTime date)
        {
            TimeSpan total = new TimeSpan(0);
            foreach (TijdRegistratie timeRegistration in tijdRegistraties)
            {
                if (timeRegistration.Gebruiker == user && timeRegistration.Datum.Date == date.Date)
                {
                    total = total.Add(timeRegistration.Tijd);
                }
            }
            return total;
        }

        /// <summary>
        /// Gets all timeregistrations of the given user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public virtual IList<TijdRegistratie> GetTimeRegistrationsFor(Gebruiker user)
        {
            IList<TijdRegistratie> userTimeRegistrations = new List<TijdRegistratie>();
            foreach (TijdRegistratie timeRegistration in tijdRegistraties)
            {
                if (timeRegistration.Gebruiker == user)
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
        public virtual IList<TijdRegistratie> GetTimeRegistrationsFor(Gebruiker user, Sprint sprint, DateTime date)
        {
            IList<TijdRegistratie> userTimeRegistrations = new List<TijdRegistratie>();
            foreach (TijdRegistratie timeRegistration in tijdRegistraties)
            {
                if (timeRegistration.Gebruiker == user && timeRegistration.Sprint == sprint && timeRegistration.Datum.ToShortDateString() == date.ToShortDateString())
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
            status = Status.Afgesloten;
            datumAfgesloten = DateTime.Now;
        }

        /// <summary>
        /// Sets this task as not-taken.
        /// </summary>
        public virtual void SetAsNotTaken()
        {
            if (behandelaar != null)
            {
                behandelaar.GeefTaakAf(this);
            }
        }

        //todo: refactoren / combine with above and create logmessage?
        /// <summary>
        /// Decouple this task from the user it was assigned to, set status as open and create a logmessage.
        /// </summary>
        public virtual void UnassignTaskAndSetSatusAsOpen(string logTitle, string logText)
        {
            if (behandelaar != null)
            {
                logText = logText + " \nWas assigned to: " + behandelaar.Gebruiker.VolledigeNaam;
                behandelaar.GeefTaakAf(this);
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
            TaskLogBericht logMessage = new TaskLogBericht(this, title, text);
            if (!logBerichten.Contains(logMessage))
            {
                logBerichten.Add(logMessage);
            }
        }

        /// <summary>
        /// Adds a timeregistration.
        /// </summary>
        /// <param name="timeRegistration">The time registration.</param>
        private void AddTimeRegistration(TijdRegistratie timeRegistration)
        {
            if (!tijdRegistraties.Contains(timeRegistration))
            {
                tijdRegistraties.Add(timeRegistration);
            }
            timeRegistration.Task = this;
        }

        /// <summary>
        /// Removes a timeregistration.
        /// </summary>
        /// <param name="timeRegistration">The time registration.</param>
        public virtual void RemoveTimeRegistration(TijdRegistratie timeRegistration)
        {
            if (tijdRegistraties.Contains(timeRegistration))
            {
                tijdRegistraties.Remove(timeRegistration);
            }
        }

        #endregion
    }
}