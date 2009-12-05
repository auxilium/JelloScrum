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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Castle.ActiveRecord;
    using Enumerations;

    /// <summary>
    /// Associatie tabel tussen gebruik en sprint, dit omdat een gebruikers type per sprint kan verschillen
    /// </summary>
    [ActiveRecord]
    public class SprintGebruiker : ModelBase
    {
        #region fields
        
        private Gebruiker gebruiker;
        private Sprint sprint;
        private SprintRole sprintRol = 0;

        private IList<Task> taken = new List<Task>();

        #endregion

        #region constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public SprintGebruiker()
        {
        }

        /// <summary>
        /// Maak een sprintgebruiker op basis van de gegeven gebruiker, sprint en sprintrol.
        /// </summary>
        /// <param name="gebruiker">De gebruiker.</param>
        /// <param name="sprint">De sprint.</param>
        public SprintGebruiker(Gebruiker gebruiker, Sprint sprint, SprintRole sprintRol)
        {
            if (gebruiker == null)
                throw new ArgumentNullException("gebruiker", "De gebruiker mag niet null zijn.");
            if (sprint == null)
                throw new ArgumentNullException("sprint", "De sprint mag niet null zijn.");
            
            gebruiker.VoegSprintGebruikerToe(this);
            sprint.AddSprintUser(this);
            this.sprintRol = sprintRol;
        }

        #endregion

        #region properties

        /// <summary>
        /// De Gebruiker
        /// </summary>
        [BelongsTo(NotNull = true)]
        public virtual Gebruiker Gebruiker
        {
            get { return gebruiker; }
            set { gebruiker = value; }
        }

        /// <summary>
        /// De Sprint
        /// </summary>
        [BelongsTo(NotNull = true)]
        public virtual Sprint Sprint
        {
            get { return sprint; }
            set { sprint = value; }
        }

        /// <summary>
        /// Rol van de gebruiker binnen deze sprint
        /// </summary>
        [Property]
        public virtual SprintRole SprintRol
        {
            get { return sprintRol; }
            set { sprintRol = value; }
        }


        /// <summary>
        /// Geeft een readonly collectie met de taken van deze gebruiker.
        /// Om een taak toe te voegen gebruik je <see cref="PakTaakOp"/>.
        /// </summary>
        /// <value>De taken van deze gebruiker.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.SaveUpdate, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<Task> Taken
        {
            get { return new ReadOnlyCollection<Task>(taken); }
        }

        #endregion

        #region methods

        /// <summary>
        /// Haalt de taken van een SprintGebruiker op met een specifieke SprintBacklogPrioriteit
        /// note: misschien moet dit naar de repository?
        /// Dit laadt eerst de lazy taken van de sprintgebruiker in en dan de lazy sprintstories van de sprint.
        /// </summary>
        /// <param name="prioriteit">De prioriteit.</param>
        /// <returns>De taken van de gebruiker met de gegeven prioriteit.</returns>
        public virtual IList<Task> GeefOpgepakteTakenMetSprintBacklogPrioriteit(Priority prioriteit)
        {
            IList<Task> tasks = new List<Task>();
            foreach (Task taak in taken)
            {
                if (taak.Status == State.Taken)
                {
                    SprintStory ss = sprint.GetSprintStoryFor(taak.Story);
                 
                    if (ss == null)
                        continue;

                    if (ss.SprintBacklogPrioriteit == prioriteit)
                        tasks.Add(taak);
                }
            }
            return tasks;
        }

        /// <summary>
        /// Geeft de door deze sprintgebruiker opgepakte taken.
        /// </summary>
        /// <returns>Een readonlycollection met opgepakte taken.</returns>
        public virtual ReadOnlyCollection<Task> GeefOpgepakteTaken()
        {
            IList<Task> opgepakteTaken = new List<Task>();
            foreach (Task task in taken)
            {
                if (task.Status == State.Taken)
                    opgepakteTaken.Add(task);
            }
            return new ReadOnlyCollection<Task>(opgepakteTaken);
        }

        /// <summary>
        /// Geeft de door deze sprintgebruiker afgesloten taken.
        /// </summary>
        /// <returns>Een readonlycollection met afgesloten taken.</returns>
        public virtual ReadOnlyCollection<Task> GeefAfgeslotenTaken()
        {
            IList<Task> afgeslotenTaken = new List<Task>();
            foreach (Task task in taken)
            {
                if (task.Status == State.Closed)
                    afgeslotenTaken.Add(task);
            }
            return new ReadOnlyCollection<Task>(afgeslotenTaken);
        }

        /// <summary>
        /// Pak de gegeven taak op
        /// </summary>
        /// <param name="taak">De taak.</param>
        public virtual void PakTaakOp(Task taak)
        {
            if (!taken.Contains(taak))
                taken.Add(taak);
            taak.Behandelaar = this;
            taak.Status = State.Taken;
        }

        /// <summary>
        /// De behandelaar heeft de taak opgegeven en wenst niet langer de behandelaar te zijn van de gegeven taak
        /// </summary>
        /// <param name="taak">De taak.</param>
        public virtual void GeefTaakAf(Task taak)
        {
            if (taken.Contains(taak))
                taken.Remove(taak);
            taak.Behandelaar = null;
            taak.Status = State.Open;
        }

        /// <summary>
        /// Voegt een rol toe aan de huidige set met sprintrollen
        /// </summary>
        /// <param name="nieuweSprintRol"></param>
        public virtual void VoegRolToe(SprintRole nieuweSprintRol)
        {
            this.sprintRol |= nieuweSprintRol;
        }

        /// <summary>
        /// Verwijder een rol uit de huidige set met sprintrollen
        /// </summary>
        /// <param name="teVerwijderenSprintRol"></param>
        public virtual void VerwijderRol(SprintRole teVerwijderenSprintRol)
        {
            sprintRol &= ~teVerwijderenSprintRol;
        }

        /// <summary>
        /// Geeft aan of deze sprintgebruiker PRECIES deze set met rollen heeft
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public virtual bool HeeftSprintRolExact(SprintRole role)
        {
            //hmm
            //return ((this.sprintRol & sprintRol) == sprintRol);
            return (sprintRol == role);
        }

        /// <summary>
        /// Geeft aan of deze sprintgebruiker minstens 1 van de gegevens set met sprintrollen heeft
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public virtual bool HeeftSprintRol(SprintRole role)
        {
            return ((sprintRol & role) != 0);
        }

        #endregion
        /// <summary>
        /// Neemt de gegeven taak over van de huidige behandelaar
        /// </summary>
        /// <param name="task"></param>
        public virtual void NeemTaakOver(Task task)
        {
            if (task == null) 
                throw new ArgumentNullException("task");

            if (task.Behandelaar != null)
                task.Behandelaar.GeefTaakAf(task);

            PakTaakOp(task);
        }

        /// <summary>
        /// Koppelt deze sprintgebruiker van de sprint en gebruiker af, zodat deze verwijderd kan worden
        /// </summary>
        public virtual void KoppelSprintGebruikerLos()
        {
            gebruiker.VerwijderSprintGebruiker(this);
            sprint.RemoveSprintUser(this);
        }
    }
}