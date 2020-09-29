using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data
{
    public class ConexionesERP
    {
        public static string GetConnectionString()
        {
            
            string ip = "fixed.database.windows.net";
            string password = "admin*2016";
            string user = "administrador";
            string InitialCatalog = "AgricolaYTransporte";
            
            /*
            string ip = "localhost";
            string password = "admin*2016";
            string user = "sa";
            string InitialCatalog = "DBERP_GRAFINPREN";
            */
            /*
            string ip = "10.100.5.140";
            string password = "admin*2016";
            string user = "sa";
            string InitialCatalog = "DBERP";
            */
            return "data source=" + ip + ";initial catalog=" + InitialCatalog + ";user id=" + user + ";password=" + password + ";MultipleActiveResultSets=True;";
        }
    }
}
