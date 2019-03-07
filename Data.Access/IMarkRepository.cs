using System.Collections.Generic;

namespace Data.Access
{
    public interface IMarkRepository
    {
        MarkModel GetById(string id);
        IEnumerable<MarkModel> GetAll();
        IEnumerable<MarkModel> GetAllDemo();
    }
}
