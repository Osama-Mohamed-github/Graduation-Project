using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.Cloud.Firestore;
using myGP.Models_Controllers;
using myGP.Models;
using System.Threading.Tasks;

namespace myGP.Controllers
{
    public class adminController : Controller
    {
        VoterController vv;
        FirestoreDb db;
        public adminController()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "asp-mvc-with-web.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("asp-mvc-with-web");
            vv = new VoterController();
        }

        // GET: admin
        public ActionResult adminLogin() //Done
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> adminLogin(string user,string pass) //Done
        {
            if (await vv.Check_For_Admin_Data(user, pass)){
                Admin ad = await vv.Get_Admin_data(user, pass);
                return RedirectToAction("homeAdmin", "admin");
            }
            else
                return HttpNotFound();
        }
        public ActionResult homeAdmin() //Done
        {
            return View("homeAdmin");
        }
        public async Task<ActionResult> manageDisabled()
        {
            List<Disabled_People> req = await vv.Retrieve_Disabled_Requests();
            ViewData["MyData"] = req;
            return View(req);
        }


        public async Task<ActionResult> manageCandidate ()
        {
            List<Request_Candidates> req= await vv.Retrieve_Candidates_Request();
            ViewData["MyData"] = req;
            return View(req);
        }
    }
}