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
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        /// <param name="naam">De naam.</param>
        /// <param name="omschrijving">De omschrijving.</param>
        public Project(string naam, string omschrijving)
        {
            this.naam = naam;
            this.omschrijving = omschrijving;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the naam.
        /// </summary>
        /// <value>The naam.</value>
        [Property, ValidateNonEmpty("Vul een naam in.")]
        public virtual string Naam
        {
            get { return naam; }
            set { naam = value; }
        }

        /// <summary>
        /// Gets or sets the omschrijving.
        /// </summary>
        /// <value>The omschrijving.</value>
        [Property(SqlType = "ntext")]
        public virtual string Omschrijving
        {
            get { return omschrijving; }
            set { omschrijving = value; }
        }

        /// <summary>
        /// Geeft een readonly lijst met alle stories die bij dit project horen.
        /// Let Op: om een story toe te voegen gebruik je <see cref="VoegStoryToe(Story)"/>
        /// </summary>
        /// <value>De stories.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true,
            Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<Story> Stories
        {
            get { return new ReadOnlyCollection<Story>(stories); }
        }

        /// <summary>
        /// Geeft een readonly lijst met alle sprints die bij dit project horen.
        /// Let Op: om een sprint toe te voegen gebruik je <see cref="VoegSprintToe(Sprint)"/>
        /// </summary>
        /// <value>De sprints.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true,
            Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<Sprint> Sprints
        {
            get { return new ReadOnlyCollection<Sprint>(sprints); }
        }

        /// <summary>
        /// De lijst van projecten die op de shortlist staan
        /// </summary>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<ProjectShortList> ProjectShortList
        {
            get { return new ReadOnlyCollection<ProjectShortList>(projectShortList); }
        }

        #endregion

        #region methods

        /// <summary>
        /// Voeg een story toe aan dit project
        /// </summary>
        /// <param name="story">De story.</param>
        public virtual void VoegStoryToe(Story story)
        {
            if (!stories.Contains(story))
                stories.Add(story);
            story.Project = this;
        }

        /// <summary>
        /// Voeg een sprint toe aan dit project.
        /// </summary>
        /// <param name="sprint">De sprint.</param>
        public virtual void VoegSprintToe(Sprint sprint)
        {
            if (!sprints.Contains(sprint))
                sprints.Add(sprint);
            sprint.Project = this;
        }

        /// <summary>
        /// Geeft alle stories uit het productbacklog waar nog geen prioriteit aan gegeven is.
        /// </summary>
        /// <returns>Alle stories zonder prioriteit</returns>
        public virtual IList<Story> GeefStoriesZonderMoSCoWPrioriteit()
        {
            List<Story> moscowStories = new List<Story>();
            foreach (Story story in Stories)
            {
                if (story.ProductBacklogPrioriteit == Prioriteit.Onbekend)
                {
                    moscowStories.Add(story);
                }
            }
            return moscowStories;
        }

        /// <summary>
        /// Geeft de stories die ingepland mogen worden.
        /// </summary>
        /// <returns></returns>
        public virtual IList<Story> GeefStoriesDieIngeplandMogenWorden()
        {
            List<Story> plannableStories = new List<Story>();
            foreach (Story story in Stories)
            {
                if(story.IsTePlannen)
                    plannableStories.Add(story);
            }
            return plannableStories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IList<Sprint> GeefNietAfgerondeSprints()
        {
            IList<Sprint> nietAfgerondeSprints = new List<Sprint>();
            foreach (Sprint sprint in sprints)
            {
                if(!sprint.IsAfgesloten)
                {
                    nietAfgerondeSprints.Add(sprint);    
                }
                
            }

            return nietAfgerondeSprints;
        }

        #endregion
    }
}