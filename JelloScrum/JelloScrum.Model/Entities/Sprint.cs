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
    /// Represents a Sprint
    /// </summary>
    [ActiveRecord]
    public class Sprint : ModelBase
    {
        #region fields

        private string doel = string.Empty;
        private string omschrijving = string.Empty;
        private DateTime startDatum = DateTime.Now;
        private DateTime eindDatum = DateTime.Now;
        private int werkDagen;
        private bool afgesloten = false;
        private TimeSpan beschikbareUren = new TimeSpan();
        private int aantalManDagen = 0;

        private Project project = null;

        private IList<SprintStory> sprintStories = new List<SprintStory>();
        private IList<SprintGebruiker> sprintGebruikers = new List<SprintGebruiker>();

        #endregion

        #region constructors

        /// <summary>
        /// Default parameterless ctor
        /// </summary>
        public Sprint()
        {
        }

        /// <summary>
        /// Ctor met Project
        /// </summary>
        /// <param name="project"></param>
        public Sprint(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            project.VoegSprintToe(this);
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the doel of this sprint.
        /// </summary>
        /// <value>The doel.</value>
        [Property]
        public virtual string Doel
        {
            get { return doel; }
            set { doel = value; }
        }

        /// <summary>
        /// Gets or sets the omschrijving.
        /// </summary>
        /// <value>The omschrijving.</value>
        [Property(SqlType = "ntext")]
        public virtual string Omschrijving
        {
            get { return omschrijving; }
            set { omschrijving = value; }
        }

        /// <summary>
        /// Gets or sets the start datum on which this sprint will start.
        /// </summary>
        /// <value>The start datum.</value>
        [Property]
        public virtual DateTime StartDatum
        {
            get { return startDatum; }
            set { startDatum = value; }
        }

        /// <summary>
        /// Gets or sets the eind datum on which this sprint will end.
        /// </summary>
        /// <value>The eind datum.</value>
        [Property]
        public virtual DateTime EindDatum
        {
            get { return eindDatum; }
            set { eindDatum = value; }
        }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        [BelongsTo(NotNull = true)]
        public virtual Project Project
        {
            get { return project; }
            set { project = value; }
        }

        /// <summary>
        /// Geeft een readonly collectie met sprintstories. 
        /// Let Op: Om een nieuwe sprintstory aan te maken en toe te voegen gebruik je <see cref="MaakSprintStoryVoor(Story)"/>
        /// </summary>
        /// <value>The stories.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase, OrderBy = "SprintBacklogPrioriteit")]
        public virtual IList<SprintStory> SprintStories
        {
            get { return new ReadOnlyCollection<SprintStory>(sprintStories); }
        }

        /// <summary>
        /// Gets or sets the gebruikers.
        /// </summary>
        /// <value>The gebruikers.</value>
        //[HasAndBelongsToMany(typeof(Gebruiker), Table = "Sprint_Gebruiker", ColumnRef = "gebruiker_id", ColumnKey = "sprint_id", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true)]
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<SprintGebruiker> SprintGebruikers
        {
            get { return new ReadOnlyCollection<SprintGebruiker>(sprintGebruikers); }
        }

        /// <summary>
        /// 
        /// </summary>
        [Property(ColumnType = "TimeSpan")]
        public virtual TimeSpan BeschikbareUren
        {
            get { return beschikbareUren; }
            set { beschikbareUren = value; }
        }


        /// <summary>
        /// Gets or sets het aantal werkdagen van deze sprint.
        /// </summary>
        /// <value>The werk dagen.</value>
        [Property]
        public virtual int WerkDagen
        {
            get { return werkDagen; }
            set { werkDagen = value; }
        }

        /// <summary>
        /// Geeft of zet de status van de Sprint <see cref="Sprint"/> als afgesloten.
        /// </summary>
        /// <value><c>true</c> if afgesloten; else, <c>false</c>.</value>
        [Property]
        public virtual bool IsAfgesloten
        {
            get { return afgesloten; }
            set { afgesloten = value; }
        }

        #endregion

        #region methods

        /// <summary>
        /// Voeg een sprintstory toe aan deze sprint. toet
        /// </summary>
        /// <param name="sprintStory">De sprintstory.</param>
        protected internal virtual void VoegSprintStoryToe(SprintStory sprintStory)
        {
            sprintStory.Sprint = this;
            if (!sprintStories.Contains(sprintStory))
            {
                sprintStories.Add(sprintStory);
            }
        }

        /// <summary>
        /// Verwijdert de gegeven story uit deze sprint
        /// </summary>
        /// <param name="story"></param>
        public virtual void VerwijderStory(Story story)
        {
            SprintStory sprintStory = GeefSprintStoryVanStory(story);

            if (sprintStory == null)
                return;

            sprintStories.Remove(sprintStory);
            sprintStory.Sprint = null;
        }

        /// <summary>
        /// Voeg een gebruiker toe aan deze sprint. Als er al een sprintgebruiker voor deze gebruiker in deze sprint bestaat
        /// wordt er geen nieuwe springebruiker gemaakt, maar deze bestaande teruggegeven.
        /// </summary>
        /// <param name="gebruiker">The gebruiker.</param>
        /// <param name="sprintRol">The sprint rol.</param>
        /// <returns>De nieuwe sprintgebruiker.</returns>
        public virtual SprintGebruiker VoegGebruikerToe(Gebruiker gebruiker, SprintRol sprintRol)
        {
            SprintGebruiker sprintGebruiker = GeefSprintGebruikerVoor(gebruiker);

            if (sprintGebruiker != null)
                return sprintGebruiker;

            return new SprintGebruiker(gebruiker, this, sprintRol);
        }

        /// <summary>
        /// Verwijder de gegeven gebruiker uit deze sprint, door de sprintgebruiker los te koppelen van sprint en de gegeven gebruiker.
        /// </summary>
        /// <param name="gebruiker"></param>
        public virtual void VerwijderGebruiker(Gebruiker gebruiker)
        {
            if (gebruiker == null)
                throw new ArgumentNullException("gebruiker");

            SprintGebruiker sprintGebruiker = gebruiker.GeefSprintGebruikerVoor(this);
            if (sprintGebruiker == null)
                return;

            sprintGebruiker.KoppelSprintGebruikerLos();
        }

        /// <summary>
        /// Verwijdert een sprintGebruiker uit deze sprint
        /// </summary>
        /// <param name="sprintGebruiker"></param>
        protected internal virtual void VerwijderSprintGebruiker(SprintGebruiker sprintGebruiker)
        {
            sprintGebruikers.Remove(sprintGebruiker);
            sprintGebruiker.Sprint = null;
        }

        /// <summary>
        /// Voeg de sprintgebruiker toe aan de collectie van sprintgebruikers.
        /// </summary>
        /// <param name="sprintGebruiker">De sprintgebruiker.</param>
        protected internal virtual void VoegSprintGebruikerToe(SprintGebruiker sprintGebruiker)
        {
            if (!sprintGebruikers.Contains(sprintGebruiker))
            {
                sprintGebruikers.Add(sprintGebruiker);
            }
            sprintGebruiker.Sprint = this;
        }

        /// <summary>
        /// Maak een sprintstory voor de gegeven story en leg alle relaties.
        /// </summary>
        /// <param name="story">De story.</param>
        public virtual SprintStory MaakSprintStoryVoor(Story story)
        {
            foreach (SprintStory ss in sprintStories)
            {
                if (ss.Story == story)
                {
                    return ss;
                }
            }

            return new SprintStory(this, story, story.Schatting);
        }

        /// <summary>
        /// Voegs the werk dag toe.
        /// </summary>
        /// <param name="werkDag">The werk dag.</param>
        public virtual void VoegWerkDagToe(WerkDag werkDag)
        {
            if (HeeftWerkDag(werkDag) == false)
            {
                werkDagen += (int) werkDag;
            }
        }

        /// <summary>
        /// Heeft deze sprint de gegeven werkdag?
        /// </summary>
        /// <param name="werkDag">De werkdag.</param>
        /// <returns>true als de werkdag in de werkdagen collectie zit, anders false</returns>
        public virtual bool HeeftWerkDag(WerkDag werkDag)
        {
            return (int) werkDag == ((int) werkDag & werkDagen);
        }

        /// <summary>
        /// Berekent de totaal aan deze sprint bestede tijd.
        /// </summary>
        /// <returns>De totaal bestede tijd</returns>
        public virtual TimeSpan TotaalBestedeTijd()
        {
            TimeSpan totaal = new TimeSpan(0);
            foreach (SprintStory sprintStory in sprintStories)
            {
                totaal = totaal.Add(sprintStory.Story.TotaalBestedeTijd());
            }
            return totaal;
        }

        /// <summary>
        /// Geeft de nog niet afgesloten sprint stories.
        /// </summary>
        /// <returns>Alle nog niet afgesloten sprintstories</returns>
        public virtual IList<SprintStory> GeefNogNietAfgeslotenSprintStories()
        {
            IList<SprintStory> stories = new List<SprintStory>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                if (sprintStory.Status != Status.Afgesloten)
                {
                    stories.Add(sprintStory);
                }
            }
            return stories;
        }

        /// <summary>
        ///  Geeft de nog niet afgesloten sprint stories met een bepaalde SprintBacklog prioriteit
        /// </summary>
        /// <param name="prioriteit">De prioriteit.</param>
        /// <returns>De nog niet afgesloten sprintstories met de gegeven prioriteit</returns>
        public virtual IList<SprintStory> GeefNogNietAfgeslotenSprintStories(Prioriteit prioriteit)
        {
            IList<SprintStory> stories = new List<SprintStory>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                if (sprintStory.IsVolledigeOpgepakt == false && sprintStory.SprintBacklogPrioriteit == prioriteit)
                {
                    stories.Add(sprintStory);
                }
            }
            return stories;
        }

        /// <summary>
        /// Geeft de afgesloten sprint stories met een bepaalde SprintBacklog prioriteit
        /// </summary>
        /// <param name="prioriteit">De prioriteit.</param>
        /// <returns>De afgesloten sprintstories met de gegeven prioriteit.</returns>
        public virtual IList<SprintStory> GeefDeelsOfGeheleAfgeslotenSprintStories(Prioriteit prioriteit)
        {
            IList<SprintStory> stories = new List<SprintStory>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                if (sprintStory.SprintBacklogPrioriteit == prioriteit)
                {
                    foreach (Task task in sprintStory.Story.Tasks)
                    {
                        // als een taak afgesloten is, is de story deels afgesloten.
                        if (task.Status == Status.Afgesloten)
                        {
                            stories.Add(sprintStory);
                            break;
                        }
                    }
                }
            }
            return stories;
        }

        /// <summary>
        /// Geeft alle stories die gekoppeld zijn aan deze sprint
        /// </summary>
        /// <returns></returns>
        public virtual IList<Story> GeefAlleIngeplandeStories()
        {
            IList<Story> stories = new List<Story>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                stories.Add(sprintStory.Story);
            }
            return stories;

        }

        /// <summary>
        /// Geeft de nog niet opgepakte sprint stories.
        /// todo: unit test voor schrijven
        /// </summary>
        /// <param name="prioriteit">De prioriteit.</param>
        /// <returns>De nog niet opgepakte sprintstories met de gegeven prioriteit</returns>
        public virtual IList<SprintStory> GeefNogNietAfgeslotenSprintStories(string prioriteit)
        {
            Prioriteit p = (Prioriteit) Enum.Parse(typeof (Prioriteit), prioriteit);
            return GeefNogNietAfgeslotenSprintStories(p);
        }

        /// <summary>
        /// Geeft de afgesloten sprint stories.
        /// todo: unit test voor schrijven
        /// </summary>
        /// <param name="prioriteit">De prioriteit.</param>
        /// <returns>De afgesloten sprintstories met de gegeven prioriteit.</returns>
        public virtual IList<SprintStory> GeefDeelsOfGeheleAfgeslotenSprintStories(string prioriteit)
        {
            Prioriteit p = (Prioriteit) Enum.Parse(typeof (Prioriteit), prioriteit);
            return GeefDeelsOfGeheleAfgeslotenSprintStories(p);
        }

        /// <summary>
        /// Geeft de SprintStory van een story.
        /// </summary>
        /// <param name="story">De story.</param>
        /// <returns>De sprintstory van de gegeven story binnen deze sprint.</returns>
        public virtual SprintStory GeefSprintStoryVanStory(Story story)
        {
            foreach (SprintStory sprintStory in sprintStories)
            {
                if (sprintStory.Story == story)
                {
                    return sprintStory;
                }
            }
            return null;
        }

        /// <summary>
        /// Deze method zorgt voor de synchronisatie van de huidige sprintgebruikers met de gegevenlijst
        /// </summary>
        /// <param name="gebruikersLijst"></param>
        public virtual void VerwerkNieuweIngedeeldeGebruikersLijst(IList<Gebruiker> gebruikersLijst)
        {
            // de huidige lijst doorlopen en alles dat niet in de nieuwe lijst voorkomt verwijderen.
            foreach (SprintGebruiker sprintGebruiker in new List<SprintGebruiker>(sprintGebruikers))
            {
                if (gebruikersLijst.Contains(sprintGebruiker.Gebruiker))
                    continue;

                sprintGebruiker.KoppelSprintGebruikerLos();
            }

            foreach (Gebruiker gebruiker in gebruikersLijst)
            {
                if (GeefSprintGebruikerVoor(gebruiker) == null)
                {
                    VoegGebruikerToe(gebruiker, 0);
                }
            }
        }

        /// <summary>
        /// Geeft een lijst met alle gebruikers die ingedeeld zijn
        /// </summary>
        /// <returns>Een lijst met actieve gebruikers</returns>
        public virtual IList<Gebruiker> GeefAlleActieveGebruikers()
        {
            IList<Gebruiker> gebruikersLijst = new List<Gebruiker>();
            foreach (SprintGebruiker sprintGebruiker in sprintGebruikers)
            {
                gebruikersLijst.Add(sprintGebruiker.Gebruiker);
            }

            return gebruikersLijst;
        }

        /// <summary>
        /// Sluit een sprint af.
        /// Vind alle taken voor deze sprint die als status opgepakt hebben en zet deze weer op nietopgepakt.
        /// Schrijf een logbericht dat de status veranderd is door het sluiten van de taak (en wie er mee bezig was enz.)
        /// todo: met query object? 
        /// </summary>
        public virtual IList<Task> SluitSprintAf()
        {
            IList<Task> taken = new List<Task>();

            foreach (SprintStory sprintStory in sprintStories)
            {
                foreach (Task task in sprintStory.Story.Tasks)
                {
                    if (task.Status == Status.Opgepakt)
                    {
                        taken.Add(task);
                        task.OntKoppelTaakEnZetStatusAlsNietOpgepakt("Sprint gesloten", "Deze sprint is afgesloten. ");
                    }
                }
            }

            IsAfgesloten = true;

            return taken;
        }
        /// <summary>
        /// Zoekt naar de sprintgebruiker die hoort bij deze sprint en gebruiker. 
        ///
        /// </summary>
        /// <param name="gebruiker"></param>
        /// <returns>N</returns>
        public virtual SprintGebruiker GeefSprintGebruikerVoor(Gebruiker gebruiker)
        {
            foreach (SprintGebruiker sg in sprintGebruikers)
            {
                if (sg.Gebruiker == gebruiker)
                {
                    return sg;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Geeft alle taken terug die bij deze sprint horen.
        /// </summary>
        /// <returns>Lijst met taken</returns>
        public virtual IList<Task> GeefAlleTakenVanSprint()
        {
            IList<Task> sprintTasks = new List<Task>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                foreach (Task task in sprintStory.Story.Tasks)
                {
                    sprintTasks.Add(task);
                }
            }
            return sprintTasks;
        }

        /// <summary>
        /// Geeft alle door andere gebruikers opgepakte taken terug die bij deze sprint horen.
        /// </summary>
        /// <returns>Lijst met taken</returns>
        public virtual IList<Task> GeefAndermansOpenTakenVanSprint(SprintGebruiker sprintGebruiker)
        {
            IList<Task> sprintTasks = new List<Task>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                foreach (Task task in sprintStory.Story.Tasks)
                {
                    if (task.Status != Status.Opgepakt && task.Behandelaar != sprintGebruiker)
                        sprintTasks.Add(task);
                }
            }
            return sprintTasks;
        }

        /// <summary>
        /// Geeft alle taken met bepaalde status terug die bij deze sprint horen.
        /// </summary>
        /// <returns>Lijst met taken</returns>
        public virtual IList<Task> GeefAlleTakenVanSprint(Status status)
        {
            IList<Task> sprintTasks = new List<Task>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                foreach (Task task in sprintStory.Story.Tasks)
                {
                    if(task.Status == status)
                        sprintTasks.Add(task);
                }
            }
            return sprintTasks;
        }

        /// <summary>
        /// Geeft alle taken terug die niet door de gespecificeerde sprintgebruiker zijn opgepakt.
        /// </summary>
        /// <returns>Lijst met taken</returns>
        public virtual IList<Task> GeefAndermansOfNietOpgepakteTaken(SprintGebruiker gebruiker)
        {
            IList<Task> sprintTasks = new List<Task>();
            foreach (SprintStory sprintStory in sprintStories)
            {
                foreach (Task task in sprintStory.Story.Tasks)
                {
                    if (task.Behandelaar != gebruiker)
                        sprintTasks.Add(task);
                }
            }
            return sprintTasks;
        }

        /// <summary>
        /// Geeft de totaal tijd van alle stories in de sprint
        /// </summary>
        /// <returns></returns>
        public virtual TimeSpan GeefTijdTotaalAlleStories()
        {
            TimeSpan totaalTijd = new TimeSpan();

            foreach (SprintStory sprintStory in sprintStories)
            {
                totaalTijd += sprintStory.Story.Schatting;
            }

            return totaalTijd;
        }


        /// <summary>
        /// Geeft aan hoeveel tijd er nog beschikbaar is in de sprint
        /// </summary>
        /// <returns></returns>
        public virtual TimeSpan ResterendBeschikbareUren()
        {
            TimeSpan resterendeUren = BeschikbareUren - GeefTijdTotaalAlleStories();

            return resterendeUren;
        }

        #endregion
        
        /// <summary>
        /// Hier moet nog zinnig commentaar... en anders mailt Roelof wel een keer dat het nog een keer moet...
        /// </summary>
        /// <param name="werkdag"></param>
        /// <returns></returns>
        public virtual TimeSpan GeefNietAfgeslotenStoriesTotaalSchattingTotEnMetDatum(DateTime werkdag)
        {
            TimeSpan totaalTijd = new TimeSpan();

            foreach (SprintStory sprintStory in sprintStories)
            {
                if (sprintStory.Story.Status != Status.Afgesloten || (sprintStory.Story.Status == Status.Afgesloten && sprintStory.Story.DatumAfgesloten.Value.Date > werkdag.Date))
                    totaalTijd += sprintStory.Story.Schatting;
            }

            return totaalTijd;
        }

    }
}