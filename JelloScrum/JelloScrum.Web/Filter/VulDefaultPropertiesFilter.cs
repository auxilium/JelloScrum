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

namespace JelloScrum.Web.Filter
{
    using System.Collections.Generic;
    using Castle.MonoRail.Framework;
    using Container;
    using Model.Entities;
    using Model.IRepositories;

    /// <summary>
    /// Vul de default properties die altijd in de propertybag moeten zitten.
    /// </summary>
    public class VulDefaultPropertiesFilter : Filter
    {
        private readonly IProjectRepository projectRepository = IoC.Resolve<IProjectRepository>();

        /// <summary>
        /// Vul de default properties in de propertybag.
        /// </summary>
        /// <param name="context">The MonoRail request context</param>
        /// <param name="controller">The controller instance</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <returns>
        /// 	<c>true</c> if the request should proceed, otherwise <c>false</c>
        /// </returns>
        protected override bool OnBeforeAction(IEngineContext context, IController controller,
                                               IControllerContext controllerContext)
        {
            IList<Project> projecten = new List<Project>();

            User ingelogdeGebruiker = (User)context.CurrentUser;

            /* TODO:hier moet een lijstje inkomen met jou eigen lijst met projecten */
            foreach (SprintUser sprintGebruiker in ingelogdeGebruiker.SprintUsers)
            {
                if (!projecten.Contains(sprintGebruiker.Sprint.Project))
                    projecten.Add(sprintGebruiker.Sprint.Project);

                if(projecten.Count > 3)
                {
                    break;
                }
            }

            controllerContext.PropertyBag.Add("projects", projecten);
            controllerContext.PropertyBag.Add("currentUser", context.CurrentUser);

            return true;
        }
    }
}