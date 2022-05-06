namespace BiPapyon.Common.Events
{
    public class UserEmailChangedEvent
    {
        public string OldEmail { get; set; }
        public string NewEmail { get; set; }
    }
}
