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
    /// The MoSCoW priority
    /// </summary>
    public enum Priority
    {
        /// <summary>
        /// Must have
        /// </summary>
        Must = 0,

        /// <summary>
        /// Should have
        /// </summary>
        Should = 1,

        /// <summary>
        /// Could have
        /// </summary>
        Could = 2,

        /// <summary>
        /// Won't have this time, but would like to in the future
        /// </summary>
        Would = 3,

        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 4
    }
}
