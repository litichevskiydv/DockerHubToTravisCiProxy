namespace DockerHubToTravisCiProxy.Controllers
{
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class TravisCiIntegrationOptions
    {
        public string ApiKey { get; set; }

        public string ImageValidatorBranch { get; set; }
    }
}