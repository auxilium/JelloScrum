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

        private User user;
        private Project project;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectShortList"/> class.
        /// </summary>
        public ProjectShortList()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectShortList"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="project">The project.</param>
        public ProjectShortList(User user, Project project)
        {
            this.user = user;
            this.project = project;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The user
        /// </summary>
        [BelongsTo(Column = "JelloScrumUser")]
        public virtual User User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// The project
        /// </summary>
        [BelongsTo]
        public virtual Project Project
        {
            get { return project; }
            set { project = value; }
        }

        #endregion
    }
}
