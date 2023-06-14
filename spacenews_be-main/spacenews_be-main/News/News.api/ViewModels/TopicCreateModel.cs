namespace News.api.ViewModels
{
    public class TopicCreateModel
    {
        public string Name { get; set; }
    }
    
    public class TopicUpdateModel : TopicCreateModel
    {
        public int Id { get; set; }
    }
}
