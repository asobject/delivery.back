using Application.Features.Chat.Commands.GenerateAi;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[Route("api/chat")]
[ApiController]
public class ChatController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] GenerateAiCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }
}
