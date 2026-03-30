using InoxServer.Domain.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace InoxServer.Infrastructure.Services.EmailService
{
    internal class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailVerificationAsync(
            string toEmail,
            string toName,
            string verificationLink,
            CancellationToken cancellationToken = default)
        {
            var smtpSettings = _configuration.GetSection("Smtp");

            var host = smtpSettings["Host"]!;
            var port = int.Parse(smtpSettings["Port"]!);
            var username = smtpSettings["Username"]!;
            var password = smtpSettings["Password"]!;
            var fromName = smtpSettings["FromName"]!;
            var fromEmail = smtpSettings["FromEmail"]!;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = "Xác thực tài khoản của bạn";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = BuildVerificationEmailHtml(toName, verificationLink)
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(username, password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }

        public async Task ResendVerificationEmailAsync(
            string toEmail,
            string toName,
            string verificationLink,
            CancellationToken cancellationToken = default)
        {
            var smtpSettings = _configuration.GetSection("Smtp");

            var host = smtpSettings["Host"]!;
            var port = int.Parse(smtpSettings["Port"]!);
            var username = smtpSettings["Username"]!;
            var password = smtpSettings["Password"]!;
            var fromName = smtpSettings["FromName"]!;
            var fromEmail = smtpSettings["FromEmail"]!;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = "Gửi lại email xác thực tài khoản";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = BuildResendEmailHtml(toName, verificationLink)
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(username, password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }

        private static string BuildResendEmailHtml(string name, string verificationLink)
        {
            return $"""
                <!DOCTYPE html>
                <html lang="vi">
                <head>
                  <meta charset="UTF-8" />
                  <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
                  <title>Xác thực tài khoản</title>
                </head>
                <body style="margin:0;padding:0;background:#f4f4f4;font-family:Arial,sans-serif;">
                  <table width="100%" cellpadding="0" cellspacing="0" style="background:#f4f4f4;padding:30px 0;">
                    <tr>
                      <td align="center">
                        <table width="600" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:8px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,0.08);">
                          <tr>
                            <td style="background:#1a1a2e;padding:30px;text-align:center;">
                              <h1 style="color:#ffffff;margin:0;font-size:24px;">InoxTN</h1>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding:40px 40px 30px;">
                              <h2 style="color:#1a1a2e;margin:0 0 16px;">Xin chào, {name}!</h2>
                              <p style="color:#555;line-height:1.6;margin:0 0 24px;">
                                Bạn vừa yêu cầu gửi lại email xác thực tài khoản <strong>InoxTN</strong>.
                                Vui lòng nhấn vào nút bên dưới để hoàn tất xác thực.
                              </p>
                              <table cellpadding="0" cellspacing="0">
                                <tr>
                                  <td style="border-radius:6px;background:#1a1a2e;">
                                    <a href="{verificationLink}"
                                       style="display:inline-block;padding:14px 32px;color:#ffffff;font-size:16px;font-weight:bold;text-decoration:none;border-radius:6px;">
                                      Xác thực Email
                                    </a>
                                  </td>
                                </tr>
                              </table>
                              <p style="color:#888;font-size:13px;margin:24px 0 0;line-height:1.5;">
                                Liên kết này sẽ hết hạn sau <strong>24 giờ</strong>.<br/>
                                Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td style="background:#f9f9f9;padding:20px 40px;text-align:center;color:#aaa;font-size:12px;border-top:1px solid #eee;">
                              &copy; {DateTime.UtcNow.Year} InoxTN. Mọi quyền được bảo lưu.
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>
                """;
        }

        private static string BuildVerificationEmailHtml(string name, string verificationLink)
        {
            return $"""
                <!DOCTYPE html>
                <html lang="vi">
                <head>
                  <meta charset="UTF-8" />
                  <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
                  <title>Xác thực tài khoản</title>
                </head>
                <body style="margin:0;padding:0;background:#f4f4f4;font-family:Arial,sans-serif;">
                  <table width="100%" cellpadding="0" cellspacing="0" style="background:#f4f4f4;padding:30px 0;">
                    <tr>
                      <td align="center">
                        <table width="600" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:8px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,0.08);">
                          <tr>
                            <td style="background:#1a1a2e;padding:30px;text-align:center;">
                              <h1 style="color:#ffffff;margin:0;font-size:24px;">InoxTN</h1>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding:40px 40px 30px;">
                              <h2 style="color:#1a1a2e;margin:0 0 16px;">Xin chào, {name}!</h2>
                              <p style="color:#555;line-height:1.6;margin:0 0 24px;">
                                Cảm ơn bạn đã đăng ký tài khoản tại <strong>InoxTN</strong>.
                                Vui lòng nhấn vào nút bên dưới để xác thực địa chỉ email của bạn.
                              </p>
                              <table cellpadding="0" cellspacing="0">
                                <tr>
                                  <td style="border-radius:6px;background:#1a1a2e;">
                                    <a href="{verificationLink}"
                                       style="display:inline-block;padding:14px 32px;color:#ffffff;font-size:16px;font-weight:bold;text-decoration:none;border-radius:6px;">
                                      Xác thực Email
                                    </a>
                                  </td>
                                </tr>
                              </table>
                              <p style="color:#888;font-size:13px;margin:24px 0 0;line-height:1.5;">
                                Liên kết này sẽ hết hạn sau <strong>24 giờ</strong>.<br/>
                                Nếu bạn không thực hiện đăng ký, vui lòng bỏ qua email này.
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td style="background:#f9f9f9;padding:20px 40px;text-align:center;color:#aaa;font-size:12px;border-top:1px solid #eee;">
                              &copy; {DateTime.UtcNow.Year} InoxTN. Mọi quyền được bảo lưu.
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>
                """;
        }

        public async Task SendPasswordResetEmailAsync(
            string toEmail,
            string toName,
            string resetLink,
            CancellationToken cancellationToken = default)
        {
            var smtpSettings = _configuration.GetSection("Smtp");

            var host = smtpSettings["Host"]!;
            var port = int.Parse(smtpSettings["Port"]!);
            var username = smtpSettings["Username"]!;
            var password = smtpSettings["Password"]!;
            var fromName = smtpSettings["FromName"]!;
            var fromEmail = smtpSettings["FromEmail"]!;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = "Đặt lại mật khẩu tài khoản InoxTN";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = BuildPasswordResetEmailHtml(toName, resetLink)
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(username, password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }

        private static string BuildPasswordResetEmailHtml(string name, string resetLink)
        {
            return $"""
                <!DOCTYPE html>
                <html lang="vi">
                <head>
                  <meta charset="UTF-8" />
                  <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
                  <title>Đặt lại mật khẩu</title>
                </head>
                <body style="margin:0;padding:0;background:#f4f4f4;font-family:Arial,sans-serif;">
                  <table width="100%" cellpadding="0" cellspacing="0" style="background:#f4f4f4;padding:30px 0;">
                    <tr>
                      <td align="center">
                        <table width="600" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:8px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,0.08);">
                          <tr>
                            <td style="background:#1a1a2e;padding:30px;text-align:center;">
                              <h1 style="color:#ffffff;margin:0;font-size:24px;">InoxTN</h1>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding:40px 40px 30px;">
                              <h2 style="color:#1a1a2e;margin:0 0 16px;">Xin chào, {name}!</h2>
                              <p style="color:#555;line-height:1.6;margin:0 0 24px;">
                                Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản <strong>InoxTN</strong> của bạn.
                                Nhấn vào nút bên dưới để tạo mật khẩu mới.
                              </p>
                              <table cellpadding="0" cellspacing="0">
                                <tr>
                                  <td style="border-radius:6px;background:#c0392b;">
                                    <a href="{resetLink}"
                                       style="display:inline-block;padding:14px 32px;color:#ffffff;font-size:16px;font-weight:bold;text-decoration:none;border-radius:6px;">
                                      Đặt lại mật khẩu
                                    </a>
                                  </td>
                                </tr>
                              </table>
                              <p style="color:#888;font-size:13px;margin:24px 0 0;line-height:1.5;">
                                Liên kết này sẽ hết hạn sau <strong>1 giờ</strong>.<br/>
                                Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td style="background:#f9f9f9;padding:20px 40px;text-align:center;color:#aaa;font-size:12px;border-top:1px solid #eee;">
                              &copy; {DateTime.UtcNow.Year} InoxTN. Mọi quyền được bảo lưu.
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>
                """;
        }
    }
}
