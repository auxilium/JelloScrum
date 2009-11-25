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
    using System;
    using Castle.MonoRail.Framework;
    using Container;
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.Services;

    /// <summary>
    /// Check if user
    /// </summary>
    public class AuthenticationFilter : Filter
    {
        private readonly IAuthenticationService authenticationService = IoC.Resolve<IAuthenticationService>();

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exec"></param>
        /// <param name="context"></param>
        /// <param name="controller"></param>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        // public bool Perform(ExecuteWhen exec, IEngineContext context, IController controller, IControllerContext controllerContext)
        protected override bool  OnBeforeAction(IEngineContext context, IController controller, IControllerContext controllerContext)
        {
            try
            {
                Gebruiker user = authenticationService.Authenticatie(context);
                controllerContext.PropertyBag.Add("currentUser", user);
            }
            catch
            {
                SendToLoginPage(context);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Redirect naar login pagina.
        /// </summary>
        /// <param name="context">Current context</param>
        private static void SendToLoginPage(IEngineContext context)
        {
            context.Response.Redirect(string.Empty, "login", "index");
        }
    }
}