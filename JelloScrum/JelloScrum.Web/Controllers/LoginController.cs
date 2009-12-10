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
    using Container;
    using Login.Authentication;
    using Model.Entities;

    /// <summary>
    /// Controller om in te loggen in het systeem
    /// </summary>
    [Layout("login")]
    public class LoginController : JelloScrumControllerBase
    {
        private readonly IAuthenticationUtil<User> authenticationService = IoC.Resolve<IAuthenticationUtil<User>>();

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
            User user = this.authenticationService.Authenticate(Context, username, password);
            if (user != null)
            {
                if (user.ActiveSprint != null)
                    Redirect("Dashboard", "index");
                else
                    Redirect("Home", "index");
            }
            else
            {
                // At this point we either have a valid user or we don't
                // If we don't the login failed, return to referrer and flash the "login failed" message.
                AddErrorMessageToFlashBag("The username / password combination is not valid.");
                RedirectToReferrer();
            }
        }

        /// <summary>
        /// Log de gebruiker uit
        /// </summary>
        public void Logout()
        {
            this.authenticationService.SignOut(Context);
            RedirectToAction("index");
        }
    }
}