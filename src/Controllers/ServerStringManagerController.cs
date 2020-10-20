using Atomus.Database;
using System.Threading.Tasks;

namespace Atomus.Service.Controllers
{
    internal static class ServerStringManagerController
    {
        internal static IResponse SearchUserControlAssembly(this ICore core, decimal MENU_ID, decimal ASSEMBLY_ID, decimal USER_ID)
        {
            IServiceDataSet serviceDataSet;

            serviceDataSet = new ServiceDataSet
            {
                ServiceName = core.GetAttribute("ServiceName"),
                TransactionScope = false
            };
            serviceDataSet["OpenControl"].ConnectionName = core.GetAttribute("DatabaseName");
            serviceDataSet["OpenControl"].CommandText = core.GetAttribute("ProcedureSelect");
            serviceDataSet["OpenControl"].AddParameter("@MENU_ID", DbType.Decimal, 18);
            serviceDataSet["OpenControl"].AddParameter("@ASSEMBLY_ID", DbType.Decimal, 18);
            serviceDataSet["OpenControl"].AddParameter("@USER_ID", DbType.Decimal, 18);

            serviceDataSet["OpenControl"].NewRow();
            serviceDataSet["OpenControl"].SetValue("@MENU_ID", MENU_ID);
            serviceDataSet["OpenControl"].SetValue("@ASSEMBLY_ID", ASSEMBLY_ID);
            serviceDataSet["OpenControl"].SetValue("@USER_ID", USER_ID);

            return core.ServiceRequest(serviceDataSet);
        }
    }
}