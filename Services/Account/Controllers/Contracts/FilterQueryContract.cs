using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Services.Account.Controllers.Contracts
{
  /// <summary>
  /// Definition for Filtering in API.
  /// </summary>
  public class FilterQueryContract {
    /// <summary>
    /// Target Field in which to filter.
    /// </summary>
    /// <value>Field Name.</value>
    [BindProperty(Name = "filter-field")]
    public string Field { get; set; }

    /// <summary>
    /// Rule by which to filter.
    /// </summary>
    /// <value>Equal or Match rule.</value>
    [BindProperty(Name = "filter-rule")]
    public string Rule { get; set; }

    /// <summary>
    /// Value by which to filter.
    /// </summary>
    /// <value>Value.</value>
    [BindProperty(Name = "filter-value")]
    public string Value { get; set; }
  }
}
