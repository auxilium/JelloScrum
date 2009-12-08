namespace JelloScrum.Model.Tests.Model
{
    using Entities;
    using Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class TaskTest : TestBase
    {
        private Task taak;
        
        public override void SetUp()
        {
            taak = new Task();

            base.SetUp();
        }

        [Test]
        public void TestSaveTaakAlsDone()
        {
            taak.ZetTaakAlsAfgesloten();

            Assert.IsTrue(taak.Status == Status.Afgesloten);
        }
    }
}