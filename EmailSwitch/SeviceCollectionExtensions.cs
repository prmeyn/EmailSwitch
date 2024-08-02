using EmailSwitch.Common;
using EmailSwitch.Database;
using EmailSwitch.Services.SendGrid;
using Microsoft.Extensions.DependencyInjection;
using uSignIn.CommonSettings.Settings;

namespace EmailSwitch
{
	public static class SeviceCollectionExtensions
	{
		public static void AddEmailSwitchServices(this IServiceCollection services)
		{
			services.AddSingleton<SettingsService>();
			services.AddSingleton<EmailSwitchInitializer>();
			services.AddSingleton<EmailSwitchGeneralInitializer>();
			services.AddSingleton<EmailSwitchDbService>();

			services.AddSingleton<SendGridInitializer>();
			services.AddScoped<SendGridService>();

			services.AddScoped<EmailSwitchService>();
		}
	}
}
