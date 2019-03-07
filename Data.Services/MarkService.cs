using Data.Model;
using Data.Repositories;
using System.Collections.Generic;

namespace Data.Services
{
    internal class MarkService : IMarkService
    {
        IMarkRepository _markRepository;

        public MarkService(IMarkRepository markRepository)
        {
            _markRepository = markRepository;
        }

        public MarkModel GetById(string id)
        {
            return _markRepository.GetById(id);
        }

        public IEnumerable<MarkModel> GetAll()
        {
            return _markRepository.GetAll();
        }

        public IEnumerable<MarkModel> GetAllDemo()
        {
            return _markRepository.GetAllDemo();
        }
    }
}
