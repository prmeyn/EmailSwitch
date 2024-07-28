using EmailSwitch.Common.DTOs;
using EmailSwitch.EmailTemplates.DTOs;
using EmailSwitch.Translations;
using HumanLanguages;
using SMSwitch.Common.DTOs;

namespace EmailSwitch.EmailTemplates
{
	public static class TemplateCreator
	{
		internal static EmailContent CreateSendOTPEmail(EmailIdentifier emailPendingVerification, MobileNumber[] verifiedMobileNumbers, EmailIdentifier[] verifiedEmails, HashSet<LanguageIsoCode> preferredLanguageIsoCodeList, UserAgent userAgent, string generatedCode, DateTimeOffset expiryDateTimeOffset)
		{
			var verifiedMobileNumberStrings = verifiedMobileNumbers.Select(x => $"+{x.CountryPhoneCode} {x.PhoneNumberAsNumericString}").ToList();
			var verifiedEmailStrings = verifiedEmails.Select(x => x.GetRawValue()).ToList();

			var preferredLanguageIsoCode = preferredLanguageIsoCodeList.First();

			var expiryTime = expiryDateTimeOffset.ToString("yyyy/MM/dd/ hh:mm:ss tt");

			return new EmailContent
			{
				Subject = TranslationKey.SendOTPEmailSubject.GetTranslation(preferredLanguageIsoCode).First(),
				PlainTextContent = $"Verification Code: {generatedCode}\n" +
								   $"Expiry Time: {expiryTime}\n" +
								   $"Verified Mobile Numbers: {string.Join(", ", verifiedMobileNumberStrings)}\n" +
								   $"Verified Emails: {string.Join(", ", verifiedEmailStrings)}",
				HtmlContent = $"<h1>Verification Code for {emailPendingVerification.GetRawValue()}: {generatedCode}</h1>" +
							  $"<p>Expiry Time: {expiryTime}</p>" +
							  $"<p>Verified Mobile Numbers: {string.Join(", ", verifiedMobileNumberStrings)}</p>" +
							  (verifiedEmailStrings.Any() ? $"<p>Verified Emails: {string.Join(", ", verifiedEmailStrings)}</p>" : "")
			};
		}
	}
}
