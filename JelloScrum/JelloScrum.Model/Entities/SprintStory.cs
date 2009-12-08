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

        private Priority sprintBacklogPriority = Priority.Unknown;
        private Sprint sprint;
        private Story story;
        private TimeSpan estimation;

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

            this.estimation = schatting;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the sprint backlog priority.
        /// </summary>
        /// <value>The sprint backlog priority.</value>
        [Property]
        public virtual Priority SprintBacklogPriority
        {
            get { return sprintBacklogPriority; }
            set { sprintBacklogPriority = value; }
        }

        /// <summary>
        /// Gets or sets the sprints this story is part of.
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
        /// Gets or sets the estimation.
        /// </summary>
        /// <value>The estimation.</value>
        [Property(ColumnType = "TimeSpan")]
        public virtual TimeSpan Estimation
        {
            get { return estimation; }
            set { estimation = value; }
        }

        #endregion

        #region derived properties

        /// <summary>
        /// Are all tasks of this sprintstory taken?
        /// </summary>
        /// <value><c>true</c> if [all tasks are taken]; otherwise, <c>false</c>.</value>
        public virtual bool AllTasksAreTaken
        {
            get
            {
                if (story.GetTasksWith(State.Open).Count == 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// The state of the story
        /// </summary>
        /// <value>The state.</value>
        public virtual State State
        {
            get { return story.State; }
        }

        #endregion
    }
}
