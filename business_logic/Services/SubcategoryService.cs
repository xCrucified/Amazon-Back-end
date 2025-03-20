using System;
using System.Collections.Generic;
using business_logic.DTOs;
using business_logic.Interfaces;
using business_logic.Specifications;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using business_logic.Entities;
using AutoMapper;
using System.Net;


namespace business_logic.Services
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly IRepository<Subcategory> _subcategoriesR;
        private readonly IMapper _mapper;
        public SubcategoryService(IRepository<Subcategory> subcategoriesRepository, IMapper mapper)
        {
            this._subcategoriesR = subcategoriesRepository;
            _mapper = mapper;
        }

        public void Create(CreateSubcategoryModel model)
        {
            var obj = _mapper.Map<Subcategory>(model);

            _subcategoriesR.Insert(obj);
            _subcategoriesR.Save();
        }

        public async Task Delete(int id)
        {
            var subcategory = _mapper.Map<SubcategoryDto>(Get(id));

            _subcategoriesR.Delete(id);
            _subcategoriesR.Save();
        }

        public async Task Edit(EditSubcategoryModel model)
        {
            var subcategory =  _mapper.Map<Subcategory>(model);
            if (subcategory == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            _subcategoriesR.Update(subcategory);
            _subcategoriesR.Save();
        }

        public IEnumerable<SubcategoryDto> GetAll()
        {
            var subcategories = _subcategoriesR.GetAll() ?? Enumerable.Empty<Subcategory>();
            return _mapper.Map<IEnumerable<SubcategoryDto>>(subcategories);
        }

        public async Task<SubcategoryDto> Get(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var subcategory = await _subcategoriesR.GetItemBySpec(new SubcategorySpecs.ById(id));
            if (subcategory == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            return _mapper.Map<SubcategoryDto>(subcategory);
        }
    }
}
