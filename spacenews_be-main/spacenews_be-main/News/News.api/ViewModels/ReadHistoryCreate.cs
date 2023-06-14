namespace News.api.ViewModels
{
    public class ReadHistoryCreate
    {
        public string? UserID { get; set; }
        public string? EventsID { get; set; }
    }

    public class ReadHistoryUpdate : ReadHistoryCreate
    {
        public string? EventsID { get; set;}

    }
}
