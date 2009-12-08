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
    using System.Globalization;

    using Boo.Lang;
    using Model.Helpers;


    ///<summary>
    /// Datum berekeningen
    ///</summary>
    public class DateHelper
    {
        /// <summary>
        /// Reken de datum van de eerste maandag in het verleden aan de hand van de datum die opgegeven is.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetMonday(DateTime date)
        {
            DateTime dateTime = date.AddDays(-1*Convert.ToInt16(date.DayOfWeek));
            return dateTime.AddDays(1);
        }

        /// <summary>
        /// Returns a IList of 5 DateTimes that are the work day's of the given monday
        /// </summary>
        /// <param name="monday"></param>
        /// <returns></returns>
        public static DateRange GetWorkWeekFromMonday(DateTime monday)
        {
            return GetWorkWeek(monday);
        }

        /// <summary>
        /// Returns a IList of 5 DateTimes that are the work day's of the week you are in right now.
        /// </summary>
        /// <returns></returns>
        public static DateRange GetCurrentWorkWeek()
        {
            return GetWorkWeek(GetMonday(DateTime.Now));
        }

        /// <summary>
        /// Returns a IList of 5 DateTimes that are the work day's of the given date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateRange GetWorkWeekFromGivenDate(DateTime date)
        {
            return GetWorkWeek(GetMonday(date));
        }

        /// <summary>
        /// Maakt een lijst met datums voor een werkweerk aan de hand van een meegegeven maandag.
        /// </summary>
        /// <param name="monday"></param>
        /// <returns></returns>
        private static DateRange GetWorkWeek(DateTime monday)
        {
            return new DateRange(monday, monday.AddDays(4));
        }
    
        /// <summary>
        /// Geeft voor een bepaalde datum het weeknummer terug
        /// </summary>
        /// <param name="date">datum</param>
        /// <returns>weeknummer</returns>
        public static int GetWeekNumber(DateTime date)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
    }
}