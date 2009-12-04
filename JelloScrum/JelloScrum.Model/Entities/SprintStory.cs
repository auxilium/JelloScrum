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
    using Enumerations;

    /// <summary>
    /// Represents a Story within a sprint. The story in the sprint will hold information 
    /// for the story that is only availible in this sprint. It's possible that a 
    /// story isn't finished in one sprint and so will be moved back to the 
    /// productbacklog. Once this is done the story could be placed in another 
    /// sprint with another status, priority and with another Gebruiker.
    /// </summary>
    [ActiveRecord]
    public class SprintStory : ModelBase
    {
        #region fields

        private Prioriteit sprintBacklogPrioriteit = Prioriteit.Onbekend;
        private Sprint sprint = null;
        private Story story = null;
        private TimeSpan schatting;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SprintStory"/> class.
        /// </summary>
        public SprintStory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SprintStory"/> class.
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        /// <param name="story">The story.</param>
        /// <param name="schatting">The schatting.</param>
        public SprintStory(Sprint sprint, Story story, TimeSpan schatting)
        {
            if (sprint == null)
                throw new ArgumentNullException("sprint", "De sprint mag niet null zijn.");
            if (story == null)
                throw new ArgumentNullException("story", "De story mag niet null zijn.");

            sprint.AddSprintStory(this);
            story.AddSprintStory(this);

            this.schatting = schatting;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the sprint backlog prioriteit.
        /// </summary>
        /// <value>The sprint backlog prioriteit.</value>
        [Property]
        public virtual Prioriteit SprintBacklogPrioriteit
        {
            get { return sprintBacklogPrioriteit; }
            set { sprintBacklogPrioriteit = value; }
        }

        /// <summary>
        /// Gets or sets the sprints where this story is part of.
        /// </summary>
        /// <value>The sprint.</value>
        [BelongsTo(NotNull = true)]
        public virtual Sprint Sprint
        {
            get { return sprint; }
            set { sprint = value; }
        }

        /// <summary>
        /// Gets or sets the story.
        /// </summary>
        /// <value>The story.</value>
        [BelongsTo(NotNull = true)]
        public virtual Story Story
        {
            get { return story; }
            set { story = value; }
        }

        /// <summary>
        /// Gets or sets the schatting.
        /// </summary>
        /// <value>The schatting.</value>
        [Property(ColumnType = "TimeSpan")]
        public virtual TimeSpan Schatting
        {
            get { return schatting; }
            set { schatting = value; }
        }

        #endregion

        #region derived properties

        /// <summary>
        /// Is deze sprintstory volledige opgepakt met alle onderliggende taken?
        /// </summary>
        /// <value>
        /// 	<c>true</c> als alle taken van de story van deze sprintstory zijn opgepakt, anders <c>false</c>.
        /// </value>
        public virtual bool IsVolledigeOpgepakt
        {
            get
            {
                if (story.GetTasksWith(Status.NietOpgepakt).Count == 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// De status van de story
        /// </summary>
        /// <value>De status van de Story.</value>
        public virtual Status Status
        {
            get { return story.Status; }
        }

        #endregion
    }
}
