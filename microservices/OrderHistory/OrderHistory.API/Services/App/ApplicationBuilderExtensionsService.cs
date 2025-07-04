using BuildingBlocks.Exceptions;

namespace OrderHistory.API.Services.App
{
    public static class ApplicationBuilderExtensionsService
    {
        public static void ConfigurePipeline(this WebApplication app)
        {
            app.UseCors("myCors");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.MapControllers();
        }
    }
}
