namespace FICCI_API.DTO.Account
{
    public class MailConfig
    {
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public bool SslEnabled { get; set; } = true;
    }
    public class EmailModel
    {
        public string strReceiver { get; set; }
        public string strSubject { get; set; }
        public string strBody { get; set; }
        public List<string> ccEmails { get; set; }
    }
}
