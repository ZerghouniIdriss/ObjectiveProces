using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace OkrsEntreprise.DAL.EntityFrameworkConfiguration
{
    internal class TenantCommandInterceptor : IDbCommandInterceptor
    {
        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            SetTenantParameterValue(command);
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext) { }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            SetTenantParameterValue(command);
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext) { }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            SetTenantParameterValue(command);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) { }

        private static void SetTenantParameterValue(DbCommand command)
        {
            // var identity = HttpContext.Current.User; 

            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            if ((command == null) || (command.Parameters.Count == 0) || identity == null)
            {
                return;
            }

            //var userClaim = identity.Claims.SingleOrDefault(c => c.Value=="TenantId");
            var userClaim =  identity.FindFirst("TenantId");
            if (userClaim != null  && userClaim.Value != String.Empty  )
            {
                var tenantId = userClaim.Value;
                // Enumerate all command parameters and assign the correct value in the one we added inside query visitor
                foreach (DbParameter param in command.Parameters)
                {
                    if (param.ParameterName != TenantAwareAttribute.TenantIdFilterParameterName)
                        return;
                    param.Value = tenantId;
                }
            }
        }
    }
}