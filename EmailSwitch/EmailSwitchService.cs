using EmailSwitch.Common;
using EmailSwitch.Common.DTOs;
using EmailSwitch.Services.SendGrid;
using HumanLanguages;
using SMSwitch.Common.DTOs;

namespace EmailSwitch
{
	public sealed class EmailSwitchService : IServiceEmails
	{
		private readonly EmailSwitchInitializer _emailSwitchInitializer;

		private readonly SendGridService _sendGridService;

		public EmailSwitchService(EmailSwitchInitializer emailSwitchInitializer, SendGridService sendGridService)
		{
			_emailSwitchInitializer = emailSwitchInitializer;
			_sendGridService = sendGridService;
		}

		public async Task<EmailSwitchResponseSendOTP> SendOTP(EmailIdentifier email, MobileNumber[] verifiedMobileNumbers, EmailIdentifier[] verifiedEmails, HashSet<LanguageIsoCode> preferredLanguageIsoCodeList, UserAgent userAgent)
		{
			return await _sendGridService.SendOTP(email, verifiedMobileNumbers, verifiedEmails, preferredLanguageIsoCodeList, userAgent);
		}

		public async Task<EmailSwitchhResponseVerifyOTP> VerifyOTP(EmailIdentifier email, string OTP)
		{
			return await _sendGridService.VerifyOTP(email, OTP);
		}
	}
}
