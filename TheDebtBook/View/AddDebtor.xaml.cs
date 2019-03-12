using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TheDebtBook.View
{
    /// <summary>
    /// Interaction logic for AddDeptor.xaml
    /// </summary>
    public partial class AddDebtor : Window
    {

        class DialogData : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            // This method is called by the Set accessor of each property.  
            // The CallerMemberName attribute that is applied to the optional propertyName  
            // parameter causes the property name of the caller to be substituted as an argument.  
            private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }


            private string name;
            public string Name
            {
                get { return name; }
                set
                {
                    name = value;
                    NotifyPropertyChanged();
                }
            }

            private double debt = 0.0;
            public double Debt
            {
                get { return debt; }
                set
                {
                    debt = value;
                    NotifyPropertyChanged();
                }
            }
        }


        DialogData data = new DialogData();
        

        public string DebtorName
        {
            get { return data.Name; }
            set { data.Name = value; }
        }

        public string InitialValue
        {
            get { return data.Debt.ToString(); }
            set { data.Debt = Convert.ToDouble(value); }
        }


        public AddDebtor()
        {
            InitializeComponent();
            DataContext = data;
            saveBtn.Click += SaveBtn_Click;
            cancelBtn.Click += CancelBtn_Click;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Fired when the Apply button is pressed
        public event EventHandler Apply;
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // Validate all controls
            if (ValidateBindings(this))
            {

                // Let modeless clients know
                if (Apply != null) { Apply(this, EventArgs.Empty); }

                // Don't close the dialog 'til Close is pressed
                //DialogResult = true;
            }
        }

        public static bool ValidateBindings(DependencyObject parent)
        {
            // Validate all the bindings on the parent
            bool valid = true;
            LocalValueEnumerator localValues = parent.GetLocalValueEnumerator();
            while (localValues.MoveNext())
            {
                LocalValueEntry entry = localValues.Current;
                if (BindingOperations.IsDataBound(parent, entry.Property))
                {
                    Binding binding = BindingOperations.GetBinding(parent, entry.Property);
                    foreach (ValidationRule rule in binding.ValidationRules)
                    {
                        // TODO: where to get correct culture info?
                        ValidationResult result = rule.Validate(parent.GetValue(entry.Property), null);
                        if (!result.IsValid)
                        {
                            BindingExpression expression = BindingOperations.GetBindingExpression(parent, entry.Property);
                            Validation.MarkInvalid(expression, new ValidationError(rule, expression, result.ErrorContent, null));
                            valid = false;
                        }
                    }
                }
            }

            // Validate all the bindings on the children
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (!ValidateBindings(child)) { valid = false; }
            }

            return valid;
        }


    }

    public class FolderMustExist : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (!Directory.Exists((string)value))
            {
                return new ValidationResult(false, "Folder doesn't exist");
            }

            return new ValidationResult(true, null);
        }
    }

    public class NonZeroLength : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty((string)value))
            {
                return new ValidationResult(false, "Please enter something");
            }

            return new ValidationResult(true, null);
        }
    }


}
