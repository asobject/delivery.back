

using Domain.Interfaces;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Infrastructure.Services;

public class OllamaResponseGenerator(
    IHttpClientFactory httpClientFactory,
    IOptions<OllamaSettings> options) : IAiResponseGenerator
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Ollama");
    private readonly OllamaSettings _settings = options.Value;

    public async Task<string> GenerateResponseAsync(string prompt)
    {
        var messages = new List<object>
        {
            new { role = "system", content = GetSystemPrompt() },
            new { role = "user", content = "Где мой заказ?" },
            new { role = "assistant", content = "Отследить посылку можно: 1) В личном кабинете 2) По трек-номеру из Email на главной странице" },
            new { role = "user", content = prompt }
        };

        var request = new
        {
            model = _settings.Model,
            messages,
            stream = false
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/api/chat",
            request
        );

        var result = await response.Content
            .ReadFromJsonAsync<OllamaResponse>();

        return result?.Message.Content ?? string.Empty;
    }

    private static string GetSystemPrompt() => """
        Ты русскоязычный AI-ассистент службы поддержки междугородней доставки грузов. 
        Всегда сохраняй доброжелательный профессиональный тон.
        
        Жёсткие правила:
        1. Отвечай ТОЛЬКО на русском языке
        2. Помогай ТОЛЬКО с вопросами по доставке
        3. Используй точные формулировки из базы знаний
        4. Запрещено упоминать ИИ, алгоритмы или внутренние процессы
        
        Сценарии ответов:
        - Отслеживание: личный кабинет или трек-номер
        - Проблемы: контакт delivery.dev@yandex.ru
        - Оформление: форма на сайте
        """;

    private record OllamaResponse(Message Message);
    private record Message(string Content);
}