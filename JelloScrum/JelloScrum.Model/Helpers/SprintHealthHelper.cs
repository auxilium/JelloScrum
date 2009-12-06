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
        /// Total hours estimated in this sprint
        /// </summary>
        public decimal HoursTotalEstimatedInSprint
        {
            get { return hoursTotalEstimatedInSprint; }
            set { hoursTotalEstimatedInSprint = value; }
        }

        /// <summary>
        /// Total hours spent in this sprint
        /// </summary>
        public decimal HoursTotalRegisteredInSprint
        {
            get { return hoursTotalRegisteredInSprint; }
            set { hoursTotalRegisteredInSprint = value; }
        }

        /// <summary>
        /// Total time estimated for completed stories in this sprint
        /// </summary>
        public decimal HoursTotalEstimatedForCompletedStoriesInSprint
        {
            get { return hoursTotalEstimatedForCompletedStoriesInSprint; }
            set { hoursTotalEstimatedForCompletedStoriesInSprint = value; }
        }

        /// <summary>
        /// Total time spent on closed stories in this sprint
        /// </summary>
        public decimal HoursTotalRegisteredForCompletedStoriesInSprint
        {
            get { return hoursTotalRegisteredForCompletedStoriesInSprint; }
            set { hoursTotalRegisteredForCompletedStoriesInSprint = value; }
        }

        /// <summary>
        /// Total amount of storypoints in this sprint
        /// </summary>
        public int StoryPointsTotalEstimatedInSprint
        {
            get { return storyPointsTotalEstimatedInSprint; }
            set { storyPointsTotalEstimatedInSprint = value; }
        }

        /// <summary>
        /// Total amount of story points estimated for closed stories in this sprint
        /// </summary>
        public int StoryPointsTotalEstimatedForCompletedStoriesInSprint
        {
            get { return storyPointsTotalEstimatedForCompletedStoriesInSprint; }
            set { storyPointsTotalEstimatedForCompletedStoriesInSprint = value; }
        }

        /// <summary>
        /// Number of days this sprint takes (for all developers). 
        /// If 2 developers work 4 weeks this should be 40
        /// </summary>
        public decimal NumberOfDaysInSprint
        {
            get { return numberOfDaysInSprint; }
            set { numberOfDaysInSprint = value; }
        }

        /// <summary>
        /// Gets the amount of storypoints that are closed in this sprint each day
        /// </summary>
        public decimal StoryPointVelocity
        {
            get
            {
                if (numberOfDaysInSprint == 0)
                    return 0;

                return storyPointsTotalEstimatedForCompletedStoriesInSprint/numberOfDaysInSprint;
            }
        }

        /// <summary>
        /// Hours velocity
        /// </summary>
        public decimal HoursVelocity
        {
            get
            {
                if (numberOfDaysInSprint == 0)
                    return 0;
                
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
        /// Completed tasks
        /// </summary>
        public int CompletedTasks
        {
            get { return completedTasks; }
            set { completedTasks = value; }
        }

        /// <summary>
        /// Incomplete tasks
        /// </summary>
        public int IncompleteTasks
        {
            get { return incompleteTasks; }
            set { incompleteTasks = value; }
        }

        /// <summary>
        /// Percentage of closed tasks
        /// </summary>
        public decimal CompletedTasksPercentage()
        {
            if (TotalTasks > 0)
                return 100 * ((decimal)completedTasks / TotalTasks);
            
            return 0;
        }

        /// <summary>
        /// Rounded percentage closed tasks
        /// </summary>
        /// <param name="numberOfDecimals"></param>
        /// <returns></returns>
        public decimal CompletedTasksPercentage(int numberOfDecimals)
        {
            return Math.Round(CompletedTasksPercentage(), numberOfDecimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Percentage incomplete tasks
        /// </summary>
        public decimal IncompleteTasksPercentage()
        {
            return 100 - CompletedTasksPercentage();
        }

        /// <summary>
        /// Rounded percentage incomplete tasks
        /// </summary>
        public decimal IncompleteTasksPercentage(int numberOfDecimals)
        {
            return Math.Round(IncompleteTasksPercentage(), numberOfDecimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Percentage taken tasks
        /// </summary>
        public decimal AssignedTasksPercentage()
        {
            if (TotalTasks > 0)
                return 100 * ((decimal)assignedTasks / TotalTasks);
            
            return 0;
        }

        /// <summary>
        /// Rounded percentage taken tasks
        /// </summary>
        /// <param name="numberOfDecimals"></param>
        /// <returns></returns>
        public decimal AssignedTasksPercentage(int numberOfDecimals)
        {
            return Math.Round(AssignedTasksPercentage(), numberOfDecimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Percentage open tasks
        /// </summary>
        public decimal UnassignedTasksPercentage()
        {
            return 100 - AssignedTasksPercentage();
        }

        /// <summary>
        /// Rounded percentage open tasks
        /// </summary>
        public decimal UnassignedTasksPercentage(int numberOfDecimals)
        {
            return Math.Round(UnassignedTasksPercentage(), numberOfDecimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Total tasks
        /// </summary>
        public int TotalTasks
        {
            get { return totalTasks; }
            set { totalTasks = value; }
        }

        /// <summary>
        /// Taken tasks
        /// </summary>
        public int AssignedTasks
        {
            get { return assignedTasks; }
            set { assignedTasks = value; }
        }

        /// <summary>
        /// Open tasks
        /// </summary>
        public int UnassignedTasks
        {
            get { return unassignedTasks; }
            set { unassignedTasks = value; }
        }
    }

    /// <summary>
    /// Used to calculate all kinds of statistics.
    /// </summary>
    public static class SprintHealthHelper
    {
        /// <summary>
        /// Calculates information about the velocity of the given sprint
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
                    if (sprintStory.Story.StoryPoints != StoryPoint.Unknown)
                        velocity.StoryPointsTotalEstimatedInSprint += sprintStory.Story.StoryPointsValue;

                    if (sprintStory.Status == State.Closed)
                    {
                        velocity.HoursTotalEstimatedForCompletedStoriesInSprint +=
                            Convert.ToDecimal(sprintStory.Schatting.TotalHours);
                        if (sprintStory.Story.StoryPoints != StoryPoint.Unknown)
                            velocity.StoryPointsTotalEstimatedForCompletedStoriesInSprint +=
                                sprintStory.Story.StoryPointsValue;
                    }

                    foreach (Task task in sprintStory.Story.Tasks)
                    {
                        foreach (TijdRegistratie tijdRegistratie in task.TijdRegistraties)
                        {
                            if (tijdRegistratie.Sprint == sprintStory.Sprint)
                            {
                                velocity.HoursTotalRegisteredInSprint += Convert.ToDecimal(tijdRegistratie.Tijd.TotalHours);

                                if (task.Status == State.Closed)
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
        /// Calculates the taskprogress of the given sprint
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        public static TaskProgress GetTaskProgress(Sprint sprint)
        {
            TaskProgress taskProgress = new TaskProgress();
        
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

                if (task.Status == State.Closed)
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