using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WcfServiceLibrary2.Classes;

namespace WcfServiceLibrary2
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service1 : IService1
    {
        Context model = new Context();

        public void AddAccount(string email, string password, string name, string city,
            string country, DateTime birthday, string gender, double latitude, double longitude)
        {
            if (GetAccount(email, password) == false)
            {
                string[] name_family = name.Split(' ');

                Directory.CreateDirectory($@"Accounts\{name_family[0] + " " + name_family[1]}\Images");

                //File.Copy(@"no_avatar.png", $"Accounts/{name_family[0] + " " + name_family[1]}/Images/1.png", true);

                var user = model.User.Add(new User
                {
                    Name = name_family[0],
                    LastName = name_family[1],
                    LatiTude = latitude,
                    LongiTude = longitude,
                    Avatarka = "no_avatar.png",
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

        public List<User> DefaultFilter(string email)
        {
            // Создаём пустой список пользователей
            List<User> users = new List<User>();

            // Пользователь для которого делается список людей
            User user = model.User.Single(t => t.Email == email);

            // Возраст нашего пользователя
            int user_age = DateTime.Now.Year - user.Birthday.Year;

            // Список пользователей из города нашего пользователя
            // И противоположный пол 
            var users_rec = model.User.Where(t => t.City == user.City)
                .Where(t => t.Gender != user.Gender)
                .ToList();

            foreach (var item in users_rec)
            {
                //int other_user_age = DateTime.Now.Year - item.Birthday.Year;

                //// Если небольшая разница в возрасте
                //// тогда пользователь добавляется в список
                //if(user_age == other_user_age
                //    || user_age + 1 == other_user_age
                //    || user_age == other_user_age + 1
                //    || user_age + 2 == other_user_age
                //    || user_age == other_user_age + 2)
                //{
                    users.Add(item);
                //}
            }

            if (users.Count > 0)
                return users;
            else
                return null;
        }
        
        public double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        {
            double distance = 0;

            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                        + Math.Cos(lat1 / 180 * Math.PI) * Math.Cos(lat2 / 180 * Math.PI)
                        * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate radius of earth
            // For this you can assume any of the two points.

            double radiusE = 6378135; // Equatorial radius, in metres
            double radiusP = 6356750; // Polar Radius

            //Numerator part of function
            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            //Denominator part of the function
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                            + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            //Calculate distance in meters.
            distance = radius * c;

            return distance; // distance in meters
        }

        public double GetLatiTude(string email) => model.User.Single(t => t.Email == email).LatiTude;

        public double GetLongiTude(string email) => model.User.Single(t => t.Email == email).LongiTude;

        public string GetName(string email) => model.User.FirstOrDefault(t => t.Email == email).Name
            + " " + model.User.FirstOrDefault(t => t.Email == email).LastName;

        public List<Photos> GetPhotos(User user)
        {
            if (model.Photos.Any(t => t.UserID == user.UserId))
                return model.Photos.Where(t => t.UserID == user.UserId).ToList();
            else
                return null;
        }

        public List<Hobbies> GetHobbies(User user)
        {
            if (model.Hobbies.Any(t => t.UserID == user.UserId))
                return model.Hobbies.Where(t => t.UserID == user.UserId).ToList();
            else
                return null;
        }

        public User GetUser(string email) => model.User.Where(t => t.Email == email).FirstOrDefault();

        public void AddLike(User user_u, User user_who)
        {
            model.Likes.Add(new Likes { User_Liked_ID = user_u, User_Who_Liked_ID = user_who, Date_Like = DateTime.Now });

            model.SaveChanges();
        }

        public void UpdateUser(string name, string lastname, DateTime birthday,
           string coloreye, string colorhaircut, string faith,
           string gender, string job,
           string descriptions, string education,
           string[] hobbies, User user)
        {
            User user1 = model.User.Where(t => t.UserId == user.UserId).FirstOrDefault();

            user1.Name = name;
            user1.LastName = lastname;
            user1.Description = descriptions;
            user1.Education = education;
            user1.Birthday = birthday;
            user1.Job = job;
            user1.ColorEye = coloreye;
            user1.ColorHairCut = colorhaircut;
            user1.Faith = faith;
            user1.Gender = gender;

            if (hobbies.Count() != 0)
            {
                var model12 = model.Hobbies.Select(t => t.Hobbie);

                foreach (var item2 in hobbies)
                {
                    if (!model12.Contains(item2))
                        model.Hobbies.Add(new Hobbies { Hobbie = item2, UserID = user1.UserId });
                }
            }

            model.SaveChanges();
        }

        public void AddPhoto(string image, User user)
        {
            model.Photos.Add(new Photos { Photo = image, UserID = user.UserId });

            model.SaveChanges();
        }

        public void AddPhoto(ImageBrush image, User user)
        {
           
        }

        public ImageBrush GetImage(User user)
        {
            if (File.Exists(user.Avatarka))
            {
                ImageBrush imageBrush = new ImageBrush();

                imageBrush.ImageSource = new BitmapImage(new Uri(user.Avatarka, UriKind.Relative));

                return imageBrush;
            }
            else
                return null;
        }
    }
}
