using OkrsEntreprise.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Framework.Photo
{
    public interface IPhotoManager
    {
        IEnumerable<PhotoViewModel> Get();
        PhotoActionResult Delete(string fileName);
        Task<IEnumerable<PhotoViewModel>> Add(HttpRequestMessage request);
        bool FileExists(string fileName);
    }
}
