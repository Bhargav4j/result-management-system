using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Services;

namespace RMS.Web.Pages.Students;

public class CreateModel : PageModel
{
    private readonly IStudentService _studentService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IStudentService studentService, ILogger<CreateModel> logger)
    {
        _studentService = studentService;
        _logger = logger;
        Input = new InputModel();
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string Gender { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(50)]
        [Phone]
        public string? Phone { get; set; }

        [Required]
        [StringLength(50)]
        public string Program { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Level { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Session { get; set; } = string.Empty;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var student = new Student
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                MiddleName = Input.MiddleName,
                Gender = Input.Gender,
                BirthDate = Input.BirthDate,
                Email = Input.Email,
                Phone = Input.Phone,
                Program = Input.Program,
                Level = Input.Level,
                Session = Input.Session,
                UserCreated = User.Identity?.Name ?? "System",
                DateCreated = DateTime.Now,
                IsActive = true
            };

            await _studentService.CreateAsync(student);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student");
            ModelState.AddModelError(string.Empty, "Error creating student.");
            return Page();
        }
    }
}
