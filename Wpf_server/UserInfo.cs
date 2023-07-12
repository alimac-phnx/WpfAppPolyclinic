using System.ComponentModel.DataAnnotations;

namespace Wpf_server
{
    public class UserInfo : User
    {
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]{2,50}\s?'?.?-?([а-яА-ЯёЁa-zA-Z]{2,50})?'?\s?\d?\d?\d?\s?\.?$", ErrorMessage = "Invalid WorkPLace provided")]
        public string WorkPlace { get; set; }
        [RegularExpression(@"[^!@#$%^&*()_]$", ErrorMessage = "Invalid WorkPosition provided")]
        public string WorkPosition { get; set; }
        [RegularExpression(@"[^!@#$%^&*()_]$", ErrorMessage = "Invalid EMail provided")]
        public string EMail { get; set; }
        public UserInfo(string surname, string name, string secname, string birthday, string adress, string phonenumber, string pol, string workPlace,
            string workPosition, string eMail)
            : base(surname, name, secname, birthday, adress, phonenumber, pol)
        {
            WorkPlace = workPlace;
            WorkPosition = workPosition;
            EMail = eMail;
        }
        public UserInfo()
        {
        }
    }
}
