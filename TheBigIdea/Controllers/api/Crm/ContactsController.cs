using System.Threading.Tasks;
using System.Web.Http;
using MongoDb.Services;
using TheBigIdea.Helpers;
using MongoDb.Models.Dto.Crm;

namespace TheBigIdea.Controllers.api.Crm
{
    public class ContactsController : ApiController
    {
        private readonly IContactsService _contactsService;
        private readonly IContactValidator _contactValidator;

        /// <summary>
        ///     Ctor
        /// </summary>
        public ContactsController(IContactsService contactsService, IContactValidator contactValidator)
        {
            _contactsService = contactsService;
            _contactValidator = contactValidator;
        }

        // GET api/<controller>
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _contactsService.GetAll());
        }

        //// GET api/<controller>/5
        public async Task<IHttpActionResult> Get(string id)
        {
            if (await _contactsService.GetById(id) == null) return BadRequest("The customField does not exist!");
            return Ok(await _contactsService.GetById(id));
        }

        // POST api/<controller>
        public async Task<IHttpActionResult> Post(ContactDto contactDto)
        {
            var errorMessage = await _contactValidator.ValidateContact(contactDto);
            if (!string.IsNullOrEmpty(errorMessage))
                return BadRequest(errorMessage);
            return Ok(await _contactsService.Add(contactDto));
        }

        // PUT api/<controller>/5
        public async Task<IHttpActionResult> Put(string id, ContactDto contactDto)
        {
            if (contactDto.Id != id || string.IsNullOrEmpty(id))
                return BadRequest("The customField Id is not valid!");
            var errorMessage = await _contactValidator.ValidateContact(contactDto);
            if (!string.IsNullOrEmpty(errorMessage)) return BadRequest(errorMessage);
            return Ok(await _contactsService.Update(contactDto));
        }

        // DELETE api/<controller>/5
        public async Task<IHttpActionResult> Delete(string id)
        {
            if (await _contactsService.GetById(id) == null)
                return BadRequest("The customField does not exist!");
            return Ok(await _contactsService.DeleteById(id));
        }
    }
}