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
    /// A logmessage
    /// </summary>
    public class LogMessage<T> : ModelBase where T : ILoggable
    {
        #region fields

        private string title = string.Empty;
        private string text = string.Empty;
        private DateTime date = DateTime.Now;
        private T logObject;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage&lt;T&gt;"/> class.
        /// </summary>
        public LogMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        public LogMessage(T logObject)
        {
            this.logObject = logObject;
            date = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        public LogMessage(T logObject, string title, string text)
        {
            this.logObject = logObject;
            this.title = title;
            this.text = text;
            date = DateTime.Now;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Property]
        public virtual string Title
        {
            get { return title; }
            set { title = value; }
        }

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

        #endregion
    }

    /// <summary>
    /// A task logmessage
    /// </summary>
    [ActiveRecord(Table = "LogMessage", DiscriminatorColumn = "Type", DiscriminatorType = "string", DiscriminatorValue = "Task")]
    public class TaskLogMessage : LogMessage<Task>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLogMessage"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        public TaskLogMessage(Task task, string title, string text) : base(task, title, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLogMessage"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public TaskLogMessage(Task task) : base(task)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLogMessage"/> class.
        /// </summary>
        public TaskLogMessage()
        {
        }
        
        /// <summary>
        /// Gets or sets the task.
        /// </summary>
        /// <value>The task.</value>
        [BelongsTo]
        public override Task LogObject
        {
            get { return base.LogObject; }
            set { base.LogObject = value; }
        }
    }

    /// <summary>
    /// A story logmessage
    /// </summary>
    [ActiveRecord(Table = "LogMessage", DiscriminatorColumn = "Type", DiscriminatorType = "string", DiscriminatorValue = "Story")]
    public class StoryLogMessage : LogMessage<Story>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryLogMessage"/> class.
        /// </summary>
        /// <param name="story">The story.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        public StoryLogMessage(Story story, string title, string text) : base(story, title, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryLogMessage"/> class.
        /// </summary>
        /// <param name="story">The story.</param>
        public StoryLogMessage(Story story) : base(story)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryLogMessage"/> class.
        /// </summary>
        public StoryLogMessage()
        {
        }

        /// <summary>
        /// Gets or sets the story
        /// </summary>
        /// <value>The story.</value>
        [BelongsTo]
        public override Story LogObject
        {
            get { return base.LogObject; }
            set { base.LogObject = value; }
        }
    }
}