using Microsoft.AspNetCore.Identity;

namespace Order.Project.Web.Models
{
    public class UserModel 
    {
        //[PersonalData]
        public string? Address { get; set; }

        //[PersonalData]
        public int Postcode { get; set; }

        //[PersonalData]
        public string? City { get; set; }

        //[PersonalData]
        public string? Country { get; set; }
       
        //[PersonalData]
        public string? Name { get; set; }

        //[PersonalData]
        public DateTime? DateBirth { get; set; }

        public string Idmodel { get; set; }
    }
}
