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
    using System;
    using Castle.MonoRail.Framework;
    using Container;
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.Services;

    /// <summary>
    /// Controller om in te loggen in het systeem
    /// </summary>
    [Layout("login")]
    public class LoginController : JelloScrumControllerBase
    {
        private readonly ILoginService loginService = IoC.Resolve<ILoginService>();
        private readonly IAuthenticationService authenticationService = IoC.Resolve<IAuthenticationService>();

        /// <summary>
        /// 
        /// </summary>
        public void Index()
        {
        }
        /// <summary>
        /// Log de gebruiker in
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Login(string username, string password)
        {
            Gebruiker gebruiker;
            try
            {
                gebruiker = loginService.LDapGebruikerCheck(username, password);
                success(gebruiker);
            }
            catch
            {
                try
                {
                    gebruiker = loginService.Login(password, username);
                    success(gebruiker);
                }
                catch (Exception e)
                {
                    AddErrorMessageToFlashBag(e.Message);
                    RedirectToReferrer();
                }
            }
        }

        /// <summary>
        /// Het is gelukt
        /// </summary>
        /// <param name="gebruiker"></param>
        private void success(Gebruiker gebruiker)
        {
            Context.CurrentUser = gebruiker;
            authenticationService.SetAuthCookie(gebruiker, Context);

            if (gebruiker.ActieveSprint!=null)
                Redirect("Dashboard", "index");
            else 
                Redirect("Home", "index");
        }

        /// <summary>
        /// Log de gebruiker uit
        /// </summary>
        public void Logout()
        {
            authenticationService.SignOut(Context);
            RedirectToAction("index");
        }
    }
}