

using Domain.Interfaces;
using MediatR;

namespace Application.Features.Chat.Commands.GenerateAi;

public class GenerateACommandHandler(IAiResponseGenerator aiGenerator)
        : IRequestHandler<GenerateAiCommand, string>
{
    public async Task<string> Handle(
        GenerateAiCommand request,
        CancellationToken cancellationToken
    ) => await aiGenerator.GenerateResponseAsync(request.Prompt);
}
