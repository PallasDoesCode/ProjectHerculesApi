using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ScarnManagerApi.Controllers
{
    [ApiController]
    [Route( "[controller]" )]
    public class AboutController : ControllerBase
    {

        private readonly ILogger _logger;

        public AboutController( ILogger<AboutController> logger )
        {
            _logger = logger;
        }

        [HttpGet]
        [Route( "version" )]
        public IActionResult Get()
        {
            var info = new
            {
                Service = new
                {
                    Name = GetShortName(),
                    Version = GetAssemblyVersion()
                }
            };

            _logger.LogDebug( "GetVersion - {info}", info );

            return Ok( info );
        }

        private string GetShortName()
        {
            var name = Assembly.GetCallingAssembly().GetName().Name;
            var lastIndex = name.LastIndexOf( '.' );
            var shortName = name.Substring( lastIndex + 1 );
            return shortName;
        }

        private string GetAssemblyVersion()
        {
            var assemblyName = Assembly.GetCallingAssembly().GetName();
            return assemblyName.Version.ToString();        }
    }
}
