using EmailSwitch.Common;
using EmailSwitch.Common.DTOs;
using HumanLanguages;
using Microsoft.Extensions.Logging;
using SMSwitch.Common.DTOs;

namespace EmailSwitch.Services.SendGrid
{
	public sealed class SendGridService : IServiceEmails
	{
		private readonly SendGridInitializer _sendGridInitializer;
		private readonly ILogger<SendGridService> _logger;

		public SendGridService(SendGridInitializer sendGridInitializer, ILogger<SendGridService> logger)
		{
			_sendGridInitializer = sendGridInitializer;
			_logger = logger;
		}

		public async Task<EmailSwitchResponseSendOTP> SendOTP(EmailIdentifier email, MobileNumber[] verifiedMobileNumbers, EmailIdentifier[] verifiedEmails, HashSet<LanguageIsoCode> preferredLanguageIsoCodeList, UserAgent userAgent)
		{
			throw new NotImplementedException();
		}

		public async Task<EmailSwitchhResponseVerifyOTP> VerifyOTP(EmailIdentifier email, string OTP)
		{
			throw new NotImplementedException();
		}
	}
}
