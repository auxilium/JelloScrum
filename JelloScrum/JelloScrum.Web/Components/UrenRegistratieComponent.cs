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

namespace JelloScrum.Web.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Castle.MonoRail.Framework;
    using Helpers;
    using Model.Entities;
    using Model.Enumerations;

    /// <summary>
    /// Viewcomponent voor het registreren van uren aan taken
    /// </summary>
    [ViewComponentDetails("UrenRegistratieComponent")]
    public class UrenRegistratieComponent : ViewComponent
    {
        private Sprint sprint;
        private SprintGebruiker sprintGebruiker;
        private DateTime maandag;
        private DateTime dinsdag;
        private DateTime woensdag;
        private DateTime donderdag;
        private DateTime vrijdag;
        private string formaction = string.Empty;
        private readonly StringBuilder component = new StringBuilder();
        
        public override void Initialize()
        {
            maandag = ((DateTime) ComponentParams["maandag"]);
            sprint = ((Sprint) ComponentParams["sprint"]);
            sprintGebruiker = ((SprintGebruiker) ComponentParams["sprintGebruiker"]);
            formaction = ((string)ComponentParams["formaction"]);

            dinsdag = maandag.AddDays(1);
            woensdag = maandag.AddDays(2);
            donderdag = maandag.AddDays(3);
            vrijdag = maandag.AddDays(4);

            base.Initialize();
        }

        public override void Render()
        {
            //eerst een scriptje toevoegen
            component.Append("<script type='text/javascript' charset='utf-8' src='/Content/Javascript/urenregistratiecomponent.js'></script>");
            //daarna de formtag met gespecificeerde action plus wat hidden fields
            component.Append("<form id='urenregistratieform' action='" + formaction + "' method='post'>");
            component.Append("<input type='hidden' name='sprintId' value='" + sprint.Id + "'/>");
            component.Append("<input type='hidden' name='maandag' value='" + maandag.ToString("dd-MM-yyyy") + "'/>");
            //de header
            component.Append("<div id='weekSelector' style='float:left;'>");
            component.Append("<a href='urenregistreren.rails?sprintId=" + sprint.Id + "&maandag=" + maandag.AddDays(-7).ToString("dd-MM-yy") + "'>Vorige week</a>&nbsp;");
            component.Append("<span id='weekNr'>Huidige week " + DateHelper.GetWeekNumber(maandag) + "</span>&nbsp;");
            component.Append("<a href='urenregistreren.rails?sprintId="  +sprint.Id + "&maandag=" + maandag.AddDays(7).ToString("dd-MM-yy") + "'>Volgende week</a>&nbsp;");
            component.Append("</div>");
            RenderSaveButton();

            //het component zelf is een table waar je uren in kunt vullen
            component.Append("<table class='tablesorter' id='urenregistratie' style='float: left;'>");
            //daarna de table header, deze bestaat uit 3 rijen, 1 met de dagaanduiding en 2 met totalen
            component.Append("<thead>");
            component.Append("<tr>");
            component.Append("<th>Story</th>");
            //nu de dagen, van ma t/m vr
            component.Append("<th class='header_dag" + HighlightVandaag(maandag) + "'>Ma " + maandag.ToString("dd-MM-yy") + "</th>");
            component.Append("<th class='header_dag" + HighlightVandaag(dinsdag) + "'>Di " + dinsdag.ToString("dd-MM-yy") + "</th>");
            component.Append("<th class='header_dag" + HighlightVandaag(woensdag) + "'>Wo " + woensdag.ToString("dd-MM-yy") + "</th>");
            component.Append("<th class='header_dag" + HighlightVandaag(donderdag) + "'>Do " + donderdag.ToString("dd-MM-yy") + "</th>");
            component.Append("<th class='header_dag" + HighlightVandaag(vrijdag) + "'>Vr " + vrijdag.ToString("dd-MM-yy") + "</th>");
            component.Append("</tr>");
            //de tweede rij bevat het totaal aantal geboekte uren
            component.Append("<tr>");
            component.Append("<th style='padding:4px;'>Totaal</th>");
            component.Append("<th class='header_totaal" + HighlightVandaag(maandag) + "'>" + BerekenTotaalGeregistreerdeTijd(maandag) + "</th>");
            component.Append("<th class='header_totaal" + HighlightVandaag(dinsdag) + "'>" + BerekenTotaalGeregistreerdeTijd(dinsdag) + "</th>");
            component.Append("<th class='header_totaal" + HighlightVandaag(woensdag) + "'>" + BerekenTotaalGeregistreerdeTijd(woensdag) + "</th>");
            component.Append("<th class='header_totaal" + HighlightVandaag(donderdag) + "'>" + BerekenTotaalGeregistreerdeTijd(donderdag) + "</th>");
            component.Append("<th class='header_totaal" + HighlightVandaag(vrijdag) + "'>" + BerekenTotaalGeregistreerdeTijd(vrijdag) + "</th>");
            component.Append("</tr>");
            //de derde rij bevat het totaal aantal zelf geboekte uren
            component.Append("<tr>");
            component.Append("<th style='padding:4px;'>Eigen uren</th>");
            component.Append("<th class='header_totaal" + HighlightVandaag(maandag) + "'>" + BerekenTotaalGeregistreerdeTijd(maandag, sprintGebruiker) + "</th>");
            component.Append("<th class='header_totaal" + HighlightVandaag(dinsdag) + "'>" + BerekenTotaalGeregistreerdeTijd(dinsdag, sprintGebruiker) + "</th>");
            component.Append("<th class='header_totaal" + HighlightVandaag(woensdag) + "'>" + BerekenTotaalGeregistreerdeTijd(woensdag, sprintGebruiker) + "</th>");
            component.Append("<th class='header_totaal" + HighlightVandaag(donderdag) + "'>" + BerekenTotaalGeregistreerdeTijd(donderdag, sprintGebruiker) + "</th>");
            component.Append("<th class='header_totaal" + HighlightVandaag(vrijdag) + "'>" + BerekenTotaalGeregistreerdeTijd(vrijdag, sprintGebruiker) + "</th>");
            component.Append("</tr>");
            component.Append("</thead>");
            
            //nu de inhoud van de tabel
            component.Append("<tbody>");
            //we gaan de taken per userstory groeperen
            foreach (SprintStory sprintStory in sprint.SprintStories)
            {
                RenderSprintStory(sprintStory);
            }

            component.Append("</tbody>");
            component.Append("</table>");
            RenderSaveButton();
            component.Append("</form>");

            RenderText(component.ToString());
        }

        
        private void RenderSaveButton()
        {
            this.component.Append("<div id='opslaan' style='float:right;'>");
            this.component.Append("<input type='submit' value='Opslaan' class='button opslaan'/>");
            this.component.Append("</div>");
        }

        /// <summary>
        /// Geeft de css class terug voor het highlighten van de huidige dag
        /// </summary>
        /// <param name="dag"></param>
        /// <returns></returns>
        private static string HighlightVandaag(DateTime dag)
        {
            if (dag.Date == DateTime.Today)
                return " today";
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprintStory"></param>
        private void RenderSprintStory(SprintStory sprintStory)
        {
            //de eerste row bevat de sprintstory
            this.component.Append("<tr>");
            this.component.Append("<td class='story'><span style='font-weight: bold;padding-right: 10px;'>" + sprintStory.Story.Titel + "</span>" + new OpmaakHelper().UrenStatus(sprintStory.Story) + " status: " + sprintStory.Status + "</td>");
            //aantal zelf geboekte uren op story per dag
            this.component.Append("<td class='story" + HighlightVandaag(maandag) + "'>&nbsp;</td>");
            this.component.Append("<td class='story" + HighlightVandaag(dinsdag) + "'>&nbsp;</td>");
            this.component.Append("<td class='story" + HighlightVandaag(woensdag) + "'>&nbsp;</td>");
            this.component.Append("<td class='story" + HighlightVandaag(donderdag) + "'>&nbsp;</td>");
            this.component.Append("<td class='story" + HighlightVandaag(vrijdag) + "'>&nbsp;</td>");
            this.component.Append("</tr>");
            foreach (Task task in sprintStory.Story.Tasks)
            {
                RenderTaskRow(task);
            }
           
        }


        private void RenderTaskRow(Task task)
        {
            this.component.Append("<tr class='expand-child'>");
            this.component.Append("<td class='" + HighlightEigenTaak(task) + "'><span style='padding-right: 10px;padding-left:20px;'>Taak: <a href='/CommentaarBericht/TaakCommentaar.rails?Id=" + task.Id + "&height=800&width=600' class='thickbox' style='color: #000000;'>" + task.Titel + "</a> status: " + task.Status + "</td>");
            RenderTaskRegistrationInput(task, maandag);
            RenderTaskRegistrationInput(task, dinsdag);
            RenderTaskRegistrationInput(task, woensdag);
            RenderTaskRegistrationInput(task, donderdag);
            RenderTaskRegistrationInput(task, vrijdag);
            this.component.Append("</tr>");
        }

        private string HighlightEigenTaak(Task task)
        {
            if (task.Behandelaar == sprintGebruiker && task.Status != Status.Afgesloten)
                return " owntask";
            return string.Empty;
        }

        private void RenderTaskRegistrationInput(Task task, DateTime dag)
        {
            Guid index = Guid.NewGuid();
            this.component.Append("<td class='task_registration" + HighlightVandaag(dag) + HighlightEigenTaak(task) + "'>");
            if (dag <= DateTime.Today)
            {
                //this.component.Append("<div class='uren_readonly " + HighlightVandaag(dag)  + "'>" + TimeSpanHelper.TimeSpanInMinuten(task.TotaalBestedeTijd(this.sprintGebruiker.Gebruiker, dag)) + "</div>");
                this.component.Append("<div class='uren_input" + HighlightVandaag(dag) + HighlightEigenTaak(task) + "'>");
                this.component.Append("<input type='text' name='urenregistratie[" + index + "].AantalUren' id='urenregistratie[" + index + "]_AantalUren' value='" + TimeSpanHelper.TimeSpanInMinuten(task.TotaalBestedeTijd(this.sprintGebruiker.Gebruiker, dag)) + "'/>");
                this.component.Append("<input type='hidden' name='urenregistratie[" + index + "].Dag' id='urenregistratie[" + index + "]_Dag' value='" + dag.ToString("dd-MM-yyyy") + "'/>");
                this.component.Append("<input type='hidden' name='urenregistratie[" + index + "].Task.Id' id='urenregistratie[" + index + "]_Task.Id' value='" + task.Id + "'/>");
                this.component.Append("<input type='hidden' name='urenregistratie[" + index + "].SprintGebruiker.Id' id='urenregistratie[" + index + "]_SprintGebruiker_Id' value='" + this.sprintGebruiker.Id + "'/>");
                this.component.Append("</div>");
            }
            else
            {
                this.component.Append("&nbsp;");
            }
            this.component.Append("</td>");

        }

        /// <summary>
        /// Bereken de totaal geregistreerde tijd op de gespecificeerde dag van de gespecificeerde gebruiker
        /// </summary>
        /// <param name="dag"></param>
        /// <param name="gebruiker"></param>
        /// <returns></returns>
        private string BerekenTotaalGeregistreerdeTijd(DateTime dag, SprintGebruiker gebruiker)
        {
            double total = 0;
            foreach (Task task in sprint.GeefAlleTakenVanSprint())
            {
                total += TimeSpanHelper.TimeSpanInMinuten(task.TotaalBestedeTijd(gebruiker.Gebruiker, dag));
            }
            return total.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dag"></param>
        /// <returns></returns>
        private string BerekenTotaalGeregistreerdeTijd(DateTime dag)
        {
            double total = 0;
            foreach (Task task in sprint.GeefAlleTakenVanSprint())
            {
                total += TimeSpanHelper.TimeSpanInMinuten(task.TotaalBestedeTijd(dag));
            }
            return total.ToString();
        }
    }
}