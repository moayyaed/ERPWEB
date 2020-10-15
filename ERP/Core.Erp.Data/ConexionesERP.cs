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
            string ip = string.Empty;
            string password = string.Empty;
            string user = string.Empty;
            string InitialCatalog = string.Empty;
            string Cadena = "WEB";
            switch (Cadena)
            {
                case "WEB":
                    ip = "fixed.database.windows.net";
                    password = "admin*2016";
                    user = "administrador";
                    InitialCatalog = "AgricolaYTransporte";
                    break;

                case "GRAFINPREN":
                    ip = "10.100.5.140";
                    password = "admin*2016";
                    user = "sa";
                    InitialCatalog = "DBERP";
                    break;

                case "LOCAL":
                    ip = "localhost";
                    password = "admin*2016";
                    user = "sa";
                    InitialCatalog = "DBERP_GRAFINPREN";
                    break;
            }
            
            
            return "data source=" + ip + ";initial catalog=" + InitialCatalog + ";user id=" + user + ";password=" + password + ";MultipleActiveResultSets=True;";
        }
    }
}
