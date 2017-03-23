using POS.Windows;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS.DesktopClient.ViewModels.MainPos
{
    public class CalculatorViewModel:BindableViewModelBase
    {
        public CalculatorViewModel()
        {
           
            TBText = String.Empty;
            IsNumberEnable = true;
            IsDotEnable = true;
            NumberClickCommand = new DelegateCommand<string>(NumberClick);
            DotClickCommand = new DelegateCommand(DotClick);
            BackspaceCommand = new DelegateCommand(Backspace);
        }
        private void Backspace()
        {
            int len = TBText.Length;
            if (len > 0)
                TBText = TBText.Substring(0, len - 1);
            if (TBText.Length == 0)
                IsBackspaceEnable = false;
            IsNumberEnable = CanNumberClick();
            IsDotEnable = CanDotClick();
        }
        private void NumberClick(string num)
        {
            TBText += num;
            IsBackspaceEnable = true;
            IsNumberEnable = CanNumberClick();
        }
        private bool CanNumberClick()
        {
            Regex regex = new Regex(@"[0-9]+[\.]?[0-9]{0,1}");
            Match match = regex.Match(TBText);
            return match.Length==TBText.Length || TBText == String.Empty;
        }
        private void DotClick()
        {
            TBText += ".";
            IsDotEnable = false;
        }
        private bool CanDotClick()
        {
            return !TBText.Contains('.');
        }
        public string TBText
        {
            get { return GetValue(() => TBText); }
            set { SetValue(() => TBText, value); }
        }

        public bool IsNumberEnable
        {
            get { return GetValue(() => IsNumberEnable); }
            set { SetValue(() => IsNumberEnable, value); }
        }

        public bool IsDotEnable
        {
            get { return GetValue(() => IsDotEnable); }
            set { SetValue(() => IsDotEnable, value); }
        }
        public bool IsBackspaceEnable
        {
            get { return GetValue(() => IsBackspaceEnable); }
            set { SetValue(() => IsBackspaceEnable, value); }
        }
        public DelegateCommand<string> NumberClickCommand { get; set; }
        public DelegateCommand DotClickCommand { get; set; }
        public DelegateCommand BackspaceCommand { get; set; }
        
    }
}
