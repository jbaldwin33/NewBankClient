using MVVMFramework.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace NewBankWpfClient
{
    public class ViewModelValidation : ViewModel, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> errorDictionary = new Dictionary<string, List<string>>();
        public bool HasErrors => errorDictionary.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (!HasErrors || string.IsNullOrEmpty(propertyName) || !errorDictionary.ContainsKey(propertyName))
                return null;

            if (errorDictionary[propertyName].Count <= 0)
                throw new ArgumentException(propertyName);

            return errorDictionary[propertyName];
        }

        public void ClearValidationErrors()
        {
            errorDictionary.Clear();
        }
        protected bool SetProperty<T>(ref T field, T newValue, string errorMessage, string propertyName = null)
        {
            errorDictionary.Add(propertyName, new List<string> { errorMessage });
            return base.SetProperty(ref field, newValue, propertyName: propertyName);
        }

        private void ErrorsChangedHandler(object sender, DataErrorsChangedEventArgs e) => ErrorsChanged.Invoke(sender, e);
    }
}
