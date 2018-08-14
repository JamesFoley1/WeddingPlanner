using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models{

    public class Wedding : BaseEntity {

        [Required(ErrorMessage = "You must provide a wedder!")]
        public string WedderOne {get;set;}

        [Required(ErrorMessage = "You must provide a wedder!")]
        public string WedderTwo {get;set;}

        [Required(ErrorMessage="You must select a Date.")]
        public DateTime Date {get;set;}

        [Required(ErrorMessage = "You must provide an adress.")]
        public string Address {get;set;}
        
        [ForeignKey("User")]
        public int creatorid {get;set;}

        public User User {get;set;}

        public List<Rsvp> Guests {get;set;}

        public Wedding(){
            Guests = new List<Rsvp>();
        }
    }
}