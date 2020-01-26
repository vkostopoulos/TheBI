using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MongoDb.Enums;
using MongoDb.Models;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;
using MongoDb.Repository;
using MongoDB.Bson;

namespace MongoDb.Services.Impl
{
    public class CustomFieldsService : ICustomFieldsService
    {
        private readonly IRepository<CustomField> _repository;

        public CustomFieldsService(IRepository<CustomField> repository)
        {
            _repository = repository;
        }

        public async Task<IList<CustomFieldDto>> GetAll()
        {
            var result = await _repository.GetAllAsync(EntityType.CustomField).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<IList<CustomFieldDto>>(result);
        }

        public async Task<CustomFieldDto> GetById(string id)
        {
            var result = await _repository.GetByIdAsync(id).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<CustomFieldDto>(result);
        }

        public async Task<bool> CustomFieldExists(string name)
        {
            var result = await _repository.ExistsAsync(x => x.Name == name).ConfigureAwait(false);
            return result;
        }

        public async Task<CustomFieldDto> Add(CustomFieldDto customFieldDto)
        {
            var customField = Mapper.Map<CustomField>(customFieldDto);
            customField.Id = ObjectId.GenerateNewId().ToString();
            ;
            var result = await _repository.AddAsync(customField).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<CustomFieldDto>(result);
        }

        public async Task<CustomFieldDto> Update(CustomFieldDto customFieldDto)
        {
            var customField = await _repository.GetByIdAsync(customFieldDto.Id).ConfigureAwait(false);
            if (customField == null)
                return null;
            customField = Mapper.Map(customFieldDto.FixMeUp(), customField);
            var result = await _repository.UpdateAsync(customField).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<CustomFieldDto>(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            await _repository.DeleteAsync(id).ConfigureAwait(false);
            return true;
        }
    }
}