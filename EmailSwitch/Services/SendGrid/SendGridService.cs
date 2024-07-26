using EmailSwitch.Common;
using EmailSwitch.Common.DTOs;
using HumanLanguages;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
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
			try
			{
				var fromEmail = new EmailAddress(_sendGridInitializer.SendGridSettings.SendGridPrivateSettings.From);
				var sendGridMessage = MailHelper.CreateSingleEmail(
							   from: fromEmail,
							   to: new EmailAddress(email.GetRawValue()),
							   subject: "TODO",
							   plainTextContent:"TODO",
							   htmlContent:"<h1>TODO</h1>"
						   );
				sendGridMessage.SetReplyTo(fromEmail);
				var sendEmailRequest = await _sendGridInitializer.SendGridClient.SendEmailAsync(sendGridMessage);
				return await Task.FromResult(new EmailSwitchResponseSendOTP() {
					IsSent = sendEmailRequest.IsSuccessStatusCode,
					OtpLength = _sendGridInitializer.SendGridSettings.OtpLength
				});
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"SendUserInboxVerificationCode error: {e.Message}");
			}
			return await Task.FromResult(new EmailSwitchResponseSendOTP()
			{
				IsSent = false
			});
		}

		public async Task<EmailSwitchhResponseVerifyOTP> VerifyOTP(EmailIdentifier email, string OTP)
		{
			throw new NotImplementedException();
		}
	}
}
