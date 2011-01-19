using System;

using Caliberweb.Core.Specification;

namespace Caliberweb.Core.Verizon
{
    public class VerizonRecord
    {
        public DateTime Date { get; set; }
        public int Minutes { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }

        public override string ToString()
        {
            return string.Format("{0:MM/dd/yyyy hh:mm tt} {1:00} {2,-10} [{3}]", Date, Minutes, Number, Description);
        }

        public static ISpec<VerizonRecord> NumberIs(string number)
        {
            return Spec.Create<VerizonRecord>(r => r.Number.Equals(number, StringComparison.InvariantCultureIgnoreCase));
        }

        public static ISpec<VerizonRecord> NumberIsNot(string number)
        {
            return NumberIs(number).Negate();
        }

        public static ISpec<VerizonRecord> NotTollFree
        {
            get { return DescriptionIsNot("TOLL-FREE"); }
        }

        public static ISpec<VerizonRecord> DescriptionIs(string description)
        {
            return Spec.Create<VerizonRecord>(r => r.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase));
        }

        public static ISpec<VerizonRecord> DescriptionIsNot(string description)
        {
            return DescriptionIs(description).Negate();
        }

        public static ISpec<VerizonRecord> MinutesGreaterThan(int minutes)
        {
            return Spec.Create<VerizonRecord>(r => r.Minutes > minutes);
        }

        public static ISpec<VerizonRecord> AfterDate(DateTime date)
        {
            return Spec.Create<VerizonRecord>(r => r.Date > date);
        }
    }
}