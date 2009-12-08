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

namespace JelloScrum.Model.Helpers
{
    using FileHelpers;

    /// <summary>
    /// Used to export the sprintbacklog to .xls
    /// </summary>
    [DelimitedRecord("|")] 
    public class SprintBackLog
    {
        #region fields

        private string id;
        private string title;
        private string sprintPriority;
        private string projectPriority;
        private string type;
        private string points;
        private string estimation;
        private string tasks;
        private string description;

        #endregion

        #region Properties

        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Priority of a story in the sprint
        /// </summary>
        public string SprintPriority
        {
            get { return sprintPriority; }
            set { sprintPriority = value; }
        }

        /// <summary>
        /// Priority of a story in the project
        /// </summary>
        public string ProjectPriority
        {
            get { return projectPriority; }
            set { projectPriority = value; }
        }
        /// <summary>
        /// Type of the story
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Story points
        /// </summary>
        public string Points
        {
            get { return points; }
            set { points = value; }
        }

        /// <summary>
        /// Estimation
        /// </summary>
        public string Estimation
        {
            get { return estimation; }
            set { estimation = value; }
        }

        /// <summary>
        /// Tasks
        /// </summary>
        public string Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }

        #endregion
    }
}
