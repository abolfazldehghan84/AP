using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Models;
using Project.Models;

namespace project.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStudent(Student model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Students.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewAllStudents");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddTeacher()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTeacher(Teacher model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Teachers.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewAllTeachers");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCourse(Course model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Courses.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewAllCourses");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddClass()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddClass(Class model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Classes.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewAllClasses");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignTeacherToClass()
        {
            ViewBag.Teachers = _context.Teachers.ToList();
            ViewBag.Classes = _context.Classes.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignTeacherToClass(int teacherId, int classId)
        {
            var exists = await _context.ClassAssignments
                .AnyAsync(x => x.TeacherId == teacherId && x.ClassId == classId);

            if (!exists)
            {
                _context.ClassAssignments.Add(new ClassAssignment
                {
                    TeacherId = teacherId,
                    ClassId = classId,
                    AssignedDate = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewAllClasses");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignStudentToClass()
        {
            ViewBag.Students = _context.Students.ToList();
            ViewBag.Classes = _context.Classes.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignStudentToClass(int studentId, int classId)
        {
            var exists = await _context.ClassStudents
                .AnyAsync(x => x.StudentId == studentId && x.ClassId == classId);

            if (!exists)
            {
                _context.ClassStudents.Add(new ClassStudent
                {
                    StudentId = studentId,
                    ClassId = classId
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewAllClasses");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnassignTeacherFromClass(int teacherId, int classId)
        {
            var assignment = await _context.ClassAssignments
                .FirstOrDefaultAsync(x => x.TeacherId == teacherId && x.ClassId == classId);

            if (assignment != null)
            {
                _context.ClassAssignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewAllClasses");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnassignStudentFromClass(int studentId, int classId)
        {
            var enrollment = await _context.ClassStudents
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.ClassId == classId);

            if (enrollment != null)
            {
                _context.ClassStudents.Remove(enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewAllClasses");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewAllStudents");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewAllTeachers");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewAllCourses");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var classItem = await _context.Classes.FindAsync(id);
            if (classItem != null)
            {
                _context.Classes.Remove(classItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewAllClasses");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ViewAllStudents()
        {
            var students = _context.Students
                .Include(s => s.Faculty)
                .ToList();

            return View(students);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ViewAllTeachers()
        {
            var teachers = _context.Teachers
                .Include(t => t.Faculty)
                .ToList();

            return View(teachers);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ViewAllCourses()
        {
            var courses = _context.Courses
                .Include(c => c.Faculty)
                .ToList();

            return View(courses);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ViewAllClasses()
        {
            var classes = _context.Classes
                .Include(c => c.Course)
                .Include(c => c.TimeSlot)
                .Include(c => c.ClassAssignments).ThenInclude(a => a.Teacher)
                .Include(c => c.ClassStudents).ThenInclude(cs => cs.Student)
                .ToList();

            return View(classes);
        }

    }
}
