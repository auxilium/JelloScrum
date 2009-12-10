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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Castle.MonoRail.Framework;
    using Helpers;

    /// <summary>
    /// De controller voor het dashboard
    /// </summary>
    [Helper(typeof (BurnDown2Helper))]
    public class DashboardController : SecureController
    {
        /// <summary>
        /// 
        /// </summary>
        public void Index()
        {
            if (CurrentUser.ActiveSprint == null)
            {
                Redirect("home", "index");
                return;
            }
            NameValueCollection args = new NameValueCollection();
            args.Add("sprintId", CurrentUser.ActiveSprint.Id.ToString());
            Redirect("sprint", "sprint", args);
        }
    }
}