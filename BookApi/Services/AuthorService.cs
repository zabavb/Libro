﻿using AutoMapper;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using BookAPI.Services.Interfaces;
using Library.Common;

namespace BookAPI.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AuthorService> _logger;
        S3StorageService _storageService;

        public AuthorService(
            ILogger<AuthorService> logger,
            IMapper mapper,
            IAuthorRepository authorRepository,
            S3StorageService storageService)
        {
            _mapper = mapper;
            _authorRepository = authorRepository;
            _logger = logger;
            _storageService = storageService;
        }

        public async Task<PaginatedResult<AuthorDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm,
            AuthorFilter? filter, AuthorSort? sort)
        {
            var authors = await _authorRepository.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);
            if (authors == null || authors.Items == null)
            {
                _logger.LogWarning("No authors found");
                throw new InvalidOperationException("Failed to fetch authors.");
            }

            return new PaginatedResult<AuthorDto>
            {
                Items = _mapper.Map<ICollection<AuthorDto>>(authors.Items),
                TotalCount = authors.TotalCount,
                PageNumber = authors.PageNumber,
                PageSize = pageSize
            };
        }

        public async Task<AuthorDto> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                _logger.LogWarning($"No author with id {id}");
                return null;
            }

            _logger.LogInformation($"Successfully found author with id {id}");
            return _mapper.Map<AuthorDto>(author);
        }

        public async Task /*<AuthorDto>*/ CreateAsync(AuthorRequest request)
        {
            var author = _mapper.Map<Author>(request);
            author.Id = Guid.NewGuid();

            var filesHelper = new FilesHelper(_storageService, "libro-book");

            if (request.Image != null)
            {
                author.ImageUrl = await filesHelper.UploadImageFromFormAsync(request.Image, author.Id, GlobalConstants.authorFolderImage);
            }

            try
            {
                await _authorRepository.CreateAsync(author);
                _logger.LogInformation($"Successfully created author with id {request.AuthorId}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to create author. Error: {ex.Message}");
            }

            // return _mapper.Map<AuthorDto>(author);
        }

        public async Task /*<AuthorDto>*/ UpdateAsync(Guid id, AuthorRequest request)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(id);
            var filesHelper = new FilesHelper(_storageService, "libro-book");

            if (existingAuthor == null)
            {
                _logger.LogWarning($"UpdateAuthorAsync returns null");
                // return null;
            }

            try
            {
                _mapper.Map(request, existingAuthor);

                if (!string.IsNullOrEmpty(existingAuthor.ImageUrl) && request.Image != null)
                {
                    await _storageService.DeleteAsync(GlobalConstants.bucketName, existingAuthor.ImageUrl);
                    existingAuthor.ImageUrl = null;
                }

                if (request.Image != null)
                {
                    existingAuthor.ImageUrl = await filesHelper.UploadImageFromFormAsync(request.Image, id, GlobalConstants.authorFolderImage);
                }

                await _authorRepository.UpdateAsync(existingAuthor);
                _logger.LogInformation($"Successfully updated author with id {request.AuthorId}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to update author. Error: {ex.Message}");
            }

            // return _mapper.Map<AuthorDto>(existingAuthor);
        }

        public async Task /*<bool>*/ DeleteAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                _logger.LogWarning($"DeleteAuthorAsync returns null");
                // return false;
            }

            try
            {
                if (!string.IsNullOrEmpty(author.ImageUrl))
                {
                    await _storageService.DeleteAsync(GlobalConstants.bucketName, author.ImageUrl);
                }

                await _authorRepository.DeleteAsync(id);
                _logger.LogInformation($"Successfully deleted author with id {id}");
                // return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to delete autor. Error: {ex.Message}");
                // return false;
            }
        }
    }
}