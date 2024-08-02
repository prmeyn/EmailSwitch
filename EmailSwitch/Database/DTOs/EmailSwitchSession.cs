using EmailSwitch.Common;
using EmailSwitch.EmailTemplates.DTOs;
using MongoDB.Bson.Serialization.Attributes;

namespace EmailSwitch.Database.DTOs
{
	public sealed class EmailSwitchSession
	{
		[BsonId]
		public required string SessionId { get; init; }
		public required string EmailId { get; init; }
		public required DateTimeOffset StartTimeUTC { get; init; }
		public DateTimeOffset? SuccessfullyVerifiedTimestampUTC { get; set; }
		public required DateTimeOffset ExpiryTimeUTC { get; init; }
		public Queue<EmailProvider>? EmailProvidersQueue { get; set; }
		public List<AttemptDetailsSendOTP> SentAttempts { get; set; } = [];
		public List<DateTimeOffset> FailedVerificationAttemptsDateTimeOffset { get; set; } = [];
		public EmailContent SendOTPEmail { get; set; }

		internal bool HasNotExpired(byte maximumFailedAttemptsToVerify) =>
			(EmailProvidersQueue?.Any() ?? true) && // if it has become empty from failed attempts then it has expired
			FailedVerificationAttemptsDateTimeOffset.Count() < maximumFailedAttemptsToVerify &&
			SuccessfullyVerifiedTimestampUTC == null &&
			DateTimeOffset.UtcNow < ExpiryTimeUTC;
	}
}
