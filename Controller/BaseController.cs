
using Microsoft.AspNetCore.Mvc;
using FICCI_API.ModelsEF;
using FICCI_API.Models;

using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.PortableExecutable;
using FICCI_API.DTO;
using System.IO;

namespace FICCI_API.Controller
{
    public class BaseController : ControllerBase
    {
      public readonly FICCI_DB_APPLICATIONSContext _context;
       
        public BaseController(FICCI_DB_APPLICATIONSContext context)
        {
            this._context = context;

        }


     
        [NonAction]
        public string AssignhtmlBody(string EmailLink, string custName, string CityCode, string PAN, string GST, string ContactPerson, string? PhoneNo)
        {
            //string template = $@"Dear User,
            //                    <br/>Following Customer has been assigned in the Invoice portal for your necessary approval.
            //                   <br/><strong>Customer No:</strong> {customerNo}
            //                   <br/><strong>Customer Name:</strong> {custName}
            //                   <br/><strong>Customer City:</strong> {CityCode}
            //                   <br/><strong>Customer PAN No:</strong>{PAN}
            //                   <br/><strong>Customer GST No:</strong>{GST}
            //                  <br/>To Access Invoice Portal: <a href='{EmailLink}' class='cta-button'>Click Here</a>
            //                  <br/>Note:To open the Invoice portal, please open it in Microsoft Edge or In Google Chrome
            //                  <br/><br/><br/>Regards
            //                  <br/>FICCI Team";
            string template = $@"<p style=""font-family: Arial, sans-serif; font-size: 13px;"">Dear User,<br><br>Following Customer has been assigned in the Invoice portal for your necessary approval.<br><br><strong>Customer Name</strong>   : {custName} <br><strong>Customer City</strong> : {CityCode}<br> <strong>Customer PAN No</strong>. : {PAN}<br><strong>Customer GST No</strong>. : {GST}<br><strong>Customer Contact Person</strong>. : {ContactPerson}<br><strong>Customer Phone No.</strong>. : {PhoneNo}<br><br>To Access Invoice Portal : <a href='{EmailLink}'>Click Here</a><br><strong>Note</strong>: To open the Invoice portal, please open it in Microsoft Edge or In Google Chrome Regards <strong>FICCI Team</strong><br><br><strong>Regards</strong><br>FICCI Team";

            return template;

        }

        [NonAction]
        public string ApprovalhtmlBody(string header, string EmailLink, string custName, string CityCode, string PAN, string GST, string ContactPerson, string? PhoneNo)
        {
            //string template = $@"Dear User,</br>Following Customer has been  {header} in the Invoice portal:
            //                   <br/><strong>Customer No:</strong> {customerNo}
            //                   <br/><strong>Customer Name:</strong> {custName}
            //                   <br/><strong>Customer City:</strong> {CityCode}
            //                   <br/><strong>Customer PAN No:</strong>{PAN}
            //                   <br/><strong>Customer GST No:</strong>{GST}
            //                  <br/>To Access Invoice Portal: <a href='{EmailLink}' class='cta-button'>Click Here</a>
            //                  <br/>Note:To open the Invoice portal, please open it in Microsoft Edge or In Google Chrome
            //                  <br/><br/>Regards
            //                  <br/>FICCI Team";

            string template = $@"<p style=""font-family: Arial, sans-serif; font-size: 13px;"">Dear User,<br><br>Following Customer has been {header} in the Invoice Portal:<br><br><strong>Customer Name</strong>   : {custName} <br><strong>Customer City</strong> : {CityCode}<br> <strong>Customer PAN No</strong>. : {PAN}<br><strong>Customer GST No</strong>. : {GST}<br><strong>Customer Contact Person</strong>. : {ContactPerson}<br><strong>Customer Phone No.</strong>. : {PhoneNo}<br><br>To Access Invoice Portal : <a href='{EmailLink}'>Click Here</a><br><strong>Note</strong>: To open the Invoice portal, please open it in Microsoft Edge or In Google Chrome Regards <strong>FICCI Team</strong><br><br><strong>Regards</strong><br>FICCI Team";
            return template;

        }



        [NonAction]
        public void SendEmail(string MailTo, string MailCC, string MailSubject, string MailBody, MySettings? _mySettings)
        {
            if (Convert.ToBoolean(_mySettings.MailFlag))
            {
                try
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(_mySettings.Username, _mySettings.MailFrom);


                        mail.To.Add("nikhil.vig@teamcomputers.com");

                        //string ccEmail = _mySettings.MailCc;
                        //if (!string.IsNullOrEmpty(ccEmail))
                        //{
                        //    foreach (string item in ccEmail.Split(new char[] { ';', ',' }))
                        //    {
                        //        mail.CC.Add(item);
                        //    }
                        //}
                        mail.CC.Add("gautam.v@teamcomputers.com");

                        //string bccEmail = _mySettings.MailBcc;
                        //if (!string.IsNullOrEmpty(bccEmail))
                        //{
                        //    foreach (string item in bccEmail.Split(new char[] { ';', ',' }))
                        //    {
                        //        mail.Bcc.Add(item);
                        //    }
                        //}


                        mail.Subject = MailSubject;
                        mail.IsBodyHtml = true;
                        mail.Body = MailBody;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = _mySettings.Host;
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(_mySettings.Username, _mySettings.Password);
                        smtp.EnableSsl = Convert.ToBoolean(_mySettings.EnableSsl);
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = Convert.ToInt32(_mySettings.Port);
                        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        smtp.Send(mail);


                    }
                }
                catch (Exception ex)
                {



                }
            }
            else
            {
                //  log.Error("Mail flag has been disabled");
            }

        }
        [NonAction]
        public bool SendEmailData(string MailTo, string MailCC, string MailSubject, string MailBody, MySettings? _mySettings,IFormFile? attachments)
        {
            if (Convert.ToBoolean(_mySettings.MailFlag))
            {
                try
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(_mySettings.Username, _mySettings.MailFrom);


                        mail.To.Add(MailTo);

                        string ccEmail = _mySettings.MailCc;
                        if (!string.IsNullOrEmpty(ccEmail))
                        {
                            foreach (string item in ccEmail.Split(new char[] { ';', ',' }))
                            {
                                mail.CC.Add(item);
                            }
                        }


                        string bccEmail = _mySettings.MailBcc;
                        if (!string.IsNullOrEmpty(bccEmail))
                        {
                            foreach (string item in bccEmail.Split(new char[] { ';', ',' }))
                            {
                                mail.Bcc.Add(item);
                            }
                        }
                        if (attachments != null && attachments.Length > 0)
                        {
                            MemoryStream ms = new MemoryStream();
                            
                                attachments.CopyTo(ms);
                                ms.Seek(0, SeekOrigin.Begin);
                                Attachment fileAttachment = new Attachment(ms, attachments.FileName);
                                mail.Attachments.Add(fileAttachment);
                                ms.Flush();
                            
                        }

                        mail.Subject = MailSubject;
                        mail.IsBodyHtml = true;
                        mail.Body = MailBody;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = _mySettings.Host;
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(_mySettings.Username, _mySettings.Password);
                        smtp.EnableSsl = Convert.ToBoolean(_mySettings.EnableSsl);
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = Convert.ToInt32(_mySettings.Port);
                        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                          smtp.Send(mail);


                    }
                    return true;

                }

                catch (Exception ex)
                {

                    return false;

                }
            }
            else
            {
                return false;
                //  log.Error("Mail flag has been disabled");
            }

        }
    }
}

