using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class ClaimsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ClaimsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Claim claim, IFormFile[] documents)
    {
        if (ModelState.IsValid)
        {
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync(); // Save to get the ClaimID

            // Handle file uploads
            if (documents != null)
            {
                foreach (var document in documents)
                {
                    if (document.Length > 0)
                    {
                        var filePath = Path.Combine("uploads", document.FileName);

                        // Create the uploads directory if it doesn't exist
                        if (!Directory.Exists("uploads"))
                        {
                            Directory.CreateDirectory("uploads");
                        }

                        // Save the uploaded file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await document.CopyToAsync(stream);
                        }

                        // Add the supporting document to the database
                        var supportingDocument = new SupportingDocument
                        {
                            ClaimID = claim.ClaimID, // Use the ClaimID from the newly created claim
                            FilePath = filePath
                        };
                        _context.SupportingDocuments.Add(supportingDocument);
                    }
                }
                await _context.SaveChangesAsync(); // Save changes to the database
            }
            return RedirectToAction(nameof(Index));
        }
        return View(claim);
    }

    [HttpGet]
    public IActionResult Index()
    {
        var claims = _context.Claims.ToList();
        return View(claims);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim != null)
        {
            claim.Status = "Approved";
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim != null)
        {
            claim.Status = "Rejected";
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
