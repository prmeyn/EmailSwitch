using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmailSwitch.Common
{
	public class EmailSwitchGeneralInitializer
	{
		public readonly EmailSwitchGeneralSettings EmailSwitchGeneralSettings;
		public readonly IConfigurationSection EmailSwitchSettings;
		public EmailSwitchGeneralInitializer(IConfiguration configuration, ILogger<EmailSwitchGeneralInitializer> logger)
		{
			EmailSwitchSettings = configuration.GetSection(ConstantStrings.EmailSwitchSettingsName);

			byte defaultLength = 6;
			var otpLength = byte.TryParse(EmailSwitchSettings["OtpLength"], out byte l) ? l : defaultLength;

			var signatureLogoPath = EmailSwitchSettings["SignatureLogoPath"];

			if (string.IsNullOrWhiteSpace(signatureLogoPath))
			{
				throw new ArgumentException("Logo not found!");
			}
			byte[] signatureLogoInBytes = [];
			try {
				signatureLogoInBytes = File.ReadAllBytes(signatureLogoPath);
			} catch (Exception ex) {
				logger.LogCritical(ex, "Logo not found!");
			}
			

			EmailSwitchGeneralSettings = new EmailSwitchGeneralSettings()
			{
				OtpLength = otpLength,
				SignatureLogoBytes = signatureLogoInBytes
			};
		}
	}
}
