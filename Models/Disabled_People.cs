﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Cloud.Firestore;
namespace myGP.Models
{
    [FirestoreData]
    public class Disabled_People
    {
        [FirestoreProperty]
        public string Fb_id { get; set; }
        [FirestoreProperty]
        public string National_id { get; set; }
        [FirestoreProperty]
        public string address { get; set; }
        [FirestoreProperty]
        public string Phone { get; set; }
        [FirestoreProperty]
        public string link { get; set; }
    }
}