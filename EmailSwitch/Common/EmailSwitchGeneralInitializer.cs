using Microsoft.Extensions.Configuration;

namespace EmailSwitch.Common
{
	public class EmailSwitchGeneralInitializer
	{
		public readonly EmailSwitchGeneralSettings EmailSwitchGeneralSettings;
		public readonly IConfigurationSection EmailSwitchSettings;
		public EmailSwitchGeneralInitializer(IConfiguration configuration)
		{
			EmailSwitchSettings = configuration.GetSection(ConstantStrings.EmailSwitchSettingsName);

			byte defaultLength = 6;
			var otpLength = byte.TryParse(EmailSwitchSettings["OtpLength"], out byte l) ? l : defaultLength;

			EmailSwitchGeneralSettings = new EmailSwitchGeneralSettings()
			{
				OtpLength = otpLength
			};
		}
	}
}
