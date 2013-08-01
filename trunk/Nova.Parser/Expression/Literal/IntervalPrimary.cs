using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class IntervalPrimary : Literal
    {
        public enum UnitType
        {
            MicroSecond,
            Second,
            Minute,
            Hour,
            Day,
            Week,
            Month,
            Quarter,
            Year,
            SecondMicrosecond,
            MinuteMicrosecond,
            MinuteSecond,
            HourMicrosecond,
            HourSecond,
            HourMinute,
            DayMicrosecond,
            DaySecond,
            DayMinute,
            DayHour,
            YearMonth
        }

        public IntervalPrimary(IExpression quantity, UnitType unit)
            : base()
        {
            if (quantity == null) throw new ArgumentNullException("quantity expression is null");
            if (unit == null) throw new ArgumentNullException("unit of time is null");

            this.Quantity = quantity;
            this.Unit = unit;
        }

        public IExpression Quantity { get; protected set; }
        public UnitType Unit { get; protected set; }

        public static UnitType ConvertToUnitType(string unitName)
        {
            if (String.IsNullOrEmpty(unitName))
                throw new ArgumentNullException("unitName");
            UnitType type = UnitType.Day;

            if (Enum.TryParse<UnitType>(unitName, out type))
                return type;
            throw new ArgumentException("can't convert String " + unitName + " to UnitType");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<IntervalPrimary>(this);
        }
    }
}
