namespace JelloScrum.Tests.Database
{
    using Helpers;
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.IRepositories;
    using NUnit.Framework;
    using Rhino.Commons;

    [TestFixture]
    public class ProjectRepositoryProjectOpslaanTest : DBTestBase
    {
        private IProjectRepository ProjectRepository;
        private Project project;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            ProjectRepositoryHelper<Project>.ConfigureerProjectRepository();
            ProjectRepository = IoC.Resolve<IProjectRepository>();
            project = new Project("ProjectNaam", "ProjectOmschrijving");
        }

        /// <summary>
        /// Dit lijkt me meer een soort voorbeeld db test. De test zelf is vrij kansloos.
        /// </summary>
        [Test]
        public void TestNieuwProjectOpslaan()
        {
            ProjectRepository.Save(project);

            Project dbProject = ProjectRepository.Get(project.Id);

            Assert.AreEqual(dbProject.Naam, "ProjectNaam");
        }
    }
}