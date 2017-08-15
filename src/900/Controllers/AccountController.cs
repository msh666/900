using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using _900.Models;

namespace _900.Controllers
{
    public class AccountController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly InnoSheetContext _context;

        public AccountController(InnoSheetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            //return RedirectToAction("Login", new { code = "codesdasdjasklfdjalfkjdalfks" });
            return Redirect("https://telegramlogin.com/token/970833511");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string code, string state)
        {
            var values = new Dictionary<string, string> {
                { "code", code },
                { "client_id", "970833511" },
                { "client_secret", "O37NmUUnDXX8Wf0Cy_tZ"}
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://telegramlogin.com/code", content);

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic profile = JsonConvert.DeserializeObject(responseString);

            //await Authenticate("1231231231", "TESTUSER");
            if (profile.error == null)
            {
                string telegramId = profile.telegram_user.telegram_id;
                string telegramName = profile.telegram_user.name;
                
                if(!IsRegistered(telegramId))
                    RegisterUser(telegramId, telegramName);

                await Authenticate(telegramId, telegramName);
            }
            return RedirectToAction("Index", "Schedule");
        }

        public async Task Authenticate(string id, string name)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.NameIdentifier, id)
            };

            var claimId = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(claimId), new AuthenticationProperties
            {
                IsPersistent = true
            });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Schedule");
        }

        //Проверяет, есть ли такой пользовательский ид в базе
        public bool IsRegistered(string telegramId)
        {
            var users = _context.User.Where(x => x.TelegramId.ToString() == telegramId);
            return users.Any();
        }

        //Добавляет в базу пользователя
        public void RegisterUser(string id, string name)
        {
            _context.User.Add(new User() {TelegramId = Convert.ToInt32(id), TelegramName = name});
            _context.SaveChanges();
        }
    }
}
