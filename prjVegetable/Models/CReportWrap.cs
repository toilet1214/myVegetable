using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace prjVegetable.Models
{
    public class CReportWrap
    {
        private TReport _report = null;
        public TReport report
        {
            get { return _report; }
            set { _report = value; }
        }
        public CReportWrap()
        {
            _report = new TReport();
        }

        public int FId 
        { 
            get { return _report.FId; } 
            set { _report.FId = value; } 
        }
        public int FPersonId
        {
            get { return _report.FPersonId; }
            set { _report.FPersonId = value; }
        }
        [Required(ErrorMessage = "必填")]
        [DisplayName("類別")]
        public string FClass
        {
            get { return _report.FClass; }
            set { _report.FClass = value; }
        }
        [Required(ErrorMessage = "必填")]
        [DisplayName("標題")]
        public string FTitle
        {
            get { return _report.FTitle; }
            set { _report.FTitle = value; }
        }
        [Required(ErrorMessage = "必填")]
        [DisplayName("詳細內容")]
        public string FDescription
        {
            get { return _report.FDescription; }
            set { _report.FDescription = value; }
        }

        public DateTime FTime
        {
            get { return _report.FTime; }
            set { _report.FTime = value; }
        }
    }
}
