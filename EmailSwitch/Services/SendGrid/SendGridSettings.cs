using EmailSwitch.Common;

namespace EmailSwitch.Services.SendGrid
{
	public sealed class SendGridSettings : EmailSwitchGeneralSettings
	{
		public required SendGridPrivateSettings SendGridPrivateSettings { get; init; }
	}

	public sealed class SendGridPrivateSettings
	{
		public required string From { get; init; }
		//public required string Host { get; init; }
		//public required int Port { get; init; }
		//public required string Username { get; init; }
		public required string Password { get; init; }
		//public required string SecureSocketOptions { get; init; }
	}
}