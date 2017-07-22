namespace DockerHubToTravisCiProxy.Controllers.Models
{
    using JetBrains.Annotations;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PushDataDescription
    {
        public string Tag { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RepositoryDescription
    {
        public string Dockerfile { get; set; }

        public string RepoName { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DockerRequest
    {
        public PushDataDescription PushData { get; set; }

        public RepositoryDescription Repository { get; set; }
    }
}