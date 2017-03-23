using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEntities
{
    public class ModelBase : BindableModelBase
    {
        private ErrorMap _errorMap;
        public ErrorMap ErrorMap
        {
            get {
                if (_errorMap == null)
                    _errorMap = new ErrorMap();
                return _errorMap;
                }
            private set { _errorMap = value; }
        }
        protected override string OnValidate(string propertyName)
        {
            string error = base.OnValidate(propertyName);
            if (error == null || error == String.Empty)
            {
                if(ErrorMap.ContainsKey(propertyName)) 
                    ErrorMap.Remove(propertyName);
                OnValidationComplete(this, EventArgs.Empty);
                return error;
            }

            if (ErrorMap.ContainsKey(propertyName))
            {
                if (!ErrorMap[propertyName].Contains(error))
                {
                    ErrorMap[propertyName].Add(error);
                }
            }
            else
            {
                ErrorMap.Add(propertyName, new List<string>() { error });
            }
            OnValidationComplete(this, EventArgs.Empty);
            return error;

        }

        public event EventHandler OnValidationComplete; 
    }

    public class ErrorMap : Dictionary<string, List<String>>
    {
        public override string ToString()
        {
            if (this.Count == 0) return String.Empty;
            StringBuilder str = new StringBuilder();
            foreach(string key in Keys )
            {
                str.Append(this[key][0]).Append(System.Environment.NewLine);
            }
            return str.ToString();
        }
    }
}
