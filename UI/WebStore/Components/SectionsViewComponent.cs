using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionsViewComponent(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        // при асинхронной логике работы
        //public async Task<IViewComponentResult> Invoke() => View();

        // при синхронной логике работы
        public IViewComponentResult Invoke(string sectionId)
        {
            var section_id = int.TryParse(sectionId, out var id) ? id : (int?)null;

            var sections = GetSections(section_id, out var parent_section_id);

            return View(new SelectableSectionsViewModel
            {
                Sections = sections,
                CurrentSectionId = section_id,
                ParentSectionId = parent_section_id
            });
        }

        private IEnumerable<SectionViewModel> GetSections(int? sectionId, out int? parentSectionId)
        {
            parentSectionId = null;

            var sections = _ProductData.GetSections().ToArray();

            var parent_sections = sections.Where(s => s.ParentId is null);

            var parent_sections_views = parent_sections
                .Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order
                })
                .ToList();

            foreach (var parent_section in parent_sections_views)
            {
                var childs = sections.Where(s => s.ParentId == parent_section.Id);

                foreach (var child_section in childs)
                {
                    if (child_section.Id == sectionId)
                        parentSectionId = child_section.ParentId;

                    parent_section.ChildSections.Add(new SectionViewModel
                    {
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        ParentSection = parent_section
                    });
                }

                parent_section.ChildSections.Sort((a, b) => Comparer<double>.Default.Compare(a.Order, b.Order));
            }

            parent_sections_views.Sort((a, b) => Comparer<double>.Default.Compare(a.Order, b.Order));
            return parent_sections_views;
        }

        private IEnumerable<SectionViewModel> GetSections()
        {
            var sections = _ProductData.GetSections();

            var parent_sections = sections.Where(s => s.ParentId is null);

            var parent_sections_views = parent_sections
               .Select(s => new SectionViewModel
               {
                   Id = s.Id,
                   Name = s.Name,
                   Order = s.Order,
               })
               .ToList();

            foreach (var parent_section in parent_sections_views)
            {
                var childs = sections.Where(s => s.ParentId == parent_section.Id);

                foreach (var child_section in childs)
                    parent_section.ChildSections.Add(new SectionViewModel
                    {
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        ParentSection = parent_section
                    });

                parent_section.ChildSections.Sort((a, b) => Comparer<double>.Default.Compare(a.Order, b.Order));
            }

            parent_sections_views.Sort((a, b) => Comparer<double>.Default.Compare(a.Order, b.Order));

            return parent_sections_views;
        }
    }
}
