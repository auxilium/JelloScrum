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

    /// <summary>
    /// 
    /// </summary>
    public class TimeSpanHelper
    {
        /// <summary>
        /// Takes a Time like 90,00 and parses it to timespan
        /// </summary>
        /// <param name="timeSpan">90,00</param>
        /// <returns>3d 18h timespan</returns>
        public static TimeSpan Parse(string timeSpan)
        {
            try
            {
                string[] elementen = timeSpan.Split(new char[] { ',' });
                if (elementen.Length == 1)
                    return new TimeSpan(int.Parse(elementen[0]), 0, 0);
                
                if (elementen[1].Length == 1)
                   return new TimeSpan(int.Parse(elementen[0]), (60 * (int.Parse(elementen[1]))/10), 0);

                return new TimeSpan(int.Parse(elementen[0]), (60 * (int.Parse(elementen[1])) / 100), 0); 
            }
            catch(Exception)
            {
                return new TimeSpan(0,0,0);
            }
        }

        /// <summary>
        /// Geeft de timespan geformatteerd als double HH,MM.
        /// </summary>
        /// <param name="timeSpan">De timespan.</param>
        /// <returns>"HH,MM"</returns>
        public static double TimeSpanInMinuten(TimeSpan timeSpan)
        {
            return Math.Round(timeSpan.TotalMinutes/60, 2);
        }
    }
}