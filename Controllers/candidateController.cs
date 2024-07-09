using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using System.Threading;

namespace myGP.Controllers
{
    public class candidateController : Controller
    {
        private static string AuthEmail = "usamamohamed306@gmail.com";
        private static string AuthPass = "Ma-Na011";
        private static string ApiKey = "AIzaSyD_A0esVzbW69uR-R6copAzWrSEh1Il6ic";
        private static string Bucket = "asp-mvc-with-web.appspot.com";
        // GET: candidate
        public ActionResult applyForPresidency()
        {
            return View();
        }
        public ActionResult candidatePrograms()
        {
            @TempData["imgurl"]= "https://firebasestorage.googleapis.com/v0/b/asp-mvc-with-web.appspot.com/o/images%2F20190814_190338.jpg?alt=media&token=164f4218-1210-489f-864d-b9820d8e1f0d";
           // TempData.Keep();
            return View("candidatePrograms");
        }

        public ActionResult Index()
        {
            return View();
        }
        //Upload_Image
        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFileBase file)
        {
            FileStream fs;
            if (file.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Content/images/"), file.FileName);
                file.SaveAs(path);
                fs = new FileStream(Path.Combine(path), FileMode.Open);
                await Task.Run(() => uploadimage(fs, file.FileName));
            }
            return View();
        }
        public async Task<string> uploadimage(FileStream fs, string fname)
        {
            string link = null;
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPass);
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("images")
                .Child(fname)
                .PutAsync(fs, cancellation.Token);
            try
            {
                link = await task;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex);
            }
            return link;
        }
    }
}