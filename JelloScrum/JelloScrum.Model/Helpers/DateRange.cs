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

namespace JelloScrum.Model.Helpers
{
    using System;

    /// <summary>
    /// Simple daterange struct
    /// </summary>
    public struct DateRange
    {
        private DateTime start;
        private DateTime end;


        /// <summary>
        /// Initializes a new instance of the <see cref="DateRange"/> struct.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="end">The end date.</param>
        public DateRange(DateTime start, DateTime end)
        {
            //todo: check guard
            this.start = start;
            this.end = end;
        }
        
        /// <summary>
        /// Does the daterange overlap the specified date?
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public bool Overlap(DateTime date)
        {
            return start <= date && end >= date;
        }
    }
}