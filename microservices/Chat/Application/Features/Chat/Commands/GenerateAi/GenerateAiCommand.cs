using MediatR;

namespace Application.Features.Chat.Commands.GenerateAi;

public record GenerateAiCommand(string Prompt) : IRequest<string>;