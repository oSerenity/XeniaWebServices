using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XeniaWebServices
{
    public class IndexModel : PageModel
    {
        public static List<ApiItem> ApiData { get; set; } = new List<ApiItem>
            {
                new ApiItem { Id = 1, Name = "API 1", Description = "Description for API 1" },
                new ApiItem { Id = 2, Name = "API 2", Description = "Description for API 2" },
                new ApiItem { Id = 3, Name = "API 3", Description = "Description for API 3" }
            };
        public class ApiItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public void OnGet()
        {

        }
    }
}
