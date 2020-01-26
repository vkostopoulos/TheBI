using System.Threading.Tasks;
using System.Web.Http;
using MongoDb.Models.Dto.Crm;
using MongoDb.Services;
using TheBigIdea.Helpers;

namespace TheBigIdea.Controllers.api.Crm
{
    public class CustomFieldsController : ApiController
    {
        private readonly ICustomFieldsService _customFieldsService;
        private readonly ICustomFieldValidator _customFieldValidator;

        /// <summary>
        ///     Ctor
        /// </summary>
        public CustomFieldsController(ICustomFieldsService customFieldsService,
            ICustomFieldValidator customFieldValidator)
        {
            _customFieldsService = customFieldsService;
            _customFieldValidator = customFieldValidator;
        }

        // GET api/<controller>
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _customFieldsService.GetAll());
        }

        //// GET api/<controller>/5
        public async Task<IHttpActionResult> Get(string id)
        {
            return Ok(await _customFieldsService.GetById(id));
        }

        // POST api/<controller>
        public async Task<IHttpActionResult> Post(CustomFieldDto customFieldDto)
        {
            var errorMessage = await _customFieldValidator.ValidateCustomField(customFieldDto);
            if (!string.IsNullOrEmpty(errorMessage)) return BadRequest(errorMessage);
            return Ok(await _customFieldsService.Add(customFieldDto));
        }

        // PUT api/<controller>/5
        public async Task<IHttpActionResult> Put(string id, CustomFieldDto customFieldDto)
        {
            if (customFieldDto.Id != id || string.IsNullOrEmpty(id))
                return BadRequest("The customField ID is invalid!");
            var errorMessage = await _customFieldValidator.ValidateCustomField(customFieldDto);
            if (!string.IsNullOrEmpty(errorMessage)) return BadRequest(errorMessage);
            return Ok(await _customFieldsService.Update(customFieldDto));
        }

        // DELETE api/<controller>/5
        public async Task<IHttpActionResult> Delete(string id)
        {
            return Ok(await _customFieldsService.DeleteById(id));
        }
    }
}