namespace PhoneBookWebService.Models
{
    public class Contact
    {
        public string Id { get; set; } // Unique Identifier for the contact
        public string FirstName { get; set; } // First name of the contact
        public string LastName { get; set; } // Last name of the contact
        public string Birthdate { get; set; }
        public string Address { get; set; } // Address of the contact
        public string City { get; set; } // City of the contact
        public string ImagePath { get; set; } // New ImagePath field

        // You can continue to add other properties as per your requirement.
    }
}
