namespace OptionsPattern.Models
{
    public sealed class EmailSettings
    {
        public string? From { get; set; }
        public string[] To { get; set; } = Array.Empty<string>();
        public string[] Cc { get; set; } = Array.Empty<string>();
        public string[] Bcc { get; set; } = Array.Empty<string>();
        public string? SenderName { get; set; }
        public string? SmtpServer { get; set; }
        public int? Port { get; set; }
        public string? Password { get; set; }
    }
}
