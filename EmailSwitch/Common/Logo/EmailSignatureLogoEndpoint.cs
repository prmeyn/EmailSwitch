using EmailSwitch.Services.SendGrid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace EmailSwitch.Common.Logo
{
	public static class EmailSignatureLogoEndpoint
	{

		public const string EmailSignatureLogoRoute = "/logo/";
		public static RouteGroupBuilder GroupEmailSignatureLogoApisV1(this RouteGroupBuilder group)
		{
			group.MapGet(EmailSignatureLogoRoute + "{id}", async (string id,
				HttpContext httpContext,
				SendGridInitializer sendGridInitializer,
				ILogger <RouteGroupBuilder> logger) =>
			{
				try
				{
					await httpContext.Response.Body.WriteAsync(sendGridInitializer.EmailSwitchGeneralSettings.SignatureLogoBytes);
					return Results.Ok();
				}
				catch (Exception ex)
				{
					logger.LogCritical(ex, "Unable to render LOGO in email.");
					return Results.Problem("Unable to render LOGO in email.");
				}
			})
			.Produces(StatusCodes.Status200OK);

			return group;
		}
	}
}
