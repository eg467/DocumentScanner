using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentScanner.UserControls
{
    public partial class DateIncrementSelector : UserControl
    {
        public event EventHandler ValueChanged;

        public IDateTimeIncrementer Incrementer { get; private set; }

        public DateIncrementSelector()
        {
            InitializeComponent();
            this.numIntervalAmount.Value = 1;
            this.comboIntervalType.Items.AddRange(new IDateTimeIncrementer[]
            {
                new DayIncrementer(),
                new WeekIncrementer(),
                new MonthIncrementer(),
                new YearIncrementer(),
            });
            this.comboIntervalType.SelectedIndex = 2;
        }

        public string Type => Incrementer.ToString();

        public decimal IntervalAmount => this.numIntervalAmount.Value;

        private DateTime IncrementBy(DateTime start, IDateTimeIncrementer incrementer) =>
            incrementer.Add(start, (double)IntervalAmount);

        public DateTime Increment(DateTime start) => IncrementBy(start, Incrementer);

        public DateTime Decrement(DateTime start) =>
            IncrementBy(start, new NegatedIncrementor(Incrementer));

        private void numIntervalAmount_ValueChanged(object sender, EventArgs e)
        {
            OnValueChanged();
        }

        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void comboIntervalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Incrementer = this.comboIntervalType.SelectedItem as IDateTimeIncrementer;
            OnValueChanged();
        }
    }

    public interface IDateTimeIncrementer
    {
        DateTime Add(DateTime start, double amount);
    }

    public class DayIncrementer : IDateTimeIncrementer
    {
        public override string ToString() => "Days";

        public DateTime Add(DateTime start, double amount) => start.AddDays(amount);
    }

    public class WeekIncrementer : IDateTimeIncrementer
    {
        public override string ToString() => "Weeks";

        public DateTime Add(DateTime start, double amount) => start.AddDays(amount * 7);
    }

    public class MonthIncrementer : IDateTimeIncrementer
    {
        public override string ToString() => "Months";

        public DateTime Add(DateTime start, double amount) => start.AddMonths((int)amount);
    }

    public class YearIncrementer : IDateTimeIncrementer
    {
        public override string ToString() => "Years";

        public DateTime Add(DateTime start, double amount) => start.AddYears((int)amount);
    }

    public class NegatedIncrementor : IDateTimeIncrementer
    {
        private IDateTimeIncrementer _innerIncrementor;

        public override string ToString() => $"-{_innerIncrementor}";

        public NegatedIncrementor(IDateTimeIncrementer innerIncrementor)
        {
            _innerIncrementor = innerIncrementor;
        }

        public DateTime Add(DateTime start, double amount) =>
            _innerIncrementor.Add(start, -amount);
    }
}