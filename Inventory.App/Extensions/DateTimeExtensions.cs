using System;
using System.Globalization;

namespace Inventory.Extensions
{
    /// <summary>
    /// Implements a (helper) class to convert date and time values.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts from DateTime to TimaSpan.
        /// </summary>
        /// <param name="dt">The source DateTime value.</param>
        /// <returns>Returns the time portion of DateTime in the form of TimeSpan if succeeded, null otherwise.</returns>
        public static TimeSpan? DateTimeToTimeSpan(this DateTime dt)
        {
            TimeSpan FResult;
            try
            {
                FResult = dt - dt.Date;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }

            return FResult;
        }

        /// <summary>
        /// Converts from Timespan to DateTime.
        /// </summary>
        /// <param name="ts">The source TimeSpan value.</param>
        /// <returns>Returns a DateTime filled with date equals to mindate and time equals to time in timespan if succeeded, null otherwise.</returns>
        public static DateTime? TimeSpanToDateTime(this TimeSpan ts)
        {
            DateTime? FResult = null;
            try
            {
                string year = string.Format("{0:0000}", DateTime.MinValue.Date.Year);
                string month = string.Format("{0:00}", DateTime.MinValue.Date.Month);
                string day = string.Format("{0:00}", DateTime.MinValue.Date.Day);

                string hours = string.Format("{0:00}", ts.Hours);
                string minutes = string.Format("{0:00}", ts.Minutes);
                string seconds = string.Format("{0:00}", ts.Seconds);

                string dSep = "-"; string tSep = ":"; string dtSep = "T";

                // yyyy-mm-ddTHH:mm:ss
                string dtStr = string.Concat(year, dSep, month, dSep, day, dtSep, hours, tSep, minutes, tSep, seconds);

                DateTime dt;
                if (DateTime.TryParseExact(dtStr, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dt))
                {
                    FResult = dt;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }

            return FResult;
        }

        /// <summary>
        /// Converts from DateTime to DateTimeOffSet.
        /// </summary>
        /// <param name="dt">The source DateTime to convert.</param>
        /// <returns>Returns a DateTimeOffSet if succeeded, null otherwise.</returns>
        public static DateTimeOffset? DateTimeToDateTimeOffSet(this DateTime dt)
        {
            try
            {
                return new DateTimeOffset(dt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Converts from DateTimeOffSet to DateTime.
        /// </summary>
        /// <param name="dto">The source DateTimeOffSet to convert.</param>
        /// <returns>Returns a DateTime if succeeded, null otherwise.</returns>
        public static DateTime? DateTimeOffSetToDateTime(this DateTimeOffset dto)
        {
            try
            {
                return dto.DateTime;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Объеденить <see cref="DateTimeOffset"/> с <see cref="TimeSpan"/> для получения Даты с временем
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTimeOffset? CombineDateTimeOffsetWithTime(this DateTimeOffset? dto, TimeSpan time)
        {
            DateTimeOffset? dateTime = dto;
            if (dateTime != null)
            {
                var date = dto.Value;
                dateTime = new DateTimeOffset(date.Year, date.Month, date.Day,
                    time.Hours, time.Minutes, time.Seconds, date.Offset);
            }
            return dateTime;
        }

        public static DateTimeOffset CombineDateTimeOffsetWithTime(this DateTimeOffset dto, TimeSpan time)
        {
            DateTimeOffset dateTime = dto;
            if (dateTime != null)
            {
                var date = dto;
                dateTime = new DateTimeOffset(date.Year, date.Month, date.Day,
                    time.Hours, time.Minutes, time.Seconds, date.Offset);
            }
            return dateTime;
        }

        public static DateTimeOffset CombineDateTimeOffsetWithTimeNotNull(this DateTimeOffset dto, TimeSpan time)
        {
            DateTimeOffset dateTime = dto;
            if (dateTime != null)
            {
                var date = dto;
                dateTime = new DateTimeOffset(date.Year, date.Month, date.Day,
                    time.Hours, time.Minutes, time.Seconds, date.Offset);
            }
            return dateTime;
        }

        public static TimeSpan GetTimeOfDayOrDefault(this DateTimeOffset? dto)
        {
            if (dto.HasValue)
            {
                return dto.Value.TimeOfDay;
            }
            return default(TimeSpan);
        }

        public static TimeSpan GetTimeOfDayOrDefault(this DateTimeOffset dto)
        {
            if (dto != null)
            {
                return dto.TimeOfDay;
            }
            return default(TimeSpan);
        }
    }
}
