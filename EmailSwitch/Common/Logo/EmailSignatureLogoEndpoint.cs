using EmailSwitch.Database;
using EmailSwitch.Services.SendGrid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace EmailSwitch.Common.Logo
{
	public static class EmailSignatureLogoEndpoint
	{
		public static string EmailSignatureLogoRelativeUrl(string id) => $"{ConstantStrings.EmailSwitchGroupName}{EmailSignatureLogoRoute}{id}";

		public const string EmailSignatureLogoRoute = "/logo/";
		public static RouteGroupBuilder GroupEmailSignatureLogoApisV1(this RouteGroupBuilder group)
		{
			group.MapGet(EmailSignatureLogoRoute + "{id}", async (string id,
				HttpContext httpContext,
				SendGridInitializer sendGridInitializer,
				EmailSwitchDbService emailSwitchDbService,
				ILogger <RouteGroupBuilder> logger) =>
			{
				try
				{
					_ = emailSwitchDbService.RegisterRenderRequest(id);
					await httpContext.Response.Body.WriteAsync(sendGridInitializer.EmailSwitchGeneralSettings.SignatureLogoBytes);
				}
				catch (Exception ex)
				{
					logger.LogCritical(ex, "Unable to render LOGO in email :{ID}.", id);
				}
			})
			.Produces(StatusCodes.Status200OK);

			return group;
		}
	}
}
