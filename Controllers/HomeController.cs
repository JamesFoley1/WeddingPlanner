using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private Context _context;
        public HomeController(Context context){
            _context = context;
        }

        public IActionResult Index() {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Reg(){
            HttpContext.Session.Clear();
            return View("Register");
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterUser user){
            if(ModelState.IsValid){
                PasswordHasher<RegisterUser> Hasher = new PasswordHasher<RegisterUser>();
                user.Password = Hasher.HashPassword(user, user.Password);

                User _User = new User();
                _User.FirstName = user.FirstName;
                _User.LastName = user.LastName;
                _User.Email = user.Email;
                _User.Password = user.Password;

                _context.Add(_User);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("Id", _User.Id);
                Console.WriteLine(HttpContext.Session.GetInt32("Id"));
                return RedirectToAction("Dashboard");
            }
            else {
                return View("Register");
            }
        }

        public IActionResult Login(string email, string password){
            // retreive a user from db based on email first
            User _User = _context.users.SingleOrDefault(user => user.Email == email);
            if(_User != null){
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(_User, _User.Password, password)){
                    HttpContext.Session.SetInt32("Id", _User.Id);
                    Console.WriteLine(HttpContext.Session.GetInt32("Id"));
                    return RedirectToAction("Dashboard");
                }
                else {
                    return View("Index");
                }
            }
            else{
                ViewBag.Errors = "Invalid email address or password.";
                return View("Index");
            }
        }

        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard(){
            if(HttpContext.Session.GetInt32("Id") != null){
                User _User = _context.users.SingleOrDefault(u => u.Id == HttpContext.Session.GetInt32("Id"));
                ViewBag.user = _User;
                List<Wedding> _Wedding = _context.weddings.Include(r => r.Guests).ToList();
                return View("Dashboard", _Wedding);
                
            }
            else {
                return View("Index");
            }
        }

        [HttpGet]
        [Route("Wedding")]
        public IActionResult Wedding(){
            return View("Wedding");
        }

        [HttpPost]
        [Route("NewWedding")]
        public IActionResult NewWedding(Wedding wedding){
            if(ModelState.IsValid){
                Wedding _wedding = new Wedding();
                _wedding.WedderOne = wedding.WedderOne;
                _wedding.WedderTwo = wedding.WedderTwo;
                _wedding.Date = wedding.Date;
                _wedding.Address = wedding.Address;
                _wedding.creatorid = (int)HttpContext.Session.GetInt32("Id");

                _context.weddings.Add(_wedding);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else{
                return View("Wedding");
            }
        }

        [HttpGet]
        [Route("Rsvp/{W_Id}")]
        public IActionResult Rsvp(int W_Id){
            Rsvp rsvp = new Rsvp();
            rsvp.Attending = 1;
            rsvp.userid = (int)HttpContext.Session.GetInt32("Id");
            rsvp.weddingid = W_Id;
            _context.rsvps.Add(rsvp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("UnRsvp/{W_Id}")]
        public IActionResult UnRsvp(int W_Id){
            Rsvp rsvp = _context.rsvps.Where(r => r.weddingid == W_Id).Where(r => r.userid == (int)HttpContext.Session.GetInt32("Id")).ToList().First();
            _context.rsvps.Remove(rsvp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("ThisWedding/{W_Id}")]
        public IActionResult ThisWedding(int W_Id){
            Wedding ThisWedding = _context.weddings.Include(rsvp => rsvp.Guests).ThenInclude(r => r.User).SingleOrDefault(w => w.Id == W_Id);
            return View("Thiswedding", ThisWedding);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
