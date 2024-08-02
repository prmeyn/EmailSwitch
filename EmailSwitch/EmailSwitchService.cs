using EmailSwitch.Common;
using EmailSwitch.Common.DTOs;
using EmailSwitch.Database;
using EmailSwitch.Database.DTOs;
using EmailSwitch.Services.SendGrid;
using HumanLanguages;
using Microsoft.Extensions.Logging;
using MongoDbTokenManager.Database;
using SMSwitch.Common.DTOs;
using SMSwitch.Database;

namespace EmailSwitch
{
	public sealed class EmailSwitchService
	{
		private readonly EmailSwitchInitializer _emailSwitchInitializer;
		private readonly SendGridService _sendGridService;
		private readonly EmailSwitchDbService _emailSwitchDbService;
		private readonly MongoDbTokenService _mongoDbTokenService;
		private readonly ILogger<EmailSwitchService> _logger;

		public EmailSwitchService(
			EmailSwitchInitializer emailSwitchInitializer,
			SendGridService sendGridService,
			EmailSwitchDbService emailSwitchDbService,
			MongoDbTokenService mongoDbTokenService,
		ILogger<EmailSwitchService> logger
			)
		{
			_emailSwitchInitializer = emailSwitchInitializer;
			_sendGridService = sendGridService;
			_emailSwitchDbService = emailSwitchDbService;
			_mongoDbTokenService = mongoDbTokenService;
			_logger = logger;
		}

		public async Task<EmailSwitchResponseSendOTP> SendOTP(EmailIdentifier email, MobileNumber[] verifiedMobileNumbers, EmailIdentifier[] verifiedEmails, HashSet<LanguageIsoCode> preferredLanguageIsoCodeList, UserAgent userAgent)
		{
			EmailSwitchResponseSendOTP responseSendOTP = null;
			EmailSwitchSession session = null;
			try 
			{
				session = await _emailSwitchDbService.GetOrCreateAndGetLatestSession(email, verifiedMobileNumbers, verifiedEmails, preferredLanguageIsoCodeList, userAgent);
				Queue<EmailProvider> emailProvidersQueue = null;
				if (session.EmailProvidersQueue?.Any() ?? false)
				{
					emailProvidersQueue = session.EmailProvidersQueue;
				}
				else
				{
					emailProvidersQueue = new();
					HashSet<EmailProvider> emailProviders = _emailSwitchInitializer.EmailControls.Priority;

					for (int i = 0; i < _emailSwitchInitializer.EmailControls.MaxRoundRobinAttempts; i++)
					{
						foreach (EmailProvider emailProvider in emailProviders)
						{
							emailProvidersQueue.Enqueue(emailProvider);
						}
					}
				}

				if (emailProvidersQueue.Count == 0)
				{
					return new EmailSwitchResponseSendOTP()
					{
						IsSent = false
					};
				}

				while (emailProvidersQueue.Any())
				{
					if (session.SentAttempts?.Any() ?? false)
					{
						emailProvidersQueue.Dequeue();
						if (!emailProvidersQueue.Any())
						{
							break;
						}
					}
					responseSendOTP = emailProvidersQueue.Peek() switch
					{
						EmailProvider.SendGrid => await _sendGridService.SendOTP(email, session.SendOTPEmail),
						_ => throw new NotImplementedException(),
					};

					session.SentAttempts.Add(new AttemptDetailsSendOTP(DateTimeOffset.UtcNow, emailProvidersQueue.Peek(), responseSendOTP.IsSent));
					if (responseSendOTP.IsSent)
					{
						break;
					}
				}

				session.EmailProvidersQueue = emailProvidersQueue;
				await _emailSwitchDbService.UpdateSession(session);

				if (responseSendOTP == null || !responseSendOTP.IsSent)
				{
					_logger.LogCritical("Unable to send OTP to {Email} with SessionId: {SessionId}", email, session?.SessionId);
				}
			}
			catch (Exception exception) 
			{
				_logger.LogCritical(exception, "Unable to send OTP to {Email} with SessionId: {SessionId}", email, session?.SessionId);
			}
			return responseSendOTP ?? new EmailSwitchResponseSendOTP() { IsSent = false };
		}

		public async Task<EmailSwitchhResponseVerifyOTP> VerifyOTP(EmailIdentifier email, string OTP)
		{
			var session = await _emailSwitchDbService.GetLatestSession(email);
			var verified = await _mongoDbTokenService.Validate(session.SessionId, token: OTP);
			if (verified)
			{
				await _mongoDbTokenService.Consume(session.SessionId);
			}

			if (session is not null)
			{
				session.FailedVerificationAttemptsDateTimeOffset.Add(DateTimeOffset.UtcNow);
				await _emailSwitchDbService.UpdateSession(session);
			}
			else
			{
				_logger.LogInformation("Session not found: Unable to verify OTP for {Email} with OTP: {OTP}", email, OTP);
			}
			return new EmailSwitchhResponseVerifyOTP()
			{
				Verified = verified
			};
		}
	}
}
