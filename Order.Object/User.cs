using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
//using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace Order.Object
{
    public class User : Microsoft.AspNetCore.Identity.IdentityUser
    {
        /// <summary>
        /// PersonalData will delete personal data from user, also included in download of personal data
        /// </summary>
        [PersonalData]
        public string? Address { get; set; }

        [PersonalData]
        public int Postcode { get; set; }

        [PersonalData]
        public string? City { get; set; }

        [PersonalData]
        public string? Country { get; set; }

        [Required]
        [PersonalData]
        public string? Name { get; set; }

        [PersonalData]
        public DateTime? DateBirth { get; set; }
    }
}
