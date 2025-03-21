﻿namespace BookAPI.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<IEnumerable<string>> GetLanguagesAsync();
        Task<string> GetLanguageByIdAsync(int id);
    }
}
