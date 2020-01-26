using System.Threading.Tasks;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;
using MongoDb.Services;

namespace TheBigIdea.Helpers.Impl
{
    public class ActionValidator : IActionValidator
    {
        private readonly IActionsService _actionsService;

        public ActionValidator(IActionsService actionsService)
        {
            _actionsService = actionsService;
        }

        public async Task<string> ValidateAction(ActionDto actionDto)
        {
            if (string.IsNullOrEmpty(actionDto.Id))
            {
                // Check customfields
                var result = await _actionsService.ActionExists(actionDto.Name);
                return result ? $"Action with Name #{actionDto.Name} already exists!" : string.Empty;
            }
            else
            {
                // Check customfields
                var result = await _actionsService.GetById(actionDto.Id);
                if (result?.Name == actionDto.Name) return string.Empty;
                var exists = await _actionsService.ActionExists(actionDto.Name);
                return exists ? $"Action with Name #{actionDto.Name} already exists!" : string.Empty;
            }
        }
    }
}