using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Cashflow.Services.Account.Services;
using Cashflow.Services.Account.Controllers.Contracts;
using Cashflow.Services.Account.Services.Entities;

namespace Cashflow.Services.Account.Controllers
{
  /// <summary>
  /// Account Controller for API.
  /// </summary>
  [ApiController]
  [Route("account")]
  public class AccountController : ControllerBase
  {
    private readonly ILogger<AccountController> _logger;
    private readonly Mapper _mapper;
    private readonly AccountService _service;

    public AccountController(ILogger<AccountController> logger, AccountService service)
    {
      _logger = logger;
      _service = service;
      var config = new MapperConfiguration(expression => {
        // Configuration for receiving collection
        expression.CreateMap<SortQueryContract, AccountSort>();
        expression.CreateMap<FilterQueryContract, AccountFilter>();
        expression.CreateMap<AccountEntityFull, AccountCollectionItemContract>();
        // Configuration for receiving item by id
        expression.CreateMap<AccountEntityBase, AccountGetEntity>();
        // Configuration for creating item
        expression.CreateMap<AccountToCreateContract, AccountEntityBase>();
        expression.CreateMap<AccountEntityFull, AccountCreatedContract>();
        // Configuration for editing item by id
        expression.CreateMap<AccountEntityBase, AccountEditContract>();
      });
      _mapper = new Mapper(config);
    }

    /// <summary>
    /// Provides Account collection.
    /// </summary>
    /// <param name="filter">Defines filters.</param>
    /// <param name="sort">Defines sort rule.</param>
    /// <returns>Account Collection Action Result.</returns>
    [HttpGet]
    public async Task<ActionResult> GetCollection(
      [FromQuery] FilterQueryContract[] filters,
      [FromQuery] SortQueryContract sort)
    {
      var sorting = _mapper.Map<AccountSort>(sort);
      var filteringList = _mapper.Map<List<AccountFilter>>(filters);
      var entityCollection = await _service.GetCollection(filteringList, sorting);
      var resultCollection = _mapper.Map<List<AccountCollectionItemContract>>(entityCollection);
      return Ok(resultCollection);
    }

    /// <summary>
    /// Provides Account by Id.
    /// </summary>
    /// <param name="id">Target Account Id.</param>
    /// <returns>Result of target Account or Not Found Result.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
      var entity = await _service.Get(id);
      if (entity == null)
      {
        return NotFound();
      }
      var result = _mapper.Map<AccountGetEntity>(entity);
      return Ok(result);
    }

    /// <summary>
    /// Creates new Account.
    /// </summary>
    /// <param name="newAccount">Account to create.</param>
    /// <returns>Result of Creation the Account with id and Location url to getting.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(AccountToCreateContract newAccount)
    {
      var entryEntity = _mapper.Map<AccountEntityBase>(newAccount);
      var createdEntity = await _service.Create(entryEntity);
      var createdResult = _mapper.Map<AccountCreatedContract>(createdEntity);
      return CreatedAtAction(nameof(Get), new { id = createdResult.Id }, createdResult);
    }

    /// <summary>
    /// Edits existing Account.
    /// </summary>
    /// <param name="id">Id of account.</param>
    /// <param name="account">Updated Account data.</param>
    /// <returns>Result without content.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(string id, AccountEditContract account)
    {
      var entityEntry = _mapper.Map<AccountEntityBase>(account);
      await _service.Update(id, entityEntry);
      return NoContent();
    }

    /// <summary>
    /// Removes Account.
    /// </summary>
    /// <param name="id">Id of Account to remove.</param>
    /// <returns>Result without content.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
      await _service.Remove(id);
      return NoContent();
    }
  }
}
