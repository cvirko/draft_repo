using Auth.Api.FilterAttributes;
using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;
using Microsoft.Extensions.Options;

namespace Auth.Api.Controllers.Test
{
    [DevOnlyActionFilter]
    [Authorize(AuthConsts.AUTHENTICATION_SCHEME)]
    [ApiVersion(1.0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TestController(IFileBuilder file, IOptionsSnapshot<FilesOptions> option) : ControllerBase
    {
        private readonly IFileBuilder _file = file;
        private readonly FilesOptions _options = option.Value;

        [AllowAnonymous]
        [Route("File/Errors")]
        [HttpGet]
        public FileContentResult FileErrors()
        {
            var errors = _file.GetFromJson(_options.ErrorsTextPath);
            return File(errors.ToByteArray(), MIMEType.Json, "ValidationErrorMessages");
        }
    }
}
