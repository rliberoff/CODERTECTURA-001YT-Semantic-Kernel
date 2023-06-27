using System.ComponentModel.DataAnnotations;

namespace SemanticKernelDemo.Web.Controllers.Models;

public class SummarizeRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Input { get; init; }
}
