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
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Castle.ActiveRecord;
    using JelloScrum.Model.Entities;
    using Model;
    using Model.Enumerations;
    using NHibernate;
    using NHibernate.Criterion;
    using QueryObjects;

    /// <summary>
    /// BurnDown functionalteiten
    /// </summary>
    public class BurnDown2Helper
    {
        /// <summary>
        /// Berekent de totaal geschatten tijd
        /// </summary>
        /// <returns></returns>
        public TimeSpan TotaalGeschatteTijd(Sprint sprint)
        {
            TimeSpan totaal = new TimeSpan();
            foreach (SprintStory sprintStory in sprint.SprintStories)
            {
                totaal += sprintStory.Schatting;
            }
            return totaal;
        }

        /// <summary>
        /// voorheen AantaldagenTotNu
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        public int AantalWerkdagenTotNu(Sprint sprint)
        {
            return CalculateWerkdagen(sprint, DateTime.Today);
        }

        /// <summary>
        /// Voorheen TotaalSprintTijd
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        public int AantalWerkdagenSprint(Sprint sprint)
        {
            return CalculateWerkdagen(sprint, null);
        }

        /// <summary>
        /// // zondag willen we zaterdag van maken
        /// // checken of deze bestaat, anders steeds 1 dag eraf
        /// // anders return value
        /// </summary>
        /// <param name="date"></param>
        /// <param name="toeter"></param>
        /// <returns></returns>
        public double laatsteWaarde(DateTime date, IDictionary<DateTime, double> toeter)
        {
            DateTime gisteren = date.AddDays(-1);
            while (!toeter.ContainsKey(gisteren.Date))
            {
                gisteren = gisteren.AddDays(-1);
            }
            return toeter[gisteren.Date];
        }

        /// <summary>
        /// Hier worde de datapoint voor de burndown berekent
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        public IDictionary<DateTime, double> Werk(Sprint sprint)
        {
            TotaalBestedenTijdOpSprintQuery query = new TotaalBestedenTijdOpSprintQuery();
            query.sprint = sprint;

            ICriteria crit = query.GetQuery(ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(typeof(ModelBase)));
            crit.AddOrder(Order.Asc("Datum"));
            IList result = crit.List();
            IDictionary<DateTime, double> dataPoints = new SortedDictionary<DateTime, double>();

            double totaal = TotaalGeschatteTijd(sprint).TotalHours;
            foreach (TijdRegistratie registratie in result)
            {
                if (!dataPoints.ContainsKey(registratie.Datum.Date))
                {
                    dataPoints.Add(registratie.Datum.Date, totaal);
                }
                totaal -= registratie.Tijd.TotalHours;
                dataPoints[registratie.Datum.Date] = totaal;
            }

            DateTime temp = sprint.StartDatum;
            while (temp <= sprint.EindDatum)
            {
                if (temp.Date <= DateTime.Today)
                {
                    if (temp <= sprint.StartDatum && !dataPoints.ContainsKey(temp.Date))
                    {
                        //Als er de eerste dag niet gewerkt is en hij probeer de dag er voor te pakken heb je een infinte loop..
                        dataPoints.Add(temp.Date, TotaalGeschatteTijd(sprint).TotalHours);
                    }

                    if (temp.DayOfWeek != DayOfWeek.Saturday && temp.DayOfWeek != DayOfWeek.Sunday)
                    {
                        if (!dataPoints.ContainsKey(temp.Date))
                        {
                            dataPoints.Add(temp.Date, laatsteWaarde(temp, dataPoints));
                        }
                    }
                }
                temp = temp.AddDays(1);
            }

            return dataPoints;
        }

        private int CalculateWerkdagen(Sprint sprint, DateTime? date)
        {
            if (date == null)
            {
                date = sprint.EindDatum.Date;
            }

            int dayCount = 0;
            DateTime temp = sprint.StartDatum;
            while (temp <= date)
            {
                if (temp.DayOfWeek != DayOfWeek.Saturday && temp.DayOfWeek != DayOfWeek.Sunday)
                {
                    dayCount += 1;
                }
                temp = temp.AddDays(1);
            }
            return dayCount;
        }

        ///
        private string GenereerJavascript(Sprint sprint)
        {

            DateTime startdatum = sprint.StartDatum;
            DateTime einddatum = sprint.EindDatum;

            DateTime temp = startdatum;

            IList<DateTime> werkdagen = new List<DateTime>();

            while (temp <= einddatum)
            {
                if (temp.DayOfWeek != DayOfWeek.Saturday && temp.DayOfWeek != DayOfWeek.Sunday)
                {
                    werkdagen.Add(temp);
                }
                temp = temp.AddDays(1);
            }

            //Zaken voor de balkjes
            AlleTijdregistratiesTussenDatumsPerSprint alleTijdregistratiesTussenDatumsPerSprint = new AlleTijdregistratiesTussenDatumsPerSprint();
            alleTijdregistratiesTussenDatumsPerSprint.startDate = startdatum;
            alleTijdregistratiesTussenDatumsPerSprint.endDate = einddatum;
            alleTijdregistratiesTussenDatumsPerSprint.sprint = sprint;

            ICriteria crit = alleTijdregistratiesTussenDatumsPerSprint.GetQuery(ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(typeof(ModelBase)));

            IList<TijdRegistratie> tijdregistraties = crit.List<TijdRegistratie>();

            IDictionary<DateTime, TimeSpan> totaleTijdPerDag = new Dictionary<DateTime, TimeSpan>();
            foreach (TijdRegistratie registratie in tijdregistraties)
            {
                if (!totaleTijdPerDag.ContainsKey(registratie.Datum.Date))
                {
                    totaleTijdPerDag.Add(registratie.Datum.Date, TimeSpan.Zero);
                }

                totaleTijdPerDag[registratie.Datum.Date] += registratie.Tijd;
            }

            TimeSpan totaalbestedeTijd = new TimeSpan();

            //Zaken voor de ideale lijn
            double totaalTijd = sprint.GetTotalTimeEstimatedForAllStories().TotalHours;
            double urenPerDag = totaalTijd / (werkdagen.Count - 1);
            double aantalNogTeBestedenUren = totaalTijd;

            StringBuilder stringbuilder = new StringBuilder();
            StringBuilder arrayString = new StringBuilder();
            StringBuilder arrayTicks = new StringBuilder();
            StringBuilder arrayDone = new StringBuilder();
            StringBuilder tabelMetGegevens = new StringBuilder();

            int i = 0;

            if (totaalTijd > 0)
            {
                int chartWidth = ((werkdagen.Count - 1)*40);
                double chartHeight = Math.Round((totaalTijd*2) + 150, 0);
                
                if (chartWidth < 500)
                    chartWidth = 500;

                if (chartHeight < 300)
                    chartHeight = 300;

                stringbuilder.AppendLine("swfobject.embedSWF('/Content/open-flash-chart.swf', 'burndown_chart', '" +
                                         chartWidth + "', '" + (chartHeight) +
                                         "', '9.0.0');");

                stringbuilder.AppendLine("function open_flash_chart_data()");
                stringbuilder.AppendLine("{");
                stringbuilder.AppendLine("return JSON.stringify(data_1);");
                stringbuilder.AppendLine("}");


                stringbuilder.Append("var data_1 = {'tooltip': { 'mouse': 2}, 'elements': [{'type': 'line','values':");

                arrayString.Append("[");

                
                double vandaagBestedeUren = 0;
                double teveelBestedeUrenVandaag = 0;
                double afhalenVanNietAfgeslotenUren = 0;
                double gewonnenTijd = 0;
                double resterendeUren = 0;
                double reductieTijdTeVeel = 0;



                tabelMetGegevens.Append("<table class=\"tablesorter\">");
                tabelMetGegevens.Append("<thead>");
                tabelMetGegevens.Append("<tr><th>Datum</th><th>Resterende uren</th><th>nietAfgerond</th><th>Afhalen</th><th>Teveel</th><th>gewonnen</th><th>gewerkte uren</th></tr>");
                tabelMetGegevens.Append("</thead>");
                tabelMetGegevens.Append("<tbody>");

                foreach (DateTime werkdag in werkdagen)
                {
                    //IDEALE LIJN
                    arrayTicks.Append("'" + werkdag.DayOfWeek + " " + werkdag.ToShortDateString() + "'");

                    arrayString.Append("" + Math.Round(totaalTijd, 0) + "");


                    totaalTijd = totaalTijd - urenPerDag;
                    
                    //Hier gaan we de bars opbouwen.
                    vandaagBestedeUren = TotaalGewerkteUrenVanNietAfgerondeStoriesVoorDatum(werkdag.Date, sprint);//Groen

                    //Rodebalk
                    gewonnenTijd = GewonnenUrenAanStoriesVoorDatum(werkdag.Date, sprint);
                    teveelBestedeUrenVandaag = TeveelGewerkteUrenAanStoriesVoorDatum(werkdag.Date, sprint);//Rood
                    
                    if(gewonnenTijd > teveelBestedeUrenVandaag)
                    {
                        reductieTijdTeVeel = 0;//We zetten teveel besteed op 0 omdat er meer is gewonnen dan teveel is gewerkt.
                        afhalenVanNietAfgeslotenUren = vandaagBestedeUren;// + reductieTijd;
                    }
                    else if(gewonnenTijd < teveelBestedeUrenVandaag)
                    {
                        reductieTijdTeVeel = teveelBestedeUrenVandaag - gewonnenTijd;
                        afhalenVanNietAfgeslotenUren = vandaagBestedeUren + reductieTijdTeVeel;
                    }
                    else
                    {
                        afhalenVanNietAfgeslotenUren = vandaagBestedeUren ;
                    }
                    

                    //Nu gaan we de blauwe balk opbouwen
                    
                    resterendeUren = Math.Round(sprint.GetTotalEstimatedTimeForNotClosedStoriesTil(werkdag).TotalHours - afhalenVanNietAfgeslotenUren); //Blauw

                    tabelMetGegevens.Append("<tr><td>" + werkdag.DayOfWeek + " " + werkdag.ToShortDateString() + "</td><td>" + resterendeUren + "</td><td>" + sprint.GetTotalEstimatedTimeForNotClosedStoriesTil(werkdag).TotalHours + "</td><td>" + afhalenVanNietAfgeslotenUren + "</td><td>" + teveelBestedeUrenVandaag + "</td><td>" + gewonnenTijd + "</td><td>" + vandaagBestedeUren + "</td></tr>");

                    arrayDone.Append("[" + resterendeUren + "," + Math.Round(reductieTijdTeVeel) + "," + Math.Round(vandaagBestedeUren) + "]");


                    //Rest
                    if (werkdag != werkdagen[werkdagen.Count - 1])
                    {
                        arrayString.Append(",");
                        arrayTicks.Append(",");
                        arrayDone.Append(",");
                    }
                }
                tabelMetGegevens.Append("</tbody>");
                tabelMetGegevens.Append("</table>");

                arrayString.Append("]}");

                stringbuilder.Append(arrayString);

                stringbuilder.Append(",{'type': 'bar_stack', 'colours': [ '#6699ff', '#ff3333' , '#66cc33' ], 'values': [" +
                                     arrayDone + "]");
                stringbuilder.Append(", 'keys': [ { 'colour': '#6699ff', 'text': 'Nog uit te voeren uren werk', 'font-size': 14 }, { 'colour': '#ff3333', 'text': 'Niet ingeschat werk (overshoot)', 'font-size': 14 }, { 'colour': '#66cc33', 'text': 'Gewerkte uren', 'font-size': 14 } ]");
                stringbuilder.Append("}");
                stringbuilder.Append("], 'x_axis': { 'labels': { 'rotate': 300, 'labels': [" + arrayTicks +
                                     "]} }, 'y_axis': {'min':    0,'max':    " +
                                     Math.Round((sprint.GetTotalTimeEstimatedForAllStories().TotalHours), 0) +
                                     ", 'steps' : 10, 'offset': 300}, 'bg_colour': '#FFFFFF', 'title': { 'text': 'Burndown chart', 'style': 'font-size: 20px;' }");
                

                stringbuilder.Append("};");
            }
            else
            {
                stringbuilder.AppendLine("$('#burndown_chart').html('Er is geen schatting ingevuld bij de stories, er kan dus geen burndown gemaakt worden.');");
            }

            //Deze kan je aanzetten om meer inzicht te krijgen in wat er in de burndown gebeurd
            //stringbuilder.AppendLine("$('#burndown_uitleg').html('" + tabelMetGegevens + "');");


            return stringbuilder.ToString();

  
  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="sprint"></param>
        /// <returns></returns>
        private double BerekenReedsBestedeUren(DateTime dateTime, Sprint sprint)
        {
          
            double totaalUren = 0;
            foreach (SprintStory sprintStory in sprint.SprintStories)
            {
              
            }
            return totaalUren;
        }

        private double TotaalGewerkteUrenVanNietAfgerondeStoriesVoorDatum(DateTime dateTime, Sprint sprint)
        {
            //We gaan hier de gewerkte uren terug geven van de opgegeven datum
            //Dit doen we zolang de storie nog niet is afgesloten of tot aan de datum dat hij afgesloten is.

            //Zijn er meer uren gewerkt dan geschat dan geven we de schatting terug. Het teveel aan gewerkte uren wordt verwerkt in een andere functie.

            double totaalGewerkteTijd = 0;
            foreach (SprintStory sprintStory in sprint.SprintStories)
            {
                if (sprintStory.Status == Status.Afgesloten && dateTime.Date < sprintStory.Story.DatumAfgesloten.Value.Date)
                {
                    //Als er meer werk is gedaan dan dat er is geschat, sturen we de schatting mee. het teveel aan uren zal in rood worden weergegeven.
                    
                    
                    if (sprintStory.Story.TotaalBestedeTijd(sprint.StartDatum, dateTime) < sprintStory.Schatting)
                    {
                        totaalGewerkteTijd += TimeSpanHelper.TimeSpanInMinuten(sprintStory.Story.TotaalBestedeTijd(sprint.StartDatum, dateTime));
                    }
                    else
                    {
                        //er is meer tijd gewerkt dan geschat dus we gaan de schatting terug geven.
                        totaalGewerkteTijd += TimeSpanHelper.TimeSpanInMinuten(sprintStory.Schatting);
                    }

                }
                else if (sprintStory.Status != Status.Afgesloten)
                {
                    //Hier gaan we als de story nog niet is afgesloten.
                    if (sprintStory.Story.TotaalBestedeTijd(sprint.StartDatum, dateTime) < sprintStory.Schatting)
                    {
                        totaalGewerkteTijd += TimeSpanHelper.TimeSpanInMinuten(sprintStory.Story.TotaalBestedeTijd(sprint.StartDatum, dateTime));
                    }
                    else
                    {
                        //er is meer tijd gewerkt dan geschat dus we gaan de schatting terug geven.
                        totaalGewerkteTijd += TimeSpanHelper.TimeSpanInMinuten(sprintStory.Schatting);
                    }
                }
            }
            return totaalGewerkteTijd;
        }

        private double TeveelGewerkteUrenAanStoriesVoorDatum(DateTime dateTime, Sprint sprint)
        {
            double totaalTeVeelGewerkteTijd = 0;
            foreach (SprintStory sprintStory in sprint.SprintStories)
            {
               /* if (sprintStory.Status == Status.Afgesloten && dateTime.Date < sprintStory.Story.DatumAfgesloten.Value.Date)
                {
                    //Als er meer werk is gedaan dan dat er is geschat, sturen we de schatting mee. het teveel aan uren zal in rood worden weergegeven.


                    if (sprintStory.Story.TotaalBestedeTijd(sprint.StartDatum, dateTime) > sprintStory.Schatting)
                    {
                        totaalTeVeelGewerkteTijd += TimeSpanHelper.TimeSpanInMinuten(sprintStory.Story.TotaalBestedeTijd(sprint.StartDatum, dateTime) - sprintStory.Schatting);
                    }
                    

                }
                else if (sprintStory.Status != Status.Afgesloten)
                {*/
                    //Hier gaan we als de story nog niet is afgesloten.
                    if (sprintStory.Story.TotaalBestedeTijd(sprint.StartDatum, dateTime) > sprintStory.Schatting)
                    {
                        totaalTeVeelGewerkteTijd += TimeSpanHelper.TimeSpanInMinuten(sprintStory.Story.TotaalBestedeTijd(sprint.StartDatum, dateTime) - sprintStory.Schatting);
                    }
                //}
            }

            return totaalTeVeelGewerkteTijd;
        }

        private double GewonnenUrenAanStoriesVoorDatum(DateTime dateTime, Sprint sprint)
        {
            double gewonnenUren = 0;

            foreach (SprintStory sprintStory in sprint.SprintStories)
            {
                if (sprintStory.Status == Status.Afgesloten && dateTime.Date >= sprintStory.Story.DatumAfgesloten.Value.Date && sprintStory.Story.TotaalBestedeTijd(sprint.StartDatum, dateTime) < sprintStory.Schatting)
                {
                    //Als het werk sneller is afgerond voor afgesloten stories betekend dit dat we extra ruimte hebben verkregen om te ontwikkellen.
                    gewonnenUren += TimeSpanHelper.TimeSpanInMinuten(sprintStory.Schatting - sprintStory.Story.TotaalBestedeTijd(sprint.StartDatum, dateTime));

                }
            }

            return gewonnenUren;
        }
    }
}