using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Data
{
    interface ISQlite
    {
        string GetDatabasePath(string filename);
    }
}
