using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace mtanksl.OpenTibia.Tests
{
    [TestClass]
    public class DateTests
    {
        [TestMethod]
        public void GetDayInterval()
        {
            // 15 00:00:00 in 24h interval = 16 00:00:00

                var now = new DateTime(2025, 01, 15, 0, 0, 0);  
            
                var next = GetDayInterval(now, 24 * 60 * 60 * 1000);
            
                Assert.AreEqual(new DateTime(2025, 01, 16, 0, 0, 0), next);

            // 15 23:59:59 in 24h interval = 16 00:00:00

                now = new DateTime(2025, 01, 15, 23, 59, 59);   
            
                next = GetDayInterval(now, 24 * 60 * 60 * 1000); 
            
                Assert.AreEqual(new DateTime(2025, 01, 16, 0, 0, 0), next);

            // 15 00:00:00 in 6h interval = 15 06:00:00

                now = new DateTime(2025, 01, 15, 0, 0, 0);   
            
                next = GetDayInterval(now, 6 * 60 * 60 * 1000); 
            
                Assert.AreEqual(new DateTime(2025, 01, 15, 6, 0, 0), next);

            // 15 05:59:59 in 6h interval = 15 06:00:00
                            
                now = new DateTime(2025, 01, 15, 5, 59, 59);  
            
                next = GetDayInterval(now, 6 * 60 * 60 * 1000); Assert.AreEqual(new DateTime(2025, 01, 15, 6, 0, 0), next);

            // 15 06:00:00 in 6h interval = 15 12:00:00
                            
                now = new DateTime(2025, 01, 15, 6, 0, 0);   
            
                next = GetDayInterval(now, 6 * 60 * 60 * 1000); 
            
                Assert.AreEqual(new DateTime(2025, 01, 15, 12, 0, 0), next);

            // 15 07:00:00 in 6h interval = 15 12:00:00
                        
                now = new DateTime(2025, 01, 15, 7, 0, 0);   
            
                next = GetDayInterval(now, 6 * 60 * 60 * 1000); 
            
                Assert.AreEqual(new DateTime(2025, 01, 15, 12, 0, 0), next);

            // 15 13:00:00 in 6h interval = 15 18:00:00
              
                now = new DateTime(2025, 01, 15, 13, 0, 0); 
            
                next = GetDayInterval(now, 6 * 60 * 60 * 1000); 
            
                Assert.AreEqual(new DateTime(2025, 01, 15, 18, 0, 0), next);

            // 15 19:00:00 in 6h interval = 16 00:00:00
            
                now = new DateTime(2025, 01, 15, 19, 0, 0);   
            
                next = GetDayInterval(now, 6 * 60 * 60 * 1000);
            
                Assert.AreEqual(new DateTime(2025, 01, 16, 0, 0, 0), next);

            // 15 14:59:59 in 6h interval with 3h offset = 15 15:00:00
            
                now = new DateTime(2025, 01, 15, 14, 59, 59);   
            
                next = GetDayInterval(now.AddHours(3), 6 * 60 * 60 * 1000).AddHours(-3);
            
                Assert.AreEqual(new DateTime(2025, 01, 15, 15, 0, 0), next);

            // 15 15:00:00 in 6h interval with 3h offset = 15 21:00:00
            
                now = new DateTime(2025, 01, 15, 15, 00, 00);   
            
                next = GetDayInterval(now.AddHours(3), 6 * 60 * 60 * 1000).AddHours(-3);
            
                Assert.AreEqual(new DateTime(2025, 01, 15, 21, 0, 0), next);
        }

        private static DateTime GetDayInterval(DateTime now, int millisecond)
        {
            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0).AddMilliseconds(Round(now.Hour * 60 * 60 * 1000 + now.Minute * 60 * 1000 + now.Second * 1000 + now.Millisecond, millisecond) ).AddMilliseconds(millisecond);
        }

        private static DateTime GetSecondInterval(DateTime now, int millisecond)
        {
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0).AddMilliseconds(Round(now.Second * 1000 + now.Millisecond, millisecond) ).AddMilliseconds(millisecond);
        }

        private static int Round(double value, int round)
        {
            return (int)Math.Floor(value / round) * round;
        }
    }
}