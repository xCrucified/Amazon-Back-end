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
        private readonly IRepository<Subcategory> _subcategoriesRepository;
        private readonly IMapper _mapper;
        public SubcategoryService(IRepository<Subcategory> subcategoriesRepository, IMapper mapper)
        {
            this._subcategoriesRepository = subcategoriesRepository;
            _mapper = mapper;
        }

        public void Create(CreateSubcategoryModel model)
        {
            var obj = _mapper.Map<Subcategory>(model);

            _subcategoriesRepository.Insert(obj);
            _subcategoriesRepository.Save();
        }

        public async Task Delete(int id)
        {
            var subcategory = await _subcategoriesRepository.GetItemBySpec(new SubcategorySpecs.ById(id));
            if (subcategory == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);
            _subcategoriesRepository.Delete(id);
            _subcategoriesRepository.Save();
        }

        public async Task Edit(EditSubcategoryModel model)
        {
            var subcategory =  _mapper.Map<Subcategory>(model);
            if (subcategory == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            _subcategoriesRepository.Update(subcategory);
            _subcategoriesRepository.Save();
        }

        public IEnumerable<SubcategoryDto> GetAll()
        {
            return _mapper.Map<IEnumerable<SubcategoryDto>>(_subcategoriesRepository.GetAll());
        }

        public async Task<SubcategoryDto> GetById(int id)
        {
            if (id < 0) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            var product = await _subcategoriesRepository.GetItemBySpec(new SubcategorySpecs.ById(id));
            if (product == null) throw new HttpException(Errors.ItemNotFound, HttpStatusCode.BadRequest);

            return _mapper.Map<SubcategoryDto>(product);
        }
    }
}
