using AutoMapper;
using BLL.DTOs;
using BLL.Entities;
using BLL.Interfaces;
using BLL.Specifications;
using System.Net;

namespace BLL.Services
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly IRepository<Subcategory> _subcategoriesRepo;
        private readonly IMapper _mapper;

        public SubcategoryService(IRepository<Subcategory> subcategoriesRepository, IMapper mapper)
        {
            _subcategoriesRepo = subcategoriesRepository;
            _mapper = mapper;
        }

        public async Task Create(CreateSubcategoryModel model)
        {
            var obj = _mapper.Map<Subcategory>(model);

            await _subcategoriesRepo.InsertAsync(obj);
            await _subcategoriesRepo.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new HttpException("Subcategory not found.", HttpStatusCode.BadRequest);

            var subcategoryToDelete = await _subcategoriesRepo.GetItemBySpec(new SubcategorySpecs.ById(id));

            if (subcategoryToDelete == null)
                throw new HttpException("Subcategory not found.", HttpStatusCode.NotFound);

            await _subcategoriesRepo.DeleteAsync(subcategoryToDelete);
            await _subcategoriesRepo.SaveChangesAsync();
        }

        public async Task Edit(EditSubcategoryModel model)
        {
            var existingSubcategory = await _subcategoriesRepo.GetItemBySpec(new SubcategorySpecs.ById(model.Id));

            if (existingSubcategory == null)
                throw new HttpException("Subcategory not found.", HttpStatusCode.NotFound);

            _mapper.Map(model, existingSubcategory);

            await _subcategoriesRepo.UpdateAsync(existingSubcategory);
            await _subcategoriesRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<SubcategoryDto>> GetAll()
        {
            var subcategories = await _subcategoriesRepo.GetListBySpec(new SubcategorySpecs.All());
            return _mapper.Map<List<SubcategoryDto>>(subcategories);
        }

        public async Task<SubcategoryDto> Get(int id)
        {
            if (id <= 0)
                throw new HttpException("Subcategory not found.", HttpStatusCode.BadRequest);

            var subcategory = await _subcategoriesRepo.GetItemBySpec(new SubcategorySpecs.ById(id));
            if (subcategory == null)
                throw new HttpException("Subcategory not found.", HttpStatusCode.NotFound);

            return _mapper.Map<SubcategoryDto>(subcategory);
        }

        public async Task<IEnumerable<SubcategoryDto>> GetSubcategoriesByCategoryAsync(int id)
        {
            var subcategories = await _subcategoriesRepo.GetListBySpec(new SubcategorySpecs.ByCategory(id));

            if (subcategories == null || !subcategories.Any())
                throw new HttpException("Subcategories not found for the given category.", HttpStatusCode.NotFound);

            return _mapper.Map<IEnumerable<SubcategoryDto>>(subcategories);
        }

        public async Task<IEnumerable<SubcategoryDto>> GetSubcategoriesByCategoryNameAsync(string categoryName)
        {
            var subcategories = await _subcategoriesRepo.GetListBySpec(new SubcategorySpecs.ByCategoryName(categoryName));

            if (subcategories == null || !subcategories.Any())
                throw new HttpException("Subcategories not found for the given category name.", HttpStatusCode.NotFound);

            return _mapper.Map<IEnumerable<SubcategoryDto>>(subcategories);
        }
    }
}