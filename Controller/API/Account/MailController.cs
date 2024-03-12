﻿using FICCI_API.DTO.Account;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace FICCI_API.Controller.API.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : BaseController
    {
        private readonly IConfiguration _config;
        private MailConfig _mailConfig;
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        private readonly MySettings _mySettings;
    
        public MailController(FICCI_DB_APPLICATIONSContext dbContext, IConfiguration config, IOptions<MySettings> mySettings) : base(dbContext)
        {
            _dbContext = dbContext;
            _config = config;
            _mailConfig = GetMailConfig();
            _mySettings = mySettings.Value;

        }
        private MailConfig GetMailConfig()
        {
            return new MailConfig
            {
                SenderEmail = "",
                SenderPassword ="",
                Port = 587,
                Host ="smtp.teamcomputers.com",
                SslEnabled =true
            };
        }
        [HttpPost]
        public async Task<IActionResult> InvoiceAccountMail([FromForm] MailDTO mail)
        {
            try
            {
                // mail.ResourceType = "Customer";
                if (mail == null || mail.ResourceId < 0)
                {
                    return BadRequest(new { status = false, message = "Invalid Data" });
                }


                var emailAttachments = new List<Attachment>();
                
                
                var resu = MailMethod(mail.MailTo, mail.MailCC, mail.MailSubject, mail.MailBody, "Invoice", mail.LoginId, mail.ResourceId, mail.Attachments,3);
                
                if (resu == true)
                {
                    _dbContext.Database.ExecuteSqlRaw($"Update FICCI_IMPI_HEADER set Header_StatusId = 14 where IMPI_HEADER_ID = {mail.ResourceId}");
                    var response = new
                    {
                        status = true,
                        message = "Mail sent successfully",
                    };
                    return StatusCode(200, response);
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        message = "Mail sent unsuccessfully",
                    };
                    return StatusCode(200, response);
                }

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred." });

            }
        }

        [HttpGet("LastestEmail")]
        public async Task<IActionResult> LastestEmail(int invoiceId)
        {
            try
            {
               var resu = await _dbContext.FicciImmds.Where(x => x.ResourceId == invoiceId && x.ResourceTypeId == 3 && x.ImmdActive != false).OrderByDescending(x => x.ImmdMailSentOn).FirstOrDefaultAsync();
               // resu.ImpiHeaderAttachment = _dbContext.FicciImads.Where(x => x.ImadActive != false && x.ResourceId == k.ImpiHeaderId).ToList();
               
                if (resu == null)
                {
                    var resp = new
                    {
                        status = false,
                        message = "Cannot find Mail details",
                        data = resu
                    };
                    return StatusCode(200, resp);
                }
                var result = await _dbContext.FicciImads.Where(x => x.ResourceId == resu.ImmdId && x.ResourceTypeId == 3).ToListAsync();

                var response = new
                {
                    status = true,
                    message = "Mail details fetch successfully",
                    data = resu,
                    attachment = result
                };
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [NonAction]
        public bool MailMethod(string mailTo, string mailCC, string mailSubject, string mailBody, string? ResourceType, string LoginId, int? resourceId,List<IFormFile>? emailAttachment,int ResourceTypeId)
        {
            try
            {
                if (mailTo == null || mailSubject == null || mailBody == null)
                {
                    return false;
                }
                bool isEmailSent = SendEmailData(mailTo, mailCC, mailSubject, mailBody, _mySettings, emailAttachment);

                FicciImmd immd = new FicciImmd();
                immd.ImmdMailTo = mailTo;
                immd.ImmdMailCc = mailCC;
                immd.ImmdMailSubject = mailSubject;
                immd.ImmdMailBody = mailBody;
                immd.ImmdActive = true;
                immd.ResourceType = ResourceType;
                immd.ImmdCreatedBy = LoginId;
                immd.IsSent = isEmailSent;
                immd.ResourceId = resourceId;
                immd.ImmdMailSentOn = DateTime.Now;
                immd.ImmdCreatedOn = DateTime.Now;
                immd.ResourceTypeId = ResourceTypeId;
                _dbContext.Add(immd);
                _dbContext.SaveChanges();
                int returnid = immd.ImmdId;
                if(returnid != null)
                {
                    UploadFile1(emailAttachment, LoginId, returnid, "Invoice_Mail_Attachment", 3, "Invoice_Header_Mail", "Invoice_Account_Setting");
                }
                return isEmailSent;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[HttpGet]
        //public bool SendMail()
        //{
        //    // Sender's email address and password
        //    string senderEmail = "";
        //    string senderPassword = "";

        //    // Recipient's email address
        //    string recipientEmail = "";

        //    // Create a new MailMessage
        //    MailMessage mail = new MailMessage(senderEmail, recipientEmail);
        //    mail.Subject = "Subject of the email";
        //    mail.Body = "Body of the email";

        //    // Create a new SmtpClient
        //    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        //    smtpClient.Port = 587;
        //    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
        //    smtpClient.EnableSsl = true;

        //    try
        //    {
        //        // Send the email
        //        smtpClient.Send(mail);
        //        Console.WriteLine("Email sent successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }
        //    return true;
        //}
        //public bool SendMail(List<string> liReceiver, string strSubject, string strBody, out string sentMsg,List<string> ccEmails = null)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(_mailConfig.SenderEmail)
        //           && !string.IsNullOrEmpty(_mailConfig.SenderPassword)
        //           && !string.IsNullOrEmpty(_mailConfig.Host))
        //        {
        //            MailMessage message = new MailMessage();
        //            SmtpClient smtp = new SmtpClient();
        //            message.From = new MailAddress(_mailConfig.SenderEmail);
        //            foreach (var s in liReceiver)
        //            {
        //                message.To.Add(new MailAddress(s));
        //            }
        //            message.Subject = strSubject;
        //            message.IsBodyHtml = true;
        //            message.Body = strBody;

        //            if (ccEmails != null && ccEmails.Count > 0)
        //            {
        //                foreach (var item in ccEmails)
        //                {
        //                    message.CC.Add(new MailAddress(item.Trim()));
        //                }
        //            }
        //            smtp.Port = _mailConfig.Port;
        //            smtp.Host = _mailConfig.Host;
        //            smtp.EnableSsl = _mailConfig.SslEnabled;
        //            smtp.UseDefaultCredentials = true;
        //            smtp.Credentials = new NetworkCredential(_mailConfig.SenderEmail, _mailConfig.SenderPassword);
        //            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            smtp.Send(message);
        //            sentMsg = "Sent Success";
        //            return true;
        //        }
        //        else
        //        {
        //            sentMsg = "No mail setting found in app.config";
        //            return false;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        sentMsg = e.Message;
        //        return false;
        //    }
        //}


        //public async Task<IActionResult> GetResult(int customerid)
        //{
        //    try
        //    {
        //        var result = await _dbContext.FicciErpCustomerDetails.Where(x => x.CustomerId == customerid).FirstOrDefaultAsync();

        //        string name = result.CustomerName;
        //        string customerNo = result.CusotmerNo;
        //        string GST = result.CustomerGstNo;
        //        string Pan = result.CustomerPanNo;
        //        string city = _dbContext.Cities.Where(x => x.CityId == result.CustomerCity).Select(x => x.CityName).FirstOrDefault();
        //        string status = _dbContext.StatusMasters.Where(s => s.StatusId == result.CustomerStatus).Select(a => a.StatusName).FirstOrDefault();
        //       // SendMail(name, customerNo, GST, Pan, city, status);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

    }
}
