using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WcfServiceLibrary2.Classes;

namespace WcfServiceLibrary2
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service1 : IService1
    {
        Context model = new Context();
        public delegate void SendMessage(int chatid, Message tmpmes, int id);
        Dictionary<int, UserClient> OnlineUsers = new Dictionary<int, UserClient>();

        public void AddAccount(string email, string password, string name, string city,
            string country, DateTime birthday, string gender, double latitude, double longitude)
        {
            if (GetAccount(email, password) == false)
            {
                string[] name_family = name.Split(' ');

                Directory.CreateDirectory($@"Accounts\{name_family[0] + " " + name_family[1]}\Images");

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
                    Gender = gender,
                    chatItems = new List<ChatItemUsers>(),
                    Filters = new Filters()
                }) ;

                model.SaveChanges();

                int res = model.User.Where(x => x.Email == email)
                    .First()
                    .UserId;

                GetOnline(res);
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

        public List<User> DefaultFilter(User user)
        {
            // Создаём пустой список пользователей
            List<User> users = new List<User>();

            // Возраст нашего пользователя
            int user_age = DateTime.Now.Year - user.Birthday.Year;

            // Список пользователей из города нашего пользователя
            // И противоположный пол, с одного горла и примерно одинакого возраста,
            // Максимальная разница в возрасте 3 года
            var users_rec = model.User.Where(t => t.City == user.City
            && t.Gender != user.Gender
            && t.Birthday.Year == user.Birthday.Year
            || t.Birthday.Year + 1 == user.Birthday.Year
            || t.Birthday.Year + 2 == user.Birthday.Year
            || t.Birthday.Year + 3 == user.Birthday.Year)
                .ToList();

            users = users_rec;

            if (users.Count > 0)
                return users;
            else
                return model.User.Where(t => t.City == user.City
                && t.Gender != user.Gender).ToList();
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

        public double GetLatiTude(string email) => model.User.Where(t => t.Email == email)
            .FirstOrDefault().LatiTude;

        public double GetLongiTude(string email) => model.User.Where(t => t.Email == email)
            .FirstOrDefault().LongiTude;

        public string GetName(string email) => model.User.FirstOrDefault(t => t.Email == email).Name
            + " " + model.User.FirstOrDefault(t => t.Email == email).LastName;

        public List<byte[]> GetPhotos(User user)
        {
            if (model.Photos.Any(t => t.UserID == user.UserId))
            {
                List<byte[]> images = new List<byte[]>();

                foreach (var item in model.Photos.Where(t => t.UserID == user.UserId))
                    images.Add(File.ReadAllBytes(item.Photo));

                return images;
            }
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
            model.Likes.Add(new Likes { User_Liked_ID = user_u,
                User_Who_Liked_ID = user_who,
                Date_Like = DateTime.Now });

            model.SaveChanges();

            var r = (from n in model.Likes
                     where n.User_Who_Liked_ID.UserId == user_u.UserId &&
                     n.User_Liked_ID.UserId == user_who.UserId
                     select n).ToList();

            var r1 = (from n in model.Likes
                      where n.User_Who_Liked_ID.UserId == user_who.UserId &&
                      n.User_Liked_ID.UserId == user_u.UserId
                      select n).ToList();

            if (r.Count != 0 && r1.Count != 0)
            {
                ChatItem ci = new ChatItem { 
                    Messages = new List<Message>(),
                    Participants = new List<ChatItemUsers>(),
                    Title = "ABOBA"
                };

                ChatItemUsers ciu1 = new ChatItemUsers
                {
                    ChatItem = ci,
                    User = user_u,
                };
                ChatItemUsers ciu2 = new ChatItemUsers
                {
                    ChatItem = ci,
                    User = user_who
                };

                model.ChatItemUsers.Add(ciu1);
                model.ChatItemUsers.Add(ciu2);
                ci.Participants.Add(ciu1);
                ci.Participants.Add(ciu2);

                model.ChatItems.Add(ci);
            }
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

        public void AddPhoto(BitmapImage image, User user, string ext)
        {
            int number_photo = model.Photos.Where(t => t.PhotoID == user.UserId)
                .Count();

            if (number_photo <= 5)
            {
                string name = user.Name + " " + user.LastName;

                string name_file = number_photo + ext;

                string[] path = new string[] { Environment.CurrentDirectory + "Accounts" + name + "Images" + name_file};

                string _path = Path.Combine(path);

                File.Create(_path);

                model.Photos.Add(new Photos { Photo = _path, UserID = user.UserId });

                model.SaveChanges();

                if (ext.Contains(".png"))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));

                    using (var fileStream = new System.IO.FileStream(_path, System.IO.FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }
                }

                if(ext.Contains(".jpg") || ext.Contains(".jpeg"))
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));

                    using (var fileStream = new System.IO.FileStream(_path, System.IO.FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }
                }

            }
        }

        public bool IsExistsHobbies(User user)
        {
            if (model.Hobbies.Any(t => t.UserID == user.UserId))
                return true;
            else
                return false;
        }

        public byte[] GetImage(User user)
        {
            string[] path = new string[] { Environment.CurrentDirectory, user.Avatarka };

            string fullpath = Path.Combine(path);

            return File.ReadAllBytes(fullpath);
        }

        // Метод установки аватарки
        public void SetAvatar(User user, byte[] array)
        {
            string path = user.Avatarka;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(array);
            bitmap.EndInit();

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }


        public void GetOnline(int Id)
        {
            if (!OnlineUsers.ContainsKey(Id)) OnlineUsers.Add(Id, new UserClient { callback = OperationContext.Current.GetCallbackChannel<IMyCallback>() });
        }

        public void GetOffline(int Id)
        {
            if (OnlineUsers.ContainsKey(Id)) OnlineUsers.Remove(Id);
        }

        public void BanUser(int senderID, int bannedID)
        {
            var list = from n in model.BlackLists where n.UserID == senderID select n.UserEnemyID;
            if (list != null && !list.Contains(bannedID)) model.BlackLists.Add(new BlackList() { UserID = bannedID, ID = senderID });
        }

        public void UnbanUser(int senderID, int bannedID)
        {
            var list = from n in model.BlackLists where n.UserID == senderID select n;
            if (list != null && list.Select(t => t.UserEnemyID).Contains(bannedID)) model.BlackLists.Remove((from t in list where t.UserEnemyID == bannedID select t).FirstOrDefault());
        }

        public void ChangeFilters(int userID, Filters f)
        {
            var r = (from n in model.User where n.UserId == userID select n).FirstOrDefault();
            if (r != null && f != null) r.Filters = f;
        }

        public List<User> FindUsers(int userID)
        {
            List<User> ChoosedUsers = new List<User>();
            User CurrentUser = (from n in model.User where n.UserId == userID select n).FirstOrDefault();

            var results = from n in model.User
                          where n.UserId != userID &&
                          (((n.ColorEye == CurrentUser.Filters.ColorEye) || CurrentUser.Filters.ColorEye == null)
                          && GetDistanceBetweenPoints(n.LatiTude, n.LongiTude, CurrentUser.LatiTude, CurrentUser.LongiTude) <= CurrentUser.Filters.MaxDistance
                          && n.ColorHairCut == CurrentUser.Filters.ColorHair
                          && ((DateTime.Now.Year - n.Birthday.Year <= CurrentUser.Filters.MaxAge && DateTime.Now.Year - n.Birthday.Year >= CurrentUser.Filters.MinAge)))
                          select n;
            foreach (var r in results) ChoosedUsers.Add(r);
            return ChoosedUsers;
        }

        public List<User> GetUsersWhoLikedYou(User user)
        {
            return (from n in model.Likes where n.User_Who_Liked_ID.UserId == user.UserId select n.User_Liked_ID).ToList();
        }

    }
}
