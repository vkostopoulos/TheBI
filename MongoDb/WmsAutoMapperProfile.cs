using AutoMapper;
using MongoDb.Models;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;
using MongoDb.Models.Dto.Wms;
using MongoDb.Models.Wms;

namespace MongoDb
{
    public class WmsAutoMapperProfile : Profile
    {
        public WmsAutoMapperProfile()
        {
            CreateMap<Attribute, AttributeDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CustomFieldDto, CustomField>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Action, ActionDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Action, ActionDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Action, ActionDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Action, ActionDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Action, ActionDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Action, ActionDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Action, ActionDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Action, ActionDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Contact, ContactDto>();
            CreateMap<CustomField, CustomFieldDto>();
            CreateMap<Action, ActionDto>();
        }
    }
}