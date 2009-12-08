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

namespace JelloScrum.Repositories.Tests.Creations
{
    using Container;
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.Enumerations;
    using JelloScrum.Model.IRepositories;

    /// <summary>
    /// Methodes om allerhande entititeiten aan te maken. Gemaakte objecten worden ook gepersisteerd.
    /// </summary>
    public partial class Creation
    {
        private static IProjectRepository projectRepository = IoC.Resolve<IProjectRepository>();
        
        private static Project Persist(Project project)
        {
            return projectRepository.Save(project);
        }

        public static Project Project()
        {
            return Persist(new Project());
        }

        public static Project Project(string projectNaam)
        {
            return Persist(new Project(projectNaam, string.Empty));
        }
    }
}