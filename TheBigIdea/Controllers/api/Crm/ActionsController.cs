using System.Threading.Tasks;
using System.Web.Http;
using MongoDb.Models.Dto.Crm;
using MongoDb.Services;
using TheBigIdea.Helpers;

namespace TheBigIdea.Controllers.api.Crm
{
    public class ActionsController : ApiController
    {
        private readonly IActionsService _actionsService;
        private readonly IActionValidator _actionValidator;

        /// <summary>
        ///     Ctor
        /// </summary>
        public ActionsController(IActionsService actionsService, IActionValidator actionValidator)
        {
            _actionsService = actionsService;
            _actionValidator = actionValidator;
        }

        // GET api/<controller>
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _actionsService.GetAll());
        }

        //// GET api/<controller>/5
        public async Task<IHttpActionResult> Get(string id)
        {
            return Ok(await _actionsService.GetById(id));
        }

        // POST api/<controller>
        public async Task<IHttpActionResult> Post(ActionDto actionDto)
        {
            var errorMessage = await _actionValidator.ValidateAction(actionDto);
            if (!string.IsNullOrEmpty(errorMessage)) return BadRequest(errorMessage);
            return Ok(await _actionsService.Add(actionDto));
        }

        // PUT api/<controller>/5
        public async Task<IHttpActionResult> Put(string id, ActionDto actionDto)
        {
            if (actionDto.Id != id || string.IsNullOrEmpty(id))
                return BadRequest();
            var errorMessage = await _actionValidator.ValidateAction(actionDto);
            if (!string.IsNullOrEmpty(errorMessage)) return BadRequest(errorMessage);
            return Ok(await _actionsService.Update(actionDto));
        }

        // DELETE api/<controller>/5
        public async Task<IHttpActionResult> Delete(string id)
        {
            return Ok(await _actionsService.DeleteById(id));
        }
    }
}