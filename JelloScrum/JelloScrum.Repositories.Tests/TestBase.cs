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
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using Castle.ActiveRecord;
    using Castle.ActiveRecord.Framework;
    using Castle.ActiveRecord.Framework.Config;
    using Model;
    using NHibernate;
    using NUnit.Framework;

    public abstract class TestBase
    {
        protected SessionScope sessionScope;

       // public ISession DezeSessieDan;
    
        [SetUp]
        public virtual void SetUp()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");

            Container.IoC.Initialize();

            //delete an existing database.
            //if no file is found file.delete fails silently
            File.Delete("test.db");
            
            IConfigurationSource config = ActiveRecordSectionHandler.Instance;

            Assembly asm1 = Assembly.Load("JelloScrum.Model");
            ActiveRecordStarter.Initialize(new Assembly[] { asm1 }, config);

            ActiveRecordStarter.CreateSchema();

            sessionScope = new SessionScope(FlushAction.Never);
        }

        [TearDown]
        public void TearDown()
        {
            sessionScope.Dispose();
            ActiveRecordStarter.DropSchema();
            ActiveRecordStarter.ResetInitializationFlag();
        }
    }
}