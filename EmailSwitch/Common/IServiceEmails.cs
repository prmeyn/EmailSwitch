using EmailSwitch.Common.DTOs;
using HumanLanguages;
using SMSwitch.Common.DTOs;

namespace EmailSwitch.Common
{
	public interface IServiceEmails
	{
		Task<EmailSwitchResponseSendOTP> SendOTP(EmailIdentifier email, MobileNumber[] verifiedMobileNumbers, EmailIdentifier[] verifiedEmails, HashSet<LanguageIsoCode> preferredLanguageIsoCodeList, UserAgent userAgent);
		Task<EmailSwitchhResponseVerifyOTP> VerifyOTP(EmailIdentifier email, string OTP);
	}
}
