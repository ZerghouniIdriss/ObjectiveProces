using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface IContentRepository : IRepositoryBase<Content>
    {
    }

    public class ContentRepository : RepositoryBase<Content>, IContentRepository
    {
    }
}
