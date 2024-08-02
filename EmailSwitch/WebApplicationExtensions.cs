using EmailSwitch.Common;
using EmailSwitch.Common.Logo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace EmailSwitch
{
	public static class WebApplicationExtensions
	{
		public static WebApplication AddEmailSwitchApiEndpoints(this WebApplication app)
		{

			app.MapGroup(ConstantStrings.EmailSwitchGroupName)
				.GroupEmailSignatureLogoApisV1()
				.WithTags(ConstantStrings.EmailSwitchTagName);

			return app;
		}
	}
}
