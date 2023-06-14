using AutoMapper;
using News.api.Entities;
using News.api.ViewModels;

namespace News.api.Mapping;

public class ViewModelToEntityMappingProfile : Profile
{
    public ViewModelToEntityMappingProfile()
    {
        CreateMap<RegistrationViewModel, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        
        CreateMap<PostCreateModel, Post>();
        CreateMap<PostUpdateModel, Post>();
        CreateMap<TopicCreateModel, Topic>();
        CreateMap<TopicUpdateModel, Topic>();
        CreateMap<GroupCreateModel, Group>();
        CreateMap<MemberCreateModel, Member>();
        CreateMap<MemberUpdateModel, Member>();
        CreateMap<ReadHistoryCreate, ReadHistory>();
        CreateMap<ReadHistoryUpdate, ReadHistory>();
        CreateMap<GroupMemberCreateModel, GroupMember>();
        CreateMap<GroupMemberUpdateModel, GroupMember>();
  }
}
