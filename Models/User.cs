using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models{

    public abstract class BaseEntity {
        [Key]
        public int Id {get;set;}
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime createdAt {get;set;}
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime updatedAt {get;set;}
    }

    public class RegisterUser {

        [Required(ErrorMessage="A first name is required.")]
        [MinLength(2,ErrorMessage="First name must be at least 2 characters.")]
        [Display(Name="First Name:")]
        public string FirstName {get;set;}

        [Required(ErrorMessage="A last name is required.")]
        [MinLength(2,ErrorMessage="Last name must be at least 2 characters.")]
        [Display(Name="Last Name:")]
        public string LastName {get;set;}

        [Required(ErrorMessage="An email address must be provided.")]
        [EmailAddress(ErrorMessage="Please enter a valid email address.")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid email address.")]
        [Display(Name="Email Address:")]
        public string Email {get;set;}

        [Required(ErrorMessage="A password is required.")]
        [MinLength(8,ErrorMessage="Your password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name="Password:")]
        public string Password {get;set;}

        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage="Your password must be at least 8 characters long.")]
        [Compare("Password", ErrorMessage = "Your passwords do not match.")]
        [Display(Name="Confirm Password:")]
        public string PasswordConfirm {get;set;}
    }

    public class User : BaseEntity {
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}
        public List<Rsvp> Weddings {get;set;}

        public User(){
            Weddings = new List<Rsvp>();
        }

    }

    
}