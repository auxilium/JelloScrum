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
    using Container;
    using Model.Entities;
    using Model.IRepositories;
    using NUnit.Framework;

    [TestFixture]
    public class ProjectRepositoryTest : TestBase
    {
        #region Setup/Teardown
        private IProjectRepository ProjectRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            ProjectRepository = IoC.Resolve<IProjectRepository>();
        }

        #endregion

        /// <summary>
        /// Test of een project opgeslagen wordt en teruggelezen kan worden.
        /// </summary>
        [Test]
        public void TestOpgeslagenProjectKanWordenTeruggelezen()
        {
            Project project = ProjectRepository.Save(new Project("ProjectNaam", "ProjectOmschrijving"));
            //UnitOfWork.CurrentSession.Clear();
            Project dbProject = ProjectRepository.Get(project.Id);

            Assert.AreEqual(dbProject.Name, "ProjectNaam");
        }
    }
}