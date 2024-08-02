using EmailSwitch.Common.DTOs;
using EmailSwitch.EmailTemplates.DTOs;

namespace EmailSwitch.Common
{
	public interface IServiceEmails
	{
		Task<EmailSwitchResponseSendOTP> SendOTP(EmailIdentifier email, EmailContent emailContent);
	}
}
