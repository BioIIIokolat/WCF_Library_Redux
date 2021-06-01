using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Services;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WcfServiceLibrary2.Classes;
using static System.Net.Mime.MediaTypeNames;

namespace WcfServiceLibrary2
{ //[ServiceContract(CallbackContract = typeof(IMyCallback))]
    public interface IMyCallback
    {
        [OperationContract]
        void OnCallback();

        [OperationContract]
        void OnSendMessage(int chatid, Message message);
    }

    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    // [ServiceContract]
    [ServiceContract(CallbackContract = typeof(IMyCallback))]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        void UpdateUser(string name, string lastname, DateTime birthday,
            string coloreye, string colorhaircut, string faith,
            string gender, string job,
            string descriptions, string education,
            string[] hobbies, User user);

        [OperationContract]
        void AddPhoto(ImageBrush image, User user);

        [OperationContract]
        byte[] GetImage(User user);

        [OperationContract]
        bool GetAccount(string email, string password, bool partly = false);

        [OperationContract]
        void ChangePassword(string email, string password);

        [OperationContract]
        int GetCode(string email);

        [OperationContract]
        void SetAvatar(User user, byte[] array);

        [OperationContract]
        List<User> DefaultFilter(User user);

        [OperationContract]
        double GetLatiTude(string email);

        [OperationContract]
        List<byte[]> GetPhotos(User user);

        [OperationContract]
        User GetUser(string email);

        [OperationContract]
        void AddLike(User user_u, User user_who);

        [OperationContract]
        List<Hobbies> GetHobbies(User user);

        [OperationContract]
        double GetLongiTude(string email);

        [OperationContract]
        double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2);

        [OperationContract]
        void AddAccount(string email, string password, string name,
            string city, string country, DateTime birthday, string gender,
            double latitude, double longitude);

        [OperationContract]
        string GetName(string email);

        [OperationContract]
        List<TmpChatItem> GetChatItems(int UserID);
        [OperationContract]
        void SendMes(Classes.Message tmpmes, TmpChatItem tmpChatItem, int Id);
        [OperationContract]
        void GetOnline(int Id);
        [OperationContract]
        void GetOffline(int Id);
        [OperationContract]
        void BanUser(int senderID, int bannedID);
        [OperationContract]
        void UnbanUser(int senderID, int bannedID);
        [OperationContract]
        void ChangeFilters(int userID, Filters f);

        [OperationContract]
        List<User> FindUsers(int userID);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        List<User> GetUsersWhoLikedYou(User user);

        [OperationContract]
        List<User> GetUsersWhoWasBannedByYou(User user);
    }

    // Используйте контракт данных, как показано на следующем примере, чтобы добавить сложные типы к сервисным операциям.
    // В проект можно добавлять XSD-файлы. После построения проекта вы можете напрямую использовать в нем определенные типы данных с пространством имен "WcfServiceLibrary2.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
