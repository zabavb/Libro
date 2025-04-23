using AutoMapper;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using BookAPI.Services.Interfaces;
using Library.Common;

namespace BookAPI.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SubCategoryService> _logger;

        public SubCategoryService(ISubCategoryRepository subCategoryRepository, IMapper mapper,
            ILogger<SubCategoryService> logger)
        {
            _subCategoryRepository = subCategoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task /*<SubCategoryDto>*/ CreateAsync(SubCategoryDto subCategoryDto)
        {
            var subCategory = _mapper.Map<SubCategory>(subCategoryDto);
            subCategory.Id = Guid.NewGuid();

            await _subCategoryRepository.CreateAsync(subCategory);

            // return _mapper.Map<SubCategoryDto>(subCategory);
        }

        public async Task /*<bool>*/ DeleteAsync(Guid id)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(id);
            // if (subCategory is null) return false;

            await _subCategoryRepository.DeleteAsync(id);
            // return true;
        }

        public async Task<PaginatedResult<SubCategoryDto>> GetAllAsync(
            int pageNumber, int pageSize, string? searchTerm,
            SubCategoryFilter? filter, SubCategorySort? sort)
        {
            var subCategories =
                await _subCategoryRepository.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);
            if (subCategories == null || subCategories.Items == null)
            {
                throw new InvalidOperationException("Failed to fetch subcategories.");
            }

            return new PaginatedResult<SubCategoryDto>
            {
                Items = _mapper.Map<ICollection<SubCategoryDto>>(subCategories.Items),
                TotalCount = subCategories.TotalCount,
                PageNumber = subCategories.PageNumber,
                PageSize = subCategories.PageSize
            };
        }


        public async Task<SubCategoryDto> GetByIdAsync(Guid id)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(id);
            return subCategory is not null ? _mapper.Map<SubCategoryDto>(subCategory) : null;
        }

        public async Task /*<SubCategoryDto>*/ UpdateAsync(SubCategoryDto subCategoryDto)
        {
            var existingSubCategory = await _subCategoryRepository.GetByIdAsync(subCategoryDto.SubCategoryId);

            if (existingSubCategory == null)
            {
                // return null;
            }

            _mapper.Map(subCategoryDto, existingSubCategory);
            await _subCategoryRepository.UpdateAsync(existingSubCategory);

            // return _mapper.Map<SubCategoryDto>(existingSubCategory);
        }
    }
}