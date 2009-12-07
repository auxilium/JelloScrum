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

namespace JelloScrum.Model.Tests.Creations
{
    using Entities;
    using Enumerations;

    public partial class Creation
    {
        public static Task Task()
        {
            return new Task();
        }

        public static Task TaskMetCompleteHierarchie()
        {
            User gebruiker = Gebruiker();
                       
            Sprint sprint = Sprint();
            project.AddSprint(sprint);

            Story story = StoryMetSprintStoryEnSprintBacklogPrioriteit(gebruiker, Priority.Must, sprint);
            //new Story(project, gebruiker, null, StoryType.UserStory);
            //project.VoegStoryToe(story);
            //sprint.MaakSprintStoryVoor(story);
            Task task = Task();
            story.AddTask(task);

            SprintGebruiker sprintGebruiker = sprint.AddUser(gebruiker, SprintRole.Developer);
            sprintGebruiker.PakTaakOp(task);
            return task;
        }
    }
}