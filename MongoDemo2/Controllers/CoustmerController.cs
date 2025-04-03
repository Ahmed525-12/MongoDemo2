using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDemo2.Entitie;
using MongoDemo2.ServiceHandler;

namespace MongoDemo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoustmerController : ControllerBase
    {
        private readonly IMongoCollection<Coustmer> _coustmers;

        public CoustmerController(MongoDbService mongoDbService)
        {
            _coustmers = mongoDbService.GetCollection<Coustmer>();
        }

        [HttpGet]
        public async Task<IEnumerable<Coustmer>> Get()
        {
            return await _coustmers.Find(FilterDefinition<Coustmer>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Coustmer>> GetbyId(string id)
        {
            var filter = Builders<Coustmer>.Filter.Eq(x => x.Id, id);
            var coustmer = await _coustmers.Find(filter).FirstOrDefaultAsync();
            return Ok(coustmer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Coustmer coustmer)
        {
            coustmer.Id = null;
            await _coustmers.InsertOneAsync(coustmer);
            return Ok(coustmer);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Coustmer coustmer)
        {
            var filter = Builders<Coustmer>.Filter.Eq(x => x.Id, coustmer.Id);
            await _coustmers.ReplaceOneAsync(filter, coustmer);
            return Ok(coustmer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Update(string id)
        {
            var filter = Builders<Coustmer>.Filter.Eq(x => x.Id, id);
            await _coustmers.DeleteOneAsync(filter);
            return Ok(new { message = "delete sussefully" });
        }
    }
}