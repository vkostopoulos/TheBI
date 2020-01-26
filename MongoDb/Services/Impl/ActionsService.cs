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
    public class ActionsService : IActionsService
    {
        private readonly IRepository<Action> _repository;

        public ActionsService(IRepository<Action> repository)
        {
            _repository = repository;
        }

        public async Task<IList<ActionDto>> GetAll()
        {
            var result = await _repository.GetAllAsync(EntityType.Action).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<IList<ActionDto>>(result);
        }

        public async Task<ActionDto> GetById(string id)
        {
            var result = await _repository.GetByIdAsync(id).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<ActionDto>(result);
        }

        public async Task<bool> ActionExists(string name)
        {
            var result = await _repository.ExistsAsync(x => x.Name == name).ConfigureAwait(false);
            return result;
        }

        public async Task<ActionDto> Add(ActionDto actionDto)
        {
            var action = Mapper.Map<Action>(actionDto);
            action.Id = ObjectId.GenerateNewId().ToString();
            var result = await _repository.AddAsync(action).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<ActionDto>(result);
        }

        public async Task<ActionDto> Update(ActionDto actionDto)
        {
            var action = await _repository.GetByIdAsync(actionDto.Id).ConfigureAwait(false);
            if (action == null) return null;
            action = Mapper.Map(actionDto.FixMeUp(), action);
            var result = await _repository.UpdateAsync(action).ConfigureAwait(false);
            return result == null ? null : Mapper.Map<ActionDto>(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            await _repository.DeleteAsync(id).ConfigureAwait(false);
            return true;
        }
    }
}