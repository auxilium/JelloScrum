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
    using System.Collections.Generic;
    using Entities;
    using Enumerations;

    /// <summary>
    /// Struct containing information about the velocity statistic
    /// </summary>
    public struct Velocity
    {
        private decimal hoursTotalEstimatedInSprint;
        private decimal hoursTotalRegisteredInSprint;
        private decimal hoursTotalEstimatedForCompletedStoriesInSprint;
        private decimal hoursTotalRegisteredForCompletedStoriesInSprint;

        private int storyPointsTotalEstimatedInSprint;
        private int storyPointsTotalEstimatedForCompletedStoriesInSprint;

        private decimal numberOfDaysInSprint;

        /// <summary>
        /// Totaal aantal uren dat ingepland is voor de sprint
        /// </summary>
        public decimal HoursTotalEstimatedInSprint
        {
            get { return hoursTotalEstimatedInSprint; }
            set { hoursTotalEstimatedInSprint = value; }
        }

        /// <summary>
        /// Totaal aantal uren geregistreerd op stories uit de sprint
        /// </summary>
        public decimal HoursTotalRegisteredInSprint
        {
            get { return hoursTotalRegisteredInSprint; }
            set { hoursTotalRegisteredInSprint = value; }
        }

        /// <summary>
        /// Totaal aantal geschatte uren voor afgeronde stories uit de sprint.
        /// </summary>
        public decimal HoursTotalEstimatedForCompletedStoriesInSprint
        {
            get { return hoursTotalEstimatedForCompletedStoriesInSprint; }
            set { hoursTotalEstimatedForCompletedStoriesInSprint = value; }
        }

        /// <summary>
        /// Totaal aantal geregistreerde uren voor afgeronde stories uit de sprint.
        /// </summary>
        public decimal HoursTotalRegisteredForCompletedStoriesInSprint
        {
            get { return hoursTotalRegisteredForCompletedStoriesInSprint; }
            set { hoursTotalRegisteredForCompletedStoriesInSprint = value; }
        }

        /// <summary>
        /// Totaal aantal storypunten in de sprint
        /// </summary>
        public int StoryPointsTotalEstimatedInSprint
        {
            get { return storyPointsTotalEstimatedInSprint; }
            set { storyPointsTotalEstimatedInSprint = value; }
        }

        /// <summary>
        /// Totaal geschatte storypunten voor afgeronde stories
        /// </summary>
        public int StoryPointsTotalEstimatedForCompletedStoriesInSprint
        {
            get { return storyPointsTotalEstimatedForCompletedStoriesInSprint; }
            set { storyPointsTotalEstimatedForCompletedStoriesInSprint = value; }
        }

        /// <summary>
        /// Aantal dagen voor deze sprint (voor alle developers). Dus voor 2 developers die 4 weken werken vul je hier 40 in.
        /// </summary>
        public decimal NumberOfDaysInSprint
        {
            get { return numberOfDaysInSprint; }
            set { numberOfDaysInSprint = value; }
        }

        /// <summary>
        /// Geeft het aantal storypoints dat afgerond is in deze sprint per dag
        /// </summary>
        public decimal StoryPointVelocity
        {
            get
            {
                if (numberOfDaysInSprint == 0)
                    return 0;
                else 
                    return storyPointsTotalEstimatedForCompletedStoriesInSprint/numberOfDaysInSprint;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal HoursVelocity
        {
            get
            {
                if (numberOfDaysInSprint == 0)
                    return 0;
                else 
                    return hoursTotalEstimatedForCompletedStoriesInSprint / numberOfDaysInSprint;
            }
        }
    }

    /// <summary>
    /// struct containing information about taskprogress
    /// </summary>
    public struct TaskProgress
    {
        private int completedTasks;
        private int incompleteTasks;
        private int totalTasks;
        private int assignedTasks;
        private int unassignedTasks;

        /// <summary>
        /// Aantal taken die afgerond zijn
        /// </summary>
        public int CompletedTasks
        {
            get { return completedTasks; }
            set { completedTasks = value; }
        }

        /// <summary>
        /// Aantal taken nog niet afgerond
        /// </summary>
        public int IncompleteTasks
        {
            get { return incompleteTasks; }
            set { incompleteTasks = value; }
        }

        /// <summary>
        /// Percentage afgeronde taken
        /// </summary>
        public decimal CompletedTasksPercentage()
        {
            if (TotalTasks > 0)
                return 100 * ((decimal)completedTasks / TotalTasks);
            else
                return 0;
        }

        /// <summary>
        /// Afgerond percentage afgeronde taken
        /// </summary>
        /// <param name="numberOfDecimals"></param>
        /// <returns></returns>
        public decimal CompletedTasksPercentage(int numberOfDecimals)
        {
            return Math.Round(CompletedTasksPercentage(), numberOfDecimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Percentage niet-afgeronde taken
        /// </summary>
        public decimal IncompleteTasksPercentage()
        {
            return 100 - CompletedTasksPercentage();
        }

        /// <summary>
        /// Percentage niet-afgeronde taken
        /// </summary>
        public decimal IncompleteTasksPercentage(int numberOfDecimals)
        {
            return Math.Round(IncompleteTasksPercentage(), numberOfDecimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Percentage afgeronde taken
        /// </summary>
        public decimal AssignedTasksPercentage()
        {
            if (TotalTasks > 0)
                return 100 * ((decimal)assignedTasks / TotalTasks);
            else
                return 0;
        }

        /// <summary>
        /// Afgerond percentage afgeronde taken
        /// </summary>
        /// <param name="numberOfDecimals"></param>
        /// <returns></returns>
        public decimal AssignedTasksPercentage(int numberOfDecimals)
        {
            return Math.Round(AssignedTasksPercentage(), numberOfDecimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Percentage niet-afgeronde taken
        /// </summary>
        public decimal UnassignedTasksPercentage()
        {
            return 100 - AssignedTasksPercentage();
        }

        /// <summary>
        /// Percentage niet-afgeronde taken
        /// </summary>
        public decimal UnassignedTasksPercentage(int numberOfDecimals)
        {
            return Math.Round(UnassignedTasksPercentage(), numberOfDecimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 
        /// </summary>
        public int TotalTasks
        {
            get { return totalTasks; }
            set { totalTasks = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AssignedTasks
        {
            get { return assignedTasks; }
            set { assignedTasks = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int UnassignedTasks
        {
            get { return unassignedTasks; }
            set { unassignedTasks = value; }
        }
    }

    /// <summary>
    /// Deze class kan allerhande al dan niet nutteloze statistieken berekenen
    /// </summary>
    public static class SprintHealthHelper
    {
        /// <summary>
        /// Berekent informatie over de velocity van de gegeven sprint
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        public static Velocity GetVelocity(Sprint sprint)
        {
            Velocity velocity = new Velocity();

            if(sprint != null)
            {
                velocity.NumberOfDaysInSprint = sprint.WerkDagen;

                foreach (SprintStory sprintStory in sprint.SprintStories)
                {
                    velocity.HoursTotalEstimatedInSprint += Convert.ToDecimal(sprintStory.Schatting.TotalHours);
                    if (sprintStory.Story.StoryPoints != StoryPoint.Onbekend)
                        velocity.StoryPointsTotalEstimatedInSprint += sprintStory.Story.WaardeStoryPoints;

                    if (sprintStory.Status == Status.Afgesloten)
                    {
                        velocity.HoursTotalEstimatedForCompletedStoriesInSprint +=
                            Convert.ToDecimal(sprintStory.Schatting.TotalHours);
                        if (sprintStory.Story.StoryPoints != StoryPoint.Onbekend)
                            velocity.StoryPointsTotalEstimatedForCompletedStoriesInSprint +=
                                sprintStory.Story.WaardeStoryPoints;
                    }

                    foreach (Task task in sprintStory.Story.Tasks)
                    {
                        foreach (TijdRegistratie tijdRegistratie in task.TijdRegistraties)
                        {
                            if (tijdRegistratie.Sprint == sprintStory.Sprint)
                            {
                                velocity.HoursTotalRegisteredInSprint += Convert.ToDecimal(tijdRegistratie.Tijd.TotalHours);

                                if (task.Status == Status.Afgesloten)
                                {
                                    velocity.HoursTotalRegisteredForCompletedStoriesInSprint +=
                                        Convert.ToDecimal(tijdRegistratie.Tijd.TotalHours);
                                }
                            }
                        }
                    }
                }
            }

            return velocity;
        }

        /// <summary>
        /// Bereken de taak voortgang van een sprint.
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        public static TaskProgress GetTaskProgress(Sprint sprint)
        {
            TaskProgress taskProgress = new TaskProgress();

            //Dit kan niet in het model, dus loopen, of een List<Task> als argument geven.
//            TakenVanSprintQuery takenVanSprintQuery = new TakenVanSprintQuery();
//            takenVanSprintQuery.Sprint = sprint;
//
//            IList tasks = takenVanSprintQuery.GetQuery(UnitOfWork.CurrentSession).List();

            List<Task> tasks = new List<Task>();
            
            if(sprint != null)
            {
                foreach (SprintStory sprintStory in sprint.SprintStories)
                {
                    tasks.AddRange(sprintStory.Story.Tasks);
                }
            }

            foreach (Task task in tasks)
            {
                taskProgress.TotalTasks++;

                if (task.Status == Status.Afgesloten)
                    taskProgress.CompletedTasks++;
                else
                    taskProgress.IncompleteTasks++;

                if (task.Behandelaar == null)
                    taskProgress.UnassignedTasks++;
                else
                    taskProgress.AssignedTasks++;
            }

            return taskProgress;
        }
    }
}