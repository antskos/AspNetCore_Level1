using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class SitemapController : Controller
    {
        public IActionResult Index([FromServices]IProductData productData)
        {
            // узлы со статическим содержимым
            var nodes = new List<SitemapNode>()
            {
                new SitemapNode(Url.Action("Index", "Home")),
                new SitemapNode(Url.Action("ContactUs", "Home")),
                new SitemapNode(Url.Action("Blogs", "Home")),
                new SitemapNode(Url.Action("BlogSingle", "Home")),
                new SitemapNode(Url.Action("Shop", "Catalog")),
                new SitemapNode(Url.Action("Index", "WebApiTest")),
            };

            // узлы с динамическим содержимым
            nodes.AddRange(productData.GetSections().Select(s => new SitemapNode(Url.Action("Shop", "Catalog", new { SectionId = s.Id }))));

            foreach (var brand in productData.GetBrands())
                nodes.Add(new SitemapNode(Url.Action("Shop", "Catalog", new { BrandId = brand.Id })));

            foreach (var product in productData.GetProducts())
                nodes.Add(new SitemapNode(Url.Action("Details", "Catalog", new { product.Id })));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
