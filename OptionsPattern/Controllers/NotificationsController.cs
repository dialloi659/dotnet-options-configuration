using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OptionsPattern.Models;

namespace OptionsPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly EmailSettings _emailSettingsOptions;
        private readonly EmailSettings _emailSettingsOptionsMonitor;
        private readonly EmailSettings _emailSettingsOptionsSnapshot;

        public NotificationsController(
            IOptions<EmailSettings> emailOptions,
            IOptionsMonitor<EmailSettings> emailOptionsMonitor,
            IOptionsSnapshot<EmailSettings> emailOptionsSnapshot
            ) {

            // IOptions
            _emailSettingsOptions = emailOptions.Value;

            // IOptionsSnapshot
            _emailSettingsOptionsSnapshot = emailOptionsSnapshot.Value;

            // IOptionsMonitor
            _emailSettingsOptionsMonitor = emailOptionsMonitor.CurrentValue;
            emailOptionsMonitor.OnChange(HandleEmailSettingsChanges);
        }


        [HttpGet("email-options")]
        public IActionResult GetEmailOptions() => Ok(Map(_emailSettingsOptions));

        [HttpGet("email-options-monitor")]
        public IActionResult GetEmailOptionsMonitor() => Ok(Map(_emailSettingsOptionsMonitor));

        [HttpGet("email-options-snapshot")]
        public IActionResult GetEmailOptionsSnapShot() => Ok(Map(_emailSettingsOptionsSnapshot));
 

        private static void HandleEmailSettingsChanges(EmailSettings settings)
        {
            Console.WriteLine("Changes occured!");

            // Assuming the From value of EmailSettings has been changed, displays the new value
            Console.WriteLine("From: {0}", settings.From);
        }

        private static EmailSenderData Map(EmailSettings emailSettings)
        {
            return new EmailSenderData
            {
                From = emailSettings.From,
                To = emailSettings.To,
                Cc = emailSettings.Cc,
                Bcc = emailSettings.Bcc
            };
        }
    }
}
