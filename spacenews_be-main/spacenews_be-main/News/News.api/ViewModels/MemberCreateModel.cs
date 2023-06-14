namespace News.api.ViewModels
{
    public class MemberCreateModel
    {
        public string Name { get; set; }
        public string Email { get; set; } 
    }
    public class MemberUpdateModel : MemberCreateModel
    {
        public int Id { get; set; }
    }
}