namespace News.api.ViewModels
{
    public class PostCreateModel
    {
        public DateTime? Date { get; set; }
        public DateTime? Time { get; set; }
        public string? Location { get; set; }
        public string Image { get; set; }
        public int Priority { get; set; } = 1;
        public string Content { get; set; }
        public bool ShowInSlider { get; set; }
        public int? TopicID { get; set; }
        public string? GroupID { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
    }

    public class PostUpdateModel : PostCreateModel
    {
        public int Id { get; set; }
    }
}
