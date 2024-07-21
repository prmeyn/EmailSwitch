namespace EmailSwitch.Common.DTOs
{
	public sealed class EmailSwitchResponseSendOTP
	{
		public bool IsSent { get; set; }
		public byte OtpLength { get; set; }
	}
}