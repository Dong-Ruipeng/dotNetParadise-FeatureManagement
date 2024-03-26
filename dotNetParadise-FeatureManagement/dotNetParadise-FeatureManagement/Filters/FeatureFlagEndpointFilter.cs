using Microsoft.FeatureManagement;

namespace dotNetParadise_FeatureManagement.Filters;


public abstract class FeatureFlagEndpointFilter(IFeatureManager featureManager) : IEndpointFilter
{
    protected abstract string FeatureFlag { get; }

    private readonly IFeatureManager _featureManager = featureManager;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var isEnabled = await _featureManager.IsEnabledAsync(FeatureFlag);
        if (!isEnabled)
        {
            return TypedResults.NotFound();
        }
        return await next(context);
    }
}

