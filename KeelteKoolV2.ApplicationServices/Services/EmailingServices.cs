using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;

namespace KeelteKoolV2.ApplicationServices.Services
{
    public class EmailingServices : IEmailingServices
    {
        public EmailingServices()
        {
        }

        public void SendEmail(EmailDTO dto)
        {
            // Письма отключены. Ничего не делаем.
            return;
        }

        public void SendEmailToken(EmailTokenDTO dto, string token)
        {
            // Письма отключены. Ничего не делаем.
            return;
        }
    }
}