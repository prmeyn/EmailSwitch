using EmailSwitch.Common;
using EmailSwitch.Common.DTOs;
using HumanLanguages;
using SMSwitch.Common.DTOs;

namespace EmailSwitch.Services.SendGrid
{
	public sealed class SendGridService : IServiceEmails
	{
		public Task<EmailSwitchResponseSendOTP> SendOTP(EmailIdentifier email, MobileNumber[] verifiedMobileNumbers, EmailIdentifier[] verifiedEmails, HashSet<LanguageIsoCode> preferredLanguageIsoCodeList, UserAgent userAgent)
		{
			throw new NotImplementedException();
		}

		public Task<EmailSwitchhResponseVerifyOTP> VerifyOTP(EmailIdentifier email, string OTP)
		{
			throw new NotImplementedException();
		}
	}
}
