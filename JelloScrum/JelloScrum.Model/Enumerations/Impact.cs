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
    /// Set's the impact of a bug
    /// </summary>
    public enum Impact
    {
        /// <summary>
        /// De impact is blokkerend
        /// </summary>
        Blokkerend = 0,

        /// <summary>
        /// De impact is kritisch
        /// </summary>
        Kritische = 1,

        /// <summary>
        /// De impact is normaal
        /// </summary>
        Normaal = 2,

        /// <summary>
        /// De impact is klein
        /// </summary>
        Klein = 3,

        /// <summary>
        /// De impact is toebehoren?
        /// </summary>
        Toebehoren = 4
    }
}