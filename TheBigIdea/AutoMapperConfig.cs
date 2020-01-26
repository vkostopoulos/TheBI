using AutoMapper;
using MongoDb;

namespace TheBigIdea
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x => { x.AddProfile<CrmAutoMapperProfile>(); });
        }
    }
}