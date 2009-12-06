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
    /// Een logbericht
    /// </summary>
    public class LogBericht<T> : ModelBase where T : ILoggable
    {
        private string titel = string.Empty;
        private string tekst = string.Empty;
        private DateTime datum = DateTime.Now;
        private T logObject = default(T);


        /// <summary>
        /// Initializes a new instance of the <see cref="LogBericht&lt;T&gt;"/> class.
        /// </summary>
        public LogBericht()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LogBericht&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        public LogBericht(T logObject)
        {
            this.logObject = logObject;
            datum = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogBericht&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        /// <param name="titel">The titel.</param>
        /// <param name="tekst">The tekst.</param>
        public LogBericht(T logObject, string titel, string tekst)
        {
            this.logObject = logObject;
            this.titel = titel;
            this.tekst = tekst;
            datum = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the titel.
        /// </summary>
        /// <value>The titel.</value>
        [Property]
        public virtual string Titel
        {
            get { return titel; }
            set { titel = value; }
        }

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
    }

    /// <summary>
    /// Een tasklogbericht
    /// </summary>
    [ActiveRecord(Table = "LogBericht", DiscriminatorColumn = "Type", DiscriminatorType = "string", DiscriminatorValue = "Task")]
    public class TaskLogBericht : LogBericht<Task>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLogBericht"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        /// <param name="titel">The titel.</param>
        /// <param name="tekst">The tekst.</param>
        public TaskLogBericht(Task logObject, string titel, string tekst) : base(logObject, titel, tekst)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLogBericht"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        public TaskLogBericht(Task logObject) : base(logObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLogBericht"/> class.
        /// </summary>
        public TaskLogBericht()
        {
        }
        
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
    }

    /// <summary>
    /// Een storylogbericht
    /// </summary>
    [ActiveRecord(Table = "LogBericht", DiscriminatorColumn = "Type", DiscriminatorType = "string", DiscriminatorValue = "Story")]
    public class StoryLogBericht : LogBericht<Story>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryLogBericht"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        /// <param name="titel">The titel.</param>
        /// <param name="tekst">The tekst.</param>
        public StoryLogBericht(Story logObject, string titel, string tekst)
            : base(logObject, titel, tekst)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryLogBericht"/> class.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        public StoryLogBericht(Story logObject)
            : base(logObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryLogBericht"/> class.
        /// </summary>
        public StoryLogBericht()
        {
        }

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
    }
}