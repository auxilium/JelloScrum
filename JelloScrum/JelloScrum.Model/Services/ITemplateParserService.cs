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

namespace JelloScrum.Model.Services
{
    using System.Collections.Generic;

    /// <summary>
    /// De interface van de TemplateParserService
    /// </summary>
    public interface ITemplateParserService
    {
        /// <summary>
        /// Deze functie parsed een template adhv de gespecificeerde argumenten
        /// </summary>
        /// <param name="templateFolder">folder binnen de viewdirectory waar de brail template zich bevindt</param>
        /// <param name="templateName">naam van de brail template</param>
        /// <param name="propertyBag">eventuele items die tijdens het parsen van de template nodig zijn</param>
        /// <returns>een string met het resultaat van het parsen van het gespecificeerde template</returns>
        string Parse(string templateFolder, string templateName, Dictionary<string, object> propertyBag);
    }
}