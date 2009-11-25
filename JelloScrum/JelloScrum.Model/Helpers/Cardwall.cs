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
    /// Hulp klasse bij het exporteren van de cardwall
    /// </summary>
    [DelimitedRecord("|")] 
    public class Cardwall
    {
        #region Fields
        
        private string storyId;
        private string storyTitle;
        private string storyPrioriteit;
        private string storyGeschatteUren;
        
        private string taskOpenId;
        private string taskOpenTitle;
        private string taskOpenBehandelaar;
        private string taskOpenBestedeUren;

        private string taskInProgressId;
        private string taskInProgressTitle;
        private string taskInProgressBehandelaar;
        private string taskInProgressBestedeUren;

        private string taskDoneId;
        private string taskDoneTitle;
        private string taskDoneBehandelaar;
        private string taskDoneBestedeUren;

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
        /// Story titel
        /// </summary>
        public string StoryTitle
        {
            get { return storyTitle; }
            set { storyTitle = value; }
        }

        /// <summary>
        /// Story sprint prioriteit
        /// </summary>
        public string StoryPrioriteit
        {
            get { return storyPrioriteit; }
            set { storyPrioriteit = value; }
        }

        /// <summary>
        /// Geschatte uren voor story
        /// </summary>
        public string StoryGeschatteUren
        {
            get { return storyGeschatteUren; }
            set { storyGeschatteUren = value; }
        }

        /// <summary>
        /// Taak id
        /// </summary>
        public string TaskOpenId
        {
            get { return taskOpenId; }
            set { taskOpenId = value; }
        }

        /// <summary>
        /// Taak title
        /// </summary>
        public string TaskOpenTitle
        {
            get { return taskOpenTitle; }
            set { taskOpenTitle = value; }
        }

        /// <summary>
        /// Taak behandelaar
        /// </summary>
        public string TaskOpenBehandelaar
        {
            get { return taskOpenBehandelaar; }
            set { taskOpenBehandelaar = value; }
        }

        /// <summary>
        /// Bestede uren aan taak
        /// </summary>
        public string TaskOpenBestedeUren
        {
            get { return taskOpenBestedeUren; }
            set { taskOpenBestedeUren = value; }
        }

        /// <summary>
        /// Taak id
        /// </summary>
        public string TaskInProgressId
        {
            get { return taskInProgressId; }
            set { taskInProgressId = value; }
        }

        /// <summary>
        /// Taak title
        /// </summary>
        public string TaskInProgressTitle
        {
            get { return taskInProgressTitle; }
            set { taskInProgressTitle = value; }
        }

        /// <summary>
        /// Taak behandelaar
        /// </summary>
        public string TaskInProgressBehandelaar
        {
            get { return taskInProgressBehandelaar; }
            set { taskInProgressBehandelaar = value; }
        }

        /// <summary>
        /// Bestede uren aan taak
        /// </summary>
        public string TaskInProgressBestedeUren
        {
            get { return taskInProgressBestedeUren; }
            set { taskInProgressBestedeUren = value; }
        }

        /// <summary>
        /// Taak id
        /// </summary>
        public string TaskDoneId
        {
            get { return taskDoneId; }
            set { taskDoneId = value; }
        }

        /// <summary>
        /// Taak title
        /// </summary>
        public string TaskDoneTitle
        {
            get { return taskDoneTitle; }
            set { taskDoneTitle = value; }
        }

        /// <summary>
        /// Taak behandelaar
        /// </summary>
        public string TaskDoneBehandelaar
        {
            get { return taskDoneBehandelaar; }
            set { taskDoneBehandelaar = value; }
        }

        /// <summary>
        /// Bestede uren aan taak
        /// </summary>
        public string TaskDoneBestedeUren
        {
            get { return taskDoneBestedeUren; }
            set { taskDoneBestedeUren = value; }
        }

        #endregion
    }
}
