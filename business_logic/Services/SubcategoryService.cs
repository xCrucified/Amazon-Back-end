using System;
using System.Collections.Generic;
using business_logic.DTOs;
using business_logic.Interfaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using business_logic.Entities;
using AutoMapper;


namespace business_logic.Services
{
    public class SubcategoryService// : ISubcategoryService
    {
        private readonly IRepository<Subcategory> _subcategoriesRepository;
        private readonly IMapper _mapper;
        public SubcategoryService(IRepository<Subcategory> subcategoriesRepository, IMapper mapper)
        {
            _subcategoriesRepository = subcategoriesRepository;
            _mapper = mapper;
        }
        //public async Task<SubcategoryDto> CreateSubcategoryAsync(CreateSubcategoryModel model)
        //{
        //    var subcategory = _mapper.Map<Subcategory>(model);
        //    var createdSubcategory = await _subcategoriesRepository.CreateSubcategoryAsync(subcategory);
        //    return _mapper.Map<SubcategoryDto>(createdSubcategory);
        //}
        //public async Task DeleteSubcategoryAsync(int id)
        //{
        //    await _subcategoriesRepository.DeleteSubcategoryAsync(id);
        //}
        //public async Task<SubcategoryDto> EditSubcategoryAsync(EditSubcategoryModel model)
        //{   
        //    var subcategory = _mapper.Map<Subcategory>(model);
        //    var editedSubcategory = await _subcategoriesRepository.EditSubcategoryAsync(subcategory);
        //    return _mapper.Map<SubcategoryDto>(editedSubcategory);
        //}
        //public async Task<IEnumerable<SubcategoryDto>> GetSubcategoriesAsync()
        //{
        //    var subcategories = await _subcategoriesRepository.GetSubcategoriesAsync();
        //    return _mapper.Map<IEnumerable<SubcategoryDto>>(subcategories);
        //}
        //public async Task<SubcategoryDto> GetSubcategoryByIdAsync(int id)
        //{
        //    var subcategory = await _subcategoriesRepository.GetSubcategoryByIdAsync(id);
        //    return _mapper.Map<SubcategoryDto>(subcategory);
        //}
    }
}
