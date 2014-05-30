using OpenRasta.Codecs;
using OpenRasta.Configuration;
using reactivedemosite.Adapters.API.Handlers;
using reactivedemosite.Adapters.API.Resources;

namespace reactivedemosite.Adapters.API.Configuration
{
    public class OpenRastaApiConfiguration : IConfigurationSource
    {
        public void Configure()
        {
            using (OpenRastaConfiguration.Manual)
            {
                ResourceSpace.Uses.PipelineContributor<DependencyPipelineContributor>();

                ResourceSpace.Has.ResourcesOfType<CategoryViewModel>()
                    .AtUri("/categories")
                    .HandledBy<CategoriesEndPointHandler>()
                    .TranscodedBy<JsonDataContractCodec>()
                    .ForMediaType("application/json")
                    .ForExtension("js")
                    .ForExtension("json");

                ResourceSpace.Has.ResourcesOfType<CategoryViewModel>()
                    .AtUri("/categories/{id}")
                    .HandledBy<CategoriesEndPointHandler>()
                    .TranscodedBy<JsonDataContractCodec>()
                    .ForMediaType("application/json")
                    .ForExtension("js")
                    .ForExtension("json");
            }
        }
    }
}