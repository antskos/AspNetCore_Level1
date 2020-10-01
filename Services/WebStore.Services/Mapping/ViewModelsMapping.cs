using AutoMapper;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Employees;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Mapping
{
    public class ViewModelsMapping : Profile
    {
        public ViewModelsMapping()
        {
            CreateMap<ProductDTO, ProductViewModel>().
                ForMember(vm => vm.Brand, opt => opt.MapFrom(pr => pr.Brand.Name)).
                ReverseMap();       // данные будут передаваться и в обратную сторону

            CreateMap<Employee, EmployeeViewModel>().
                ReverseMap();       // если классы одинаковы, то не надо указывать какие свойства на какие проецировать
        }
    }
}
