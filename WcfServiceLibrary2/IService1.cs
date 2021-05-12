using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary2
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        bool GetAccount(string email, string password, bool partly = false);

        [OperationContract]
        void ChangePassword(string email, string password);

        [OperationContract]
        int GetCode(string email);

        [OperationContract]
        List<User> DefaultFilter(string email);

        [OperationContract]
        double GetLatiTude(string email);

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
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Добавьте здесь операции служб
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
