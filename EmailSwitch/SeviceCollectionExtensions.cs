using EmailSwitch.Common;
using EmailSwitch.Database.DTOs;
using EmailSwitch.Services.SendGrid;
using Microsoft.Extensions.DependencyInjection;

namespace EmailSwitch
{
	public static class SeviceCollectionExtensions
	{
		public static void AddEmailSwitchServices(this IServiceCollection services)
		{
			services.AddSingleton<EmailSwitchInitializer>();
			services.AddSingleton<EmailSwitchDbService>();

			services.AddSingleton<SendGridInitializer>();
			services.AddScoped<SendGridService>();

			services.AddScoped<EmailSwitchService>();
		}
	}
}
