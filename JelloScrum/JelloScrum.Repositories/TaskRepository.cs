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

namespace JelloScrum.Repositories
{
    using Exceptions;
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.Enumerations;
    using JelloScrum.Model.IRepositories;

    public class TaskRepository : JelloScrumRepository<Task>, ITaskRepository
    {
        #region ITaskRepository Members
                

        public override void Delete(Task task)
        {
            if (task.TimeRegistrations.Count != 0)
            {
                throw new JelloScrumRepositoryException("Er zijn nog Tijdregistraties gekoppeld");
            }
            base.Delete(task);
        }
        #endregion
    }
}