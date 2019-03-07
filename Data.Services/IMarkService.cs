using Data.Model;
using System.Collections.Generic;

namespace Data.Services
{
    public interface IMarkService
    {
        MarkModel GetById(string id);
        IEnumerable<MarkModel> GetAll();
        IEnumerable<MarkModel> GetAllDemo();
    }
}
