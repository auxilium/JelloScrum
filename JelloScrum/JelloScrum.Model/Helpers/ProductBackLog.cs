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
    /// Hulp klasse bij het exporteren van de productbacklog
    /// </summary>
    [DelimitedRecord("|")] 
    public class ProductBackLog
    {
        #region Fields
        private string id;
        private string title;
        private string prioriteit;
        private string type;
        private string omschrijving;
        private string punten;
        private string schatting;
        private string taken;

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
        /// Title project
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Prioriteit van Story
        /// </summary>
        public string Prioriteit
        {
            get { return prioriteit; }
            set { prioriteit = value; }
        }

        /// <summary>
        /// Type story
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Omschrijving story
        /// </summary>
        public string Omschrijving
        {
            get { return omschrijving; }
            set { omschrijving = value; }
        }

        /// <summary>
        /// Story punten
        /// </summary>
        public string Punten
        {
            get { return punten; }
            set { punten = value; }
        }

        /// <summary>
        /// Urenschatting voor story
        /// </summary>
        public string Schatting
        {
            get { return schatting; }
            set { schatting = value; }
        }

        /// <summary>
        /// Aantal taken bij story
        /// </summary>
        public string Taken
        {
            get { return taken; }
            set { taken = value; }
        }

        #endregion
    }
}
