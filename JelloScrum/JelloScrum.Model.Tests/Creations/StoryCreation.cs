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
    using System;
    using Entities;
    using Enumerations;

    public partial class Creation
    {
        static readonly Project project = new Project();
        
        public static Story Story()
        {
            return Story(new Gebruiker());
        }

        public static Story Story(Gebruiker gebruiker)
        {
            return new Story(project, gebruiker, null, StoryType.UserStory);
        }

        public static Story StoryMetSprintStory(Gebruiker gebruiker)
        {
            return StoryMetSprintStoryEnSprintBacklogPrioriteit(gebruiker, Prioriteit.Onbekend, Sprint());
            
        }

        public static Story StoryMetSprintStoryEnSprintBacklogPrioriteit(Gebruiker gebruiker, Prioriteit prioriteit, Sprint sprint)
        {
            Story story = Story(gebruiker);

            sprint.CreateSprintStoryFor(story);
            story.SprintStories[0].SprintBacklogPrioriteit = prioriteit;

            return story;
        }
        
        public static Story Story(Project project, StoryPoint storyPoints, int hoursPerStoryPoint, Prioriteit moscowPrio, Gebruiker aangemaaktDoor)
        {
            Story story = new Story(project, aangemaaktDoor, Impact.Normaal, StoryType.UserStory);
            story.StoryPoints = storyPoints;
            story.Schatting = new TimeSpan(story.WaardeStoryPoints*hoursPerStoryPoint);
            story.ProductBacklogPrioriteit = moscowPrio;
            return story;
        }
    }
}