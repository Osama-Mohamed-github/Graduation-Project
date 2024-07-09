using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.Cloud.Firestore;
using myGP.Models;
using myGP.Models_Controllers;
using System.Threading.Tasks;
using myGP.Helper;
using System.IO;
using System.Collections;
using System.Drawing;

namespace myGP.Controllers
{
    public class VotersController : Controller
    {
        FirestoreDb db;
        Voter v;
        Date_Helper dt;
        Image_Helper img;
        public VotersController()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "asp-mvc-with-web.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("asp-mvc-with-web");
            v = new Voter();
            dt = new Date_Helper();
            img = new Image_Helper();
        }

        public  ActionResult Index()
        {

            // Admin ad = new Admin();
            //ad.User_name = "Osama";
            //ad.password = "Ma-Na011";
            //ad.National_id = "30012171403033";
            //VoterController vv = new VoterController();
            //    vv.Add_Admin(ad);
            // string fb_id=await vv.Get_Admin_data("Osama", "Ma-Na011");
            //  ----------------------------------------------------------------

            //VoterController vv = new VoterController();
            //bool result=await vv.GetNearestMachine("القليوبية", "شبرا الخيمة ثان");
            //if (result == true) return Content("Done");
            //else
            //return Content("Not Done");



            VoterController vv = new VoterController();
            Address a = new Address();
            a.house_number = 1;
            a.streert_name = "ش مسجد الصحابة";
            a.area = "شبرا الخيمة ثان";
            a.city = "القليوبية";

           
            Voter v = new Voter();
            v.Address = a;
            v.National_id = "30012171403033";
            v.Name = "اسامة محمد سليمان مسعود";
            v.Marital_status = "اعزب";
            v.Job = "طالب";
            //DateTime dd = dt.ConvertToDate_FromString("17/12/2000");
            //v.BirthDate = dd;
           // int age = Math.Abs((dd.Year) - (DateTime.Now.Year));
            //v.Age = age;
            v.Religion = "مسلم";
            vv.Add_Voter(v);
            return Content("Done");














            /*
         //   Address a = new Address();
          //  a.house_number = 1;
           // a.streert_name = "ش مسجد الصحابة";
           // a.area = "شبرا الخيمة ثان";
       //     a.city = "القليوبية";
            Voter v = new Voter();
            VoterController vv = new VoterController();
            v.National_id = "30012171403033";
            v.Name = "Osama";
          //  v.Address = a;
            v.Marital_status="Single";
            v.Job = "Student";
         //   v.BirthDate = dt.ConvertToDate_FromString("17/12/2000");
         //   v.Age = Math.Abs((v.BirthDate.Year) - (DateTime.Now.Year));
            v.Religion ="Musilm";
            vv.Add_Voter(v);
            // vv.Apply_Candidate_Request(c);
           */

            /*
            DocumentReference coll = db.Collection("Voters").Document("_voter");
            Dictionary<string, object> map = new Dictionary<string, object>();
            Dictionary<string, object> addr = new Dictionary<string, object>()
            {
                {"House_Number",1 },
                {"Street_Name","ش صيدلية الدكتور اسماء" },
                {"Area","ابو زعبل" },
                {"City","القليوبية" }
            };

            DateTime dd=dt.ConvertToDate_FromString("15/01/1999");
            int age = Math.Abs((dd.Year) - (DateTime.Now.Year));
            Dictionary<string, object> dic = new Dictionary<string, object>()
            {
                {"National_id","29901151403235" },
                {"Name","احمد محمود محمد اسماعيل" },
                {"Martial_Status","متزوج" },
                {"BirthDate",dd.ToString() },
                {"Age",age },
                {"Religion","مسلم" }
            };
            dic.Add("Address", addr);
            map.Add("Second_Voter", dic);
            coll.UpdateAsync(map);
            */

            /*
            VoterController vv = new VoterController();
            bool result =await vv.GetNearestMachine();
            if (result == true)
                return Content("Done");
            else
                return Content("Not Done");
            */








            /*
            DocumentReference doc = db.Collection("Machines").Document("6UuiNdK9VMlKs380vLzP");
            Dictionary<string, object> map = new Dictionary<string, object>();
            Dictionary<string, object> addr = new Dictionary<string, object>()
            {
                {"Street_Name","ش مسجد الصحابة" },
                {"Area","شبرا الخيمة ثان" },
                {"City","القليوبية" }
            };
            Dictionary<string, object> dic = new Dictionary<string, object>()
            {
                {"Id","103" },
                {"Model","Y" },
                {"Working_Status","Worked" }
            };
            dic.Add("Address", addr);
            map.Add("Third_Machine", dic);
            doc.UpdateAsync(map);

            */









            /*
            DocumentReference coll = db.Collection("Admins").Document("_admin");
            Dictionary<string, object> dic = new Dictionary<string, object>();
            coll.SetAsync(dic);
            */
        }





        /*        
 public async Task<ActionResult> Index()
 {
     VoterController vv = new VoterController();
     bool result=await vv.GetNearestMachine();
     if (result == true)
         return Content("Done");
     else return Content("Not Done");
 }
        */
        /*
 [HttpPost]
 public async Task<ActionResult> Index(HttpPostedFileBase file)
 {
     VoterController vv = new VoterController();
     FileStream stream;
     string path = Path.Combine(Server.MapPath("C:\\"), file.FileName);
     file.SaveAs(path);
     stream = new FileStream(Path.Combine(path), FileMode.Open);
     await Task.Run(() => vv.UpLoad(stream, file.FileName));
     return Content("Done");
 }
 */

        /*
         public async Task<ActionResult> Index()
         {
             VoterController vv = new VoterController();
             bool result =await vv.SearchForSpecificCandidate("Ahmed Mohamed");
             if(result == true)
                 return Content("Name Exist");
             else 
                 return Content("Name dosen't exist");
         }
        */


        /* 1-
        public async Task<ActionResult> Index()
        {
            VoterController vv = new VoterController();
            bool result=  await vv.Allow_To_Vote("12");
            if (result == true)
            {
                return Content("Success");
            }
            else
            {
                return Content("Failure");
            }
        }
        */

        /* 2--
        public async Task<ActionResult> Index()
        {
            VoterController vv = new VoterController();
            bool result = await vv.AreVotingOrNot("12");
            if (result == true)
            {
                return Content("Success");
            }
            else
            {
                return Content("Failure");
            }
        }
        */

        /*
        public ActionResult Index()
        {/*
            DocumentReference coll = db.Collection("New_Voters").Document("G4aXb78QVtfR3lDq9OAF");
            Dictionary<string, object> dic = new Dictionary<string, object>()
            {
                {"National_id","12" },
                {"Name","osama" }

            };
            Dictionary<string,object> address = new Dictionary<string, object>()
            {
                {"House_Number","2" },
                {"Street_Name","Ahmed Orabi" },
                {"Area","Fathia" },
                {"City","Cairo" }
            };
            dic.Add("Address",address);
            coll.UpdateAsync(dic);
            */
        /*
        string d = "17/12/2000";           
        DocumentReference document = db.Collection("New_Voters").Document("G4aXb78QVtfR3lDq9OAF");
        DateTime dd = dt.ConvertToDate_FromString(d);
        v.Age = Math.Abs((dd.Year) - (DateTime.Now.Year));
        Dictionary<string, object> dict = new Dictionary<string, object>()
        {
            {"Age",v.Age},
        };
        document.UpdateAsync(dict);
        return Content("Done");

    }
*/



    }
}