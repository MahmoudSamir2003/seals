using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SalesSuite.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } = "User"; // Admin / User

    }
}

//// Models/Customer.cs
//namespace SalesSuite.Models
//{
//    public class Customer
//    {
//        public int Id { get; set; }
//        public string FullName { get; set; } = "";
//        public string? Phone { get; set; }
//        public string? Email { get; set; }
//        public string? Address { get; set; }
//        public string? City { get; set; }
//        public string? Notes { get; set; }
//        public DateTime CreatedAt { get; set; }
//    }
//}