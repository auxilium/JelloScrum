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
    using Castle.ActiveRecord;

    /// <summary>
    /// Represents a ProjectShortList
    /// </summary>
    [ActiveRecord]
    public class ProjectShortList: ModelBase
    {
        #region fields

        private Gebruiker gebruiker;
        private Project project;

        #endregion

        #region constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ProjectShortList()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectShortList(Gebruiker gebruiker, Project project)
        {
            this.gebruiker = gebruiker;
            this.project = project;
        }

        #endregion

        #region Properties

        /// <summary>
        /// De gebruiker
        /// </summary>
        [BelongsTo]
        public virtual Gebruiker Gebruiker
        {
            get { return gebruiker; }
            set { gebruiker = value; }
        }

        /// <summary>
        /// Project van de shortlist
        /// </summary>
        [BelongsTo]
        public virtual Project Project
        {
            get { return project; }
            set { project = value; }
        }

        #endregion

        #region Methodes


        #endregion
    }
}
