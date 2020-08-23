using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Cashflow.Services.Account.Services.Entities;

namespace Cashflow.Services.Account.Services
{
  /// <summary>
  /// Providers Account Data from database.
  /// </summary>
  public class AccountService
  {
    /// <summary>
    /// Configures mongodb collection to use case insensitive.
    /// </summary>
    /// <remarks>
    /// See https://stackoverflow.com/a/50779179/13913427
    /// </remarks>
    private readonly Collation caseInsensitiveCollation;

    /// <summary>
    /// Represents Account Collection in database.
    /// </summary>
    public readonly IMongoCollection<BsonDocument> Accounts;

    /// <summary>
    /// Providers Account Data from database.
    /// </summary>
    /// <param name="database">Mongo Database.</param>
    public AccountService(IMongoDatabase database)
    {
      caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);
      Accounts = database.GetCollection<BsonDocument>("accounts");
    }

    /// <summary>
    /// Providers Account collection.
    /// </summary>
    /// <param name="filters">Define filter rules.</param>
    /// <param name="sort">Define sort rule.</param>
    /// <returns>Enumerable Account Collection.</returns>
    public async Task<List<AccountEntityFull>> GetCollection(
      List<AccountFilter> filters = null,
      AccountSort sort = null)
    {
      // A basic filtering and sorting solution is implemented here.
      // TODO: Find a more flexible solution and implement it.

      var findOptions = new FindOptions {Collation = caseInsensitiveCollation};
      var filterBuilder = new FilterDefinitionBuilder<BsonDocument>();
      var sortBuilder = new SortDefinitionBuilder<BsonDocument>();

      var filterDefinition = filterBuilder.Empty;
      var sortDefinition = sortBuilder.Ascending("name");

      if (sort?.Field != null || sort?.Order != null)
      {
        sort.Field = sort.Field ?? "name";
        sortDefinition = sort.Order == "desc"
                       ? sortBuilder.Descending(sort.Field)
                       : sortBuilder.Ascending(sort.Field);
      }

      if (filters != null)
      {
        foreach (var filter in filters)
        {
          var pattern = filter.Rule == "equal" ? $"^{filter.Value}$"
                      : filter.Value;
          var exp = new BsonRegularExpression(pattern, "i");
          filterDefinition = filterDefinition & filterBuilder.Regex(filter.Field, exp);
        }
      }

      return await Accounts.Find(filterDefinition, findOptions)
                           .Sort(sortDefinition)
                           .As<AccountEntityFull>()
                           .ToListAsync();
    }

    /// <summary>
    /// Provides Account by Id or null.
    /// </summary>
    /// <param name="id">Target Account Id.</param>
    /// <returns>Target Account or null.</returns>
    public async Task<AccountEntityBase> Get(string id)
    {
      var result = await Accounts.Find(new BsonDocument("_id", new ObjectId(id)))
                                 .FirstOrDefaultAsync();
      return result != null ? BsonSerializer.Deserialize<AccountEntityBase>(result) : null;
    }

    /// <summary>
    /// Creates new Account.
    /// </summary>
    /// <param name="account">Account to set.</param>
    /// <returns>Setted Account with id.</returns>
    public async Task<AccountEntityFull> Create(AccountEntityBase account)
    {
      var entity = account.ToBsonDocument();
      await Accounts.InsertOneAsync(entity);
      return BsonSerializer.Deserialize<AccountEntityFull>(entity);
    }

    /// <summary>
    /// Update Exist Account.
    /// </summary>
    /// <param name="account">Exist Account.</param>
    /// <returns>Updated Account.</returns>
    public async Task<AccountEntityFull> Update(string id, AccountEntityBase account)
    {
      var entity = account.ToBsonDocument();
      await Accounts.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(id)), entity);
      return BsonSerializer.Deserialize<AccountEntityFull>(entity);
    }

    /// <summary>
    /// Remove Account.
    /// </summary>
    /// <param name="id">Account Id.</param>
    public async Task Remove(string id)
    {
      await Accounts.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
    }
  }
}
