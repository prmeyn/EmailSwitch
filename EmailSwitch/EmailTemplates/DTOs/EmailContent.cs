namespace EmailSwitch.EmailTemplates.DTOs
{
	public sealed class EmailContent
	{
		public string Subject { get; init; }
		public string PlainTextContent { get; init; }
		public string HtmlContent { get; init; }
	}
}
