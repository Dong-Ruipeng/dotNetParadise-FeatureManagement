using Microsoft.FeatureManagement;

namespace dotNetParadise_FeatureManagement.Filters;

public class UserApiFeatureFilter(IFeatureManager featureManager) : FeatureFlagEndpointFilter(featureManager)
{
    protected override string FeatureFlag => "featureUserApi";
}
