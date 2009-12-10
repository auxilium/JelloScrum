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
    /// Used to export the cardwall to .xls
    /// </summary>
    [DelimitedRecord("|")] 
    public class Cardwall
    {
        #region Fields
        
        private string storyId;
        private string storyTitle;
        private string storyPriority;
        private string storyEstimatedHours;
        
        private string taskOpenId;
        private string taskOpenTitle;
        private string taskOpenAssignee;
        private string taskOpenTimeSpent;

        private string taskInProgressId;
        private string taskInProgressTitle;
        private string taskInProgressAsignee;
        private string taskInProgressTimeSpent;

        private string taskDoneId;
        private string taskDoneTitle;
        private string taskDoneAsignee;
        private string taskDoneTimeSpent;

        #endregion

        #region Properties

        /// <summary>
        /// Story Id
        /// </summary>
        public string StoryId
        {
            get { return storyId; }
            set { storyId = value; }
        }

        /// <summary>
        /// Story title
        /// </summary>
        public string StoryTitle
        {
            get { return storyTitle; }
            set { storyTitle = value; }
        }

        /// <summary>
        /// Story sprint priority
        /// </summary>
        public string StoryPriority
        {
            get { return storyPriority; }
            set { storyPriority = value; }
        }

        /// <summary>
        /// Estimated hours for story
        /// </summary>
        public string StoryEstimatedHours
        {
            get { return storyEstimatedHours; }
            set { storyEstimatedHours = value; }
        }

        /// <summary>
        /// Open task id
        /// </summary>
        public string TaskOpenId
        {
            get { return taskOpenId; }
            set { taskOpenId = value; }
        }

        /// <summary>
        /// Open task title
        /// </summary>
        public string TaskOpenTitle
        {
            get { return taskOpenTitle; }
            set { taskOpenTitle = value; }
        }

        /// <summary>
        /// Open task assigned user?? todo: this should not be possible?
        /// </summary>
        public string TaskOpenAssignee
        {
            get { return taskOpenAssignee; }
            set { taskOpenAssignee = value; }
        }

        /// <summary>
        /// Open task time spent
        /// </summary>
        public string TaskOpenTimeSpent
        {
            get { return taskOpenTimeSpent; }
            set { taskOpenTimeSpent = value; }
        }

        /// <summary>
        /// Taken task id
        /// </summary>
        public string TaskInProgressId
        {
            get { return taskInProgressId; }
            set { taskInProgressId = value; }
        }

        /// <summary>
        /// Taken task title
        /// </summary>
        public string TaskInProgressTitle
        {
            get { return taskInProgressTitle; }
            set { taskInProgressTitle = value; }
        }

        /// <summary>
        /// Taken task assigned user
        /// </summary>
        public string TaskInProgressAssignee
        {
            get { return taskInProgressAsignee; }
            set { taskInProgressAsignee = value; }
        }

        /// <summary>
        /// Taken task time spent
        /// </summary>
        public string TaskInProgressTimeSpent
        {
            get { return taskInProgressTimeSpent; }
            set { taskInProgressTimeSpent = value; }
        }

        /// <summary>
        /// Done task id
        /// </summary>
        public string TaskDoneId
        {
            get { return taskDoneId; }
            set { taskDoneId = value; }
        }

        /// <summary>
        /// Done task title
        /// </summary>
        public string TaskDoneTitle
        {
            get { return taskDoneTitle; }
            set { taskDoneTitle = value; }
        }

        /// <summary>
        /// Done task assigned user
        /// </summary>
        public string TaskDoneAssignee
        {
            get { return taskDoneAsignee; }
            set { taskDoneAsignee = value; }
        }

        /// <summary>
        /// Done task time spent
        /// </summary>
        public string TaskDoneTimeSpent
        {
            get { return taskDoneTimeSpent; }
            set { taskDoneTimeSpent = value; }
        }

        #endregion
    }
}
