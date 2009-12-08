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

namespace JelloScrum.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Castle.ActiveRecord;
    using Enumerations;

    /// <summary>
    /// Represents a user in a sprint
    /// </summary>
    [ActiveRecord]
    public class SprintUser : ModelBase
    {
        #region fields
        
        private User user;
        private Sprint sprint;
        private SprintRole sprintRole = 0;

        private IList<Task> tasks = new List<Task>();

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SprintUser"/> class.
        /// </summary>
        public SprintUser()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SprintUser"/> class for the given user, sprint and role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="sprint">The sprint.</param>
        /// <param name="role">The role.</param>
        public SprintUser(User user, Sprint sprint, SprintRole role)
        {
            if (user == null)
                throw new ArgumentNullException("user", "The user can not be null.");
            if (sprint == null)
                throw new ArgumentNullException("sprint", "The sprint can not be null.");
            
            user.AddSprintUser(this);
            sprint.AddSprintUser(this);
            this.sprintRole = role;
        }

        #endregion

        #region properties

        /// <summary>
        /// The user
        /// </summary>
        [BelongsTo(NotNull = true, Column = "JelloScrumUser")]
        public virtual User User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// The sprint
        /// </summary>
        [BelongsTo(NotNull = true)]
        public virtual Sprint Sprint
        {
            get { return sprint; }
            set { sprint = value; }
        }

        /// <summary>
        /// Role of the user in this sprint
        /// </summary>
        [Property]
        public virtual SprintRole SprintRole
        {
            get { return sprintRole; }
            set { sprintRole = value; }
        }

        /// <summary>
        /// Gets a readonly collection of the tasks this user has.
        /// To add a task, use <see cref="TakeTask"/>.
        /// </summary>
        /// <value>The tasks.</value>
        [HasMany(Cascade = ManyRelationCascadeEnum.SaveUpdate, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<Task> Tasks
        {
            get { return new ReadOnlyCollection<Task>(tasks); }
        }

        #endregion

        #region methods

        /// <summary>
        /// Gets all tasks this user has taken with the given sprintbacklog priority
        /// todo: expensive!
        /// </summary>
        /// <param name="priority">The sprintbacklog priority.</param>
        /// <returns></returns>
        public virtual IList<Task> GetTakenTasksWithSprintBacklogPriority(Priority priority)
        {
            IList<Task> tmpTasks = new List<Task>();
            foreach (Task task in tasks)
            {
                if (task.State == State.Taken)
                {
                    SprintStory sprintStory = sprint.GetSprintStoryFor(task.Story);
                 
                    if (sprintStory == null)
                        continue;

                    if (sprintStory.SprintBacklogPriority == priority)
                        tmpTasks.Add(task);
                }
            }
            return tmpTasks;
        }

        /// <summary>
        /// Gets a readonly collection of the tasks this user has taken
        /// </summary>
        /// <returns></returns>
        public virtual ReadOnlyCollection<Task> GetTakenTasks()
        {
            IList<Task> takenTasks = new List<Task>();
            foreach (Task task in tasks)
            {
                if (task.State == State.Taken)
                    takenTasks.Add(task);
            }
            return new ReadOnlyCollection<Task>(takenTasks);
        }

        /// <summary>
        /// Gets a readonly collection of the tasks this user has closed.
        /// </summary>
        /// <returns></returns>
        public virtual ReadOnlyCollection<Task> GetClosedTasks()
        {
            IList<Task> closedTasks = new List<Task>();
            foreach (Task task in tasks)
            {
                if (task.State == State.Closed)
                    closedTasks.Add(task);
            }
            return new ReadOnlyCollection<Task>(closedTasks);
        }

        /// <summary>
        /// Take the given task
        /// </summary>
        /// <param name="task">The task.</param>
        public virtual void TakeTask(Task task)
        {
            if (!tasks.Contains(task))
                tasks.Add(task);
            task.AssignedUser = this;
            task.State = State.Taken;
        }

        /// <summary>
        /// The assigned user gives the task up and is no longer the assigned user. 
        /// The tasks state is set to open again.
        /// </summary>
        /// <param name="task">The taks.</param>
        public virtual void UnAssignTask(Task task)
        {
            if (tasks.Contains(task))
                tasks.Remove(task);
            task.AssignedUser = null;
            task.State = State.Open;
        }

        /// <summary>
        /// Adds the given sprintrole to the current set of sprintroles.
        /// </summary>
        /// <param name="role">The role.</param>
        public virtual void AddRole(SprintRole role)
        {
            this.sprintRole |= role;
        }

        /// <summary>
        /// Removes the given role from the current set of roles.
        /// </summary>
        /// <param name="role">The role.</param>
        public virtual void RemoveRole(SprintRole role)
        {
            sprintRole &= ~role;
        }

        /// <summary>
        /// Determines if this sprintuser has EXACTLY the same sprintrole as the given role
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public virtual bool HasExactlyThisSprintRole(SprintRole role)
        {
            return (sprintRole == role);
        }

        /// <summary>
        /// Determines if this sprintuser has the given sprintrole
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public virtual bool HasSprintRole(SprintRole role)
        {
            return ((sprintRole & role) != 0);
        }
        
        /// <summary>
        /// Take over the given task from its current assigned user
        /// </summary>
        /// <param name="task">The task.</param>
        public virtual void TakeOverTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            if (task.AssignedUser != null)
                task.AssignedUser.UnAssignTask(task);

            TakeTask(task);
        }

        /// <summary>
        /// Decouple this sprintuser from its sprint and user so that it can be deleted.
        /// </summary>
        public virtual void DecoupleSprintUser()
        {
            user.RemoveSprintUser(this);
            sprint.RemoveSprintUser(this);
        }

        #endregion
    }
}