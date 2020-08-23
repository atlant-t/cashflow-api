namespace Cashflow.Services.Account.Controllers.Contracts
{
  /// <summary>
  /// Updated account data to save from API.
  /// </summary>
  public class AccountEditContract
  {
    /// <summary>
    /// Updated Account Name to save to store.
    /// </summary>
    /// <value>Account Name.</value>
    public string Name { get; set; }
  }
}
