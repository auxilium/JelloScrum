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
    using Castle.ActiveRecord;
    using JelloScrum.Model.Entities;
    using Model;
    using NHibernate;
    using NHibernate.Criterion;
    using QueryObjects;

    /// <summary>
    /// BurnDown functionalteiten
    /// </summary>
    public static class BurnDownHelper
    {
        /// <summary>
        /// Berekent de totaal geschatten tijd
        /// </summary>
        /// <returns></returns>
        public static TimeSpan TotaalGeschatteTijd(Sprint sprint)
        {
            TimeSpan totaal = new TimeSpan();
            foreach (SprintStory sprintStory in sprint.SprintStories)
            {
                totaal += sprintStory.Estimation;
            }
            return totaal;
        }

        /// <summary>
        /// voorheen AantaldagenTotNu
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        public static int AantalWerkdagenTotNu(Sprint sprint)
        {
            return CalculateWerkdagen(sprint, DateTime.Today);
        }

        /// <summary>
        /// Voorheen TotaalSprintTijd
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        public static int AantalWerkdagenSprint(Sprint sprint)
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
        public static double laatsteWaarde(DateTime date, IDictionary<DateTime, double> toeter)
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
        public static IDictionary<DateTime, double> Werk(Sprint sprint)
        {
            TotalTimeSpentOnClosedTasksInSprintQuery query = new TotalTimeSpentOnClosedTasksInSprintQuery();
            query.Sprint = sprint;

            ICriteria crit = query.GetQuery(ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(typeof(ModelBase)));
            crit.AddOrder(Order.Asc("Date"));
            IList result = crit.List();
            IDictionary<DateTime, double> dataPoints = new SortedDictionary<DateTime, double>();

            double totaal = TotaalGeschatteTijd(sprint).TotalHours;
            foreach (TimeRegistration registratie in result)
            {
                if (!dataPoints.ContainsKey(registratie.Date.Date))
                {
                    dataPoints.Add(registratie.Date.Date, totaal);
                }
                totaal -= registratie.Time.TotalHours;
                dataPoints[registratie.Date.Date] = totaal;
            }

            DateTime temp = sprint.StartDate;
            while (temp <= sprint.EndDate)
            {
                if (temp.Date <= DateTime.Today)
                {
                    if (temp <= sprint.StartDate && !dataPoints.ContainsKey(temp.Date))
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

        private static int CalculateWerkdagen(Sprint sprint, DateTime? date)
        {
            if (date == null)
            {
                date = sprint.EndDate.Date;
            }

            int dayCount = 0;
            DateTime temp = sprint.StartDate;
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
    }
}