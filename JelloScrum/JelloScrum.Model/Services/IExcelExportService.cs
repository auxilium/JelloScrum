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
    /// Interface for the ExcelExportService
    /// </summary>
    public interface IExcelExportService
    {
        /// <summary>
        /// Create an .xls file with all stories from the given backlog and return the filename of this .xls
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        string ExportProjectBacklog(Project project);

        /// <summary>
        /// Create an .xls file with all stories from the given sprint and return the filename of this .xls
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        /// <returns></returns>
        string ExportSprintBacklog(Sprint sprint);

        /// <summary>
        /// Create an .xls file with all stories and tasks from the sprint cardwall and return the filename of this .xls
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        /// <returns></returns>
        string ExportSprintCardwall(Sprint sprint);

    }
}