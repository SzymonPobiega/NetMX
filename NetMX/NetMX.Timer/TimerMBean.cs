using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Timer
{
   public interface TimerMBean
   {
      /// <summary>
      /// Creates a new timer notification with the specified type, message and userData and inserts it into 
      /// the list of notifications with a given date and a null period and number of occurrences.
      /// 
      /// The timer notification will be handled once at the specified date.
      /// 
      /// If the timer notification to be inserted has a date that is before the current date, the method 
      /// behaves as if the specified date were the current date and the notification is delivered immediately. 
      /// </summary>
      /// <param name="type">The timer notification type.</param>
      /// <param name="message">The timer notification detailed message.</param>
      /// <param name="userData">The timer notification user data object.</param>
      /// <param name="date">The date when the notification occurs.</param>
      /// <returns>The identifier of the new created timer notification.</returns>
      int AddNotification(string type, string message, object userData, DateTime date);
      /// <summary>
      /// Creates a new timer notification with the specified type, message and userData and inserts it into 
      /// the list of notifications with a given date and period and a null number of occurrences.
      /// 
      /// The timer notification will repeat continuously using the timer period using a fixed-delay execution 
      /// scheme, as specified in Timer. In order to use a fixed-rate execution scheme, use 
      /// <see cref="AddNotification(string,string,object,DateTime,TimeSpan,long,bool)"/>      
      /// 
      /// If the timer notification to be inserted has a date that is before the current date, 
      /// the method behaves as if the specified date were the current date. The first notification is delivered 
      /// immediately and the subsequent ones are spaced as specified by the period parameter. 
      /// </summary>
      /// <param name="type">The timer notification type.</param>
      /// <param name="message">The timer notification detailed message.</param>
      /// <param name="userData">The timer notification user data object.</param>
      /// <param name="date">The date when the notification occurs.</param>
      /// <param name="period">The period of the timer notification.</param>
      /// <returns>The identifier of the new created timer notification.</returns>
      int AddNotification(string type, string message, object userData, DateTime date, TimeSpan period);
      /// <summary>
      /// </summary>
      /// <param name="type">The timer notification type.</param>
      /// <param name="message">The timer notification detailed message.</param>
      /// <param name="userData">The timer notification user data object.</param>
      /// <param name="date">The date when the notification occurs.</param>
      /// <param name="period">The period of the timer notification.</param>
      /// <param name="nbOccurences">The total number the timer notification will be emitted.</param>
      /// <returns>The identifier of the new created timer notification.</returns>
      int AddNotification(string type, string message, object userData, DateTime date, TimeSpan period, long nbOccurences);
      /// <summary>
      /// </summary>
      /// <param name="type">The timer notification type.</param>
      /// <param name="message">The timer notification detailed message.</param>
      /// <param name="userData">The timer notification user data object.</param>
      /// <param name="date">The date when the notification occurs.</param>
      /// <param name="period">The period of the timer notification.</param>
      /// <param name="nbOccurences">The total number the timer notification will be emitted.</param>
      /// <param name="fixedRate">If true and if the notification is periodic, the notification is scheduled 
      /// with a fixed-rate execution scheme. If false and if the notification is periodic, the notification 
      /// is scheduled with a fixed-delay execution scheme. Ignored if the notification is not periodic.</param>
      /// <returns>The identifier of the new created timer notification.</returns>
      int AddNotification(string type, string message, object userData, DateTime date, TimeSpan period, long nbOccurences, bool fixedRate);
   }
}
