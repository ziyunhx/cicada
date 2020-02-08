using Cicada.Configuration.EmailSenderConfigs;
using Cicada.Core.Models;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Mailgun;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Cicada.Services
{
    public class MailgunEmailSender : IEmailSender
    {
        private ISender _client;
        private readonly MailgunConfiguration _configuration;
        private readonly ILogger<MailgunEmailSender> _logger;

        public MailgunEmailSender(ILogger<MailgunEmailSender> logger, ISender client, MailgunConfiguration configuration)
        {
            _logger = logger;
            _client = client;
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Email.DefaultSender = _client;

            var msg = Email.From(_configuration.SourceEmail, _configuration.SourceName)
                .To(email)
                .Subject(subject)
                .Body(htmlMessage);

            var response = await msg.SendAsync();

            if (response.Successful)
            {
                _logger.LogInformation($"Email: {email}, subject: {subject}, message: {htmlMessage} successfully sent");
            }
            else
            {
                var errorMessage = response.ErrorMessages == null ? "" : string.Join(',', response.ErrorMessages);

                _logger.LogError($"Response with message id {response.MessageId} and body {errorMessage} after sending email: {email}, subject: {subject}");
            }
        }
    }
}
