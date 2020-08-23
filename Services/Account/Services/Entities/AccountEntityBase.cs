using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cashflow.Services.Account.Services.Entities
{
  /// <summary>
  /// Basic Account data.
  /// </summary>
  [BsonIgnoreExtraElements]
  public class AccountEntityBase
  {
    /// <summary>
    /// Account Item Name defined by User.
    /// </summary>
    /// <value>Account Name.</value>
    [BsonElement("name")]
    public string Name { get; set; }
  }
}
