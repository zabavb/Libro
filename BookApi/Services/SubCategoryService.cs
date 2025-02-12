using AutoMapper;
using BookApi.Models;
using BookApi.Repositories;
using BookAPI.Repositories.Interfaces;
using Library.Extensions;

namespace BookApi.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IMapper _mapper;

        public SubCategoryService(ISubCategoryRepository subCategoryRepository, IMapper mapper)
        {
            _subCategoryRepository = subCategoryRepository;
            _mapper = mapper;
        }

        public async Task<SubCategoryDto> CreateSubCategoryAsync(SubCategoryDto subCategoryDto)
        {
            var subCategory = _mapper.Map<SubCategory>(subCategoryDto);
            subCategory.Id = Guid.NewGuid();

            await _subCategoryRepository.CreateAsync(subCategory);

            return _mapper.Map<SubCategoryDto>(subCategory);
        }

        public async Task<bool> DeleteSubCategoryAsync(Guid id)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(id);
            if (subCategory is null) return false;

            await _subCategoryRepository.DeleteAsync(id);
            return true;
        }

        public async Task<PaginatedResult<SubCategoryDto>> GetSubCategoriesAsync(int pageNumber, int pageSize)
        {
            var subCategories = await _subCategoryRepository.GetAllAsync(pageNumber, pageSize);
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


        public async Task<SubCategoryDto> GetSubCategoryByIdAsync(Guid id)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(id);
            return subCategory is not null ? _mapper.Map<SubCategoryDto>(subCategory) : null;
        }

        public async Task<SubCategoryDto> UpdateSubCategoryAsync(Guid id, SubCategoryDto subCategoryDto)
        {
            var existingSubCategory = await _subCategoryRepository.GetByIdAsync(id);

            if (existingSubCategory == null)
            {
                return null;
            }

            _mapper.Map(subCategoryDto, existingSubCategory);
            await _subCategoryRepository.UpdateAsync(existingSubCategory);

            return _mapper.Map<SubCategoryDto>(existingSubCategory);
        }
    }
}
