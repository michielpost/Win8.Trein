using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Models;

namespace ActueelNS.Services.Interfaces
{
    public interface ISearchHistoryService
    {

        Task<List<SearchHistory>> Add(SearchHistory h);
        Task<List<SearchHistory>> Delete(SearchHistory h);

        Task<List<SearchHistory>> GetAll();

        Task Clear();

        Task SaveList(List<SearchHistory> list);
    }
}
