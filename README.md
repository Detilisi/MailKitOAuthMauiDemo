# MailKit OAuth Demo
A .NET MAUI demo that integrates Gmail OAuth2 and MailKit for sending and receiving email messages. This application showcases how to implement secure login using OAuth2, enabling users to access their Gmail accounts without compromising security. It utilizes MailKit's SMTP client for composing and sending emails, ensuring reliable delivery, while the IMAP client allows users to fetch and manage their inbox, providing a complete email experience. This demo serves as a practical reference for developers working with authentication and mailing services in cross-platform apps.

## Features
- **Secure OAuth2 Authentication:** Connect your email account securely using Gmail OAuth2.
- **Email Message Composing and Sending:** Utilizes MailKit's SMTP client for reliable email delivery.
- **Email Receiving:** Fetches and manages emails using MailKit's IMAP client.
- **.NET MAUI Cross-Platform Compatibility:** Seamlessly runs on iOS, Android, and Windows devices.

## Tech Stack
- [.NET MAUI](https://github.com/dotnet/maui)
- [MailKit](https://github.com/jstedfast/MailKit)
- [Google.Apis.Auth](https://developers.google.com/identity/protocols/oauth2)

## References
For more info on "Using OAuth2 With GMail (IMAP, POP3 or SMTP)": [MailKit GMail OAuth2 Documentation](https://github.com/jstedfast/MailKit/blob/master/GMailOAuth2.md)

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
