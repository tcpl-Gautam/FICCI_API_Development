
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
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FICCI_API.Controller.API;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;

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
        public string InvoiceAssignhtmlBody(string EmailLink, string custName, string CityCode, string PAN, string GST, string ContactPerson, string? PhoneNo, string projectCode, string projectName, string customerCode)
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
            string template = $@"<p style=""font-family: Arial, sans-serif; font-size: 13px;"">
Dear User,<br><br>
Following Performa Invoice has been assigned in the Invoice portal for your necessary approval.<br><br>
<strong>Project Code</strong>   : {projectCode} <br>
<strong>Project Name</strong>   : {projectName} <br>
<strong>Customer Code</strong>   : {customerCode} <br>
<strong>Customer Name</strong>   : {custName} <br>
<strong>Customer City</strong> : {CityCode}<br>
<strong>Customer PAN No</strong>. : {PAN}<br>
<strong>Customer GST No</strong>. : {GST}<br>
To Access Invoice Portal : <a href='{EmailLink}'>Click Here</a><br>
<strong>Note</strong>: To open the Invoice portal, please open it in Microsoft Edge or In Google Chrome Regards <strong>FICCI Team</strong><br><br>
<strong>Regards</strong><br>FICCI Team";

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

            string template = $@"<p style=""font-family: Arial, sans-serif; font-size: 13px;"">Dear User,<br><br>Following Customer has been {header} in the Invoice Portal:<br><br><strong>Customer Name</strong>   : {custName} <br><strong>Customer City</strong> : {CityCode}<br> <strong>Customer PAN No</strong>. : {PAN}<br><strong>Customer GST No</strong>. : {GST}<br><strong>Customer Contact Person</strong>. : {ContactPerson}<br><strong>Customer Phone No.</strong>. : {PhoneNo}<br><br>To Access Invoice Portal : <a href='{EmailLink}'>Click Here</a><br><strong>Note</strong>: To open the Invoice portal, please open it in Microsoft Edge or In Google Chrome.<br><br><strong>Regards</strong><br>FICCI Team";
            return template;

        }

        [NonAction]
        public string InvoiceApprovalhtmlBody(string header, string EmailLink, string customerCode, string custName, string CityCode, string PAN, string GST, string projectName, string projectCode)
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

            string template = $@"<p style=""font-family: Arial, sans-serif; font-size: 13px;"">Dear User,<br><br>
 Performa Invoice has been {header} in the Invoice Portal:<br><br>
