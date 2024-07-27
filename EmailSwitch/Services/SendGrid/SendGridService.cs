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
				/*
				  var id = sendOTPRequest.email;
            var validityInSeconds = 600; // 10 mins

            var generatedCode = await _mongoDbTokenService.GenerateCode(
                            logId: typeof(SendOTP).FullName,
                            id: id,
                            relativeUrl: $"/v1{QrEndpoints.RenderQrCodeRoute}",
							validityInSeconds: validityInSeconds,
                            numberOfDigits: OtpCodeLength);

            var qrCodeUri = new Uri(_settingsService.BaseUri, generatedCode.QrCodeRelativeUrl);

            string deepLinkToVerify = $"{generatedCode.Code}";

            deepLinkToVerify = httpRequest.GetDeviceType() switch
            {
                //UserAgent.Android => $"{_settingsService.Android.Scheme}://{_settingsService.Android.Host}/{_settingsService.Android.VerifyEmailPath}/{deepLinkToVerify}",
                //UserAgent.iOS => $"{_settingsService.iOS.Scheme}://{_settingsService.iOS.Host}/{_settingsService.iOS.VerifyEmailPath}/{deepLinkToVerify}",
                _ => deepLinkToVerify,
            };

            var expiryDateTimeOffset = DateTimeOffset.UtcNow.AddSeconds(validityInSeconds).AddSeconds(-15);
            var firstCountryPhoneCodeAndPhoneNumber = sendOTPRequest.verifiedMobileNumbers.First().CountryPhoneCodeAndPhoneNumber;

            Dictionary<string, string> dynamicTemplateData = new() {
                                { "qr_code_url", qrCodeUri.ToString() },
                                { "deep_link_to_verify", deepLinkToVerify},
                                { "code_expirytime_utc", expiryDateTimeOffset.ToString() },
                                { "first_mobile", firstCountryPhoneCodeAndPhoneNumber },
                                { "verified_mobile_numbers", string.Join(",", sendOTPRequest.verifiedMobileNumbers.Where(pn => pn.CountryPhoneCodeAndPhoneNumber != firstCountryPhoneCodeAndPhoneNumber).Select(m => $"+{m.CountryPhoneCodeAsNumericString} {m.PhoneNumberAsNumericString}")) },
                                { "current_email", sendOTPRequest.email },
                                { "emails_owned_visibility", sendOTPRequest.verifiedEmails.Any() ? "" : "hidden" },
                                { "emails_owned", string.Join(",", new List<string>(sendOTPRequest.verifiedEmails)) }
                            };
				 */
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
