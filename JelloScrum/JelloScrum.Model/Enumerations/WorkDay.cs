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
    /// The workdays
    /// </summary>
    public enum WorkDay
    {
        /// <summary>
        /// Monday
        /// </summary>
        Monday = 1,

        /// <summary>
        /// Tuesday
        /// </summary>
        Tuesday = 2,

        /// <summary>
        /// Wednesday
        /// </summary>
        Wednesday = 4,

        /// <summary>
        /// Thursday
        /// </summary>
        Thursday = 8,

        /// <summary>
        /// Friday
        /// </summary>
        Friday = 16,

        /// <summary>
        /// Saturday
        /// </summary>
        Saturday = 32,

        /// <summary>
        /// Sunday
        /// </summary>
        Sunday = 64
    }
}
