using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace StarCatalog.DataAccessLayer
{
    /// <summary>
    /// Настраивает провайдер SQL Server для Entity Framework 6.
    /// </summary>
    public class StarDbConfiguration : DbConfiguration
    {
        /// <summary>
        /// Регистрирует службы провайдера SQL Server.
        /// </summary>
        public StarDbConfiguration()
        {
            SetProviderServices(
                "System.Data.SqlClient",
                SqlProviderServices.Instance);
        }
    }
}