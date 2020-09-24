using AutoMapper;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Employees;
using WebStore.Domain.ViewModels;

namespace WebStore.Infrastructure.AutoMapperProfiles
{
    public class ViewModelsMapping : Profile
    {
        public ViewModelsMapping()
        {
            CreateMap<Product, ProductViewModel>().
                ForMember(vm => vm.Brand, opt => opt.MapFrom(pr => pr.Brand.Name)).
                ReverseMap();       // данные будут передаваться и в обратную сторону

            CreateMap<Employee, EmployeeViewModel>().
                ReverseMap();       // если классы одинаковы, то не надо указывать какие свойства на какие проецировать
        }
    }
}
