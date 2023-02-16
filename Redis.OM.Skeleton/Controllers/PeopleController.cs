using Microsoft.AspNetCore.Mvc;
using Redis.OM.Searching;
using Redis.OM.Skeleton.Model;

namespace Redis.OM.Skeleton.Controllers;

[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    private readonly RedisCollection<Person> _people;
    private readonly RedisConnectionProvider _provider;

    public PeopleController(RedisConnectionProvider provider)
    {
        _provider = provider;
        _people = (RedisCollection<Person>)provider.RedisCollection<Person>();
    }

    [HttpPost]
    public async Task<Person> AddPerson([FromBody] Person person)
    {
        await _people.InsertAsync(person);
        return person;
    }

    [HttpGet("filterAge")]
    public IList<Person> FilterByAge([FromQuery] int minAge, [FromQuery] int maxAge = 120)
    {        
        return _people.Where(x => x.Age >= minAge && x.Age <= maxAge).ToList();
    }

    [HttpGet("filterName")]
    public IList<Person> FilterByName([FromQuery] string firstName, [FromQuery] string lastName)
    {
        return _people.Where(x => x.FirstName == firstName && x.LastName == lastName).ToList();
    }

    [HttpGet("postalCode")]
    public IList<Person> FilterByPostalCode([FromQuery] string postalCode)
    {
        return _people.Where(x => x.Address!.PostalCode == postalCode).ToList();
    }
}
