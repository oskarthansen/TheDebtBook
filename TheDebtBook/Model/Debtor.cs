using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDebtBook.Model
{
    public class Debtor
    {
        private string name;
        private List<DebtAndDate> debtAndDate = new List<DebtAndDate>();
        private double debt = 0.0;

        public Debtor()
        {
            
        }
        public Debtor(string DebtorName)
        {
            name = DebtorName;
        }
        
        public double Debt
        {
            get { return debt; }
            set { debt = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<DebtAndDate> DebtAndDate
        {
            get { return debtAndDate; }
            set { debtAndDate = value; }
        }
    }
}
