namespace JelloScrum.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using anrControls;
    using Castle.MonoRail.Framework;
    using Helpers;
    using Model.Entities;
    using Model.Enumerations;

    /// <summary>
    /// In what mode should the component display itself.
    /// </summary>
    public enum StoryAndTasksComponentMode
    {
        /// <summary>
        /// Show my tasks
        /// </summary>
        MyTasks = 0,
        /// <summary>
        /// Show the taken tasks
        /// </summary>
        TakenTasks = 1,
        /// <summary>
        /// Show the closed tasks
        /// </summary>
        ClosedTasks = 2,
        /// <summary>
        /// Show the open tasks
        /// </summary>
        OpenTasks =3
    }


    /// <summary>
    /// Component to show lists of stories with their tasks and functionalities related to those.
    /// </summary>
    [ViewComponentDetails("StoryAndTasksComponent")]
    public class StoryAndTasksComponent : ViewComponent
    {
        private IList<Task> tasks;
        private Sprint sprint;
        private StoryAndTasksComponentMode mode;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            tasks = (IList<Task>) ComponentParams["tasks"];
            sprint = (Sprint) ComponentParams["sprint"];
            mode = (StoryAndTasksComponentMode) ComponentParams["mode"];
        }

        /// <summary>
        /// Renders a list of tasks grouped by their stories.
        /// </summary>
        public override void Render()
        {
            StringBuilder sb = new StringBuilder();
            IList<Story> uniqueStories = new List<Story>();
            
            foreach (Task task in tasks)
            {
                if (!uniqueStories.Contains(task.Story))
                    uniqueStories.Add(task.Story);
            }

            sb.AppendLine("<ul class='storylist'>");
            
            foreach (Story story in uniqueStories)
            {
                sb.AppendLine(RenderStory(story, tasks));
            }

            sb.AppendLine("</ul>");

            RenderText(sb.ToString());
        }

        /// <summary>
        /// Render the story.
        /// </summary>
        /// <param name="story">The story.</param>
        /// <param name="allTasks">All tasks.</param>
        /// <returns></returns>
        private string RenderStory(Story story, ICollection<Task> allTasks)
        {
            StringBuilder sb = new StringBuilder();

            SprintStory ss = sprint.GeefSprintStoryVanStory(story);
            string priority = "<img src='/content/images/moscow_unknown.png' title='Unknown'/>";
            string prioritycss = "sunknown";
            
            
            sb.AppendLine("<li class='storyli'>");
            sb.AppendLine("<div class='sheader'>");

            if (ss != null)
            {
                switch (ss.SprintBacklogPrioriteit)
                {
                    case Prioriteit.Must:
                        priority = "<img src='/content/images/moscow_must.png' title='Must'/>";
                        prioritycss = "smust";
                        break;
                    case Prioriteit.Should:
                        priority = "<img src='/content/images/moscow_should.png' title='Should'/>";
                        prioritycss = "sshould";
                        break;
                    case Prioriteit.Could:
                        priority = "<img src='/content/images/moscow_could.png' title='Could'/>";
                        prioritycss = "scould";
                        break;
                    case Prioriteit.Would:
                        priority = "<img src='/content/images/moscow_wont.png' title='Won't'/>";
                        prioritycss = "swont";
                        break;
                    case Prioriteit.Onbekend:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            sb.AppendFormat("<span class='moscow'>{0}</span>{1}", priority, story.Titel);
            sb.AppendFormat("<span class='time'>{0}</span>", OpmaakHelper.UrenStatus(story.Schatting, story.TotaalBestedeTijd()));
            sb.AppendLine("</div>");
            sb.AppendFormat("<div class='stext'>{0}</div>", RenderMarkdown(story.Omschrijving));
            sb.AppendFormat("<div class='stasks {0}'>", prioritycss);

            sb.AppendLine("<ul class='tasklist'>");

            foreach (Task task in story.Tasks)
            {
                if (allTasks.Contains(task))
                {
                    sb.AppendLine(RenderTask(task));
                }
            }
            
            sb.AppendLine("</ul>");

            sb.AppendLine("</div>");
            sb.AppendLine("</li>");

            return sb.ToString();
        }

        /// <summary>
        /// Render the task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        private string RenderTask(Task task)
        {
            StringBuilder sb = new StringBuilder();
          
            sb.AppendLine("<li class='taskli'>");
            sb.AppendFormat("<div class='theader'>{0}<span class='time'>[{1}]</span></div>", task.Titel, OpmaakHelper.Tijd(task.TotaalBestedeTijd()));
            sb.AppendLine("<div class='tcontent'>");

            switch (mode)
            {
                case StoryAndTasksComponentMode.MyTasks:
                    sb.AppendFormat("<div class='tbuttons'>{0}</div>", RenderMyTasksButtons(task));
                    break;
                case StoryAndTasksComponentMode.TakenTasks:
                    sb.AppendFormat("behandelaar: {0}", task.BehandelaarNaam);
                    sb.AppendFormat("<div class='tbuttons'>{0}</div>", RenderTakenTasksButtons(task));
                    break;
                case StoryAndTasksComponentMode.ClosedTasks:
                    sb.AppendFormat("<div class='tbuttons'>{0}</div>", RenderClosedTasksButtons(task));
                    break;
                case StoryAndTasksComponentMode.OpenTasks:
                    sb.AppendFormat("<div class='tbuttons'>{0}</div>", RenderOpenTasksButtons(task));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            sb.AppendFormat("<div class='ttext'>{0}</div>", RenderMarkdown(task.Omschrijving));
            sb.AppendLine("</div>");
            sb.AppendLine("</li>");

            return sb.ToString();
        }

        /// <summary>
        /// Render the markdown text as html
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private static string RenderMarkdown(string text)
        {
            if (text == null)
                return string.Empty;

            Markdown markdown = new Markdown();
            string result = markdown.Transform(text);
            return result;
        }

        private static string RenderMyTasksButtons(Task task)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<input type='hidden' value='{0}'/>", task.Id);
            sb.AppendLine("<a class='taakAfgeven' href='#'>Taak afgeven</a> | <a class='taakSluiten' href='#'>Taak sluiten</a>");

            return sb.ToString();
        }

        private static string RenderOpenTasksButtons(Task task)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<input type='hidden' value='{0}'/>", task.Id);
            sb.AppendLine("<a class='taakOppakken' href='#'>Taak oppakken</a>");

            return sb.ToString();
        }

        private static string RenderTakenTasksButtons(Task task)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<input type='hidden' value='{0}'/>", task.Id);
            sb.AppendLine("<a class='taakOvernemen' href='#'>Taak overnemen</a>");

            return sb.ToString();
        }

        private static string RenderClosedTasksButtons(Task task)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<input type='hidden' value='{0}'/>", task.Id);
            sb.AppendLine("<a class='taakHeropenen' href='#'>Taak heropenen</a>");

            return sb.ToString();
        }
       
    }
}