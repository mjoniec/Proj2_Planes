using Data.Access.Contexts;
using Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace Data.Access
{
    /*internal - test avaliability*/
    public class MarkRepository : IMarkRepository
    {
        MarkContext _markContext;

        public MarkRepository(MarkContext markContext/*TODO config*/)
        {
            _markContext = markContext;
        }

        public MarkModel GetById(string id)
        {
            return _markContext.Marks.Find(id);
        }

        public IEnumerable<MarkModel> GetAll()
        {
            return _markContext.Marks.ToList();
        }

        /// <summary>
        /// Inserts demo data if empty table, then returns all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MarkModel> GetAllDemo()
        {
            if (!_markContext.Marks.Any())
            {
                var marks = new List<MarkModel>
                {
                    new MarkModel { Id = "1", X = 2, Y = 1 },
                    new MarkModel { Id = "2", X = 3, Y = 4 }
                };

                _markContext.Marks.AddRange(marks);
                _markContext.SaveChanges();
            }

            return GetAll();
        }
    }
}
