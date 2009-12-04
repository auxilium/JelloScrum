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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;
    using Enumerations;

    /// <summary>
    /// Represents a Project
    /// </summary>
    [ActiveRecord]
    public class Project : ModelBase
    {
        #region fields

        private string naam = string.Empty;
        private string omschrijving = string.Empty;

        private IList<Story> stories = new List<Story>();
        private IList<Sprint> sprints = new List<Sprint>();

        private IList<ProjectShortList> projectShortList = new List<ProjectShortList>();

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        public Project()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class with the given name and description.
        /// </summary>
        /// <param name="name">The naam.</param>
        /// <param name="description">The description.</param>
        public Project(string name, string description)
        {
            this.naam = name;
            this.omschrijving = description;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Property, ValidateNonEmpty("Please provide a name.")]
        public virtual string Naam
        {
            get { return naam; }
            set { naam = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Property(SqlType = "ntext")]
        public virtual string Omschrijving
        {
            get { return omschrijving; }
            set { omschrijving = value; }
        }

        /// <summary>
        /// Gets a readonly list of all stories
        /// To add a story use <see cref="AddStory(Story)"/>
        /// </summary>
        /// <value>The stories.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true,
            Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<Story> Stories
        {
            get { return new ReadOnlyCollection<Story>(stories); }
        }

        /// <summary>
        /// Gets a readonly list of all sprints belonging to this project.
        /// To add a sprint use <see cref="AddSprint(Sprint)"/>
        /// </summary>
        /// <value>De sprints.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true,
            Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<Sprint> Sprints
        {
            get { return new ReadOnlyCollection<Sprint>(sprints); }
        }

        /// <summary>
        /// Gets a readonly collection of projectshortlists
        /// </summary>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<ProjectShortList> ProjectShortList
        {
            get { return new ReadOnlyCollection<ProjectShortList>(projectShortList); }
        }

        #endregion

        #region methods

        /// <summary>
        /// Adds the given story
        /// </summary>
        /// <param name="story">The story.</param>
        public virtual void AddStory(Story story)
        {
            if (!stories.Contains(story))
                stories.Add(story);

            story.Project = this;
        }

        /// <summary>
        /// Adds the given sprint
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        public virtual void AddSprint(Sprint sprint)
        {
            if (!sprints.Contains(sprint))
                sprints.Add(sprint);

            sprint.Project = this;
        }

        /// <summary>
        /// Gets all stories without a priority.
        /// </summary>
        /// <returns></returns>
        public virtual IList<Story> GetAllStoriesWithUndefinedPriorities()
        {
            List<Story> storiesWithoudPriorities = new List<Story>();
            foreach (Story story in Stories)
            {
                if (story.ProductBacklogPrioriteit == Prioriteit.Onbekend)
                    storiesWithoudPriorities.Add(story);
            }
            return storiesWithoudPriorities;
        }

        /// <summary>
        /// Gets all stories that can be planned.
        /// </summary>
        /// <returns></returns>
        public virtual IList<Story> GetAllPlannableStories()
        {
            List<Story> plannableStories = new List<Story>();
            foreach (Story story in Stories)
            {
                if(story.IsPlannable)
                    plannableStories.Add(story);
            }
            return plannableStories;
        }

        /// <summary>
        /// Gets all open sprints
        /// </summary>
        /// <returns></returns>
        public virtual IList<Sprint> GetAllOpenSprints()
        {
            IList<Sprint> notClosedSprints = new List<Sprint>();
            foreach (Sprint sprint in sprints)
            {
                if (!sprint.IsAfgesloten)
                    notClosedSprints.Add(sprint);
            }
            return notClosedSprints;
        }

        #endregion
    }
}