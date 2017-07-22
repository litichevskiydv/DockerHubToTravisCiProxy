namespace DockerHubToTravisCiProxy.Controllers
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Flurl.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;

    [Route("[controller]")]
    public class ProxyController : Controller
    {
        private readonly ILogger<ProxyController> _logger;
        private readonly TravisCiIntegrationOptions _integrationOptions;

        public ProxyController(ILogger<ProxyController> logger, IOptionsSnapshot<TravisCiIntegrationOptions> options)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if(options == null)
                throw new ArgumentNullException(nameof(options));

            _logger = logger;
            _integrationOptions = options.Value;
        }

        [HttpPost]
        public async Task Post([FromBody] DockerRequest request)
        {
            var body = @"
{
  ""request"": {
    ""branch"":""<ImageValidatorBranch>"",
    ""config"": {
      ""env"": {
        ""IMAGE_TAG"": ""<ImageTag>""
      }
    }
  }
}"
                .Replace("<ImageValidatorBranch>", _integrationOptions.ImageValidatorBranch)
                .Replace("<ImageTag>", request.PushData.Tag);

            var labelParts = request.Repository.Dockerfile.Split('\n')
                .Single(x => x.StartsWith("LABEL", StringComparison.OrdinalIgnoreCase))
                .Split('/')
                .Select(x => x.Trim('"'))
                .ToArray();

            var response =
                await $"https://api.travis-ci.org/repo/{labelParts[labelParts.Length - 2]}%2F{labelParts[labelParts.Length - 1]}/requests"
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Travis-API-Version", "3")
                    .WithHeader("Authorization", $"token {_integrationOptions.ApiKey}")
                    .PostAsync(new StringContent(body, Encoding.UTF8, "application/json"))
                    .ReceiveJson();

            _logger.LogInformation($"Validation build was started for image {request.Repository.RepoName}:{request.PushData.Tag}");
        }
    }
}
