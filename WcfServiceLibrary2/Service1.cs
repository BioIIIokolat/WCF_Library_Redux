using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary2
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    public class Service1 : IService1
    {
        Context model = new Context();

        public void AddAccount(string email, string password, string name, string city,
            string country, DateTime birthday, string gender)
        {
            if (GetAccount(email, password) == false)
            {
                string[] name_family = name.Split(' ');

                model.User.Add(new User
                {
                    Name = name_family[0],
                    LastName = name_family[1],
                    Email = email,
                    Password = password,
                    City = city,
                    Country = country,
                    Birthday = birthday,
                    Gender = gender
                } );

                model.SaveChanges();
            }
        }

        public bool GetAccount(string email, string password, bool partly = false)
        {
            if (partly == false)
            {
                var check = model.User.Where(t => t.Email == email).Where(t => t.Password == password);

                if (check.Count() > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                var check = model.User.Where(t => t.Email == email);

                if (check.Count() > 0)
                    return true;
                else
                    return false;
            }
        }

        public int GetCode(string email)
        {
            if (GetAccount(email, "empty", true))
            {
                Random random = new Random();

                int code = random.Next(1, 1124);

                SmtpClient smtpClient = new SmtpClient();

                smtpClient = new SmtpClient("smtp.gmail.com", 587);

                string name = model.User.SingleOrDefault(t => t.Email == email).Name;

                string last_name = model.User.SingleOrDefault(t => t.Email == email).LastName;

                smtpClient.Credentials = new NetworkCredential("reduksreduksovic1337@gmail.com", "Redux1337");

                smtpClient.EnableSsl = true;

                smtpClient.Send(CreateMailMessage($"Здравствуйте {name} {last_name}, впишите этот код в приложение для подтверждения, что аккаунт ваш. Код - {code}", email));

                return code;
            }
            else 
                return 0;
        }

        private MailMessage CreateMailMessage(object text, object emailTo)
        {
            MailMessage mailMsg = new MailMessage();

            mailMsg.From = new MailAddress("reduksreduksovic1337@gmail.com");
            mailMsg.To.Add(new MailAddress(emailTo.ToString()));

            mailMsg.Subject = "Восстановление пароля [Redux]";
            mailMsg.IsBodyHtml = false;
            mailMsg.Body = text.ToString();

            return mailMsg;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public void ChangePassword(string email, string password)
        {
            User us = model.User.Single(t => t.Email == email);

            us.Password = password;

            model.SaveChanges();
        }
    }
}
