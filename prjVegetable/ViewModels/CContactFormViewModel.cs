using System.Net.Mail;
using System.Net;

namespace prjVegetable.ViewModels
{
    public class CContactFormViewModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string QuestionType { get; set; }
        public string Description { get; set; }
    }
}