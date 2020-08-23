using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cashflow.Services.Account.Services.Entities
{
  /// <summary>
  /// Full Account data, containing Basic data and Account Id.
  /// </summary>
  [BsonIgnoreExtraElements]
  public class AccountEntityFull : AccountEntityBase
  {
    /// <summary>
    /// The Id of the current Account Item in database.
    /// </summary>
    /// <value>Account Id.</value>
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
  }
}
