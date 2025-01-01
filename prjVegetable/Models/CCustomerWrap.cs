using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace prjVegetable.Models
{
    public class CCustomerWrap
    {
        private TPerson _person = null;
        public TPerson person
        {
            get { return _person; }
            set { _person = value; }
        }
        public CCustomerWrap()
        {
            _person = new TPerson();
        }
        public int FId
        {
            get { return _person.FId; }
            set { _person.FId = value; }
        }
        [Required(ErrorMessage = "必填")]
        [DisplayName("姓名")]
        public string FName
        {
            get { return _person.FName; }
            set { _person.FName = value; }
        }
        [Required(ErrorMessage = "必填")]
        [DisplayName("帳號")]
        public string FAccount
        {
            get { return _person.FAccount; }
            set { _person.FAccount = value; }
        }
        [Required(ErrorMessage = "必填")]
        [DisplayName("密碼")]
        public string FPassword
        {
            get { return _person.FPassword; }
            set { _person.FPassword = value; }
        }
        [DisplayName("性別")]
        public string FGender
        {
            get { return _person.FGender; }
            set { _person.FGender = value; }
        }
        [Required(ErrorMessage = "必填")]
        [DisplayName("生日")]
        public DateOnly FBirth
        {
            get { return _person.FBirth; }
            set { _person.FBirth = value; }
        }
        [Required(ErrorMessage = "必填")]
        [DisplayName("手機")]
        public string FPhone
        {
            get { return _person.FPhone; }
            set { _person.FPhone = value; }
        }
        [Required(ErrorMessage = "必填")]
        [DisplayName("市內電話")]
        public string FTel
        {
            get { return _person.FTel; }
            set { _person.FTel = value; }
        }
        [Required(ErrorMessage = "必填")]
        [DisplayName("地址")]
        public string FAddress
        {
            get { return _person.FAddress; }
            set { _person.FAddress = value; }
        }
        [Required(ErrorMessage = "必填")]
        [DisplayName("Email")]
        public string FEmail
        {
            get { return _person.FEmail; }
            set { _person.FEmail = value; }
        }
        [DisplayName("統編")]
        public string FUbn
        {
            get { return _person.FUbn; }
            set { _person.FUbn = value; }
        }
        [Required(ErrorMessage = "必填")]
        [Compare("FPassword", ErrorMessage = "請確認密碼一致")]
        [DisplayName("確認密碼")]
        public string ConfirmPassword { get; set; }
    }
}
