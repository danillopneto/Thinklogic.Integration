using AutoMapper;
using Thinklogic.Integration.Domain.DataContracts.Responses.Asana;
using Thinklogic.Integration.Domain.Dtos.Asana;

namespace Thinklogic.Integration.UseCases.Profiles
{
    public class AsanaProfile : Profile
    {
        public AsanaProfile()
        {
            CreateMap<AsanaCommentResponse, AsanaCommentResultDto>();
        }
    }
}
