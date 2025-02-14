using BookAPI.Models;
using BookAPI.Services.Interfaces;

namespace BookAPI.Services
{
    public class LanguageService : ILanguageService
    {
        public Task<IEnumerable<string>> GetLanguagesAsync()
        {
            var languages = Enum.GetNames(typeof(Language));
            return Task.FromResult(languages.AsEnumerable());
        }

        public Task<string> GetLanguageByIdAsync(int id)
        {
            var language = Enum.GetName(typeof(Language), id);
            return Task.FromResult(language ?? Language.OTHER.ToString());
        }
    }
}
