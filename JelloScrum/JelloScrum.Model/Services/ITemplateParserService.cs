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
    /// Interface for the TemplateParserService
    /// </summary>
    public interface ITemplateParserService
    {
        /// <summary>
        /// Parse a template with the given arguments
        /// </summary>
        /// <param name="templateFolder">folder under the viewdirectiry where the brail template exists</param>
        /// <param name="templateName">name of the brail template</param>
        /// <param name="propertyBag">propertybag for objects used in the brail template</param>
        /// <returns>the parsed template as string</returns>
        string Parse(string templateFolder, string templateName, Dictionary<string, object> propertyBag);
    }
}