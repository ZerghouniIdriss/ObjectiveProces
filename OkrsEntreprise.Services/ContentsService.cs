using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;

namespace OkrsEntreprise.Services
{
    public interface IContentsService
    {

        void Add(Content ontent); 
        void Update(params Content[] contents);
        void Update(Content content);
        void Remove(params Content[] contents);
        void Remove(  Content  content);


        Content GetById(long id);
        IList<Content> GetAll(); 
 

    }

    public class ContentsService : CRUDServiceBase<ContentRepository, Content>, IContentsService
    {

        private IContentRepository ContentRepository;
        private IActivityService _activityService;

        public ContentsService(IContentRepository ContentRepository, IActivityService activityService)
        {
            this.ContentRepository = ContentRepository;
            _activityService = activityService;
        }

       

        public Content GetById(long id)
        {
            return ContentRepository.GetSingle(x => x.Id == id);
        }

        public IList<Content> GetAll()
        {
            return ContentRepository.GetAll();
        }

      
    }
}
