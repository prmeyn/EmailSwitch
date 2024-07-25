using EmailSwitch.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmailSwitch.Services.SendGrid
{
	public sealed class SendGridInitializer: EmailSwitchGeneralInitializer
	{
		internal readonly SendGridSettings SendGridSettings;

		public SendGridInitializer(
			IConfiguration configuration,
			ILogger<SendGridInitializer> logger) : base(configuration)
		{
			try
			{

				var sendGridConfig = EmailSwitchSettings.GetSection(EmailProvider.SendGrid.ToString());

				SendGridSettings = new SendGridSettings()
				{
					OtpLength = EmailSwitchGeneralSettings.OtpLength,
					SendGridPrivateSettings = new SendGridPrivateSettings()
					{
						From = sendGridConfig["From"],
						Host = sendGridConfig["Host"],
						Username = sendGridConfig["Username"],
						Password = sendGridConfig["Password"],
						SecureSocketOptions = sendGridConfig["SecureSocketOptions"],
						Port = int.TryParse(sendGridConfig["Port"], out int portNumber) ? portNumber : 587,

					}
				};
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Unable to initialize SendGrid");
			}
		}
	}
}
