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

namespace JelloScrum.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Configuration;
    using System.Text;
    using System.Threading;
    using FileHelpers.DataLink;

    using Model.Entities;
    using Model.Enumerations;
    using Model.Services;
    using Model.Helpers;

    public class ExcelExportService : IExcelExportService
    {
        private CultureInfo currentCulture;

        /// <summary>
        /// Maakt excel file met alle stories van de projectbacklog en geeft filename terug
        /// </summary>
        /// <param name="project">Project</param>
        public string ExportProjectBacklog(Project project)
        {
            SetCultureInfo();

            string fileName = String.Format("ProductBacklog_{0}_{1}.xls", DateTime.Now.ToString("yyyy-MM-dd") ,project.Name);
           
            ExcelStorage provider = new ExcelStorage(typeof(ProductBackLog));
            provider.StartRow = 2;
            provider.StartColumn = 1;
            provider.FileName = ConfigurationManager.AppSettings["exportLocation"] + fileName;
            provider.TemplateFile = ConfigurationManager.AppSettings["productBackLogTemplate"];
            
            List<ProductBackLog> res = new List<ProductBackLog>();

            foreach (Story story in project.Stories)
            {
                ProductBackLog row = new ProductBackLog();

                row.Id = story.Id.ToString();
                row.Title = story.Title;
                row.Priority = Enum.GetName(typeof(Priority), story.ProductBacklogPriority);
                row.Type = Enum.GetName(typeof(StoryType), story.StoryType);
                row.Description = story.Description;
                row.Points = Enum.GetName(typeof(StoryPoint), story.StoryPoints);
                row.Estimation = TimeSpanInMinuten(story.Estimation).ToString();
                row.Tasks = story.Tasks.Count.ToString();

                res.Add(row);
            }

            provider.InsertRecords(res.ToArray());

            RestoreCultureInfo();

            return fileName;
        }

        /// <summary>
        /// Maakt excel file met alle stories van de sprintbacklog en geeft filename terug
        /// </summary>
        /// <param name="sprint">Sprint</param>
        public string ExportSprintBacklog(Sprint sprint)
        {
            SetCultureInfo();

            string fileName = String.Format("SprintBacklog_{0}_{1}.xls", DateTime.Now.ToString("yyyy-MM-dd"), sprint.Goal);

            ExcelStorage provider = new ExcelStorage(typeof(SprintBackLog));
            provider.StartRow = 2;
            provider.StartColumn = 1;
            provider.FileName = ConfigurationManager.AppSettings["exportLocation"] + fileName;
            provider.TemplateFile = ConfigurationManager.AppSettings["sprintBackLogTemplate"];

            List<SprintBackLog> res = new List<SprintBackLog>();

            foreach (SprintStory sprintStory in sprint.SprintStories)
            {
                SprintBackLog row = new SprintBackLog();

                row.Id = sprintStory.Id.ToString();
                row.Title = sprintStory.Story.Title;
                row.SprintPriority = Enum.GetName(typeof(Priority), sprintStory.SprintBacklogPriority);
                row.ProjectPriority = Enum.GetName(typeof(Priority), sprintStory.Story.ProductBacklogPriority);
                row.Type = Enum.GetName(typeof(StoryType), sprintStory.Story.StoryType);
                row.Description = sprintStory.Story.Description;
                row.Points = Enum.GetName(typeof(StoryPoint), sprintStory.Story.StoryPoints);
                row.Estimation = TimeSpanInMinuten(sprintStory.Story.Estimation).ToString();
                row.Tasks = sprintStory.Story.Tasks.Count.ToString();

                res.Add(row);
            }

            provider.InsertRecords(res.ToArray());

            RestoreCultureInfo();

            return fileName;
        }

        /// <summary>
        /// Maakt excel file met alle stories en taken van de sprint cardwall en geeft filename terug
        /// </summary>
        /// <param name="sprint">Sprint</param>
        public string ExportSprintCardwall(Sprint sprint)
        {
            SetCultureInfo();

            string fileName = String.Format("SprintCardwall_{0}_{1}.xls", DateTime.Now.ToString("yyyy-MM-dd"), sprint.Goal);

            ExcelStorage provider = new ExcelStorage(typeof(Cardwall));
            provider.StartRow = 3;
            provider.StartColumn = 1;
            provider.FileName = ConfigurationManager.AppSettings["exportLocation"] + fileName;
            provider.TemplateFile = ConfigurationManager.AppSettings["sprintCardwallTemplate"];

            List<Cardwall> res = new List<Cardwall>();

            foreach (SprintStory sprintStory in sprint.SprintStories)
            {
                Cardwall storyRow = new Cardwall();

                storyRow.StoryId = sprintStory.Id.ToString();
                storyRow.StoryTitle = sprintStory.Story.Title;
                storyRow.StoryPriority = Enum.GetName(typeof(Priority), sprintStory.Story.ProductBacklogPriority);
                storyRow.StoryEstimatedHours = TimeSpanInMinuten(sprintStory.Story.Estimation).ToString();
                res.Add(storyRow);

                foreach (Task task in sprintStory.Story.Tasks)
                {
                    Cardwall taskRow = new Cardwall();

                    switch(task.State)
                    {
                        case State.Open:
                            taskRow.TaskOpenId = task.Id.ToString();
                            taskRow.TaskOpenTitle = task.Title;
                            taskRow.TaskOpenAssignee = task.AssignedUserName;
                            taskRow.TaskOpenTimeSpent = TimeSpanInMinuten(task.TotalTimeSpent()).ToString();
                            break;
                        case State.Taken:
                            taskRow.TaskInProgressId = task.Id.ToString();
                            taskRow.TaskInProgressTitle = task.Title;
                            taskRow.TaskInProgressAssignee = task.AssignedUserName;
                            taskRow.TaskInProgressTimeSpent = TimeSpanInMinuten(task.TotalTimeSpent()).ToString();
                            break;
                        case State.Closed:
                            taskRow.TaskDoneId = task.Id.ToString();
                            taskRow.TaskDoneTitle = task.Title;
                            taskRow.TaskDoneAssignee = task.AssignedUserName;
                            taskRow.TaskDoneTimeSpent = TimeSpanInMinuten(task.TotalTimeSpent()).ToString();
                            break;
                    }

                    res.Add(taskRow);
                }
            }

            provider.InsertRecords(res.ToArray());

            RestoreCultureInfo();

            return fileName;
        }

        /// <summary>
        /// for the excel export to work, the currentculture MUST be en-US
        /// </summary>
        private void SetCultureInfo()
        {
           currentCulture = Thread.CurrentThread.CurrentCulture;
           Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }


        /// <summary>
        /// Restore CulturInfo
        /// </summary>
        private void RestoreCultureInfo()
        {
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        /// <summary>
        /// Geeft de timespan geformatteerd als double HH,MM.
        /// </summary>
        /// <param name="timeSpan">De timespan.</param>
        /// <returns>"HH,MM"</returns>
        public static double TimeSpanInMinuten(TimeSpan timeSpan)
        {
            return Math.Round(timeSpan.TotalMinutes / 60, 2);
        }

    }
}