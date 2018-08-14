using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models{

    public class Rsvp : BaseEntity {
        public int Attending {get;set;}
        public int userid {get;set;}
        public User User {get;set;}
        public int weddingid {get;set;}
        public Wedding Wedding {get;set;}
    }
}