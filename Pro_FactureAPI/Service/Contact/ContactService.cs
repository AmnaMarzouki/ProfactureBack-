namespace Pro_FactureAPI.Service.Contact
{
    using Pro_FactureAPI.Data;
    using Pro_FactureAPI.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;

    public class ContactService : IContactService
    {
        private readonly ProfactureDb _context;

        public ContactService(ProfactureDb context)
        {
            _context = context;
        }

        public Contact AddContactAsync(Models.Contact contact)
        {
            contact.IdContact = Guid.NewGuid();
            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return contact;
        }

        public bool DeleteContactAsync(Guid id)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.IdContact == id);
            if (contact == null)
            {
                return false;
            }

            _context.Contacts.Remove(contact);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Models.Contact> GetAllContactsAsync()
        {
            return _context.Contacts.ToList();
        }

        public Models.Contact GetContactByIdAsync(Guid id)
        {
            return _context.Contacts.FirstOrDefault(c => c.IdContact == id);
        }

        public bool SendReplyEmailAsync(string email, string replyMessage)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("metaloniac@gmail.com", "qxix rhoi crbl vlkr"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("your-email@gmail.com"),
                    Subject = "Reply from Admin",
                    Body = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                color: #333;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: 0 auto;
                                padding: 20px;
                                border: 1px solid #ccc;
                                border-radius: 5px;
                                background-color: #f9f9f9;
                            }}
                            h1 {{
                                color: #db2918;
                            }}
                            p {{
                                margin-bottom: 15px;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h1>Reply from EspritCompass</h1>
                            <p>Hello,</p>
                            <p>Thank you for contacting us. Here is our response to your query:</p>
                            <p style='color: #007bff;'><em>{replyMessage}</em></p>
                            <p>If you have any further questions, feel free to reach out to us.</p>
                            <p>Best regards,<br/>Admin</p>
                        </div>
                    </body>
                    </html>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return false;
            }
        }
    }
}
