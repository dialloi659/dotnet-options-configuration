namespace OptionsPattern.Models
{
    public class EmailSenderData
    {
        public string? From { get; set; }
        public string[] To { get; set; } = Array.Empty<string>();
        public string[] Cc { get; set; } = Array.Empty<string>();
        public string[] Bcc { get; set; } = Array.Empty<string>();
    }
}
