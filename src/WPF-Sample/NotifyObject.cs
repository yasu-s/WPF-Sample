namespace WPF_Sample
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 汎用NotifyObject
    /// </summary>
    public abstract class NotifyObject : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        /// <summary>
        /// 検証対象のプロパティ情報
        /// </summary>
        protected IDictionary<string, PropertyInfo> validPropInfos;

        /// <summary>
        /// 検証エラー情報
        /// </summary>
        protected IDictionary<string, IEnumerable<string>> errors = new Dictionary<string, IEnumerable<string>>();

        /// <summary>
        /// プロパティ値が変更されたときに発生します。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティまたはエンティティ全体で検証エラーが変更されたときに発生します。
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// <see cref="NotifyObject"/>を生成します。
        /// </summary>
        public NotifyObject()
        {
            validPropInfos = GetType().GetProperties()
                                      .Where(p => p.GetCustomAttributes<ValidationAttribute>(true).Any())
                                      .ToDictionary(p => p.Name);
        }

        /// <summary>
        /// 検証エラーがあるかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// 検証エラーがある場合 true ; それ以外 false。
        /// </value>
        public bool HasErrors
        {
            get { return errors.Values.Any(); }
        }

        /// <summary>
        /// 指定したプロパティの検証エラーを取得します。
        /// </summary>
        /// <param name="propertyName">
        /// 検証エラーを取得するプロパティ名
        /// </param>
        /// <returns>
        /// プロパティの検証エラー。
        /// </returns>
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (!validPropInfos.ContainsKey(propertyName)) return null;
            return (errors.ContainsKey(propertyName)) ? errors[propertyName] : null;
        }

        /// <summary>
        /// プロパティ変更処理
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(propertyName);

            if (validPropInfos.ContainsKey(propertyName))
            {
                SetErrors(propertyName);
                RaiseErrorChanged(propertyName);
            }
        }

        /// <summary>
        /// <see cref="PropertyChanged"/>イベントを発生させます。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// <see cref="ErrorsChanged"/>イベントを発生させます。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        private void RaiseErrorChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 指定したプロパティの検証エラーを設定します。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        private void SetErrors(string propertyName)
        {
            var value   = validPropInfos[propertyName].GetValue(this);
            var results = new List<ValidationResult>();
            var ctx     = new ValidationContext(this, null, null) { MemberName = propertyName };

            if (errors.ContainsKey(propertyName))
                errors.Remove(propertyName);

            if (!Validator.TryValidateProperty(value, ctx, results))
                errors.Add(propertyName, results.Select(r => r.ErrorMessage));
        }
    }
}
