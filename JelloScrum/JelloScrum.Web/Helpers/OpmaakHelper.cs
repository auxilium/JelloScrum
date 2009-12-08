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
    using System.Text;
    using JelloScrum.Model.Entities;

    /// <summary>
    /// Helper voor het formatteren van bepaalde zaken
    /// </summary>
    public class OpmaakHelper
    {
        /// <summary>
        /// Geeft de timespan geformatteerd als string HH:MM.
        /// </summary>
        /// <param name="timeSpan">De timespan.</param>
        /// <returns>"HH:MM"</returns>
        public static string Tijd(TimeSpan timeSpan)
        {
            StringBuilder sb = new StringBuilder();
                       
            //hours
            int totalHours = 0;
            if (timeSpan.Days != 0)
            {
                totalHours += timeSpan.Days * 24;
            }
            totalHours += timeSpan.Hours;
            sb.Append(totalHours);

            //:
            sb.Append(":");

            //minutes
            if (timeSpan.Minutes < 10 && timeSpan.Minutes > -10)
                sb.Append("0");

            if (timeSpan.Minutes < 0)
            {
                sb.Append(timeSpan.Minutes * -1);
            }else
            {
                sb.Append(timeSpan.Minutes);    
            }
            

            return sb.ToString();
        }

        /// <summary>
        /// Geeft de timespan geformatterd als string HH:MM - voorloop 0'en waar nodig
        /// </summary>
        /// <param name="timeSpan">de timespan</param>
        /// <returns>HH:MM</returns>
        public string Tijd24H(TimeSpan timeSpan)
        {
            StringBuilder sb = new StringBuilder();

            //hours
            int totalHours = 0;
            if (timeSpan.Days > 0)
            {
                totalHours += timeSpan.Days * 24;
            }
            totalHours += timeSpan.Hours;
            if(totalHours <= 9)
            {
                sb.Append("0" + totalHours);
            }
            else
            {
                sb.Append(totalHours);    
            }
            

            //:
            sb.Append(":");

            //minutes
            if (timeSpan.Minutes < 10)
                sb.Append("0");
            sb.Append(timeSpan.Minutes);

            return sb.ToString();
        }

        /// <summary>
        /// Geeft de timespan geformatterd als string HH:MM - voorloop 0'en waar nodig
        /// </summary>
        /// <param name="tijd"></param>
        /// <returns></returns>
        public string TijdMetVoorloopNullen(TimeSpan tijd)
        {
            string tmpTijd = Tijd24H(tijd);
            if (tmpTijd.Length <= 5)
                return "0" + tmpTijd;
            return tmpTijd;
        }

        /// <summary>
        /// Geeft de schatting versus de daadwerkelijke uren als HH:MM / HH:MM
        ///  - Als de daadwerkelijke uren onder de schatting zitten wordt deze groen.
        ///  - Als de daadwerkelijke uren boven de schatting zitten worden deze rood.
        /// </summary>
        /// <param name="schatting">De schatting.</param>
        /// <param name="daadwerkelijk">De daadwerkelijke uren.</param>
        /// <returns>De urenstatus.</returns>
        public static string UrenStatus(TimeSpan schatting, TimeSpan daadwerkelijk)
        {
            StringBuilder sb = new StringBuilder();
            if (schatting > daadwerkelijk)
                sb.AppendFormat("[<span class='statusOk'>{0}</span> / {1}]", Tijd(schatting), Tijd(daadwerkelijk));
            else
                sb.AppendFormat("[{0} / <span class='statusAlarm'>{1}</span>]", Tijd(schatting), Tijd(daadwerkelijk));
            return sb.ToString();
        }

        /// <summary>
        /// Geeft de schatting versus de daadwerkelijke uren als HH:MM / HH:MM
        ///  - Als de daadwerkelijke uren onder de schatting zitten wordt deze groen.
        ///  - Als de daadwerkelijke uren boven de schatting zitten worden deze rood.
        /// </summary>
        /// <param name="schatting">De schatting.</param>
        /// <param name="daadwerkelijk">De daadwerkelijke uren.</param>
        /// <returns>De urenstatus.</returns>
        public string UrenStatus(Story story)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<a href='/tijdregistratie/geeftijdregistratieoverzicht.rails?storyId={0}&width=600' class='jTip' name='Overzicht geboekte uren' id='{1}' style='color: #000000;'>", story.Id, Guid.NewGuid());
            if (story.Estimation > story.TotalTimeSpent())
                sb.AppendFormat("[<span class='statusOk'>{0}</span> / {1}]", Tijd(story.Estimation), Tijd(story.TotalTimeSpent()));
            else
                sb.AppendFormat("[{0} / <span class='statusAlarm'>{1}</span>]", Tijd(story.Estimation), Tijd(story.TotalTimeSpent()));
            sb.Append("</a>");
            return sb.ToString();
        }

        /// <summary>
        /// Kapt de text af op de gespecificeerde lengte en plaatst er een ellipsis (...) achter.
        /// </summary>
        /// <param name="text">De text.</param>
        /// <param name="length">De length.</param>
        /// <returns>De afgekapte text</returns>
        public string Ellipsis(string text, int length)
        {
            if (text == null || length == 0)
                return string.Empty;

            if (text.Length > length)
                text = text.Substring(0, length) + "&hellip;";
            return text;
        }
    }
}