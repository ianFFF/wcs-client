using SqlSugar;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;

namespace Wcs.Framework.Service
{
    public partial class FileService : BaseService<FileEntity>, IFileService
    {
        public FileService(IRepository<FileEntity> repository) : base(repository)
        {
        }
    }
}
