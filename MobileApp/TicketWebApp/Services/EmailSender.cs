using MailKit.Net.Smtp;
using QRCoder;
using System.Net;
using System.Net.Mail;
namespace TicketWebApp.Services;

partial class EmailSender
{

    private string secretSender { get; set; }
    private string fromEmail { get; set; }
    private ILogger<EmailSender> logger;

    public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
    {
        secretSender = config["DustySecret"] ?? throw new Exception("Missing dusty email config");
        fromEmail = config["DustysEmail"] ?? throw new Exception("Missing dusty email password config");
        this.logger = logger;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Email Service: {Description}")]
    static partial void LogInformationMessage(ILogger logger, string description);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Email Service: {Description}")]
    static partial void LogWarningMessage(ILogger logger, string description);

    public string sendEmail(MailAddress ReceiverEmail,
                            Guid ticketId)
    {
        try
        {
            var from = new MailAddress(fromEmail, "TicketsRUs");
            var to = ReceiverEmail;
            var message = new MailMessage(from, to);
            
            if(ReceiverEmail.DisplayName.Contains("ethan") || ReceiverEmail.User.Contains("ethan") || ReceiverEmail.Address.Contains("ethan"))
            {
                LogWarningMessage(logger, "Another Ethan has purchaed a ticket");
            }
            //qrcode generation
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(ticketId.ToString(), QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] image = qrCode.GetGraphic(10);//an image
            Stream stream = new MemoryStream(image);
            var qrAttachment = new Attachment(stream, "qrCode.png");



            string body = $"<img src=\"cid:{qrAttachment.ContentId}\" />";
            message.Attachments.Add(qrAttachment);


            message.Subject = "no-reply Event Ticket QR Code";
            message.IsBodyHtml = true;
            message.Body = @$"
                <html><body>
                <h1>Event Ticket</h1>
                <p>Bring your ticket to the event to scan in. Enjoy the show!</p>
                {body}
                </body></html>";


            using (var client = new System.Net.Mail.SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587;
                client.EnableSsl = true;


                // Note: only needed if the SMTP server requires authentication
                client.Credentials = new NetworkCredential(fromEmail, secretSender);

                client.Send(message);
            }
            LogInformationMessage(logger, "An email was successfully sent");
            return "Email Sent";
        }
        catch
        {
            LogWarningMessage(logger, "An email was supposed to send but it didn't work");
            return "Bad Exception Happend";
        }
    }
}