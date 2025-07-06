namespace Aptiverse.Worker.Models
{
    public class TaskPayload
    {
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public string TaskType { get; set; }
        public string InputText { get; set; }
    }
}
