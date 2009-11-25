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

namespace JelloScrum.Model.Enumerations
{
    /// <summary>
    /// Definieert het storytype
    /// </summary>
    public enum StoryType
    {
        /// <summary>
        /// De story is een userstory
        /// </summary>
        UserStory = 0,

        /// <summary>
        /// De story is een techstory
        /// </summary>
        TechStory = 1,

        /// <summary>
        /// De story is een impediment
        /// </summary>
        Impediment = 2,

        /// <summary>
        /// De story is een bug
        /// </summary>
        Bug = 3
    }
}
