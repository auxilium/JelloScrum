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

namespace JelloScrum.Repositories.Tests
{
    using System;
    using Container;
    using Creations;
    using JelloScrum.Model.Entities;
    using Exceptions;
    using JelloScrum.Model.IRepositories;
    using NUnit.Framework;

    [TestFixture]
    public class TaskRepositoryTest : TestBase
    {
        #region Setup/Teardown

        private ITaskRepository taskRepository;
        private Task task;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            taskRepository = IoC.Resolve<ITaskRepository>();
        }

        [Test]
        public void TestVerwijderTaskZonderGeboekteUren()
        {
            task = Creation.Task();
            taskRepository.Delete(task);
            //UnitOfWork.CurrentSession.Clear();
            Task newTask = taskRepository.Get(task.Id);
            Assert.IsNull(newTask);
        }

        [Test, ExpectedException(typeof (JelloScrumRepositoryException))]
        public void TestVerwijderTaskMetGeboekteUrenGeeftJelloScrumRepositoryException()
        {
            TimeSpan ts = new TimeSpan(25, 30, 0);

            task = Creation.TaskMetStoryAndProjectAndGebruiker();
            task.RegisterTime(task.Story.AangemaaktDoor, DateTime.Now, Creation.Sprint(task.Story.Project), new TimeSpan(8, 0, 0));
            taskRepository.Save(task);
           // UnitOfWork.CurrentSession.Clear();
            taskRepository.Delete(task);
            Assert.Fail();
        }
        #endregion
    }
}