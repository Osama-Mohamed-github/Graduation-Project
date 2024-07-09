using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myGP.Models_Controllers;
using myGP.Models;
namespace myGP.Controllers
{
    public class disabledController : Controller
    {
        VoterController vv;
        // GET: disabled
        public ActionResult serviceFor_disabled()
        {
            return View();
        }

        [HttpPost]
        public ActionResult serviceFor_disabled(string national_id,string phone,string address)
        {
            vv = new VoterController();
            Disabled_People dis = new Disabled_People();
            dis.National_id = national_id;
            dis.Phone = phone;
            dis.address = address;
            vv.Add_Disable_Request(dis);
            return View("serviceFor_disabled");
        }
        
    }
}