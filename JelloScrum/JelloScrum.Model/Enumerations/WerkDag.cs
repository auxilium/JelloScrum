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
    /// De werkdagen
    /// </summary>
    public enum WerkDag
    {
        /// <summary>
        /// Maandag paniekdag
        /// </summary>
        Maandag = 1,

        /// <summary>
        /// Dinsdag wordt er ook gewerkt
        /// </summary>
        Dinsdag = 2,

        /// <summary>
        /// Woensdag gehaktdag
        /// </summary>
        Woensdag = 4,

        /// <summary>
        /// Donderdag naar de subway
        /// </summary>
        Donderdag = 8,

        /// <summary>
        /// Vrijdag vijmibo \o/
        /// </summary>
        Vrijdag = 16,

        /// <summary>
        /// Zaterdag hebben wij een hangover
        /// </summary>
        Zaterdag = 32,

        /// <summary>
        /// Zondag naar de kerk
        /// </summary>
        Zondag = 64
    }
}
