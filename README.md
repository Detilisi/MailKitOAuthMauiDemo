
# MailKit OAuth Demo

A .NET MAUI demo that integrates Gmail OAuth2 and MailKit for sending and receiving email messages. This application showcases how to implement secure login using OAuth2, enabling users to access their Gmail accounts without compromising security. It utilizes MailKit's SMTP client for composing and sending emails, ensuring reliable delivery, while the IMAP client allows users to fetch and manage their inbox, providing a complete email experience. This demo serves as a practical reference for developers working with authentication and mailing services in cross-platform apps.

## Features:

- **Secure OAuth2 Authentication:** Connect your email account securely using GamilOAuth2.
- **Email Message Composing and Sending:** Utilizes MailKit's SMTP client for reliable email delivery.
- **Email Receiving:** Fetches and manages emails using MailKit's IMAP client.
- **.NET MAUI Cross-Platform Compatibility:** Seamlessly runs on iOS, Android, and Windows devices.

## Screenshots

### Connect via OAuth2
The main screen implements open authentication through Gmail's OAuth2, facilitating the secure retrieval of user credentials. 
These credentials are subsequently utilized to establish connections with MailKit's SMTP and IMAP clients, enabling robust email functionality.

![image](https://github.com/user-attachments/assets/2a65a493-ab5c-4377-94f5-6c6489778cf8)
![image](https://github.com/user-attachments/assets/a5dcf0bf-e355-4af2-80f9-26287950833b)

### Inbox View
Once authenticated, MailKit's IMAP client connects to the user's email account using secure credentials obtained from the main page, allowing for the retrieval of email messages.

![image](https://github.com/user-attachments/assets/f1b1645f-9400-4067-8ce6-07b5a8ff91f4)

### Send Email

Similarly, the MailKit's SMTP client uses the secure credentials obtained during authentication to successfully send email messages from the user's email account.

![image](https://github.com/user-attachments/assets/a4920f0e-c677-43ec-a725-5402216be4c2)
![image](https://github.com/user-attachments/assets/e85ca4ed-c5ca-4653-8a5d-ec6ce4549d60)

## Technologies Used

- .NET MAUI
- MailKit
- Google.Apis.Auth
- CommunityToolkit.Mvvm

## References

For more info on "Using OAuth2 With GMail (IMAP, POP3 or SMTP)" : https://github.com/jstedfast/MailKit/blob/master/GMailOAuth2.md

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
