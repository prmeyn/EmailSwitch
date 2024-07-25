namespace EmailSwitch.Common
{
	public sealed class EmailControls
	{
		public byte MaximumFailedAttemptsToVerify { get; init; }
		public int SessionTimeoutInSeconds { get; init; }
		public byte MaxRoundRobinAttempts { get; set; }
		public HashSet<EmailProvider> Priority { get; set; }
	}
}
