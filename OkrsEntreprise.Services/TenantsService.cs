using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.Services
{
    public interface ITenantsService
    {
        Tenant GetById(long id);
        IList<Tenant> GetList(Func<Tenant, bool> where, params Expression<Func<Tenant, object>>[] navigationProperties);
        IEnumerable GetAll();
        void RemoveTenant(params Tenant[] tenants);
        void UpdateTenant(params Tenant[] tenants);
     
        Tenant AddTenant(Tenant tenant);
        Tenant GetTenatByCompanyName(string CompanyName);

    }
    public class TenantsService :  ITenantsService
    {
        private ITenantRepository _tenantRepository;
       

        public TenantsService(ITenantRepository tenantRepository )
        {
            this._tenantRepository = tenantRepository; 
        }

        public Tenant GetById(long id)
        {
            return _tenantRepository.GetSingle(x => x.Id == id);
        } 

        public IList<Tenant> GetList(Func<Tenant, bool> where, params Expression<Func<Tenant, object>>[] navigationProperties)
        {
            return _tenantRepository.GetList(where, navigationProperties);
        }

        public IEnumerable GetAll()
        {
           return this._tenantRepository.GetAll();
        }

        public Tenant AddTenant(Tenant tenant)
        {
            return _tenantRepository.AddTenantWithReturnValue(tenant);
        }
        public Tenant GetTenatByCompanyName(string CompanyName)
        {
           return _tenantRepository.GetTenatIdByCompanyName(CompanyName);
        } 
        public void UpdateTenant(params Tenant[] tenants)
        {
            _tenantRepository.Update(tenants); 
        }
        public void RemoveTenant(params Tenant[] tenants)
        {
            _tenantRepository.Remove(tenants); 
        }

    }
}
