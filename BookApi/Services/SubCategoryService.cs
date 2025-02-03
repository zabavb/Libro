using AutoMapper;
using BookApi.Models;
using BookApi.Repositories;
using BookAPI.Repositories;

namespace BookApi.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SubCategoryService> _logger;
        public SubCategoryService(ISubCategoryRepository subCategoryRepository, IMapper mapper, ILogger<SubCategoryService> logger)
        {
            _subCategoryRepository = subCategoryRepository;
            _mapper = mapper;
            _logger = logger;
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

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync()
        {
            var subCategories = await _subCategoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SubCategoryDto>>(subCategories);
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
