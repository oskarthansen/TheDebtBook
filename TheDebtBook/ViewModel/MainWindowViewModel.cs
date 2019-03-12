using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TheDebtBook.Model;
using Prism;
using Prism.Commands;
using TheDebtBook.View;

namespace TheDebtBook.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        AddDebtor dlg = null;
        public event PropertyChangedEventHandler PropertyChanged;  

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")  
        {  
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }  


        private ObservableCollection<Debtor> debtors;

        public MainWindowViewModel()
        {
            debtors = new ObservableCollection<Debtor>();

        }

        public ObservableCollection<Debtor> Debtors
        {
            get { return debtors; }
            set
            {
                debtors = value;
                NotifyPropertyChanged();
            }
        }

        private Debtor currentDebtor;
        public Debtor CurrentDebtor
        {
            get { return currentDebtor; }
            set
            {
                currentDebtor = value;
                NotifyPropertyChanged();
            }
        }


        private int currentIndex = -1;
        public int CurrentIndex
        {
            get { return currentIndex; }
            set
            {
                currentIndex = value;
                NotifyPropertyChanged();
            }
        }


        #region commands
        ICommand _AddCommand;
        public ICommand AddCommand
        {
            get
            {
                return _AddCommand ??
                (_AddCommand = new DelegateCommand(PreviusCommandExecute));
            }
        }
        private void PreviusCommandExecute()
        {
            if (dlg != null)
                dlg.Focus();
            else
            {
                dlg = new AddDebtor();
                dlg.Owner = App.Current.MainWindow;
                dlg.DebtorName = null;
                dlg.InitialValue = null;

                // Listen for the Apply button and show the dialog modelessly
                dlg.Apply += new EventHandler(Dlg_Apply);
                dlg.Closed += new EventHandler(Dlg_Closed);
                dlg.Show();

            }
        }
        void Dlg_Closed(object sender, EventArgs e)
        {
            dlg.Apply -= new EventHandler(Dlg_Apply);
            dlg.Closed -= new EventHandler(Dlg_Closed);
            dlg = null;
            App.Current.MainWindow.Focus();
        }

        void Dlg_Apply(object sender, EventArgs e)
        {
            // Pull the dialog out of the event args and apply the new settings
            AddDebtor dlg = (AddDebtor)sender;

            var Debtor = new Debtor();
            Debtor.Name = dlg.DebtorName;

            var debtAndDate = new DebtAndDate(Convert.ToDouble(dlg.InitialValue), DateTime.Now);

            Debtor.DebtAndDate.Add(debtAndDate);

            debtors.Add(Debtor);
            CurrentDebtor = Debtor;

        }


        #endregion

    }
}
