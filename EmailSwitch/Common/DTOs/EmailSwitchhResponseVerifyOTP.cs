namespace EmailSwitch.Common.DTOs
{
	public sealed class EmailSwitchhResponseVerifyOTP
	{
		public bool Verified { get; init; }
		public bool Expired { get; set; } = false;
	}
}
