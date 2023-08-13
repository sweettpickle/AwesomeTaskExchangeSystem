namespace TaskManager.Application.Utils.Common.Models
{
    public class TaskResult
    {
        public string PublicId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal WriteOffAmount { get; set; }
        public decimal AccrualAmount { get; set; }
        public string Status { get; set; }
        public ParrotResult Parrot { get; set; }
    }
}
