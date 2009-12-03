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
    using System.Security.Principal;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;
    using Enumerations;

    /// <summary>
    /// De gebruiker klasse
    /// </summary>
    [ActiveRecord(Lazy = false)]
    public class Gebruiker : ModelBase, IPrincipal
    {
        #region fields

        private string naam = string.Empty;
        private string wachtwoord = null;
        private string volledigeNaam = string.Empty;
        private string salt = string.Empty;
        private string gebruikersNaam = string.Empty;
        private bool actief = true;
        private SysteemRol systeemRol = SysteemRol.Gebruiker;
        private IList<SprintGebruiker> sprintGebruikers = new List<SprintGebruiker>();
        private IList<ProjectShortList> projectShortList = new List<ProjectShortList>();
        private string email = string.Empty;
        private Sprint actieveSprint = null;
        private string bigAvatar = string.Empty;
        private string smallAvatar = string.Empty;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Gebruiker"/> class.
        /// </summary>
        public Gebruiker()
        {
        }

        /// <summary>
        /// Gebruiker constructor met gebruikers naam
        /// </summary>
        /// <param name="gebruikersNaam">De gebruikersnaam.</param>
        public Gebruiker(string gebruikersNaam)
        {
            this.gebruikersNaam = gebruikersNaam;
        }

        /// <summary>
        /// Nieuwe gebruiker aan de hand van de systeem rol
        /// </summary>
        /// <param name="rol">De rol.</param>
        public Gebruiker(SysteemRol rol)
        {
            systeemRol = rol;
        }


        /// <summary>
        /// Nieuwe gebruiker met naam en rol.
        /// </summary>
        /// <param name="naam"></param>
        /// <param name="systeemRol"></param>
        public Gebruiker(string naam, SysteemRol systeemRol)
        {
            this.naam = naam;
            this.systeemRol = systeemRol;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the naam.
        /// </summary>
        /// <value>The naam.</value>
        [Property, ValidateNonEmpty("Vul een naam in.")]
        public virtual string Naam
        {
            get { return naam; }
            set { naam = value; }
        }

        /// <summary>
        /// Gets or sets the wachtwoord.
        /// </summary>
        /// <value>The wachtwoord.</value>
        [Property]
        public virtual string Wachtwoord
        {
            get { return wachtwoord; }
            set { wachtwoord = value; }
        }

        /// <summary>
        /// Gets or sets the volledige naam.
        /// </summary>
        /// <value>The volledige naam.</value>
        [Property]
        public virtual string VolledigeNaam
        {
            get { return volledigeNaam; }
            set { volledigeNaam = value; }
        }

        /// <summary>
        /// De Salt
        /// </summary>
        [Property]
        public virtual string Salt
        {
            get { return salt; }
            set { salt = value; }
        }

        /// <summary>
        /// Gebruikers naam
        /// </summary>
        [Property]
        public virtual string GebruikersNaam
        {
            get { return gebruikersNaam; }
            set { gebruikersNaam = value; }
        }

        /// <summary>
        /// Mag de gebruiker inloggen of niet
        /// </summary>
        [Property]
        public virtual bool Actief
        {
            get { return actief; }
            set { actief = value; }
        }

        /// <summary>
        /// Het type van de gebruiker
        /// </summary>
        [Property]
        public virtual SysteemRol SysteemRol
        {
            get { return systeemRol; }
            set { systeemRol = value; }
        }

        /// <summary>
        /// De active sprint van deze gebruiker
        /// </summary>
        /// <value>De actieve sprint.</value>
        [BelongsTo]
        public virtual Sprint ActieveSprint
        {
            get { return actieveSprint; }
            set { actieveSprint = value; }
        }

        /// <summary>
        /// De lijst met sprintgebruikers
        /// Sprintgebruikers voeg je toe met behulp van <see cref="VoegSprintGebruikerToe"/>
        /// todo: nogmaals nadenken over deze cascade
        /// </summary>
        [HasMany(Cascade = ManyRelationCascadeEnum.SaveUpdate, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public IList<SprintGebruiker> SprintGebruikers
        {
            get { return new ReadOnlyCollection<SprintGebruiker>(sprintGebruikers); }
        }

        /// <summary>
        /// De lijst van projecten die op de shortlist staan
        /// </summary>
        [HasMany(Cascade = ManyRelationCascadeEnum.SaveUpdate, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<ProjectShortList> ProjectShortList
        {
            get { return new ReadOnlyCollection<ProjectShortList>(projectShortList); }
        }

        /// <summary>
        /// E-mail adres van de gebruiker
        /// </summary>
        [Property]
        public virtual string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// De avatar van deze gebruiker
        /// </summary>
        [Property]
        public virtual string BigAvatar
        {
            get { return bigAvatar; }
            set { bigAvatar = value; }
        }

        /// <summary>
        /// De avatar van deze gebruiker
        /// </summary>
        [Property]
        public virtual string SmallAvatar
        {
            get { return smallAvatar; }
            set { smallAvatar = value; }
        }

        #endregion

        #region IPrincipal Members

        /// <summary>
        /// De Identity
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.</returns>
        public virtual IIdentity Identity
        {
            get { return new GenericIdentity(gebruikersNaam, "Gebruiker"); }
        }

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <param name="role">The name of the role for which to check membership.</param>
        /// <returns>
        /// true if the current principal is a member of the specified role; otherwise, false.
        /// </returns>
        public bool IsInRole(string role)
        {
            return role == GetType().Name;
        }

        /// <summary>
        /// Heeft de gebruiker deze rol?
        /// </summary>
        /// <param name="rol">De rol.</param>
        /// <returns>
        /// 	<c>true</c> if [is in role] [the specified role]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInRole(SysteemRol rol)
        {
            return rol == systeemRol;
        }

        #endregion

        #region methods

        /// <summary>
        /// Voeg een sprintgebruiker aan de collectie van sprintgebruikers toe
        /// </summary>
        /// <param name="sprintGebruiker">De sprintgebruiker.</param>
        internal virtual void VoegSprintGebruikerToe(SprintGebruiker sprintGebruiker)
        {
            if (!sprintGebruikers.Contains(sprintGebruiker))
                sprintGebruikers.Add(sprintGebruiker);
            sprintGebruiker.Gebruiker = this;
        }

        internal virtual void VerwijderSprintGebruiker(SprintGebruiker sprintGebruiker)
        {
            sprintGebruikers.Remove(sprintGebruiker);
            sprintGebruiker.Gebruiker = null;
        }

        /// <summary>
        /// Verwijder deze gebruiker uit een sprint, door de sprintgebruiker los te koppelen van deze gebruiker en de gegeven sprint.
        /// </summary>
        /// <param name="sprint"></param>
        public virtual void VerwijderUitSprint(Sprint sprint)
        {
            if (sprint == null)
                throw new ArgumentNullException("sprint");

            SprintGebruiker sprintGebruiker = sprint.GetSprintUserFor(this);
            if (sprintGebruiker == null)
                return;

            sprintGebruiker.KoppelSprintGebruikerLos();
        }

        ///// <summary>
        ///// Voeg een sprint toe.
        ///// </summary>
        ///// <value>toe te voegen sprint.</value>
        //public virtual void VoegSprintToe(Sprint sprint)
        //{
        //    //TODO dit fixen met de nieuwe assocatie
        //    //if (sprints.Contains(sprint) == false)
        //    //{
        //    //    sprints.Add(sprint);
        //    //}
        //    //if (sprint.Gebruikers.Contains(this) == false)
        //    //{
        //    //    sprint.Gebruikers.Add(this);
        //    //}
        //}

        ///// <summary>
        ///// Verwijder een sprint.
        ///// </summary>
        ///// <value>Te verwijderen sprint.</value>
        //public virtual void VerwijderSprint(Sprint sprint)
        //{
        //    //TODO dit fixen met de nieuwe assocatie
        //    //sprints.Remove(sprint);
        //    //sprint.Gebruikers.Remove(this);
        //}

        /// <summary>
        /// Geeft de sprintgebruiker van deze gebruiker in de gegeven sprint.
        /// </summary>
        /// <param name="sprint">De sprint.</param>
        /// <returns>De sprintgebruiker</returns>
        public SprintGebruiker GeefSprintGebruikerVoor(Sprint sprint)
        {
            foreach (SprintGebruiker sprintGebruiker in sprintGebruikers)
            {
                if (sprintGebruiker.Sprint == sprint)
                    return sprintGebruiker;
            }
            return null;
            
        }

        /// <summary>
        /// Geeft de sprintgebruiker voor de actieve sprint van deze gebruiker.
        /// </summary>
        /// <returns>De sprintgebruiker</returns>
        public SprintGebruiker GeefActieveSprintGebruiker()
        {
            if (ActieveSprint == null)
                return null;

            return GeefSprintGebruikerVoor(ActieveSprint);
        }

        /// <summary>
        /// Voeg een project toe aan de ShortlistProjecten
        /// </summary>
        /// <param name="shortListProject"></param>
        public void ShortListProjectToevoegen(Project project)
        {
            foreach (ProjectShortList shortList in ProjectShortList)
            {
                if(shortList.Project.Equals(project))
                    return;
            }

            projectShortList.Add(new ProjectShortList(this,project));

        }

        /// <summary>
        /// Verwijder een project van de ShortlistProjecten
        /// </summary>
        /// <param name="shortListProject"></param>
        public void ShortListProjectVerwijderen(ProjectShortList shortListProject)
        {
            if (ProjectShortList.Contains(shortListProject))
                ProjectShortList.Remove(shortListProject);
        }

        #endregion
    }
}