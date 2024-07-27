using EmailSwitch.Common;
using EmailSwitch.Common.DTOs;
using EmailSwitch.EmailTemplates.DTOs;
using HumanLanguages;
using Microsoft.Extensions.Logging;
using MongoDbTokenManager.Database;
using SendGrid.Helpers.Mail;
using SMSwitch.Common.DTOs;

namespace EmailSwitch.Services.SendGrid
{
	public sealed class SendGridService : IServiceEmails
	{
		private readonly SendGridInitializer _sendGridInitializer;
		private readonly ILogger<SendGridService> _logger;
		private readonly MongoDbTokenService _mongoDbTokenService;

		public SendGridService(
			SendGridInitializer sendGridInitializer,
			ILogger<SendGridService> logger,
			MongoDbTokenService mongoDbTokenService)
		{
			_sendGridInitializer = sendGridInitializer;
			_logger = logger;
			_mongoDbTokenService = mongoDbTokenService;
		}

		public async Task<EmailSwitchResponseSendOTP> SendOTP(EmailIdentifier email, MobileNumber[] verifiedMobileNumbers, EmailIdentifier[] verifiedEmails, HashSet<LanguageIsoCode> preferredLanguageIsoCodeList, UserAgent userAgent)
		{
			try
			{
				var fromEmail = new EmailAddress(_sendGridInitializer.SendGridSettings.SendGridPrivateSettings.From);
				var validityInSeconds = 600; // 10 mins

				var generatedCode = await _mongoDbTokenService.Generate(
								logId: typeof(SendGridService).FullName,
								id: email.ToString(),
								validityInSeconds: validityInSeconds,
								numberOfDigits: _sendGridInitializer.SendGridSettings.OtpLength);
				EmailContent sendOTPEmail = EmailTemplates.TemplateCreator.CreateSendOTPEmail(email, verifiedMobileNumbers, verifiedEmails,preferredLanguageIsoCodeList,userAgent, generatedCode);

				var sendGridMessage = MailHelper.CreateSingleEmail(
							   from: fromEmail,
							   to: new EmailAddress(email.GetRawValue()),
							   subject: sendOTPEmail.Subject,
							   plainTextContent: sendOTPEmail.PlainTextContent,
							   htmlContent: sendOTPEmail.HtmlContent
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
