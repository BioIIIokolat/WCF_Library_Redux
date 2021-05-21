using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary2
{
    class UserClient
    {
        // ссылка на Callback-контракт клиента
        public IMyCallback callback;

        // имя пользователя
        public string name;
    }
}
