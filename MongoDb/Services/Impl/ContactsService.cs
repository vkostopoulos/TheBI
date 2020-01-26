using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MongoDb.Enums;
using MongoDb.Models;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;
using MongoDb.Models.Wms;
using MongoDb.Repository;
using MongoDB.Bson;

namespace MongoDb.Services.Impl
{
    public class ContactsService : IContactsService
    {
        private readonly IRepository<Contact> _repository;

        public ContactsService(IRepository<Contact> repository)
        {
            _repository = repository;
        }

        public async Task<IList<ContactDto>> GetAll()
        {
            var result = await _repository.GetAllAsync(EntityType.Contact).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<IList<ContactDto>>(result);
        }

        public async Task<ContactDto> GetById(string id)
        {
            var result = await _repository.GetByIdAsync(id).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<ContactDto>(result);
        }

        public async Task<ContactDto> Add(ContactDto contactDto)
        {
            var contact = Mapper.Map<Contact>(contactDto);
            contact.Id = ObjectId.GenerateNewId().ToString();
            var result = await _repository.AddAsync(contact).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<ContactDto>(result);
        }

        public async Task<ContactDto> Update(ContactDto contactDto)
        {
            var contact = await _repository.GetByIdAsync(contactDto.Id).ConfigureAwait(false);
            if (contact == null) return null;
            contact = Mapper.Map(contactDto.FixMeUp(), contact);
            var result = await _repository.UpdateAsync(contact).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<ContactDto>(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            await _repository.DeleteAsync(id).ConfigureAwait(false);
            return true;
        }
    }
}