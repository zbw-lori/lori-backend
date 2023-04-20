using System.Threading.Tasks;

namespace lori.backend.Core.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(string to, string from, string subject, string body);
}
