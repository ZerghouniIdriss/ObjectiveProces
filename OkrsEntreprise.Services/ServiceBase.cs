using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.Services
{
    public interface IServiceBase<TEntity>
    {
        void Add(TEntity activity);
        void Update(params TEntity[] departments);
        void Remove(params TEntity[] departments);

        TEntity GetById(long id); 
        IList<TEntity> GetAll();

    }
    public class CRUDServiceBase<TRepository,TEntity>: IServiceBase<TEntity> where TRepository : RepositoryBase<TEntity>, new() where TEntity: EntityBase
    {
        private TRepository tRepository;

        public CRUDServiceBase()
        {
            tRepository = new TRepository();
        }

        public TRepository Repository
        {
            get { return tRepository; }
        }

        public virtual void Add(TEntity tEntity)
        {
            Repository.Add(tEntity);
        }

        public virtual void Update(params TEntity[] tEntity)
        {
            Repository.Update(tEntity);
        }

        public virtual void Update(TEntity tEntity)
        {
            Repository.Update(tEntity);
        }

        public virtual void Remove(params TEntity[] tEntity)
        {
            Repository.Remove(tEntity);
        }

        public virtual void Remove(TEntity  tEntity)
        {
            Repository.Remove(tEntity);
        }

         
        public virtual TEntity GetById(long id)
        {
            return Repository.GetSingle(x => x.Id == id);
        }

        public virtual IList<TEntity> GetAll()
        {
            return Repository.GetAll();
        }

    }
}
