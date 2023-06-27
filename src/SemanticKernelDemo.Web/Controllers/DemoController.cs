using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;

using SemanticKernelDemo.Web.Controllers.Models;

namespace SemanticKernelDemo.Web.Controllers;

[ApiController]
[Produces(@"application/json")]
[Route(@"api/[controller]")]
public class DemoController : ControllerBase
{
    private readonly IKernel kernel;

    public DemoController(IKernel kernel)
    {
        this.kernel = kernel;
    }

    [HttpPost(@"summarize")]
    public async Task<IActionResult> Summarize([FromBody] SummarizeRequest request, CancellationToken cancellationToken)
    {
        var variables = new ContextVariables();
        variables.Set(@"input", request.Input);

        var context = await kernel.RunAsync(variables, cancellationToken, kernel.Skills.GetFunction(@"SummarizeSkill", @"Summarize"));

        return context.ErrorOccurred
                ? Problem(context.LastErrorDescription)
                : Ok(context.Result.Trim());
    }

    [HttpPost(@"ask")]
    public async Task<IActionResult> Ask([FromBody] AskRequest request, CancellationToken cancellationToken)
    {
        var variables = new ContextVariables();
        variables.Set(@"context", request.Context);
        variables.Set(@"input", request.Question);

        var context = await kernel.RunAsync(variables, cancellationToken, kernel.Skills.GetFunction(@"AskSkill", @"AnswerQuestion"));

        return context.ErrorOccurred
                ? Problem(context.LastErrorDescription)
                : Ok(context.Result.Trim());
    }
}
