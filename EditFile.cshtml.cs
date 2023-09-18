using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XeniaWebServices
{
    public class EditFileModel : PageModel
    {
        [BindProperty]
        public string TitleId { get; set; }

        [BindProperty]
        public string FileType { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Ensure that TitleId and FileType are not empty
            if (string.IsNullOrEmpty(TitleId) || string.IsNullOrEmpty(FileType))
            {
                ModelState.AddModelError("", "Title ID and File Type are required.");
                return Page();
            }

            // Define the file path based on TitleId and FileType
            var filePath = $"title/{TitleId}/{FileType}/{FileType}.json";

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                ModelState.AddModelError("", "The specified file does not exist.");
                return Page();
            }

            // You can add your file editing logic here

            // For example, you can read the file content
            var fileContent = await System.IO.File.ReadAllTextAsync(filePath);

            // Process or edit the file content as needed
            // ...

            // Write the updated content back to the file
            await System.IO.File.WriteAllTextAsync(filePath, fileContent);

            return RedirectToPage("/Index"); // Redirect to another page after editing
        }
    }
}