using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace project.Controllers
{
    public class TeacherController : Controller
    {
        [Authorize(Roles = "Teacher")]
        public IActionResult Index()
        {
            var email = User.Identity.Name;
            var teacher = _context.Teachers.FirstOrDefault(t => t.Email == email);

            if (teacher == null) return RedirectToAction("Login", "Account");

            return View(teacher);
        }
        [Authorize(Roles = "Teacher")]
        public IActionResult MyClasses()
        {
            var email = User.Identity.Name;
            var teacher = _context.Teachers.FirstOrDefault(t => t.Email == email);

            if (teacher == null) return NotFound();

            var classes = _context.ClassAssignments
                .Where(ca => ca.TeacherId == teacher.TeacherId)
                .Include(ca => ca.Class).ThenInclude(c => c.Course)
                .ToList();

            return View(classes);
        }
        [Authorize(Roles = "Teacher")]
        public IActionResult ViewClassStudents(int classId)
        {
            var students = _context.ClassStudents
                .Where(cs => cs.ClassId == classId)
                .Include(cs => cs.Student)
                .ToList();

            var classInfo = _context.Classes
                .Include(c => c.Course)
                .FirstOrDefault(c => c.ClassId == classId);

            ViewBag.ClassInfo = classInfo;

            return View(students);
        }
        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult AssignGrade(int classId)
        {
            var students = _context.ClassStudents
                .Where(cs => cs.ClassId == classId)
                .Include(cs => cs.Student)
                .ToList();

            ViewBag.ClassId = classId;
            return View(students);
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignGrade(int studentId, int classId, double grade)
        {
            var record = await _context.ClassStudents
                .FirstOrDefaultAsync(cs => cs.ClassId == classId && cs.StudentId == studentId);

            if (record == null) return NotFound();

            record.Grade = grade;
            await _context.SaveChangesAsync();

            return RedirectToAction("AssignGrade", new { classId });
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveStudentFromClass(int studentId, int classId)
        {
            var record = await _context.ClassStudents
                .FirstOrDefaultAsync(cs => cs.ClassId == classId && cs.StudentId == studentId);

            if (record != null)
            {
                _context.ClassStudents.Remove(record);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewClassStudents", new { classId });
        }


    }
}
