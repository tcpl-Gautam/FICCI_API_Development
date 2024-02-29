namespace FICCI_API.DTO.Account
{
    public class MailDTO
    {
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string LoginId { get; set; }
        public string? ResourceType { get; set; }
        public int? ResourceId { get; set; }
        public  IFormFile? Attachments { get; set; }

    }
}
