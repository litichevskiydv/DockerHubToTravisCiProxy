namespace DockerHubToTravisCiProxy.Controllers.Models
{
    public class PushDataDescription
    {
        public string tag;
    }

    public class RepositoryDescription
    {
        public string dockerfile;
    }

    public class DockerRequest
    {
        public PushDataDescription push_data { get; set; }

        public RepositoryDescription repository { get; set; }
    }
}