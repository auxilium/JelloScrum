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
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using Castle.Components.DictionaryAdapter;
    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using Container;
    using Filter;

    using JelloScrum.Repositories.Exceptions;
    using Model.IRepositories;

    /// <summary>
    /// De basecontroller voor jelloscrum.
    /// Alle controllers moeten van deze base controller erven.
    /// </summary>
    [Layout("default")]
    [Rescue("generalerror")]
    [Filter(ExecuteWhen.AfterAction, typeof(TitelFilter))]
    public abstract class JelloScrumControllerBase : ARSmartDispatcherController
    {
        public IList<string> errors;
        private DictionaryAdapterFactory adapterFactory;

        private string titel = string.Empty;

        /// <summary>
        /// Gets an adapterfactory for wrapping dictionaries
        /// Used for the session and componentparams dictionary in combination with the IDictionary interface.
        /// </summary>
        public DictionaryAdapterFactory AdapterFactory
        {
            get
            {
                if (adapterFactory == null)
                    adapterFactory = new DictionaryAdapterFactory();
                return adapterFactory;
            }
        }

        /// <summary>
        /// Titel van de pagina voor in de Default layouts
        /// </summary>
        public string Titel
        {
            get { return titel; }
            set { titel = value; }
        }

        #region Services Properties

        /// <summary>
        /// Gets the gebruiker service.
        /// </summary>
        /// <value>The gebruiker service.</value>
        public static IGebruikerRepository GebruikerRepository
        {
            get { return IoC.Resolve<IGebruikerRepository>(); }
        }

        /// <summary>
        /// Gets the project service.
        /// </summary>
        /// <value>The project service.</value>
        public static IProjectRepository ProjectRepository
        {
            get { return IoC.Resolve<IProjectRepository>(); }
        }

        /// <summary>
        /// Gets the projectShortList service.
        /// </summary>
        /// <value>The projectShortList service.</value>
        public static IProjectShortListRepository ProjectShortListRepository
        {
            get { return IoC.Resolve<IProjectShortListRepository>(); }
        }

        /// <summary>
        /// Gets the sprint service.
        /// </summary>
        /// <value>The sprint service.</value>
        public static ISprintRepository SprintRepository
        {
            get { return IoC.Resolve<ISprintRepository>(); }
        }

        /// <summary>
        /// Gets the sprint service.
        /// </summary>
        /// <value>The sprint service.</value>
        public static ISprintGebruikerRepository SprintGebruikerRepository
        {
            get { return IoC.Resolve<ISprintGebruikerRepository>(); }
        }

        /// <summary>
        /// Gets the sprint story service.
        /// </summary>
        /// <value>The sprint story service.</value>
        public static ISprintStoryRepository SprintStoryRepository
        {
            get { return IoC.Resolve<ISprintStoryRepository>(); }
        }

        /// <summary>
        /// Gets the story service.
        /// </summary>
        /// <value>The story service.</value>
        public static IStoryRepository StoryRepository
        {
            get { return IoC.Resolve<IStoryRepository>(); }
        }

        /// <summary>
        /// Gets the task service.
        /// </summary>
        /// <value>The task service.</value>
        public static ITaskRepository TaskRepository
        {
            get { return IoC.Resolve<ITaskRepository>(); }
        }

        #endregion

        #region Generieke meldingen

        /// <summary>
        /// Maakt een generieke fout melding afhandeling mogelijk
        /// </summary>
        /// <param name="errorMessage">de fout melding</param>
        protected virtual void AddErrorMessageToPropertyBag(string errorMessage)
        {
            AddMessageToPropertyBag(errorMessage, "errorMessages");
        }

        /// <summary>
        /// Maakt een generieke positieve melding afhandeling mogelijk
        /// </summary>
        /// <param name="positiveMessage">de fout melding</param>
        protected virtual void AddPositiveMessageToPropertyBag(string positiveMessage)
        {
            AddMessageToPropertyBag(positiveMessage, "positiveMessages");
        }

        /// <summary>
        /// Maakt een generieke info melding afhandeling mogelijk
        /// </summary>
        /// <param name="infoMessage">de fout melding</param>
        protected virtual void AddInfoMessageToPropertyBag(string infoMessage)
        {
            AddMessageToPropertyBag(infoMessage, "infoMessages");
        }

        /// <summary>
        /// doet het zelfde als AddErrorMessageToPropertyBag maar dan met een flash bag
        /// Deze functie moet worden gebruikt waneer een RedirectToReferrer word aangeroepen.
        /// </summary>
        /// <param name="errorMessage">de fout melding</param>
        protected virtual void AddErrorMessageToFlashBag(string errorMessage)
        {
            AddMessageToFlashBag(errorMessage, "errorMessages");
        }

        /// <summary>
        /// doet het zelfde als AddPositiveMessageToPropertyBag maar dan met een flash bag
        /// Deze functie moet worden gebruikt waneer een RedirectToReferrer word aangeroepen.
        /// </summary>
        /// <param name="positiveMessage">de fout melding</param>
        protected virtual void AddPositiveMessageToFlashBag(string positiveMessage)
        {
            AddMessageToFlashBag(positiveMessage, "positiveMessages");
        }

        /// <summary>
        /// doet het zelfde als AddInfoMessageToPropertyBag maar dan met een flash bag
        /// Deze functie moet worden gebruikt waneer een RedirectToReferrer word aangeroepen.
        /// </summary>
        /// <param name="infoMessage">de fout melding</param>
        protected virtual void AddInfoMessageToFlashBag(string infoMessage)
        {
            AddMessageToFlashBag(infoMessage, "infoMessages");
        }

        private void AddMessageToPropertyBag(string message, string Type)
        {
            List<string> messages = new List<string>();
            if (PropertyBag.Contains(Type) && PropertyBag[Type] != null)
            {
                messages.AddRange((IEnumerable<string>) PropertyBag[Type]);
            }

            if (string.IsNullOrEmpty(message))
                return;

            messages.Add(message);
            PropertyBag[Type] = messages;
        }

        private void AddMessageToFlashBag(string message, string Type)
        {
            List<string> messages = new List<string>();
            if (Flash.Contains(Type) && Flash[Type] != null)
            {
                messages.AddRange((IEnumerable<string>) Flash[Type]);
            }

            if (string.IsNullOrEmpty(message))
                return;

            messages.Add(message);
            Flash[Type] = messages;
        }
         
        #endregion
    }
}