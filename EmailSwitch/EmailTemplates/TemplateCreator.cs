using EmailSwitch.Common.DTOs;
using EmailSwitch.EmailTemplates.DTOs;
using HumanLanguages;
using SMSwitch.Common.DTOs;

namespace EmailSwitch.EmailTemplates
{
	public static class TemplateCreator
	{
		internal static EmailContent CreateSendOTPEmail(EmailIdentifier email, MobileNumber[] verifiedMobileNumbers, EmailIdentifier[] verifiedEmails, HashSet<LanguageIsoCode> preferredLanguageIsoCodeList, UserAgent userAgent, string generatedCode)
		{
			return new EmailContent {
				Subject = "TODO",
				PlainTextContent = "TODO",
				HtmlContent = "<H1>TODO</H1>"
			};
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
		}
	}
}
