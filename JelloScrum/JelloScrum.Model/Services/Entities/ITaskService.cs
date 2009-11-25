namespace JelloScrum.Model.Services
{
    using Model.Entities;

    /// <summary>
    /// 
    /// </summary>
    public interface ITaskService : IGenericService<Task>
    {
        /// <summary>
        /// Saves the sprint gebruiker.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="sprintGebruiker">The sprint gebruiker.</param>
        void SaveSprintGebruiker(Task task, SprintGebruiker sprintGebruiker);

        /// <summary>
        /// Tasks the terug in backlog.
        /// </summary>
        /// <param name="task">The task.</param>
        void PlaatsTaskTerugInBacklog(Task task);

        /// <summary>
        /// Sla de task op als Afgesloten
        /// </summary>
        /// <param name="task">The task.</param>
        void SaveTaskAlsAfgesloten(Task task);
    }
}
