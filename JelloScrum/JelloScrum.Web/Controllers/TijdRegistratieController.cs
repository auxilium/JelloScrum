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
    using Castle.MonoRail.ActiveRecordSupport;
    using Model.Entities;

    /// <summary>
    /// controller voor het weergeven van tijdregistraties
    /// </summary>
    public class TijdRegistratieController : SecureController
    {
        /// <summary>
        /// Geeft een lijstje van alle tijdregistraties van de gespecificeerde story
        /// </summary>
        /// <param name="story"></param>
        public void GeefTijdRegistratieOverzicht([ARFetch("storyId")]Story story)
        {
            PropertyBag.Add("story", story);
            CancelLayout();
        }
    }
}