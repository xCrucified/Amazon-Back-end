using AutoMapper;
using business_logic.DTOs;
using business_logic.Entities;
using business_logic.Interfaces;
using business_logic.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Category> categoryR;

        public CategoryService(IMapper mapper, IRepository<Category> categoryR)
        {
            this.mapper = mapper;
            this.categoryR = categoryR;
        }


        public void Create(CreateCategoryModel categoryModel)
        {
            categoryR.Insert(mapper.Map<Category>(categoryModel));
            categoryR.Save();
        }

        public async Task Delete(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            categoryR.Delete(id);
            categoryR.Save();
        }

        public async Task Edit(EditCategoryModel categoryEdit)
        {
            categoryR.Update(mapper.Map<Category>(categoryEdit));
            categoryR.Save();
        }


        public async Task<CategoryDto> Get(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var category = await categoryR.GetItemBySpec(new CategorySpecs.ById(id));
            if (category == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            return mapper.Map<CategoryDto>(category);
        }

        public IEnumerable<CategoryDto> GetAll()
        {
            return mapper.Map<List<CategoryDto>>(categoryR.GetAll());
        }

        async Task<IEnumerable<CategoryDto>> ICategoryService.Get(IEnumerable<int> ids)
        {
            return mapper.Map<List<CategoryDto>>(await categoryR.GetListBySpec(new CategorySpecs.ByIds(ids)));
        }
    }
}
