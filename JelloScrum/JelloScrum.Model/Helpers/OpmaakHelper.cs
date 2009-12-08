namespace JelloScrum.Model.Helpers
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
        public string Tijd(TimeSpan timeSpan)
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
        public string UrenStatus(TimeSpan schatting, TimeSpan daadwerkelijk)
        {
            StringBuilder sb = new StringBuilder();
            if (schatting > daadwerkelijk)
                sb.AppendFormat("<span class='statusOk'>{0}</span> / {1}", Tijd(schatting), Tijd(daadwerkelijk));
            else
                sb.AppendFormat("{0} / <span class='statusAlarm'>{1}</span>", Tijd(schatting), Tijd(daadwerkelijk));
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

        /// <summary>
        /// Geeft de volledige naam van de gebruiker als sting.
        /// </summary>
        /// <param name="gebruiker">De gebruiker.</param>
        /// <returns>De volledige naam.</returns>
        public string VolledigeNaam(Gebruiker gebruiker)
        {
            StringBuilder sb = new StringBuilder();

            //tja?

            return sb.ToString();
        }
    }
}