namespace Cashflow.Services.Account.Services.Entities
{
  /// <summary>
  /// Definition for Sorting.
  /// </summary>
  public class AccountSort
  {
    /// <summary>
    /// Target Field in which to sort.
    /// </summary>
    /// <value>Field to sort.</value>
    public string Field { get; set; }

    /// <summary>
    /// Selected Sort Order.
    /// </summary>
    /// <value>Order to sort.</value>
    public string Order { get; set; }
  }
}
