using System.ComponentModel.DataAnnotations;

namespace WpfApp
{
    public class UserInfo : User
    {
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]{2,14}\s?'?([а-яА-ЯёЁa-zA-Z]{2,14})?'?\s?\d?\d?\d?\s?$", ErrorMessage = "Invalid WorkPLace provided")]
        public string WorkPlace { get; set; }
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]{2,14}\s?\.?\s?\-?([а-яА-ЯёЁa-zA-Z]{2,14})?$", ErrorMessage = "Invalid WorkPosition provided")]
        public string WorkPosition { get; set; }
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid EMail provided")]
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
