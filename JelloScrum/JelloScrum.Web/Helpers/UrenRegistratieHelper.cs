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

namespace JelloScrum.Web.Helpers
{
    using System;
    using Model.Entities;

    /// <summary>
    /// Helper klasse voor het urenregistratie scherm
    /// Ieder invulvakje (= registratie van uren per taak per gebruiker per dag) is gelijk aan een urenregistratie object. 
    /// </summary>
    public class UrenRegistratieHelper
    {
        private Task task;
        private DateTime dag;
        private double aantalUren = 0;
        private SprintGebruiker sprintGebruiker;

        /// <summary>
        /// De taak waarop uren geboekt worden
        /// </summary>
        public virtual Task Task
        {
            get { return this.task; }
            set { this.task = value; }
        }

        /// <summary>
        /// De dag waarop de uren gedraaid zijn
        /// </summary>
        public virtual DateTime Dag
        {
            get { return this.dag; }
            set { this.dag = value; }
        }

        /// <summary>
        /// Het aantal uren dat gedraaid is
        /// </summary>
        public virtual double AantalUren
        {
            get { return this.aantalUren; }
            set { this.aantalUren = value; }
        }

        /// <summary>
        /// De gebruiker die de uren ingevuld heeft
        /// </summary>
        public virtual SprintGebruiker SprintGebruiker
        {
            get { return this.sprintGebruiker; }
            set { this.sprintGebruiker = value; }
        }

        /// <summary>
        /// Maakt in de taak de tijdregistraties aan
        /// </summary>
        public void MaakUrenRegistratie()
        {
            task.MaakTijdRegistratie(sprintGebruiker.Gebruiker, dag, sprintGebruiker.Sprint, TimeSpanHelper.Parse(aantalUren.ToString()));
        }
    }
}