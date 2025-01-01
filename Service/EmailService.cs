using System.Net;
using System.Net.Mail;
using UserManagementService.Model;

namespace UserManagementService.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailVerificationAsync(User user)
        {
            var verificationUrl = $"http://localhost/verify-email?token={user.EmailVerificationToken}";

            // Create the MailMessage object
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:SenderEmail"], "Hospital Management System"),
                Subject = "Please Verify Your Email Address",
                Body = $"Hello {user.FirstName},\n\nPlease click the link below to verify your email address:\n{verificationUrl}",
                IsBodyHtml = false // Use false for plain text, true for HTML content
            };

            // Add recipient
            mailMessage.To.Add(new MailAddress(user.Email, user.FirstName + " " + user.LastName));

            // Create the SMTP client and configure it
            var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:SenderEmail"],
                    _configuration["EmailSettings:SenderPassword"]
                ),
                EnableSsl = true
            };

            try
            {
                // Send the email
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the exception or handle error
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
            finally
            {
                smtpClient.Dispose();
            }
        }
    }
}
