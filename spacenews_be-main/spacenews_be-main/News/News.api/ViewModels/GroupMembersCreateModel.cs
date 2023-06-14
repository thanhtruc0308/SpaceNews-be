namespace News.api.ViewModels
{
    public class GroupMemberCreateModel
    {
        public string? MemberID { get; set; }
        public string? name { get; set; }

    }
    public class GroupMemberUpdateModel : GroupMemberCreateModel
    {
        public int Id { get; set; }
    }
}
