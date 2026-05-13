namespace TeslaNE42Vision2D.Packet
{
    public class VisionJob
    {
        public enum StatusEnum
        {
            Processing = 0,
            Completed = 1,
            Aborted = 2,
            Failed = 3,
            Faulted = 4,
        }

        public enum AssessmentEnum
        {
            NG = 0,
            OK = 1,
            NC = 2,
            NA = 3,
        }

        public int Id;
        public StatusEnum Status { get; set; } = StatusEnum.Processing;
        public AssessmentEnum Assessment;
        public string StatusMessage { get; set; } = string.Empty;
    }
}
