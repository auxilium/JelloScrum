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

namespace JelloScrum.Web.Controllers
{
    using Castle.MonoRail.Framework;
    using Filter;
    using Web.Helpers;
    using Model.Entities;

    /// <summary>
    /// Secure controller
    /// </summary>
    [Layout("default")]
    [Helper(typeof(OpmaakHelper))]
    [Helper(typeof(DateHelper))]
    [Helper(typeof(TimeSpanHelper))]
    [Filter(ExecuteWhen.BeforeAction, typeof(AuthenticationFilter), ExecutionOrder = 1)]
    [Filter(ExecuteWhen.BeforeAction, typeof(VulDefaultPropertiesFilter), ExecutionOrder = 2)]
    public abstract class SecureController : JelloScrumControllerBase
    {
        /// <summary>
        /// Gitaar. (aldus marco)
        /// </summary>
        protected Gebruiker currentUser;

        /// <summary>
        /// Ingelogde gebruiker
        /// </summary>
        public virtual Gebruiker CurrentUser
        {
            get { return Context.CurrentUser as Gebruiker; }
        }

        /// <summary>
        /// Geef sprintgebruiker van actievesprint
        /// </summary>
        public virtual SprintGebruiker ActieveSprintSprintGebruiker
        {
            get
            {
                Gebruiker gb = CurrentUser;

                if (gb.ActieveSprint != null)
                {
                    return gb.ActieveSprint.GeefSprintGebruikerVoor(gb);
                }
                else
                {
                    return null;
                }

            }
        }

    }
}