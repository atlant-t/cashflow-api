namespace Cashflow.Services.Account.Services.Entities
{
  /// <summary>
  /// Definition for Filtering.
  /// </summary>
  public class AccountFilter
  {
    /// <summary>
    /// Target Field in which to filter.
    /// </summary>
    /// <value>Field Name.</value>
    public string Field { get; set; }

    /// <summary>
    /// Rule by which to filter.
    /// </summary>
    /// <value>Equal or Match rule.</value>
    public string Rule { get; set; }

    /// <summary>
    /// Value by which to filter.
    /// </summary>
    /// <value>Value.</value>
    public string Value { get; set; }
  }
}
