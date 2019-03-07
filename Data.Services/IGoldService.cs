using System;
using System.Collections.Generic;

namespace Data.Services
{
    public interface IGoldService
    {
        IEnumerable<string> GetAll();
        string GetForToday();
        DateTime GetFirstDate();
    }
}
