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
    /// Commentaar Bericht
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommentaarBericht<T> : ModelBase where T : ILoggable
    {
        #region fields

        private string tekst = string.Empty;
        private DateTime datum = DateTime.Now;
        private T logObject = default(T);
        private Gebruiker gebruiker = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentaarBericht&lt;T&gt;"/> class.
        /// </summary>
        public CommentaarBericht()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentaarBericht&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        public CommentaarBericht(T logObject)
        {
            this.logObject = logObject;
            datum = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentaarBericht&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        /// <param name="tekst">The tekst.</param>
        public CommentaarBericht(T logObject, string tekst)
        {
            this.logObject = logObject;
            this.tekst = tekst;
            datum = DateTime.Now;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the tekst.
        /// </summary>
        /// <value>The tekst.</value>
        [Property]
        public virtual string Tekst
        {
            get { return tekst; }
            set { tekst = value; }
        }

        /// <summary>
        /// Gets or sets the datum.
        /// </summary>
        /// <value>The datum.</value>
        [Property]
        public virtual DateTime Datum
        {
            get { return datum; }
            set { datum = value; }
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
        /// Gets or sets the gebruiker.
        /// </summary>
        /// <value>The gebruiker.</value>
        [BelongsTo]
        public virtual Gebruiker Gebruiker
        {
            get { return gebruiker; }
            set { gebruiker = value; }
        }

        #endregion
    }

    /// <summary>
    /// Een tasklogbericht
    /// </summary>
    [ActiveRecord(Table = "CommentaarBericht", DiscriminatorColumn = "Type", DiscriminatorType = "string", DiscriminatorValue = "Task")]
    public class TaskCommentaarBericht : CommentaarBericht<Task>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCommentaarBericht"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        /// <param name="tekst">The tekst.</param>
        public TaskCommentaarBericht(Task logObject, string tekst) : base(logObject, tekst)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCommentaarBericht"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        public TaskCommentaarBericht(Task logObject) : base(logObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCommentaarBericht"/> class.
        /// </summary>
        public TaskCommentaarBericht()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the log object.
        /// </summary>
        /// <value>The log object.</value>
        [BelongsTo]
        public override Task LogObject
        {
            get { return base.LogObject; }
            set { base.LogObject = value; }
        }

        #endregion
    }

    /// <summary>
    /// Een storylogbericht
    /// </summary>
    [ActiveRecord(Table = "CommentaarBericht", DiscriminatorColumn = "Type", DiscriminatorType = "string", DiscriminatorValue = "Story")]
    public class StoryCommentaarBericht : CommentaarBericht<Story>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCommentaarBericht"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        /// <param name="tekst">The tekst.</param>
        public StoryCommentaarBericht(Story logObject, string tekst) : base(logObject, tekst)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCommentaarBericht"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        public StoryCommentaarBericht(Story logObject) : base(logObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCommentaarBericht"/> class.
        /// </summary>
        public StoryCommentaarBericht()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the log object.
        /// </summary>
        /// <value>The log object.</value>
        [BelongsTo]
        public override Story LogObject
        {
            get { return base.LogObject; }
            set { base.LogObject = value; }
        }

        #endregion
    }
}