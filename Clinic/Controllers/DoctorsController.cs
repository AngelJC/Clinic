using Clinic.Data;
using Clinic.Models;
using Clinic.Models.Auth;
using Clinic.Models.Catalogs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Doctors
                .Select(x => new DoctorIndex
                {
                    Id = x.ApplicationUser!.Id,
                    Name = x.ApplicationUser!.Name,
                    PaternalSurname = x.ApplicationUser.PaternalSurname,
                    MaternalSurname = x.ApplicationUser.MaternalSurname,
                    Email = x.ApplicationUser.Email!,
                    PhoneNumber = x.ApplicationUser.PhoneNumber!,
                    SpecialtyId = x.SpecialtyId,
                    SpecialtyName = x.Specialty!.Name,
                    CreationDate = x.ApplicationUser.CreationDate,
                    Status = x.ApplicationUser.Status
                }).OrderBy(x => x.PaternalSurname)
                .ThenBy(x => x.MaternalSurname)
                .ThenBy(x => x.Name).ToListAsync());
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public async Task<IActionResult> Create()
        {
            var specialties = await _context.Specialty.AsNoTracking().Select(x => new
            {
                x.Id,
                x.Name,
            }).ToListAsync();

            ViewBag.Specialties = new SelectList(specialties, "Id", "Name");
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] DoctorCreate model, [FromServices] UserManager<ApplicationUser> userManager)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = new ApplicationUser
                {
                    Name = model.Name,
                    PaternalSurname = model.PaternalSurname,
                    MaternalSurname = model.MaternalSurname,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    UserName = model.Email, // Use email as username
                    CreationDate = DateTime.UtcNow,
                    Status = StatusCore.Active,
                };
                var result = await userManager.CreateAsync(applicationUser, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicationUser, "Doctor");
                    await userManager.AddToRoleAsync(applicationUser, "ChangePassword");
                    
                    // Save doctor details
                    var doctor = new Doctor
                    {
                        ApplicationUserId = applicationUser.Id,
                        SpecialtyId = model.SpecialtyId
                    };
                    _context.Doctors.Add(doctor);
                    await _context.SaveChangesAsync();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CreationDate,Status,Name,PaternalSurname,MaternalSurname,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser != null)
            {
                _context.Users.Remove(applicationUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
