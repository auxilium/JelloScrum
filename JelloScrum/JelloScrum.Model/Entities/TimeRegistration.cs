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
    using Castle.ActiveRecord;
    using Castle.Components.Validator;

    /// <summary>
    /// The timeregistration object. This is used to register the time a user has spent on a task
    /// </summary>
    [ActiveRecord(Lazy = true)]
    public class TimeRegistration : ModelBase
    {
        #region fields

        private User user;
        private DateTime date;
        private Sprint sprint; 
        private Task task;
        private TimeSpan time;

        #endregion
        
        #region constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRegistration"/> class.
        /// </summary>
        public TimeRegistration()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRegistration"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="date">The date.</param>
        /// <param name="sprint">The sprint.</param>
        /// <param name="task">The task.</param>
        /// <param name="time">The time.</param>
        public TimeRegistration(User user, DateTime date, Sprint sprint, Task task, TimeSpan time)
        {
            if (time.TotalMilliseconds == 0)
                throw new ArgumentOutOfRangeException("time", "A timeregistration should contain time.");
            if (user == null)
                throw new ArgumentNullException("user", "The user can not be null.");
            if (sprint== null)
                throw new ArgumentNullException("sprint", "The sprint can not be null.");
            if (task == null)
                throw new ArgumentNullException("task", "The task can not be null.");
            
            this.user = user;
            this.sprint = sprint;
            this.task = task;

            this.date = date;
            this.time = time;
        }

        #endregion

        #region properties

        /// <summary>
        /// The user
        /// </summary>
        /// <value>THe user.</value>
        [BelongsTo(NotNull = true, Column = "JelloScrumUser")]
        public virtual User User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// The date
        /// </summary>
        /// <value>The date.</value>
        [Property]
        public virtual DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// The sprint
        /// </summary>
        /// <value>The sprint.</value>
        [BelongsTo(NotNull = true)]
        public virtual Sprint Sprint
        {
            get { return sprint; }
            set { sprint = value; }
        }

        /// <summary>
        /// The task.
        /// </summary>
        /// <value>The task.</value>
        [BelongsTo(NotNull = true)]
        public virtual Task Task
        {
            get { return task; }
            set { task = value; }
        }

        /// <summary>
        /// The time
        /// </summary>
        /// <value>The time.</value>
        [Property(ColumnType = "TimeSpan"), ValidateNonEmpty("Not a valid time.")]
        public virtual TimeSpan Time
        {
            get { return time; }
            set { time = value; }
        }

        #endregion

    }
}