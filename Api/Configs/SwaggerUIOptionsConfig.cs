using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Api.Configs;

public class SwaggerUIOptionsConfig : IConfigureNamedOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public SwaggerUIOptionsConfig(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(string name, SwaggerUIOptions options)
    {
        Configure(options);
    }

    public void Configure(SwaggerUIOptions options)
    {
        foreach (var groupName in _provider.ApiVersionDescriptions.Select(x => x.GroupName))
        {
            options.SwaggerEndpoint(
                $"/swagger/{groupName}/swagger.json",
                groupName.ToUpperInvariant());
        }
    }
}
