using AutoMapper;
using BLL.DTOs;
using BLL.Entities;
using BLL.Interfaces;
using BLL.Specifications;
using System.Net;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category> _categoryRepo;

        public CategoryService(IMapper mapper, IRepository<Category> categoryRepo)
        {
            _mapper = mapper;
            _categoryRepo = categoryRepo;
        }

        public async Task Create(CreateCategoryModel categoryModel)
        {
            var categoryToInsert = _mapper.Map<Category>(categoryModel);
            await _categoryRepo.InsertAsync(categoryToInsert);
            await _categoryRepo.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new HttpException("Category not found.", HttpStatusCode.BadRequest);


            var categoryToDelete = await _categoryRepo.GetItemBySpec(new CategorySpecs.ById(id));

            if (categoryToDelete == null)
                throw new HttpException("Category not found.", HttpStatusCode.NotFound);

            await _categoryRepo.DeleteAsync(categoryToDelete);
            await _categoryRepo.SaveChangesAsync();
        }

        public async Task Edit(EditCategoryModel categoryEdit)
        {
            var existingCategory = await _categoryRepo.GetItemBySpec(new CategorySpecs.ById(categoryEdit.Id));

            if (existingCategory == null)
                throw new HttpException("Category not found.", HttpStatusCode.NotFound);

            _mapper.Map(categoryEdit, existingCategory);

            await _categoryRepo.UpdateAsync(existingCategory);
            await _categoryRepo.SaveChangesAsync();
        }

        public async Task<CategoryDto> Get(int id)
        {
            if (id <= 0)
                throw new HttpException("Category not found.", HttpStatusCode.BadRequest);

            var category = await _categoryRepo.GetItemBySpec(new CategorySpecs.ById(id));
            if (category == null)
                throw new HttpException("Category not found.", HttpStatusCode.NotFound);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = await _categoryRepo.GetListBySpec(new CategorySpecs.All());
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<IEnumerable<CategoryDto>> Get(IEnumerable<int> ids)
        {
            var categories = await _categoryRepo.GetListBySpec(new CategorySpecs.ByIds(ids));
            return _mapper.Map<List<CategoryDto>>(categories);
        }
    }
}