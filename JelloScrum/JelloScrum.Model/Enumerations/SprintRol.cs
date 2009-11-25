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
    using System;

    /// <summary>
    /// Rechten binnen een sprint
    /// </summary>
    [Flags]
    public enum SprintRol
    {
        /// <summary>
        /// Administratie gebruiker
        /// </summary>
        Administratie = 1 << 0,

        /// <summary>
        /// Scrum Master
        /// </summary>
        ScrumMaster = 1 << 1,

        /// <summary>
        /// Developer rol
        /// </summary>
        Developer = 1 << 2,

        /// <summary>
        /// Product Owner
        /// </summary>
        ProductOwner = 1 << 3,

        /// <summary>
        /// Administrator van het systeem
        /// </summary>
        Administrator = 1 << 4
    }
}
