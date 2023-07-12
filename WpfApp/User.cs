using System;
using System.ComponentModel.DataAnnotations;
namespace WpfApp
{
    public class User
    {

        [Key]
        [Required(ErrorMessage = "Surname must not be empty.")]
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]{2,14}$", ErrorMessage = "Invalid surname provided")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Name must not be empty.")]
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]{2,14}$", ErrorMessage = "Invalid name provided")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Secname must not be empty.")]
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]{2,14}$", ErrorMessage = "Invalid secname provided")]
        public string Secname { get; set; }
        [Required(ErrorMessage = "Date must not be empty.")]
        [Range(typeof(DateTime), "1/1/1900", "10/12/2004", ErrorMessage = "Must be 18 y.o.")]
        public string Birthday { get; set; }
        [Required(ErrorMessage = "Adress must not be empty.")]
        [RegularExpression(@"^.*\s?\d$", ErrorMessage = "Invalid adress provided")]
        public string Adress { get; set; }
        [Required(ErrorMessage = "Number must not be empty.")]
        [RegularExpression(@"^\+\d{12}$", ErrorMessage = "Invalid number provided")]
        public string Phonenumber { get; set; }
        public string Pol { get; set; }
        public User(string surname, string name, string secname, string birthday, string adress, string phonenumber, string pol)
        {
            Surname = surname;
            Name = name;
            Secname = secname;
            Birthday = birthday;
            Adress = adress;
            Phonenumber = phonenumber;
            Pol = pol;
        }
        public User()
        {
        }
    }
}
