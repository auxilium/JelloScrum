namespace JelloScrum.Services
{
    using Model.Entities;
    using Model.Enumerations;
    using Model.Services;

    public class TaskService : GenericService<Task>, ITaskService
    {
        public void SaveSprintGebruiker(Task task, SprintGebruiker sprintGebruiker)
        {
            //TODO: Hier moet iets met het logboek gedaan worden om de history van de task bij te houden
            task.Behandelaar = sprintGebruiker;
            task.Status = Status.Opgepakt;
            sprintGebruiker.Taken.Add(task);
            Save(task);
        }
        public void PlaatsTaskTerugInBacklog(Task task)
        {
            //TODO: Hier moet iets met het logboek gedaan worden om de history van de task bij te houden
            task.Behandelaar = null;
            task.Status = Status.NietOpgepakt;
            Save(task);
        }

        public void SaveTaskAlsAfgesloten(Task task)
        {
            //TODO: Hier moet iets met het logboek gedaan worden om de history van de task bij te houden
            task.Behandelaar = null;
            task.Status = Status.Afgesloten;
            Save(task);
        }
    }
}
