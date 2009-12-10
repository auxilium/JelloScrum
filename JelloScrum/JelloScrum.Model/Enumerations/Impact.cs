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
    /// The different states of impact.
    /// </summary>
    public enum Impact
    {
        /// <summary>
        /// The impact is blokking
        /// </summary>
        Blocking = 0,

        /// <summary>
        /// The impact is critical
        /// </summary>
        Critical = 1,

        /// <summary>
        /// The impact is normal
        /// </summary>
        Normal = 2,

        /// <summary>
        /// The impact is small
        /// </summary>
        Small = 3
    }
}