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
    using Interfaces;

    /// <summary>
    /// Represents a comment
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Comment<T> : ModelBase where T : ILoggable
    {
        #region fields

        private string text = string.Empty;
        private DateTime date = DateTime.Now;
        private T logObject;
        private User user;

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment&lt;T&gt;"/> class.
        /// </summary>
        public Comment()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        public Comment(T logObject)
        {
            this.logObject = logObject;
            date = DateTime.Now;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        /// <param name="text">The text.</param>
        public Comment(T logObject, string text)
        {
            this.logObject = logObject;
            this.text = text;
            date = DateTime.Now;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [Property]
        public virtual string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        [Property]
        public virtual DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// Gets or sets the log object.
        /// </summary>
        /// <value>The log object.</value>
        public virtual T LogObject
        {
            get { return logObject; }
            set { logObject = value; }
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        [BelongsTo(Column = "JelloScrumUser")]
        public virtual User User
        {
            get { return user; }
            set { user = value; }
        }

        #endregion
    }

    /// <summary>
    /// Represents a taskcomment
    /// </summary>
    [ActiveRecord(Table = "Comment", DiscriminatorColumn = "Type", DiscriminatorType = "string", DiscriminatorValue = "Task")]
    public class TaskComment : Comment<Task>
    {
        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskComment"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        /// <param name="text">The text.</param>
        public TaskComment(Task logObject, string text) : base(logObject, text)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskComment"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        public TaskComment(Task logObject) : base(logObject)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskComment"/> class.
        /// </summary>
        public TaskComment()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the task object.
        /// </summary>
        /// <value>The task object.</value>
        [BelongsTo]
        public override Task LogObject
        {
            get { return base.LogObject; }
            set { base.LogObject = value; }
        }

        #endregion
    }

    /// <summary>
    /// Represents a storycomment
    /// </summary>
    [ActiveRecord(Table = "Comment", DiscriminatorColumn = "Type", DiscriminatorType = "string", DiscriminatorValue = "Story")]
    public class StoryComment : Comment<Story>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryComment"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        /// <param name="text">The text.</param>
        public StoryComment(Story logObject, string text) : base(logObject, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryComment"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        public StoryComment(Story logObject) : base(logObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryComment"/> class.
        /// </summary>
        public StoryComment()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the story object.
        /// </summary>
        /// <value>The story object.</value>
        [BelongsTo]
        public override Story LogObject
        {
            get { return base.LogObject; }
            set { base.LogObject = value; }
        }

        #endregion
    }
}