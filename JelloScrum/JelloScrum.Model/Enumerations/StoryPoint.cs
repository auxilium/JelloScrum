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
    /// The planningpoker storypoints
    /// </summary>
    public enum StoryPoint
    {
        /// <summary>
        /// Unknown, 0 points
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Ace, 1 point
        /// </summary>
        Ace = 1,

        /// <summary>
        /// Two, 2 points
        ///  </summary>
        Two = 2,

        /// <summary>
        /// Three, 3 points
        /// </summary>
        Three = 3,

        /// <summary>
        /// Five, 5 points
        /// </summary>
        Five = 5,

        /// <summary>
        /// Eight, 8 points
        /// </summary>
        Eight = 8,

        /// <summary>
        /// Ten, 10 points
        /// </summary>
        Ten = 10,

        /// <summary>
        /// King, too difficult to estimate
        /// </summary>
        King = 99
    }
}
