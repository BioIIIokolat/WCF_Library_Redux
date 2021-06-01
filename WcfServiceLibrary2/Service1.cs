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
using System.Windows;
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
                    Gender = gender,
                    chatItems = new List<ChatItemUsers>(),
                    Filters = new Filters(),
                    Likes = new List<Likes>()
                });

                model.SaveChanges();
                int res = model.User.Where(x => x.Email == email).First().UserId;
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

        public double GetLatiTude(string email) => model.User.Where(t => t.Email == email)
            .FirstOrDefault().LatiTude;

        public double GetLongiTude(string email) => model.User.Where(t => t.Email == email)
            .FirstOrDefault().LongiTude;

        public string GetName(string email)
        {
            return model.User.FirstOrDefault(t => t.Email == email).Name + " " + model.User.FirstOrDefault(t => t.Email == email).LastName;
        }

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

        public User GetUser(string email)
        {
            try
            {
                var r = model.User.Where(t => t.Email == email).FirstOrDefault();
                return r;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
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

        public byte[] GetImage(User user) => File.ReadAllBytes(user.Avatarka);

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

        private void ClearLikes()
        {
            for (int i = 0; i < model.Likes.Count();)
            {
                model.Likes.Remove(model.Likes.ToList()[i++]);
            }

            model.SaveChanges();
        }

        public void AddLike(User user_u, User user_who)
        {
            //if ((from n in model.Likes where n.User_Liked_ID.UserId == user_u.UserId 
            //     && n.User_Who_Liked_ID.UserId == user_who.UserId select n).Count() != 0) return;

            model.Likes.Add(new Likes { User_Liked_ID = user_u, User_Who_Liked_ID = user_who, Date_Like = DateTime.Now });

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
                // здесь
                ChatItem ci = new ChatItem { Messages = new List<Message>(), Participants = new List<ChatItemUsers>() };

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
        //public string GetPath(ChatItem chatItem, int id)
        //{
        //    List<User> fr = (from t in model.User
        //                     where t.UserId == id
        //                     select t).ToList();
        //    return fr.Where(x => x.UserId != id).FirstOrDefault().Avatarka;

        //}
        public List<TmpChatItem> GetChatItems(int UserID)
        {
            User user = (from t in model.User
                         where t.UserId == UserID
                         select t).FirstOrDefault();
            if (user.chatItems != null)
            {
                List<ChatItemUsers> s = (List<ChatItemUsers>)(from t in model.User
                                                              where t.UserId == UserID
                                                              select t.chatItems);
                List<TmpChatItem> tmpChatItems = new List<TmpChatItem>();

                for (int i = 0; i < s.Count; i++)
                {
                    TmpChatItem tmpChatItem = new TmpChatItem();
                    List<ChatItemUsers> res1 = (List<ChatItemUsers>)(from n in model.ChatItemUsers.ToList()
                                                                     where n.ChatItem.ChatItemId == s[i].ChatItemId
                                                                     select n.ChatItem.Participants);
                    foreach (var item in res1)
                    {
                        if (item.UserId != UserID)
                        {
                            tmpChatItem.Title = item.User.Name + " " + item.User.LastName;
                            tmpChatItem.ImagePath = GetImage(item.User);

                        }

                    }
                    tmpChatItem.Chatid = s[i].ChatItemId;
                    if (model.Messages.Count() > 0)
                    {

                        List<Message> res = model.Messages.ToList().Where(x => x.chatItem?.ChatItemId == s[i].ChatItemId).ToList();
                        tmpChatItem.messages = new List<Message>();
                        if (tmpChatItem.messages.Count > 0)
                        {
                            tmpChatItem.LastMessage = tmpChatItem.messages?.OrderBy(x => x.TimeSending).LastOrDefault().Mes;
                        }
                    }
                    tmpChatItems.Add(tmpChatItem);
                }

                return tmpChatItems;
            }
            return null;
        }

        private void mesage(int chatid, Message tmpmes, int id)
        {
            //Добавить проверку на онлайн
            if (!OnlineUsers.ContainsKey(id)) return;
            OnlineUsers[id].callback.OnSendMessage(chatid, tmpmes);
        }
        public void SendMes(Message tmpmes, TmpChatItem tmpChatItem, int Id)
        {
            ChatItem chatItem = (from t in model.ChatItems
                                 where t.ChatItemId == tmpChatItem.Chatid
                                 select t).FirstOrDefault();

            User user = (from t in model.User
                         where t.UserId == Id
                         select t).FirstOrDefault();

            Message message = new Message()
            {
                TimeSending = tmpmes.TimeSending,
                chatItem = chatItem,
                ImagePath = null,
                IsRecieved = false,
                Mes = tmpmes.Mes,
                user = user
            };

            List<User> users1 = (from t in model.ChatItemUsers
                                 where t.ChatItem.ChatItemId == tmpChatItem.Chatid
                                 select t.User).Distinct().ToList();

            model.Messages.Add(message);
            model.SaveChanges();
            foreach (var item in users1)
            {
                //Добавить проверку на онлайн
                SendMessage d = new SendMessage(mesage);

                d.BeginInvoke(tmpChatItem.Chatid, tmpmes, item.UserId, null, null);
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

        public List<User> GetUsersWhoWasBannedByYou(User user)
        {
            List<User> Users = new List<User>();

            List<BlackList> MyBlackList = (from n in model.BlackLists
                                           where n.UserID == user.UserId
                                           select n).ToList();

            foreach (BlackList ban in MyBlackList)
                Users.Add((from n in model.User where n.UserId == ban.UserEnemyID select n).FirstOrDefault());

            return Users;
        }

        public Service1()
        {
            //if (model.Likes.Count() == 0)
            //{
            User ME = (from n in model.User where n.UserId == 32 select n).FirstOrDefault(); //32
            for (int i = 5; i < 10; i++) AddLike(model.User.ToList()[i], ME);
            //}
            //else
            //ClearLikes();
        }
    }
}
