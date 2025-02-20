using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace prjVegetable.Models
{
    public class CPersonWrap
    {
        private TPerson _person = null;
        public TPerson person
        {
            get { return _person; }
            set { _person = value; }
        }
        public CPersonWrap()
        {
            _person = new TPerson();
        }

        public int FId
        {
            get { return _person.FId; }
            set { _person.FId = value; }
        }

        [DisplayName("姓名")]
        [Required(ErrorMessage= "必填欄位")]
        [StringLength(100, MinimumLength = 1)]
        public string FName
        {

            get { return _person.FName; }
            set { _person.FName = value; }
        }

        [DisplayName("帳號")]
        [Required(ErrorMessage = "必填欄位")]
        [StringLength(100, MinimumLength = 1)]
        public string FAccount
        {
            get { return _person.FAccount; }
            set { _person.FAccount = value; }
        }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "必填欄位")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "密碼必須包含英文字母、數字，並且至少 8 個字符長")]
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

        [DisplayName("生日")]
        public DateOnly FBirth
        {
            get { return _person.FBirth; }
            set { _person.FBirth = value; }
        }

        [DisplayName("手機")]
        [Required(ErrorMessage = "必填欄位")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "手機號碼必須是 10 位數字")]
        public string FPhone
        {
            get { return _person.FPhone; }
            set { _person.FPhone = value; }
        }

        [DisplayName("家用電話")]
        public string FTel
        {
            get { return _person.FTel; }
            set { _person.FTel = value; }
        }

        [DisplayName("地址")]
        [Required(ErrorMessage = "必填欄位")]
        public string FAddress
        {
            get { return _person.FAddress; }
            set { _person.FAddress = value; }
        }



        [DisplayName("統編")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "統一編號必須是 8 位數字")]
        public string FUbn//統編
        {
            get { return _person.FUbn; }
            set { _person.FUbn = value; }
        }

        [DisplayName("權限")]
        public int FPermission
        {
            get { return _person.FPermission; }
            set { _person.FPermission = value; }
        }

        [DisplayName("緊急聯絡人")]
        public string FEmp
        {
            get { return _person.FEmp; }
            set { _person.FEmp = value; }
        }

        [DisplayName("緊急聯絡人電話")]
        public string FEmpTel
        {
            get { return _person.FEmpTel; }
            set { _person.FEmpTel = value; }
        }

        [DisplayName("資料建立時間")]
        public DateTime FCreatedAt
        {
            get { return _person.FCreatedAt; }
            set { _person.FCreatedAt = value; }
        }

        [DisplayName("修改人")]
        public int FEditor
        {
            get { return _person.FEditor; }
            set { _person.FEditor = value; }
        }
    }
}