<strong>Project Code</strong>   : {projectCode} <br>
<strong>Project Name</strong>   : {projectName} <br>
<strong>Customer Code</strong>   : {customerCode} <br>
<strong>Customer Name</strong>   : {custName} <br>
<strong>Customer City</strong> : {CityCode}<br>
<strong>Customer PAN No</strong>. : {PAN}<br>
<strong>Customer GST No</strong>. : {GST}<br>
<br>To Access Invoice Portal : <a href='{EmailLink}'>Click Here</a><br>
<strong>Note</strong>: To open the Invoice portal, please open it in Microsoft Edge or In Google Chrome Regards <strong>FICCI Team</strong><br><br>
<strong>Regards</strong><br>FICCI Team";
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

                        string ccEmail = _mySettings.MailCc;
                        if (!string.IsNullOrEmpty(ccEmail))
                        {
                            foreach (string item in ccEmail.Split(new char[] { ';', ',' }))
                            {
                                mail.CC.Add(item);
                            }
                        }
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
                        //smtp.Send(mail);


                    }
                }
                catch (Exception ex)
                {

                    throw;

                }
            }
            else
            {
                //  log.Error("Mail flag has been disabled");
            }

        }
        [NonAction]
        public bool SendEmailData(string MailTo, string MailCC, string MailSubject, string MailBody, MySettings? _mySettings,List<IFormFile>? attachments)
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
                        if (attachments != null && attachments.Any())
                        {
                            foreach(var item in attachments)
                            {
                                MemoryStream ms = new MemoryStream();

                                item.CopyTo(ms);
                                ms.Seek(0, SeekOrigin.Begin);
                                Attachment fileAttachment = new Attachment(ms, item.FileName);
                                mail.Attachments.Add(fileAttachment);
                                ms.Flush();
                            }
                           
                            
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
                        // smtp.Send(mail);


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

        [NonAction]
        public bool CheckToken(string token)
        {
            try
            {
               var resu = _context.Userloginlogs.Where(x => x.JwtToken == token).OrderByDescending(x => x.LoginDate).FirstOrDefault();
                if (resu != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        [NonAction]
        public string UploadFile1(List<IFormFile>? file1, string loginId, int headerid, string? folder, int ResourceTypeId, string ResourceType, string ScreenName)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            string fileids = "";
            if (file1 == null || !file1.Any())
            {
                return null; // Handle invalid file
            }

            foreach (var file in file1)
            {

                // Generate a unique filename to avoid conflicts
                string uniqueFileName = timestamp;
                var fileExtension = Path.GetExtension(file.FileName);
                string folderpath = Path.Combine("wwwroot", "PurchaseInvoice", folder);

                // Combine the path where you want to store the file with the unique filename
                string filePath = Path.Combine("wwwroot", "PurchaseInvoice", folder, uniqueFileName + fileExtension);
                string savefilePath = Path.Combine("PurchaseInvoice", folder, uniqueFileName + fileExtension);
                if (!Directory.Exists(folderpath))
                {
                    // The folder does not exist, so create it
                    Directory.CreateDirectory(folderpath);

                }
                // Save the file to the specified path
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                FileInfoModel fileInfoModel = new FileInfoModel();



                fileInfoModel.FileName = file.FileName;
                fileInfoModel.Size = file.Length;
                fileInfoModel.ContentType = file.ContentType;
                int reurnId = FileMethod(fileInfoModel.FileName, fileInfoModel.Size, fileInfoModel.ContentType, savefilePath, loginId, headerid, ResourceTypeId, ResourceType, ScreenName, "");
                // Return the file path
                fileids += reurnId + ",";
            }

            return fileids.TrimEnd(',');
        }

        [NonAction]
        public string UploadFile(List<ImpiHeaderAttachment>? file1, string loginId, int headerid, string? folder, int ResourceTypeId, string ResourceType, string ScreenName)
        {
            string fileids = "";


            if (file1 == null || !file1.Any())
            {
                return null; // Handle invalid file
            }

            foreach (var item in file1)
            {
                foreach (var file in item.content)
                {
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    fileids = "";
                    // Generate a unique filename to avoid conflicts
                    string uniqueFileName = timestamp;
                    var fileExtension = Path.GetExtension(file.FileName);
                    string folderpath = Path.Combine("wwwroot", "PurchaseInvoice", folder, item.doctype);

                    // Combine the path where you want to store the file with the unique filename
                    string filePath = Path.Combine("wwwroot", "PurchaseInvoice", folder, item.doctype, uniqueFileName + fileExtension);
                    string savefilePath = Path.Combine("PurchaseInvoice", folder, item.doctype, uniqueFileName + fileExtension);
                    if (!Directory.Exists(folderpath))
                    {
                        // The folder does not exist, so create it
                        Directory.CreateDirectory(folderpath);

                    }
                    // Save the file to the specified path
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    FileInfoModel fileInfoModel = new FileInfoModel();



                    fileInfoModel.FileName = file.FileName;
                    fileInfoModel.Size = file.Length;
                    fileInfoModel.ContentType = file.ContentType;
                    int reurnId = FileMethod(fileInfoModel.FileName, fileInfoModel.Size, fileInfoModel.ContentType, savefilePath, loginId, headerid, ResourceTypeId, ResourceType, ScreenName, item.doctype);
                    // Return the file path
                    fileids += reurnId + ",";
                }
            }
            return fileids.TrimEnd(',');
        }
        [NonAction]
        public int FileMethod(string fileName, long length, string contentType, string path, string loginId, int headerid, int ResourceTypeId, string ResourceType, string ScreenName, string doctype)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    FicciImad imad = new FicciImad();
                    imad.ImadCreatedBy = loginId;
                    imad.ImadCreatedOn = DateTime.Now;
                    imad.ImadActive = true;
                    imad.ImadFileName = fileName;
                    imad.ImadFileSize = length.ToString();
                    imad.ImadFileType = contentType;
                    imad.ImadFileUrl = path;
                    imad.ImadScreenName = ScreenName;
                    imad.ResourceTypeId = ResourceTypeId;
                    imad.ResourceType = ResourceType;
                    imad.ResourceId = headerid;
                    imad.Doctype = doctype;
                    _context.Add(imad);
                    _context.SaveChanges();
                    int returnId = imad.ImadId;
                    transaction.Commit();
                    return returnId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
    }
}

