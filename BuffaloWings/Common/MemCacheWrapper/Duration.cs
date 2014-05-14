namespace Microsoft.Dldw.BuffaloWings.Common.ApplicationCache
{
    using System.Collections.Generic;

    public class Duration
    {
        public DateInterval Unit { get; private set; }
        public long Value {get; private set;}

        public Duration(DateInterval unit, long value)
        {
            Unit = unit;
            Value = value;
        }
        private readonly static Dictionary<DateInterval, long>  unit2Scale = new Dictionary<DateInterval, long>
            {
            { DateInterval.None, 0L },
            { DateInterval.Tick, 1L },
            { DateInterval.Millisecond, 10000L },
            { DateInterval.Second, 10000L * 1000 },
            { DateInterval.Minute, 10000L * 1000 * 60 },
            { DateInterval.Hour, 10000L * 1000 * 3600 },
            { DateInterval.Day, 10000L * 1000 * 3600 * 24 },
            { DateInterval.Week, 10000L * 1000 * 3600 * 24 * 7 },
            { DateInterval.Month, 10000L * 1000 * 3600 * 24 * 365 / 12 },
            { DateInterval.Quarter, 10000L * 1000 * 3600 * 24 * 365 / 4 },
            { DateInterval.Year, 10000L * 1000 * 3600 * 24 * 365 },
        };

        
        public static long UnitToScale(DateInterval unit)
        {
            return unit2Scale[unit];
        }
    }
    /// <summary>
    /// Data Interval conatins various date/time intervals (day,second, minute etc), can be used to specify time duartion 
    /// </summary>
    public enum DateInterval
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Year
        /// </summary>
        Year,

        /// <summary>
        /// Quarter
        /// </summary>
        Quarter,

        /// <summary>
        /// Month
        /// </summary>
        Month,

        /// <summary>
        /// Week
        /// </summary>
        Week,

        /// <summary>
        /// Day
        /// </summary>
        Day,

        /// <summary>
        /// Hour
        /// </summary>
        Hour,

        /// <summary>
        /// Minute
        /// </summary>
        Minute,

        /// <summary>
        /// Second
        /// </summary>
        Second,

        /// <summary>
        /// Millisecond
        /// </summary>
        Millisecond,

        /// <summary>
        /// Ticks
        /// </summary>
        Tick
    }
}
