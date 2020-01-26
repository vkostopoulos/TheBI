using AutoMapper;
using MongoDb.Models;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;
using MongoDb.Models.Wms;

namespace MongoDb
{
    public class CrmAutoMapperProfile : Profile
    {
        public CrmAutoMapperProfile()
        {
            CreateMap<ContactDto, Contact>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CustomFieldDto, CustomField>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ActionDto, Action>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Contact, ContactDto>();
            CreateMap<CustomField, CustomFieldDto>();
            CreateMap<Action, ActionDto>();
        }
    }
}