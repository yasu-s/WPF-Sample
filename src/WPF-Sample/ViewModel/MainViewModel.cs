namespace WPF_Sample.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class MainViewModel : NotifyObject
    {

        /// <summary></summary>
        private string fullName = string.Empty;

        /// <summary></summary>
        private string age = string.Empty;

        /// <summary></summary>
        private string memo = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [RegularExpression(@"\d{1,3}", ErrorMessage = "Age は数値を入力してください。")]
        public string Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Memo
        {
            get { return memo; }
            set
            {
                memo = value;
                OnPropertyChanged();
            }
        }
    }
}
