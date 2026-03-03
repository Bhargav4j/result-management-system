using Microsoft.AspNetCore.Mvc.RazorPages;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Services;

namespace RMS.Web.Pages.Courses;

public class IndexModel : PageModel
{
    private readonly ICourseService _courseService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ICourseService courseService, ILogger<IndexModel> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    public IEnumerable<Course> Courses { get; set; } = new List<Course>();

    public async Task OnGetAsync()
    {
        try
        {
            Courses = await _courseService.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving courses");
            ModelState.AddModelError(string.Empty, "Error retrieving courses.");
        }
    }
}
