using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Data
{
    interface ISQLite
    {
        string GetDatabasePath(string filename);
    }
}
