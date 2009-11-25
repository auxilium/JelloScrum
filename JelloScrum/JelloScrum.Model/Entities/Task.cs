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
    using Castle.Components.Validator;

    using Enumerations;
    using Helpers;
    using Interfaces;

    /// <summary>
    /// Represents a Task
    /// </summary>
    [ActiveRecord]
    public class Task : ModelBase, ILogable
    {
        #region Fields

        private string titel = string.Empty;
        private Story story = null;
        private string omschrijving = string.Empty;
        private Status status = Status.NietOpgepakt;
        private SprintGebruiker behandelaar;
        private DateTime? datumAfgesloten;
        private TimeSpan schatting = new TimeSpan();
        private string schattingString = string.Empty;

        private IList<TijdRegistratie> tijdRegistraties = new List<TijdRegistratie>();
        private IList<TaskLogBericht> logBerichten = new List<TaskLogBericht>();
        private IList<TaskCommentaarBericht> commentaarBerichten = new List<TaskCommentaarBericht>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class.
        /// </summary>
        public Task()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class.
        /// </summary>
        /// <param name="story">De story.</param>
        public Task(Story story)
        {
            story.VoegTaskToe(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class.
        /// </summary>
        /// <param name="omschrijving">The omschrijving.</param>
        public Task(string omschrijving)
        {
            this.omschrijving = omschrijving;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Titel van de taak
        /// </summary>
        [Property]
        public virtual string Titel
        {
            get { return titel; }
            set { titel = value; }
        }

        /// <summary>
        /// De story.
        /// </summary>
        /// <value>De story.</value>
        [BelongsTo]
        public virtual Story Story
        {
            get { return story; }
            set { story = value; }
        }

        /// <summary>
        /// De omschrijving.
        /// </summary>
        /// <value>De omschrijving.</value>
        [Property(SqlType = "ntext")]
        public virtual string Omschrijving
        {
            get { return omschrijving; }
            set { omschrijving = value; }
        }

        /// <summary>
        /// De status.
        /// </summary>
        /// <value>De status.</value>
        [Property]
        public virtual Status Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// De behandelaar.
        /// </summary>
        /// <value>De behandelaar.</value>
        [BelongsTo]
        public virtual SprintGebruiker Behandelaar
        {
            get { return behandelaar; }
            set { behandelaar = value; }
        }

        /// <summary>
        /// De tijd die geschat is voor deze story
        /// </summary>
        /// <value>De schatting.</value>
        [Property(ColumnType = "TimeSpan"), ValidateNonEmpty("Vul een schatting in.")]
        public virtual TimeSpan Schatting
        {
            get { return schatting; }
            set
            {
                schatting = value;
            }
        }

        /// <summary>
        /// Hulp property voor schatting
        /// </summary>
        public virtual string SchattingString
        {
            get { return schattingString; }
            set { schattingString = value; }
        }

        /// <summary>
        /// Geeft een readonly lijst met alle tijdregistraties die bij deze task horen.
        /// gebruik <see cref="MaakTijdRegistratie(Gebruiker, DateTime, Sprint, TimeSpan)"/> om nieuwe tijdregistraties te maken.
        /// </summary>
        /// <value>De tijdregistraties.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<TijdRegistratie> TijdRegistraties
        {
            get { return new ReadOnlyCollection<TijdRegistratie>(tijdRegistraties); }
        }

        /// <summary>
        /// De logberichten.
        /// </summary>
        /// <value>De logberichten.</value>
        [HasMany(Table = "LogBericht", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<TaskLogBericht> LogBerichten
        {
            get { return logBerichten; }
            set { logBerichten = value; }
        }

        /// <summary>
        /// Gets or sets the commentaar berichten.
        /// </summary>
        /// <value>The commentaar berichten.</value>
        [HasMany(Table = "LogBericht", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<TaskCommentaarBericht> CommentaarBerichten
        {
            get { return commentaarBerichten; }
            set { commentaarBerichten = value; }
        }

        /// <summary>
        /// Get de datum waarop de taak is afgesloten
        /// </summary>
        [Property(Access = PropertyAccess.NosetterCamelcase)]
        public virtual DateTime? DatumAfgesloten
        {
            get { return this.datumAfgesloten; }
        }
        #endregion

        #region derived properties

        /// <summary>
        /// Geef de naam van de behandelaar
        /// </summary>
        public virtual string BehandelaarNaam
        {
            get
            {
                if (Behandelaar !=null)
                    return Behandelaar.Gebruiker.Naam;
                return string.Empty;
            }
        }

        /// <summary>
        /// Geeft de nog resterende tijd voor deze taak
        /// </summary>
        public virtual TimeSpan ResterendeTijd
        {
            get
            {
                return (Schatting - TotaalBestedeTijd());
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Maak een tijdregistratie.
        /// </summary>
        /// <param name="gebruiker">De gebruiker.</param>
        /// <param name="datum">De datum.</param>
        /// <param name="sprint">De sprint.</param>
        /// <param name="tijd">De tijd.</param>
        public virtual void MaakTijdRegistratie(Gebruiker gebruiker, DateTime datum, Sprint sprint, TimeSpan tijd)
        {
            if (!story.Project.Sprints.Contains(sprint))
            {
                throw new ArgumentException("De gegeven sprint hoort niet bij dit project.", "sprint");
            }

            foreach (TijdRegistratie registratie in GeeftTijdregistratievanGebruiker(gebruiker, sprint, datum))
            {
                VerwijderTijdRegistratie(registratie);
            }
            //als de tijdregistratie 0 seconden is, dan hoeven we geen nieuwe tijdregistratie toe te voegen
            if (tijd.TotalSeconds == 0)
                return;
            
            TijdRegistratie tijdRegistratie = new TijdRegistratie(gebruiker, datum, sprint, this, tijd);
            VoegTijdRegistratieToe(tijdRegistratie);
        }

        /// <summary>
        /// Voegs the commentaar bericht toe.
        /// </summary>
        public virtual void VoegCommentaarBerichtToe(TaskCommentaarBericht bericht)
        {
            if (!commentaarBerichten.Contains(bericht))
            {
                commentaarBerichten.Add(bericht);
            }
        }

        /// <summary>
        /// Verwijder commentaarBericht van de task
        /// </summary>
        /// <param name="bericht"></param>
        public virtual void VerwijderCommentaarBericht(TaskCommentaarBericht bericht)
        {
            if(commentaarBerichten.Contains(bericht))
            {
                commentaarBerichten.Remove(bericht);
            }
        }

        /// <summary>
        /// Berekent de totaal aan deze taak bestede tijd.
        /// </summary>
        /// <returns>De totaal bestede tijd.</returns>
        public virtual TimeSpan TotaalBestedeTijd()
        {
            TimeSpan totaal = new TimeSpan(0);
            foreach (TijdRegistratie registratie in tijdRegistraties)
            {
                totaal = totaal.Add(registratie.Tijd);
            }
            return totaal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gebruiker"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public virtual TimeSpan TotaalBestedeTijd(Gebruiker gebruiker, DateRange? dateRange)
        {
            TimeSpan totaal = new TimeSpan(0);
            foreach (TijdRegistratie registratie in tijdRegistraties)
            {
                if ((gebruiker != null || registratie.Gebruiker == gebruiker) && (dateRange == null || dateRange.Value.Overlap(registratie.Datum.Date)))
                {
                    totaal = totaal.Add(registratie.Tijd);
                }
            }
            return totaal;
        }

        /// <summary>
        /// Totaal bestedTijd per dag
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual TimeSpan TotaalBestedeTijd(DateTime date)
        {
            TimeSpan totaal = new TimeSpan(0);
            foreach (TijdRegistratie registratie in tijdRegistraties)
            {
                if (registratie.Datum.Date == date.Date)
                {
                    totaal = totaal.Add(registratie.Tijd);
                }
            }
            return totaal;
        }

        /// <summary>
        /// Geeft de totaal bestede tijd aan deze taak vanaf de gespecificeerde 'vanaf' datum tot en met de gespecificeerde 'totEnMet' datum
        /// </summary>
        /// <param name="vanaf"></param>
        /// <param name="totEnMet"></param>
        /// <returns></returns>
        public virtual TimeSpan TotaalBestedeTijd(DateTime vanaf, DateTime totEnMet)
        {
            TimeSpan totaal = new TimeSpan(0);
            foreach (TijdRegistratie registratie in tijdRegistraties)
            {
                if (registratie.Datum.Date >= vanaf.Date && registratie.Datum.Date <= totEnMet.Date)
                {
                    totaal = totaal.Add(registratie.Tijd);
                }
            }
            return totaal;
        }

        /// <summary>
        /// Totaal bestedTijd per dag per gebruiker
        /// </summary>
        /// <param name="gebruiker">Gebruiker</param>
        /// <param name="date">Datum</param>
        /// <returns></returns>
        public virtual TimeSpan TotaalBestedeTijd(Gebruiker gebruiker, DateTime date)
        {
            TimeSpan totaal = new TimeSpan(0);
            foreach (TijdRegistratie registratie in tijdRegistraties)
            {
                if (registratie.Gebruiker == gebruiker && registratie.Datum.Date == date.Date)
                {
                    totaal = totaal.Add(registratie.Tijd);
                }
            }
            return totaal;
        }

        /// <summary>
        /// Geef alle registraties terug van een gebruiker
        /// </summary>
        /// <param name="gebruiker"></param>
        /// <returns></returns>
        public virtual IList<TijdRegistratie> GeeftTijdregistratievanGebruiker(Gebruiker gebruiker)
        {
            IList<TijdRegistratie> userTijdRegistraties = new List<TijdRegistratie>();
            foreach (TijdRegistratie registratie in tijdRegistraties)
            {
                if (registratie.Gebruiker == gebruiker)
                {
                    userTijdRegistraties.Add(registratie);
                }
            }
            return userTijdRegistraties;
        }

        /// <summary>
        /// Geef alle registraties terug van een gebruiker voor een bepaalde datum
        /// </summary>
        /// <param name="gebruiker">gebruiker</param>
        /// <param name="sprint">sprint</param>
        /// <param name="date">datum</param>
        /// <returns></returns>
        public virtual IList<TijdRegistratie> GeeftTijdregistratievanGebruiker(Gebruiker gebruiker, Sprint sprint, DateTime date)
        {
            IList<TijdRegistratie> userTijdRegistraties = new List<TijdRegistratie>();
            foreach (TijdRegistratie registratie in tijdRegistraties)
            {
                if (registratie.Gebruiker == gebruiker && registratie.Sprint == sprint && registratie.Datum.ToShortDateString() == date.ToShortDateString())
                {
                    userTijdRegistraties.Add(registratie);
                }
            }
            return userTijdRegistraties;
        }

        /// <summary>
        /// Sluit deze taak. De status wordt nu afgesloten.
        /// En dat datum waarop de taak is afgesloten wordt opgeslagen
        /// </summary>
        public virtual void SluitTaak()
        {
            Status = Status.Afgesloten;
            datumAfgesloten = DateTime.Now;
        }

        /// <summary>
        /// De Status zal NietOpgepakt worden en de behandelaar raakt zijn taak kwijt.
        /// todo: zie beneden.
        /// </summary>
        public virtual void ZetTaakAlsNietOpgepakt()
        {
            if (behandelaar != null)
            {
                behandelaar.GeefTaakAf(this);
            }
        }

        //todo: refactoren / samenvoegen met bovenstaande op het moment dat die ook een logbericht gaat maken
        /// <summary>
        /// Ontkoppel deze taak en zet status als niet opgepakt. Ook wordt er een logbericht van deze actie gemaakt.
        /// </summary>
        /// <param name="titel">De titel van het logbericht.</param>
        /// <param name="text">De text van het logbericht.</param>
        public virtual void OntKoppelTaakEnZetStatusAlsNietOpgepakt(string titel, string text)
        {
            if (behandelaar != null)
            {
                text = text + " \nDe behandelaar was " + behandelaar.Gebruiker.VolledigeNaam;
                behandelaar.GeefTaakAf(this);
            }

            MaakLogBericht(titel, text);
        }

        /// <summary>
        /// Maak een logbericht voor deze task met de gegeven titel en tekst.
        /// </summary>
        /// <param name="titel">De titel.</param>
        /// <param name="text">De text.</param>
        /// <returns>Een logbericht.</returns>
        private void MaakLogBericht(string titel, string text)
        {
            TaskLogBericht logBericht = new TaskLogBericht(this, titel, text);
            if (!logBerichten.Contains(logBericht))
            {
                logBerichten.Add(logBericht);
            }
        }

        /// <summary>
        /// Voeg een tijdregistratie toe.
        /// </summary>
        /// <param name="tijdRegistratie">De tijdregistratie.</param>
        private void VoegTijdRegistratieToe(TijdRegistratie tijdRegistratie)
        {
            if (!tijdRegistraties.Contains(tijdRegistratie))
            {
                tijdRegistraties.Add(tijdRegistratie);
            }
            tijdRegistratie.Task = this;
        }

        /// <summary>
        /// Verwijderd een tijdregistatie
        /// </summary>
        /// <param name="tijdRegistratie">de tijdregistratie</param>
        public virtual void VerwijderTijdRegistratie(TijdRegistratie tijdRegistratie)
        {
            if (tijdRegistraties.Contains(tijdRegistratie))
            {
                tijdRegistraties.Remove(tijdRegistratie);
            }
        }


        #endregion
    }
}