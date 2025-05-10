using BookAPI.Models;
using BookAPI.Services.Interfaces;
using Library.DTOs.Book;

namespace BookAPI.Services
{
    public class LanguageService : ILanguageService
    {
        public Task<IEnumerable<string>> GetAllAsync()
        {
            var languages = Enum.GetNames(typeof(Language));
            return Task.FromResult(languages.AsEnumerable());
        }

        public Task<string> GetByIdAsync(int id)
        {
            var language = Enum.GetName(typeof(Language), id);
            return Task.FromResult(language ?? Language.OTHER.ToString());
        }
    }
}
