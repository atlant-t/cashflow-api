using Microsoft.AspNetCore.Mvc;

namespace Cashflow.Services.Account.Controllers.Contracts
{
  /// <summary>
  /// Definition for Sorting in API.
  /// </summary>
  public class SortQueryContract {
    /// <summary>
    /// Target Field in which to filter.
    /// </summary>
    /// <value>Field Name.</value>
    [BindProperty(Name = "sort-field")]
    public string Field { get; set; }

    /// <summary>
    /// Selected Sort Order.
    /// </summary>
    /// <value>Sort order.</value>
    [BindProperty(Name = "sort-order")]
    public string Order { get; set; }
  }
}
