using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _900.Models;
#pragma warning disable CS1701 // Assuming assembly reference matches identity - sorry, resharper warnings everywhere
namespace _900.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly InnoSheetContext _context;
        private readonly StudyGroup _myGroup = new StudyGroup { GroupName = "MY", IdGroup = -1, };

        public ScheduleController(InnoSheetContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            DateTime startOfWeek = DateTime.Today.AddDays(-1 * ((int)(DateTime.Today.DayOfWeek) - 1));
            DateTime endOfWeek = startOfWeek.AddDays(7);
            var timeSlots = _context.Time.ToList();
            var currentWeekSchedule = new List<Schedule>();

            var isAuthorized = HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated;
            var user = new User();
            if (isAuthorized)
            {
                var telegramId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                user = _context.User.Single(x => x.TelegramId == Convert.ToInt32(telegramId));
                var subjectList = _context.UserSubject.Where(x => x.TelegramId == Convert.ToInt32(telegramId)).Select(x => x.IdSubject);

                ViewBag.GroupNumber = -1;
                currentWeekSchedule = _context.Schedule.Include("IdSubjectNavigation")
                        .Include("IdTimeNavigation").Where(x => x.Date >= startOfWeek.Ticks && x.Date <= endOfWeek.Ticks && subjectList.Contains(x.IdSubject)).ToList();
            }
            else
            {
                ViewBag.GroupNumber = 4;
                currentWeekSchedule =
                  _context.Schedule.Include("IdSubjectNavigation")
                      .Include("IdTimeNavigation")
                      .Where(
                          subjects =>
                              subjects.Date >= startOfWeek.Ticks && subjects.Date <= endOfWeek.Ticks &&
                              subjects.IdSubjectNavigation.IdGroupNavigation.IdGroup == 4)
                      .ToList();
                user = null;
            }

            var studyGroup = _context.StudyGroup.ToList();
            //if (isAuthorized)
            //{
            //    studyGroup.Insert(0, _myGroup);
            //}
            studyGroup.Insert(0, _myGroup);

            var scheduleGridModel = new ScheduleGridModel
            {
                User = user,
                TimeSlots = timeSlots,
                Schedule = currentWeekSchedule,
                StartOfWeek = startOfWeek,
                StudyGroups = studyGroup
            };

            return View(scheduleGridModel);

        }

        public IActionResult ShowSchedule(long start, int groupId)
        {
            var startOfWeek = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(start).Date.AddDays(1);
            ViewBag.GroupNumber = groupId;
            var scheduleGridModel = new ScheduleGridModel();
            var endOfWeek = startOfWeek.AddDays(7);
            var timeSlots = _context.Time.ToList();
            var isAuthorized = HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated;

            var studyGroup = _context.StudyGroup.ToList();
            //if (isAuthorized)
            //{
            //    studyGroup.Insert(0, _myGroup);
            //}
            studyGroup.Insert(0, _myGroup);

            if (groupId > 0)
            {
                //заглушка!!!
                groupId = groupId % 2 == 0 ? 4 : 1;
                //

                var currentWeekSchedule =
                    _context.Schedule.Include("IdSubjectNavigation")
                        .Include("IdTimeNavigation")
                        .Where(
                            subjects =>
                                subjects.Date >= startOfWeek.Ticks && subjects.Date <= endOfWeek.Ticks &&
                                subjects.IdSubjectNavigation.IdGroupNavigation.IdGroup == groupId)
                        .ToList();

                scheduleGridModel = new ScheduleGridModel
                {
                    TimeSlots = timeSlots,
                    Schedule = currentWeekSchedule,
                    StartOfWeek = startOfWeek,
                    StudyGroups = studyGroup
                };
            }
            else if (groupId == -1)
            {
                IList<Schedule> currentWeekSchedule = new List<Schedule>();
                if (isAuthorized)
                {
                    var telegramId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var subjectList = _context.UserSubject.Where(x => x.TelegramId == Convert.ToInt32(telegramId)).Select(x => x.IdSubject);

                    currentWeekSchedule = _context.Schedule.Where(x => x.Date >= startOfWeek.Ticks && x.Date <= endOfWeek.Ticks && subjectList.Contains(x.IdSubject)).Include("IdSubjectNavigation")
                        .Include("IdTimeNavigation").ToList();
                    //var currentWeekSchedule = _context.Schedule.Where(x => x.Date >= startOfWeek.Ticks && x.Date <= endOfWeek.Ticks).Join(subjectList, subject => subject.IdSubject, id => id, (up, id) => up).Include("IdSubjectNavigation")
                    //        .Include("IdTimeNavigation");    
                }


                scheduleGridModel = new ScheduleGridModel
                {
                    TimeSlots = timeSlots,
                    Schedule = currentWeekSchedule,
                    StartOfWeek = startOfWeek,
                    StudyGroups = studyGroup
                };
            }

            return PartialView("_ScheduleGrid", scheduleGridModel);
        }

        public IActionResult ShowSubjectInfo(int subjectId)
        {
            var subject = _context.Subject.Include("SubjectProfessor").Include("SubjectProfessor.IdProfessorNavigation").SingleOrDefault(x => x.IdSubject == subjectId);
            var subjectDetails = new SubjectDetails
            {
                Subject = subject,
                CanFollow = HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated,
                IsFollowed = IsFollow(subject.IdSubject)
            };
            return PartialView("_SubjectDetails", subjectDetails);
        }

        public void Follow(int subjectId)
        {
            var isAuthorized = HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated;
            if (isAuthorized && !IsFollow(subjectId))
            {
                var telegramId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.UserSubject.Add(new UserSubject
                {
                    IdSubject = subjectId,
                    TelegramId = Convert.ToInt32(telegramId)
                });
                _context.SaveChanges();
            }
        }

        public void Unfollow(int subjectId)
        {
            var isAuthorized = HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated;
            if (isAuthorized && IsFollow(subjectId))
            {
                var telegramId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var subjectUser = _context.UserSubject.Where(x => x.IdSubject == subjectId && x.TelegramId == Convert.ToInt32(telegramId));
                _context.UserSubject.Remove(subjectUser.FirstOrDefault());
                _context.SaveChanges();
            }
        }

        public bool IsFollow(int subjectId)
        {
            var isAuthorized = HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated;
            if (!isAuthorized) return false;

            var telegramId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var subjectUser = _context.UserSubject.Where(x => x.IdSubject == subjectId && x.TelegramId == Convert.ToInt32(telegramId));
            return subjectUser.Any();
        }

        public IActionResult SubjectsByDateInRange(long start, long end, int groupId, int? subjectId)
        {
            DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(start).AddDays(-1).Date;
            DateTime endDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(end).AddDays(1).Date;
            var timeSlots = _context.Time.ToList();
            var isAuthorized = HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated;
            var studyGroup = _context.StudyGroup.ToList();

            if (isAuthorized)
            {
                studyGroup.Insert(0, _myGroup);
            }

            var query = _context.Schedule.Include(x => x.IdSubjectNavigation).Include(x => x.IdTimeNavigation);
            IEnumerable<Schedule> currentWeekSchedule = new List<Schedule>();
            if (groupId > 0)
            {
                //заглушка!!!
                groupId = groupId % 2 == 0 ? 4 : 1;
                //

                currentWeekSchedule = query
                        .Where(
                            subjects =>
                                subjects.Date >= startDate.Ticks && subjects.Date <= endDate.Ticks &&
                                subjects.IdSubjectNavigation.IdGroupNavigation.IdGroup == groupId);
            }
            else if (groupId == -1)
            {
                var telegramId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var subjectList = _context.UserSubject.Where(x => x.TelegramId == Convert.ToInt32(telegramId)).Select(x => x.IdSubject);

                currentWeekSchedule = query
                    .Where(x => x.Date >= startDate.Ticks && x.Date <= endDate.Ticks && subjectList.Contains(x.IdSubject));
            }
            if (subjectId.HasValue)
            {
                currentWeekSchedule = currentWeekSchedule.Where(x => x.IdSubject == subjectId.GetValueOrDefault());
            }

            var model = new CalendarSubjectsModel
            {
                Subjects = currentWeekSchedule.Select(x => new CalendarSubjectModel { Date = new DateTime(x.Date), IdSubject = x.IdSubject, })
            };


            return Ok(model);
        }

        public IActionResult Search(string search)
        {
            if (string.IsNullOrWhiteSpace(search) || search.Length < 2)
            {
                return Ok();
            }
            search = search.Trim();
            Func<string, bool> eq = (a) => a.Trim().ToLowerInvariant().Contains(search.ToLowerInvariant());
            Func<string, bool> eq2 = (a) => a.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0; // - a bit faster

            var something = _context.Subject
                .Include(x => x.IdGroupNavigation)
                .Include(x => x.SubjectProfessor)
                .Where(x => eq2(x.SubjectFull) || eq2(x.SubjectShort) || x.SubjectProfessor.Any(y => eq2(y.IdProfessorNavigation.Professor1)))
                .Distinct().Take(6)
                .Select(x => new SearchResultSingleModel
                {
                    IdSubject = x.IdSubject,
                    SubjectShort = x.SubjectShort,
                    SubjectLong = x.SubjectFull,

                    IdGroup = x.IdGroup,
                    GroupName = x.IdGroupNavigation.GroupName,
                    Lectors = x.SubjectProfessor.Select(y => new SearchLectorModel { Id = y.IdProfessor, Name = y.IdProfessorNavigation.Professor1 }),
                })
                .ToList();

            var model = new SearchResultModel { Results = something };
            return Ok(model);
        }
    }
}
#pragma warning restore CS1701 // Assuming assembly reference matches identity
