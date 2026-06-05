namespace WORKFLOW_TUBES_KPL_ERGOLAB
{
    public class NotificationConfig
    {
        public List<NotificationTemplate> Templates { get; set; }
    }

    public class NotificationTemplate
    {
        public string Status { get; set; }
        public string Recipient { get; set; }
        public string Message { get; set; }
    }
}