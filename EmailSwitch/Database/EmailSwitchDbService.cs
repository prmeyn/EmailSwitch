using EmailSwitch.Common;
using EmailSwitch.Common.DTOs;
using EmailSwitch.Common.Logo;
using EmailSwitch.Database.DTOs;
using HumanLanguages;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDbService;
using MongoDbTokenManager.Database;
using SMSwitch.Common.DTOs;
using uSignIn.CommonSettings.Settings;

namespace EmailSwitch.Database
{
	public sealed class EmailSwitchDbService
	{
		private IMongoCollection<EmailSwitchSession> _emailSwitchSessionCollection;
		private EmailSwitchInitializer _emailSwitchInitializer;
		private MongoDbTokenService _mongoDbTokenService;
		private EmailSwitchGeneralInitializer _emailSwitchGeneralInitializer;
		private readonly SettingsService _settingsService;
		private readonly ILogger<EmailSwitchDbService> _logger;

		public EmailSwitchDbService(
			MongoService mongoService,
			EmailSwitchInitializer emailSwitchInitializer,
			MongoDbTokenService mongoDbTokenService,
			EmailSwitchGeneralInitializer emailSwitchGeneralInitializer,
			SettingsService settingsService,
			ILogger<EmailSwitchDbService> logger)
		{
			_emailSwitchSessionCollection = mongoService.Database.GetCollection<EmailSwitchSession>(nameof(EmailSwitchSession), new MongoCollectionSettings() { ReadConcern = ReadConcern.Majority, WriteConcern = WriteConcern.WMajority });

			// Create an index on CountryPhoneCodeAndPhoneNumber
			var indexKeys = Builders<EmailSwitchSession>.IndexKeys.Ascending(x => x.EmailId);
			var indexModel = new CreateIndexModel<EmailSwitchSession>(indexKeys);

			_emailSwitchInitializer = emailSwitchInitializer;
			_mongoDbTokenService = mongoDbTokenService;
			_emailSwitchGeneralInitializer = emailSwitchGeneralInitializer;
			_settingsService = settingsService;
			_logger = logger;
		}

		internal async Task<EmailSwitchSession?> GetOrCreateAndGetLatestSession(EmailIdentifier emailPendingVerification, MobileNumber[] verifiedMobileNumbers, EmailIdentifier[] verifiedEmails, HashSet<LanguageIsoCode> preferredLanguageIsoCodeList, UserAgent userAgent)
		{
			var latestSession = await GetLatestSession(emailPendingVerification);
			if (latestSession != null)
			{
				return latestSession;
			}
			latestSession = new EmailSwitchSession() 
			{
				SessionId = Guid.NewGuid().ToString(),
				EmailId = emailPendingVerification.ToString(),
				StartTimeUTC = DateTimeOffset.UtcNow,
				ExpiryTimeUTC = DateTimeOffset.UtcNow.AddSeconds(_emailSwitchInitializer.EmailControls.SessionTimeoutInSeconds)
			};

			var generatedCode = await _mongoDbTokenService.Generate(
								logId: typeof(EmailSwitchDbService).FullName,
								id: latestSession.SessionId,
								validityInSeconds: _emailSwitchInitializer.EmailControls.SessionTimeoutInSeconds,
								numberOfDigits: _emailSwitchGeneralInitializer.EmailSwitchGeneralSettings.OtpLength);

			latestSession.SendOTPEmail = EmailTemplates.TemplateCreator.CreateSendOTPEmail(
				emailPendingVerification: emailPendingVerification,
				verifiedMobileNumbers: verifiedMobileNumbers,
				verifiedEmails: verifiedEmails,
				preferredLanguageIsoCodeList: preferredLanguageIsoCodeList,
				userAgent: userAgent,
				generatedCode: generatedCode,
				expiryDateTimeOffset: DateTimeOffset.UtcNow.AddSeconds(_emailSwitchInitializer.EmailControls.SessionTimeoutInSeconds - 10),
				signatureLogoUri: new Uri(_settingsService.BaseUri, EmailSignatureLogoEndpoint.EmailSignatureLogoRelativeUrl(latestSession.SessionId)));

			await _emailSwitchSessionCollection.InsertOneAsync(latestSession);

			return latestSession;
		}

		internal async Task<EmailSwitchSession?> GetLatestSession(EmailIdentifier emailPendingVerification)
		{
			var allRecords = _emailSwitchSessionCollection.Find(Filter(emailPendingVerification));

			if (allRecords?.Any() ?? false)
			{
				return await Task.FromResult(allRecords.ToList().Where(r => r.HasNotExpired(_emailSwitchInitializer.EmailControls.MaximumFailedAttemptsToVerify))?
				.OrderByDescending(record => record.ExpiryTimeUTC)?
				.FirstOrDefault());
			}
			return null;
		}

		private FilterDefinition<EmailSwitchSession> Filter(EmailIdentifier emailPendingVerification) => Builders<EmailSwitchSession>.Filter.Eq(t => t.EmailId, emailPendingVerification.ToString());

		internal async Task UpdateSession(EmailSwitchSession session)
		{
			var options = new ReplaceOptions { IsUpsert = true };
			await _emailSwitchSessionCollection.ReplaceOneAsync(Filter(session.SessionId), session, options);
		}

		internal async Task RegisterRenderRequest(string id)
		{
			var session = _emailSwitchSessionCollection.Find(Builders<EmailSwitchSession>.Filter.Eq(t => t.SessionId, id))?.FirstOrDefault();
			if (session is not null)
			{
				session.LogoRenderedAttemptsDateTimeOffset.Add(DateTimeOffset.UtcNow);
				await UpdateSession(session);
			}
		}
	}
}
