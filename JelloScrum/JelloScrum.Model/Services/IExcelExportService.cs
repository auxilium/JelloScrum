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

namespace JelloScrum.Model.Services
{
    using Entities;

    /// <summary>
    /// Interface voor de excel export Service
    /// </summary>
    public interface IExcelExportService
    {
        /// <summary>
        /// Maakt excel file met alle stories van de projectbacklog en geeft filename terug
        /// </summary>
        /// <param name="project"></param>
        string ExportProjectBacklog(Project project);

        /// <summary>
        /// Maakt excel file met alle stories van de sprintbacklog en geeft filename terug
        /// </summary>
        /// <param name="project"></param>
        string ExportSprintBacklog(Sprint sprint);

        /// <summary>
        /// Maakt excel file met alle stories en taken van de sprint cardwall en geeft filename terug
        /// </summary>
        /// <param name="sprint">Sprint</param>
        string ExportSprintCardwall(Sprint sprint);

    }
}