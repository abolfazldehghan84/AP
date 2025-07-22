using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace project.Controllers
{
    public class StudentController : Controller
    {
        [Authorize(Roles = "Student")]
        public IActionResult Index()
        {
            var userEmail = User.Identity.Name;
            var student = _context.Students.FirstOrDefault(s => s.Email == userEmail);

            if (student == null)
                return RedirectToAction("Login", "Account");

            return View(student); // می‌تونه داشبورد یا خلاصه‌ای از اطلاعات باشه
        }

        [Authorize(Roles = "Student")]
        public IActionResult ViewMyCourses()
        {
            var userEmail = User.Identity.Name;
            var student = _context.Students.FirstOrDefault(s => s.Email == userEmail);

            if (student == null) return NotFound();

            var classes = _context.ClassStudents
                .Where(cs => cs.StudentId == student.StudentId)
                .Include(cs => cs.Class)
                    .ThenInclude(c => c.Course)
                .Include(cs => cs.Class)
                    .ThenInclude(c => c.ClassAssignments)
                        .ThenInclude(ca => ca.Teacher)
                .ToList();

            return View(classes);
        }
        [Authorize(Roles = "Student")]
        public IActionResult ViewCourseDetails(int classId)
        {
            var classItem = _context.Classes
                .Include(c => c.Course)
                .Include(c => c.TimeSlot)
                .Include(c => c.ClassAssignments).ThenInclude(a => a.Teacher)
                .FirstOrDefault(c => c.ClassId == classId);

            if (classItem == null)
                return NotFound();

            return View(classItem);
        }
        [Authorize(Roles = "Student")]
        public IActionResult ViewMyGrades()
        {
            var userEmail = User.Identity.Name;
            var student = _context.Students.FirstOrDefault(s => s.Email == userEmail);

            if (student == null) return NotFound();

            var grades = _context.ClassStudents
                .Where(cs => cs.StudentId == student.StudentId)
                .Include(cs => cs.Class)
                    .ThenInclude(c => c.Course)
                .ToList();

            return View(grades);
        }
        [Authorize(Roles = "Student")]
        public IActionResult ViewTranscript()
        {
            var userEmail = User.Identity.Name;
            var student = _context.Students
                .Include(s => s.Faculty)
                .FirstOrDefault(s => s.Email == userEmail);

            if (student == null) return NotFound();

            var passedCourses = _context.ClassStudents
                .Where(cs => cs.StudentId == student.StudentId && cs.Grade >= 10)
                .Include(cs => cs.Class).ThenInclude(c => c.Course)
                .ToList();

            var gpa = passedCourses.Any()
                ? passedCourses.Average(cs => cs.Grade.Value)
                : 0;

            ViewBag.GPA = gpa;

            return View(passedCourses);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnregisterFromClass(int classId)
        {
            var userEmail = User.Identity.Name;
            var student = _context.Students.FirstOrDefault(s => s.Email == userEmail);

            if (student == null) return NotFound();

            var enrollment = await _context.ClassStudents
                .FirstOrDefaultAsync(cs => cs.StudentId == student.StudentId && cs.ClassId == classId);

            if (enrollment != null)
            {
                _context.ClassStudents.Remove(enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewMyCourses");
        }
        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult AppealGrade(int classId)
        {
            return View(new GradeAppeal
            {
                ClassId = classId
            });
        }
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AppealGrade(GradeAppeal model)
        {
            var userEmail = User.Identity.Name;
            var student = _context.Students.FirstOrDefault(s => s.Email == userEmail);

            if (student == null || !_context.Classes.Any(c => c.ClassId == model.ClassId))
                return NotFound();

            model.StudentId = student.StudentId;
            model.Status = "Pending";
            model.SubmittedAt = DateTime.Now;

            _context.GradeAppeals.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("AppealStatus");
        }
        [Authorize(Roles = "Student")]
        public IActionResult AppealStatus()
        {
            var userEmail = User.Identity.Name;
            var student = _context.Students.FirstOrDefault(s => s.Email == userEmail);

            if (student == null) return NotFound();

            var appeals = _context.GradeAppeals
                .Where(a => a.StudentId == student.StudentId)
                .Include(a => a.Class)
                    .ThenInclude(c => c.Course)
                .ToList();

            return View(appeals);
        }
        [Authorize(Roles = "Student")]
        public IActionResult Messages()
        {
            var userEmail = User.Identity.Name;
            var student = _context.Students.FirstOrDefault(s => s.Email == userEmail);
            if (student == null) return NotFound();

            var messages = _context.Messages
                .Where(m => m.ReceiverId == student.StudentId && m.Type != "system")
                .OrderByDescending(m => m.SentAt)
                .ToList();

            return View(messages);
        }
        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult SendMessageToTeacher()
        {
            ViewBag.Teachers = _context.Teachers.ToList();
            return View();
        }
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessageToTeacher(int receiverId, string content)
        {
            var userEmail = User.Identity.Name;
            var student = _context.Students.FirstOrDefault(s => s.Email == userEmail);
            if (student == null) return NotFound();

            var message = new Message
            {
                SenderId = student.StudentId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.Now,
                IsRead = false,
                Type = "manual"
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("Messages");
        }
        [Authorize(Roles = "Student")]
        public IActionResult Notifications()
        {
            var userEmail = User.Identity.Name;
            var student = _context.Students.FirstOrDefault(s => s.Email == userEmail);
            if (student == null) return NotFound();

            var notifications = _context.Notifications
                .Where(n => n.UserId == student.StudentId)
                .OrderByDescending(n => n.SentAt)
                .ToList();

            return View(notifications);
        }

    }
}
