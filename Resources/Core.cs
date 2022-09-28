using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chetchik.Resources
{
    public static class Core
    {
        public static MainWindow mainWindow;
        public static Пользователь currentUser;
        public static Сотрудники currentSotr;
        public static chetchikEntities DB = new chetchikEntities();
    }
}
