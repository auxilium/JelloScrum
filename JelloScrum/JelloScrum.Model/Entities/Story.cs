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
    using Interfaces;

    /// <summary>
    /// Represents a Story
    /// </summary>
    [ActiveRecord]
    public class Story : ModelBase, ILogable
    {
        #region fields

        private string titel = string.Empty;
        private string omschrijving = string.Empty;
        private string howtoDemo = string.Empty;
        private string notitie = string.Empty;
        private TimeSpan schatting = new TimeSpan();

        private Project project = null;
        private Gebruiker aangemaaktDoor;
        private Impact? impact;
        private Prioriteit productBacklogPrioriteit = Prioriteit.Onbekend;
        private StoryType storyType = StoryType.UserStory;

        private IList<StoryLogBericht> logBerichten = new List<StoryLogBericht>();
        private IList<StoryCommentaarBericht> commentaarBerichten = new List<StoryCommentaarBericht>();

        private IList<SprintStory> sprintStories = new List<SprintStory>();
        private IList<Task> tasks = new List<Task>();

        private StoryPoint storyPoints = StoryPoint.Onbekend;
        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Story"/> class.
        /// </summary>
        protected Story()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Story"/> class.
        /// </summary>
        /// <param name="project">The project.</param>  
        /// <param name="aangemaaktDoor">The aangemaakt door.</param>
        /// <param name="impact">The impact.</param>
        /// <param name="storyType">Type of the story.</param>
        public Story(Project project, Gebruiker aangemaaktDoor, Impact? impact, StoryType storyType)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project", "Het project mag niet null zijn.");
            }

            if (aangemaaktDoor == null)
            {
                throw new ArgumentNullException("aangemaaktDoor", "De gebruiker die deze story aanmaakt kan niet null zijn.");
            }

            project.VoegStoryToe(this);
            this.aangemaaktDoor = aangemaaktDoor;
            this.impact = impact;
            this.storyType = storyType;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the titel.
        /// </summary>
        /// <value>The titel.</value>
        [Property, ValidateNonEmpty("Vul een titel in.")]
        public virtual string Titel
        {
            get { return titel; }
            set { titel = value; }
        }

        /// <summary>
        /// Gets or sets the omschrijving.
        /// </summary>
        /// <value>The omschrijving.</value>
        [Property(SqlType = "ntext"), ValidateNonEmpty("Vul een omschrijving in.")]
        public virtual string Omschrijving
        {
            get { return omschrijving; }
            set { omschrijving = value; }
        }

        /// <summary>
        /// Gets or sets the howto demo.
        /// </summary>
        /// <value>The howto demo.</value>
        [Property(SqlType = "ntext")]
        public virtual string HowtoDemo
        {
            get { return howtoDemo; }
            set { howtoDemo = value; }
        }

        /// <summary>
        /// Gets or sets the notitie.
        /// </summary>
        /// <value>The notitie.</value>
        [Property(SqlType = "ntext")]
        public virtual string Notitie
        {
            get { return notitie; }
            set { notitie = value; }
        }

        /// <summary>
        /// De tijd die geschat is voor deze story
        /// </summary>
        /// <value>De schatting.</value>
        [Property, ValidateNonEmpty("Vul een schatting in.")]
        public virtual TimeSpan Schatting
        {
            get { return schatting; }
            set { schatting = value; }
        }

        /// <summary>
        /// Gets or sets the product backlog.
        /// </summary>
        /// <value>The product backlog.</value>
        [BelongsTo(NotNull = true)]
        public virtual Project Project
        {
            get { return project; }
            set { project = value; }
        }

        /// <summary>
        /// De gebruiker die deze story aangemaakt heeft
        /// </summary>
        [BelongsTo(NotNull = true)]
        public virtual Gebruiker AangemaaktDoor
        {
            get { return aangemaaktDoor; }
            set { aangemaaktDoor = value; }
        }

        /// <summary>
        /// Dee impact.
        /// </summary>
        /// <value>De impact.</value>
        [Property]
        public virtual Impact? Impact
        {
            get { return impact; }
            set { impact = value; }
        }

        /// <summary>
        /// Gets or sets the product backlog prioriteit.
        /// </summary>
        /// <value>The product backlog prioriteit.</value>
        [Property]
        public virtual Prioriteit ProductBacklogPrioriteit
        {
            get { return productBacklogPrioriteit; }
            set { productBacklogPrioriteit = value; }
        }

        /// <summary>
        /// Gets or sets the type of the story.
        /// </summary>
        /// <value>The type of the story.</value>
        [Property]
        public virtual StoryType StoryType
        {
            get { return storyType; }
            set { storyType = value; }
        }

        /// <summary>
        /// Gets or sets the log berichten.
        /// </summary>
        /// <value>The log berichten.</value>
        [HasMany(Table = "LogBericht", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<StoryLogBericht> LogBerichten
        {
            get { return logBerichten; }
            set { logBerichten = value; }
        }

        /// <summary>
        /// Gets or sets the commentaren.
        /// </summary>
        /// <value>The commentaren.</value>
        [HasMany(Table = "CommentaarBericht", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true, Inverse = true)]
        public virtual IList<StoryCommentaarBericht> CommentaarBerichten
        {
            get { return commentaarBerichten; }
            set { commentaarBerichten = value; }
        }

        /// <summary>
        /// Geeft een readonly collectie met de sprintstories van deze story
        /// Let Op: om een nieuwe sprintstory te maken en toe te voegen gebruik je <see cref="Sprint.MaakSprintStoryVoor(Story)"/>
        /// </summary>
        /// <value>The sprints.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<SprintStory> SprintStories
        {
            get { return new ReadOnlyCollection<SprintStory>(sprintStories); }
        }

        /// <summary>
        /// De tasks.
        /// </summary>
        /// <value>De tasks.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<Task> Tasks
        {
            get { return new ReadOnlyCollection<Task>(tasks); }
        }

        /// <summary>
        /// Het aantal storypunten dat door middel van een planning game gerealiseerd is.
        /// </summary>
        [Property]
        public virtual StoryPoint StoryPoints
        {
            get { return storyPoints; }
            set { storyPoints = value; }
        }

        #endregion

        #region derived properties

        /// <summary>
        /// Geeft de status van deze story gebaseerd op de statussen van de tasks.
        /// - Als alle tasks afgesloten zijn dan is de story ook afgesloten
        /// - Als een of meer tasks opgepakt zijn dan is de story ook opgepakt
        /// - Als alle taken niet zijn opgepakt of afgesloten dna is de story status nietopgepakt
        /// </summary>
        /// <value>De status.</value>
        public virtual Status Status
        {
            get
            {
                int opgepakt = 0;
                int afgesloten = 0;
                int ingepland = 0;
                foreach (Task task in tasks)
                {
                    if (task.Status == Status.Opgepakt)
                    {
                        opgepakt++;
                    }
                    if (task.Status == Status.Afgesloten)
                    {
                        afgesloten++;
                    }
                }

                if (afgesloten == tasks.Count && tasks.Count > 0)
                {
                    return Status.Afgesloten;
                }
                else if (opgepakt == 0 || tasks.Count == 0)
                {
                    return Status.NietOpgepakt;
                }
                else
                {
                    return Status.Opgepakt;
                }
            }
        }

        /// <summary>
        /// Geeft aan of de story gepland kan worden
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is te plannen; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsTePlannen
        {
            get
            {
                // story is afgesloten dus niet te plannen
                if (Status == Status.Afgesloten)
                {
                    return false;
                }

                foreach (SprintStory sprintStory in SprintStories)
                {
                    // story zit in een sprint die nog niet afgesloten is. story is dus ingepland en er wordt nog aan gewerkt
                    if (!sprintStory.Sprint.IsAfgesloten)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Geef de getal waarde van de storypoint terug
        /// </summary>
        /// <returns>int met de storypoint waarde</returns>
        public virtual int WaardeStoryPoints
        {
            get
            {
                return (int) Enum.Parse(typeof (StoryPoint), Enum.GetName(typeof (StoryPoint), StoryPoints));
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Voeg een task aan deze story toe.
        /// </summary>
        /// <param name="task">De task.</param>
        public virtual void VoegTaskToe(Task task)
        {
            if (!Tasks.Contains(task))
            {
                tasks.Add(task);
            }
            task.Story = this;
        }

        /// <summary>
        /// Verwijder de gegeven task van deze story
        /// </summary>
        /// <param name="task">De task.</param>
        public virtual void VerwijderTask(Task task)
        {
            if (tasks.Contains(task))
                tasks.Remove(task);
            task.Story = null;
        }

        /// <summary>
        /// Voeg eem sprintStory aan deze story toe.
        /// </summary>
        /// <param name="sprintStory">De sprintstory.</param>
        protected internal virtual void VoegSprintStoryToe(SprintStory sprintStory)
        {
            if (!sprintStories.Contains(sprintStory))
            {
                sprintStories.Add(sprintStory);
            }
            sprintStory.Story = this;
        }

        /// <summary>
        /// Voegs the commentaar bericht toe.
        /// </summary>
        /// <param name="tekst">The tekst.</param>
        public virtual void VoegCommentaarBerichtToe(string tekst)
        {
            if (!string.IsNullOrEmpty(tekst))
            {
                StoryCommentaarBericht bericht = new StoryCommentaarBericht(this, tekst);
                commentaarBerichten.Add(bericht);
            }
        }

        /// <summary>
        /// Berekent de totaal bestede tijd aan deze story 
        /// aan de hand van de op de taken geboekte tijd.
        /// </summary>
        /// <returns>De totaal bestede tijd.</returns>
        public virtual TimeSpan TotaalBestedeTijd()
        {
            TimeSpan totaal = new TimeSpan(0);
            foreach (Task task in tasks)
            {
                totaal = totaal.Add(task.TotaalBestedeTijd());
            }
            return totaal;
        }

        /// <summary>
        /// Berekent de totaal bestede tijd aan deze story 
        /// aan de hand van de op de taken geboekte tijd tot en met de gespecificeerde datum
        /// </summary>
        /// <returns>De totaal bestede tijd.</returns>
        public virtual TimeSpan TotaalBestedeTijd(DateTime vanaf, DateTime totEnMet)
        {
            TimeSpan totaal = new TimeSpan(0);
            foreach (Task task in tasks)
            {
                totaal = totaal.Add(task.TotaalBestedeTijd(vanaf, totEnMet));
            }
            return totaal;
        }


        /// <summary>
        /// Geeft lijst met alle taken met bepaalde status.
        /// </summary>
        /// <returns>Lijst van taken met bepaalde status</returns>
        public virtual IList<Task> GeefTakenMetStatus(Status status)
        {
            IList<Task> taken = new List<Task>();
            foreach (Task task in tasks)
            {
                if (task.Status == status)
                {
                    taken.Add(task);
                }
            }
            return taken;
        }

        /// <summary>
        /// Geeft alle tijdregistraties van de taken van deze story
        /// </summary>
        /// <returns></returns>
        public virtual IList<TijdRegistratie> GeefTijdRegistraties()
        {
            List<TijdRegistratie> tijdRegistraties = new List<TijdRegistratie>();
            foreach (Task task in tasks)
            {
                tijdRegistraties.AddRange(task.TijdRegistraties);
            }
            tijdRegistraties.Sort(delegate(TijdRegistratie t1, TijdRegistratie t2)
                                      {
                                          return t1.Datum.CompareTo(t2.Datum);
                                      });

            return tijdRegistraties;
        }

        /// <summary>
        /// Check of de uren schatting van alle taken kleiner of gelijk is aan de uren schatting van de story
        /// </summary>
        /// <returns>true als uren taken kleiner of gelijk is aan uren schatting story</returns>
        public virtual bool CheckSchattingTaken()
        {
            double urenTask = 0; 
            foreach (Task task in Tasks)
            {
                urenTask += task.Schatting.TotalMinutes;
            }

            if(urenTask <= Schatting.TotalMinutes)
                return true;
            
            return false;
        }

        /// <summary>
        /// Geeft de datum waarop de laatste taak is afgesloten.
        /// Er komt alleen een datum terug als alle taken van deze story zijn afgesloten.
        /// Anders sturen we null terug.
        /// </summary>
        public virtual DateTime? DatumAfgesloten
        {
            get
            {
                if (Status != Status.Afgesloten)
                    return null;

                DateTime? laatsteDatum = null;

                foreach (Task task in tasks)
                {
                    if (laatsteDatum.HasValue == false || task.DatumAfgesloten > laatsteDatum.Value)
                    {
                        laatsteDatum = task.DatumAfgesloten;
                    }
                }
                               

                return laatsteDatum;
            }
            
        }

        #endregion

    }
}