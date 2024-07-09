using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using myGP.Models_Controllers;

namespace myGP.Controllers
{
    public class machineController : Controller
    {
        // GET: machine
       // public ActionResult enterAddress()
        //{
          //  return View();
        //}
        public async Task<ActionResult> enterAddress(string cityValue, string areaValue,string streetValue)
        {
            VoterController vv = new VoterController();
            string s =await vv.GetNearestMachine(cityValue,areaValue, streetValue);
            Session["machine_address"] = s;
            return View("enterAddress");
        }
    }
}