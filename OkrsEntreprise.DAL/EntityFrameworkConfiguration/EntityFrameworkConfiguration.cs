using System.Data.Entity;

namespace OkrsEntreprise.DAL.EntityFrameworkConfiguration
{
    public class EntityFrameworkConfiguration : DbConfiguration
    {
 
        public EntityFrameworkConfiguration(  )
        {
            AddInterceptor(new TenantCommandInterceptor( ));
            AddInterceptor(new TenantCommandTreeInterceptor());
        }
    }
}