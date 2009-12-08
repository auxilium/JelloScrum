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

namespace JelloScrum.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Castle.MonoRail.Framework;
    using Castle.MonoRail.Views.Brail;
    using JelloScrum.Model.Services;

    public class TemplateParserService : ITemplateParserService
    {
        private string viewDirectory = string.Empty;

        public TemplateParserService(string viewDirectory)
        {
            this.viewDirectory = viewDirectory;
        }

        #region ITemplateParserService Members
        /// <summary>
        /// Deze functie geeft maakt HTML van een brail template
        /// </summary>
        /// <param name="templateDirectory">Directory waar de brail template zich bevind</param>
        /// <param name="templateName">naam van de brail file</param>
        /// <param name="propertyBag">Eventueele propertybag's</param>
        /// <returns>een string met HTML</returns>
        public string Parse(string templateDirectory, string templateName, Dictionary<string, object> propertyBag)
        {
            if (string.IsNullOrEmpty(viewDirectory))
            {
                throw new Exception("Geen absoluut pad opgegeven voor de viewDirectory in de Windsor configuratie file");
            }

            if (string.IsNullOrEmpty(templateName))
            {
                throw new Exception("Template naam is niet opgegeven");
            }

            try
            {
                //als er een templatedirectory is, combineer die met de opgegeven viewDirectory
                string viewTemplateDirectory = viewDirectory;
                if (!string.IsNullOrEmpty(templateDirectory))
                    viewTemplateDirectory = Path.Combine(viewDirectory, templateDirectory);
                //maak een viewsource loader voor het laden van brail templates
                FileAssemblyViewSourceLoader viewSourceLoader = new FileAssemblyViewSourceLoader(viewTemplateDirectory);
                StandaloneBooViewEngine standaloneBooViewEngine = new StandaloneBooViewEngine(viewSourceLoader, null);
                StringWriter writer = new StringWriter();
                standaloneBooViewEngine.Process(templateName, writer, propertyBag);
                return writer.GetStringBuilder().ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}