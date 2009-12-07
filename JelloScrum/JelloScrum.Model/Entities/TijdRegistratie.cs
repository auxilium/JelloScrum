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

namespace JelloScrum.Model.Entities
{
    using System;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;

    /// <summary>
    /// Het tijdregistratie object. Hierin wordt de door de gebruiker op een
    /// taak geboekte tijd geregistreerd.
    /// </summary>
    [ActiveRecord(Lazy = true)]
    public class TijdRegistratie : ModelBase
    {
        #region fields
        private User gebruiker;
        private DateTime datum;
        private Sprint sprint; 
        private Task task;
        private TimeSpan tijd;

        #endregion
        
        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TijdRegistratie"/> class.
        /// </summary>
        public TijdRegistratie()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TijdRegistratie"/> class.
        /// </summary>
        /// <param name="gebruiker">The gebruiker.</param>
        /// <param name="datum">The datum.</param>
        /// <param name="sprint">The sprint.</param>
        /// <param name="task">The task.</param>
        /// <param name="tijd">The tijd.</param>
        public TijdRegistratie(User gebruiker, DateTime datum, Sprint sprint, Task task, TimeSpan tijd)
        {
            if (tijd.TotalMilliseconds == 0)
                throw new ArgumentOutOfRangeException("tijd", "Een tijdregistratie moet wel tijd bevatten.");
            if (gebruiker == null)
                throw new ArgumentNullException("gebruiker", "De gebruiker is null.");
            if (sprint== null)
                throw new ArgumentNullException("sprint", "De sprint is null.");
            if (task == null)
                throw new ArgumentNullException("task", "De task is null.");
            
            this.gebruiker = gebruiker;
            this.sprint = sprint;
            this.task = task;

            this.datum = datum;
            this.tijd = tijd;
        }

        #endregion

        #region properties

        /// <summary>
        /// De gebruiker.
        /// </summary>
        /// <value>De gebruiker.</value>
        [BelongsTo(NotNull = true)]
        public virtual User Gebruiker
        {
            get { return gebruiker; }
            set { gebruiker = value; }
        }

        /// <summary>
        /// De datum.
        /// </summary>
        /// <value>De datum.</value>
        [Property]
        public virtual DateTime Datum
        {
            get { return datum; }
            set { datum = value; }
        }

        /// <summary>
        /// De sprint.
        /// </summary>
        /// <value>De sprint.</value>
        [BelongsTo(NotNull = true)]
        public virtual Sprint Sprint
        {
            get { return sprint; }
            set { sprint = value; }
        }

        /// <summary>
        /// De task.
        /// </summary>
        /// <value>De task.</value>
        [BelongsTo(NotNull = true)]
        public virtual Task Task
        {
            get { return task; }
            set { task = value; }
        }

        /// <summary>
        /// De tijd.
        /// todo: waarom werkt ValidateTimeSpan niet?
        /// </summary>
        /// <value>De tijd.</value>
        [Property(ColumnType = "TimeSpan"), ValidateNonEmpty("Geen geldige tijd.")]
        public virtual TimeSpan Tijd
        {
            get { return tijd; }
            set { tijd = value; }
        }

        #endregion

    }
}