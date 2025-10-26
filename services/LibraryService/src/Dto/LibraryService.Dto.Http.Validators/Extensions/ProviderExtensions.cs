using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryService.Dto.Http.Validators.Extensions;

public static class ProviderExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidation(fv =>
        {
            fv.DisableDataAnnotationsValidation = true;
            fv.ImplicitlyValidateRootCollectionElements = true;
            fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        });
    }
}