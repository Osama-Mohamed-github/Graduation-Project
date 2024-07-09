using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Cloud.Firestore;
using System.Threading.Tasks;
using System.IO;
using myGP.Helper;
using myGP.Models;

namespace myGP.Models_Controllers
{
    public class VoterController
    {
        FirestoreDb db;
        Image_Helper img;
        
        public VoterController()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "asp-mvc-with-web.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("asp-mvc-with-web");
            img = new Image_Helper();
        }
        public async Task<string> Allow_To_Vote(string National_id)
        {
            string allow_vote = "يحق له";
            string not_allow_to_vote = "لا يحق له";
            Query Q = db.Collection("Voters").WhereEqualTo("National_ID", National_id);
            QuerySnapshot snap = await Q.GetSnapshotAsync();
            foreach (DocumentSnapshot docsnap in snap)
            {
                Voter voter = new Voter();
                if (docsnap.Exists)
                {
                    voter.Age = int.Parse(docsnap.GetValue<string>("Age"));
                    if (voter.Age >= 18) return allow_vote;
                    else return not_allow_to_vote;
                }
            }
            return null;
        }
        public async Task<bool> AreVotingOrNot(string National_id)
        {
            Query Q = db.Collection("Vote_Records");
            QuerySnapshot snap = await Q.GetSnapshotAsync();
            foreach (DocumentSnapshot docsnap in snap)
            {
                Vote vt = new Vote();
                if (docsnap.Exists)
                {
                    vt.voter_n_id = docsnap.GetValue<string>("voter_n_id");
                    if (vt.voter_n_id.Equals(National_id)) return true;
                    else return false;
                }
            }
            return false;
        }
        public async Task<bool> SearchForSpecificCandidate(string Candidate_Name)
        {
            Query Q = db.Collection("Candidates");
            QuerySnapshot snap = await Q.GetSnapshotAsync();
            foreach (DocumentSnapshot docsnap in snap)
            {
                if (docsnap.Exists)
                {
                    Candidate c = new Candidate();
                    c.Name = docsnap.GetValue<string>("Name");
                    if (c.Name.Equals(Candidate_Name)) return true;
                    else return false;
                }
            }
            return false;
        }
        public async Task<string> GetNearestMachine(string City, string Area,string Street)
        {
            Query Q = db.Collection("Machines");
            QuerySnapshot snap2 = await Q.GetSnapshotAsync();
            foreach (DocumentSnapshot docsnap in snap2)
            {
                if (docsnap.Exists)
                {
                    Dictionary<string, object> dic = docsnap.ToDictionary();
                    foreach (var item in dic)
                    {
                        if (item.Key.Equals("Address"))
                        {
                            foreach (var add in (Dictionary<string, object>)item.Value)
                            {
                                if (add.Key.Equals("City"))
                                {
                                    foreach (var area in (Dictionary<string, object>)item.Value)
                                    {
                                        if (area.Key.Equals("Area"))
                                        { 
                                            foreach (var street in (Dictionary<string, object>)item.Value) 
                                            {
                                               if (street.Key.Equals("Streert")) 
                                               {
                                                    if (add.Value.Equals(City) && area.Value.Equals(Area) && street.Value.Equals(Street))
                                                    {
                                                        return add.Value + "," + area.Value + "," + street.Value;
                                                    }
                                               }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        public async Task<bool> Check_For_National_id(string National_id)
        {
            Voter voter = new Voter();
            Query Q = db.Collection("Voters").WhereEqualTo("National_ID",National_id);
            QuerySnapshot snap = await Q.GetSnapshotAsync();
            foreach (DocumentSnapshot docsnap in snap)
            {
                if (docsnap.Exists)
                {
                    voter.National_id=docsnap.GetValue<string>("National_ID");
                    if (voter.National_id.Equals(National_id))
                        return true;//Exists
                    else
                        return false; //Not Exists
                }
            }
            return false;
        }
        //---Done--------------------------------------------------------------------------------------------
        //Admin
        public Dictionary<string,object> Map_Admin(Admin adm)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("National_id", adm.National_id);
            dic.Add("User_name", adm.User_name);
            dic.Add("Pass_word", adm.password);
            return dic;
        }
        public void Add_Admin(Admin ad)
        {
            DocumentReference doc = db.Collection("Admins").Document("za8J7fiuvGQW0Cj2oylJ");
            Dictionary<string, object> dic = Map_Admin(ad);
            doc.SetAsync(dic);
        }
        public async Task<Admin> Get_Admin_data(string user,string pass)
        {
            Admin admin = new Admin();
            Query Q = db.Collection("Admins");
            QuerySnapshot snap = await Q.GetSnapshotAsync();
            foreach(DocumentSnapshot docsnap in snap)
            {
                if (docsnap.Exists)
                {
                    admin.User_name = docsnap.GetValue<string>("User_name");
                    admin.password = docsnap.GetValue<string>("Pass_word");
                }
            }
            return admin;
        }
        public void delete_admin(string Fb_id)
        {
            DocumentReference doc = db.Collection("Admins").Document(Fb_id);
            doc.DeleteAsync();
        }
        public void update_admin(string Fb_id,Admin admin)
        {
            DocumentReference doc = db.Collection("Admins").Document(Fb_id);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("National_id", admin.National_id);
            dic.Add("User_name", admin.User_name);
            dic.Add("Pass_word", admin.password);
            doc.UpdateAsync(dic);
        }
        public async Task<bool> Check_For_Admin_Data(string user, string pass)
        {
            Query Q = db.Collection("Admins");
            QuerySnapshot snap = await Q.GetSnapshotAsync();
            foreach (DocumentSnapshot docsnap in snap)
            {
                if (docsnap.Exists)
                {
                    Admin admin = new Admin();
                    admin.User_name = docsnap.GetValue<string>("User_name");
                    admin.password = docsnap.GetValue<string>("Pass_word");
                    if (admin.User_name.Equals(user) && admin.password.Equals(pass)) return true;
                    else return false;
                }
            }
            return false;
        }
        //------------------------------------------------------------------------------------------------------------
        //Request_Disabled_People
        
        public Dictionary<string,object> Map_Disabled_Requests(Disabled_People disabled)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("National_id", disabled.National_id);
            dic.Add("Phone", disabled.Phone);
            dic.Add("Address", disabled.address);
          //  dic.Add("Link", disabled.link);
            return dic;
        }
        public void Add_Disable_Request(Disabled_People disabled)
        {
            Dictionary<string, object> dic = Map_Disabled_Requests(disabled);
            CollectionReference doc = db.Collection("Disabled");
            doc.AddAsync(dic);
        }
        public void Delete_Disabled_Request(string Fb_id)
        {
            DocumentReference doc = db.Collection("Disabled").Document(Fb_id);
            doc.DeleteAsync();
        }
        public async Task<List<Disabled_People>> Retrieve_Disabled_Requests()
        {
            List<Disabled_People> disabled_list = new List<Disabled_People>();
            Query Q = db.Collection("Disabled");
            QuerySnapshot snap = await Q.GetSnapshotAsync();
            foreach (DocumentSnapshot docsnap in snap)
            {
                Disabled_People disabled = new Disabled_People();
                if (docsnap.Exists)
                {
                    disabled.Fb_id = docsnap.Id;
                    disabled.National_id = docsnap.GetValue<string>("National_id");
                    disabled.Phone = docsnap.GetValue<string>("Phone");
                    disabled.address = docsnap.GetValue<string>("Address");
                    disabled_list.Add(disabled);
                }
            }
            return disabled_list;
        }
        public void Apply_Disabled_Request(Disabled_People disabled)
        {
            Add_Disable_Request(disabled);
        }
        //-----------------------------------------------------------------------------------------------------------
        //Request_Candidate
        public Dictionary<string, object> Map_Candidate(Candidate c)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("National_id", c.National_id);
            dic.Add("Email", c.Email);
            dic.Add("Phone", c.Name);
        //    dic.Add("Link_requird_paper", c.link_requied_paper);
            return dic;
        }
        public void Add_Candidate_Request(Candidate c)
        {

            CollectionReference doc = db.Collection("Request_Candidates");
            Dictionary<string, object> dic = Map_Candidate(c);
            doc.AddAsync(dic);
        }
        public void Delete_Candidate_Request(string Fb_id)
        {
            DocumentReference doc = db.Collection("Request_Candidates").Document(Fb_id);
            doc.DeleteAsync();
        }
        public async Task<List<Request_Candidates>> Retrieve_Candidates_Request()
        {
            List<Request_Candidates> req = new List<Request_Candidates>();

            Query Q = db.Collection("Request_Candidates");
            QuerySnapshot snap = await Q.GetSnapshotAsync();
            foreach(DocumentSnapshot docsnap in snap)
            {
                if (docsnap.Exists)
                {
                    Request_Candidates request = new Request_Candidates();

                    request.Fb_id = docsnap.Id;
                    request.National_id = docsnap.GetValue<string>("National_id");
                    request.Email = docsnap.GetValue<string>("Email");
                    request.Phone = docsnap.GetValue<string>("Phone");
                    //  candidates.link_requied_paper = docsnap.GetValue<string>("Link_requird_paper");
                    req.Add(request);

                }

            }
            return req;
        }
        public void Apply_Candidate_Request(Candidate c)
        {
            Delete_Candidate_Request(c.Fb_id);
            CollectionReference doc = db.Collection("Candidates");
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("New Candidate",c);
            doc.AddAsync(dic);
        }
        //---------------------------------------------------------------------------------------------------------   
        //On_OFF_Voting
        /*
        public async void Check_Voting()
            {
                Query Q = db.Collection("Result");
                QuerySnapshot snap = await Q.GetSnapshotAsync();
                foreach(DocumentSnapshot docsanp in snap)
                {
                    if (docsanp.Exists)
                    {
                        bool status = docsanp.ContainsField("finishd");
                        if(status.Equals("true"))
                            Console.WriteLine("Operation Finished");
                        else
                            Console.WriteLine("Operation Not Finished");
                    }
                }
            }
            public void Update_status(bool status)
            {
                DocumentReference doc = db.Collection("Result").Document("1");
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("finished", status);
                doc.UpdateAsync(dic);
            }
            public void Finish_operation()
            {
                Update_status(true);
            }
            public void Rest_operation()
            {
                Update_status(false);
            }
        */
        //---------------------------------------------------------------------------------------------
        public Dictionary<string, object> Map_Voter(Voter v)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Dictionary<string, object> address = Map_Address(v.Address);
            dic.Add("National_ID", v.National_id);
            dic.Add("Name", v.Name);
            dic.Add("Marital_status", v.Marital_status);
            dic.Add("Job", v.Job);
            dic.Add("Qualifications", v.Qualifications);
            /*     string date = dt.ConvertDateToString(v.BirthDate);
                 DateTime dd = dt.ConvertToDate_FromString(date);
                 dic.Add("BirthDate",dd);
                 int age = Math.Abs((dd.Year) - (DateTime.Now.Year));
                 dic.Add("Age",v.Age);*/
            dic.Add("Religion", v.Religion);
            dic.Add("Phone", v.Phone_Number);
            dic.Add("Address", address);
            return dic;
        }
        public Dictionary<string, object> Map_Address(Address a)
        {
            Dictionary<string, object> addr = new Dictionary<string, object>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("House_Number", a.house_number);
            dic.Add("Area", a.area);
            dic.Add("City", a.city);
            dic.Add("Street_Name", a.streert_name);
            return dic;
        }
        public void Add_Voter(Voter v)
        {
            CollectionReference doc = db.Collection("Voters");
            Dictionary<string, object> dic = Map_Voter(v);
            doc.AddAsync(dic);
        }
        public async Task<Voter> Get_Voter_Data(string National_id)
        {
            Dictionary<string, object> add = new Dictionary<string, object>();
            Voter v = new Voter();
            Query Q = db.Collection("Voters").WhereEqualTo("National_ID", National_id);
            QuerySnapshot snap = await Q.GetSnapshotAsync();
            foreach (DocumentSnapshot docsnap in snap)
            {
                if (docsnap.Exists)
                {
                    v.Fb_id = docsnap.Id;
                    v.Name = docsnap.GetValue<string>("Name");
                    v.National_id = docsnap.GetValue<string>("National_ID");
                    add = docsnap.GetValue<Dictionary<string, object>>("Address");
                    Address a = new Address();
                    a.house_number = int.Parse(add["House Number"].ToString());
                    a.streert_name = add["Streert Name"].ToString();
                    a.area = add["Area"].ToString();
                    a.city = add["City"].ToString();
                    v.Address = a;
                    v.Job = docsnap.GetValue<string>("Job");
                    v.Gender = docsnap.GetValue<string>("Gender");
                    v.Religion = docsnap.GetValue<string>("Religion");
                    v.Marital_status = docsnap.GetValue<string>("Marital_status");
                    v.RightToVote = await Allow_To_Vote(National_id);
                    if(await AreVotingOrNot(National_id))
                    {
                        string statofvote = "انتخب";
                        v.Status_Of_Voting = statofvote;
                    }else
                        v.Status_Of_Voting = "لم ينتخب بعد";
                }
            }
            return v;
        }
    }
}