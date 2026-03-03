using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Services;

namespace RMS.Web.Pages.Students;

public class IndexModel : PageModel
{
    private readonly IStudentService _studentService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IStudentService studentService, ILogger<IndexModel> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    public IEnumerable<Student> Students { get; set; } = new List<Student>();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                Students = await _studentService.SearchAsync(SearchString);
            }
            else
            {
                Students = await _studentService.GetAllAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving students");
            ModelState.AddModelError(string.Empty, "Error retrieving students.");
        }
    }
}
