using EmailSwitch.Common;

namespace EmailSwitch.Database.DTOs
{
	public record AttemptDetailsSendOTP(DateTimeOffset AttemptTimeInUTC, EmailProvider EmailProvider, bool SentSuccessfully);
}
