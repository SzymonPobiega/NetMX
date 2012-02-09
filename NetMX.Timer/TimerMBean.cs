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
      int AddNotification1(string type, string message, object userData, DateTime date);
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
      int AddNotification2(string type, string message, object userData, DateTime date, TimeSpan period);
      /// <summary>
      /// </summary>
      /// <param name="type">The timer notification type.</param>
      /// <param name="message">The timer notification detailed message.</param>
      /// <param name="userData">The timer notification user data object.</param>
      /// <param name="date">The date when the notification occurs.</param>
      /// <param name="period">The period of the timer notification.</param>
      /// <param name="nbOccurences">The total number the timer notification will be emitted.</param>
      /// <returns>The identifier of the new created timer notification.</returns>
      int AddNotification3(string type, string message, object userData, DateTime date, TimeSpan period, long nbOccurences);
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
      int AddNotification4(string type, string message, object userData, DateTime date, TimeSpan period, long nbOccurences, bool fixedRate);
      /// <summary>
      /// Gets all timer notification identifiers registered into the list of notifications.
      /// </summary>
      /// <returns>A collection of noticiation ids. An empty collections it there are no registered notifiactions.</returns>
      IEnumerable<int> GetAllNotificationIDs();
      /// <summary>
      /// Gets all the identifiers of timer notifications corresponding to the specified type.
      /// </summary>
      /// <param name="type">The timer notification type.</param>
      /// <returns>A collection of noticiation ids. An empty collections it there are no registered notifiactions with provided type.</returns>
      IEnumerable<int> GetNotificationIDs(string type);
      /// <summary>
      /// Gets the date associated to a timer notification.
      /// </summary>
      /// <param name="notifiactionId">The timer notification identifier.</param>
      /// <returns>The date.</returns>
      /// <exception cref="NotificationNotFoundException">The specified identifier does not correspond to any timer notification in the list of notifications of this timer MBean.</exception>
      DateTime GetDate(int notifiactionId);
      /// <summary>
      /// Gets the flag indicating whether a periodic notification is executed at fixed-delay or at fixed-rate.
      /// </summary>
      /// <param name="notificationId">The timer notification identifier.</param>
      /// <returns>The flag indicating whether a periodic notification is executed at fixed-delay or at fixed-rate.</returns>
      /// <exception cref="NotificationNotFoundException">The specified identifier does not correspond to any timer notification in the list of notifications of this timer MBean.</exception>
      bool GetFixedRate(int notificationId);
      /// <summary>
      /// Gets the number of timer notifications registered into the list of notifications.
      /// </summary>      
      int NotificationsCount { get; }
      /// <summary>
      /// Gets the remaining number of occurrences associated to a timer notification.
      /// </summary>
      /// <param name="notificationId">The timer notification identifier.</param>
      /// <returns>The remaining number of occurrences.</returns>
      /// <exception cref="NotificationNotFoundException">The specified identifier does not correspond to any timer notification in the list of notifications of this timer MBean.</exception>
      long GetNbOccurences(int notificationId);
      /// <summary>
      /// Gets the timer notification detailed message corresponding to the specified identifier.
      /// </summary>
      /// <param name="notificationId">The timer notification identifier.</param>
      /// <returns>The timer notification detailed message.</returns>
      /// <exception cref="NotificationNotFoundException">The specified identifier does not correspond to any timer notification in the list of notifications of this timer MBean.</exception>
      string GetNotificationMessage(int notificationId);
      /// <summary>
      /// Gets the timer notification type corresponding to the specified identifier.
      /// </summary>
      /// <param name="notificationId">The timer notification identifier.</param>
      /// <returns>The timer notification type.</returns>
      /// <exception cref="NotificationNotFoundException">The specified identifier does not correspond to any timer notification in the list of notifications of this timer MBean.</exception>
      string GetNotificationType(int notificationId);
      /// <summary>
      /// Gets the timer notification user data object corresponding to the specified identifier.
      /// </summary>
      /// <param name="notificationId">The timer notification identifier.</param>
      /// <returns>The timer notification user data object.</returns>
      /// <exception cref="NotificationNotFoundException">The specified identifier does not correspond to any timer notification in the list of notifications of this timer MBean.</exception>
      object GetNotificationUserData(int notificationId);
      /// <summary>
      /// Gets the period associated to a timer notification.
      /// </summary>
      /// <param name="notificationId">The timer notification identifier.</param>
      /// <returns>The period.</returns>
      /// <exception cref="NotificationNotFoundException">The specified identifier does not correspond to any timer notification in the list of notifications of this timer MBean.</exception>
      TimeSpan GetPeriod(int notificationId);
      /// <summary>
      /// Gets or sets the flag indicating whether or not the timer sends past notifications.
      /// </summary>
      /// <exception cref="NotificationNotFoundException">The specified identifier does not correspond to any timer notification in the list of notifications of this timer MBean.</exception>
      bool SendPastNotifications { get; set; }
      /// <summary>
      /// Tests whether the timer MBean is active.
      /// </summary>
      bool IsActive { get; }
      /// <summary>
      /// Tests whether the list of timer notifications is empty.
      /// </summary>
      bool IsEmpty { get; }
      /// <summary>
      /// Removes all the timer notifications from the list of notifications and resets the counter used to 
      /// update the timer notification identifiers.
      /// </summary>
      void RemoveAllNotifications();
      /// <summary>
      /// Removes the timer notification corresponding to the specified identifier from the list of notifications.
      /// </summary>
      /// <param name="notificationId">The timer notification identifier.</param>
      /// <exception cref="NotificationNotFoundException">The specified identifier does not correspond to any timer notification in the list of notifications of this timer MBean.</exception>
      void RemoveNotification(int notificationId);
      /// <summary>
      /// Starts the timer.
      /// If there is one or more timer notifications before the time in the list of notifications, the 
      /// notification is sent according to the <see cref="SendPastNotifications"/> flag and then, updated 
      /// according to its period and remaining number of occurrences. If the timer notification date remains 
      /// earlier than the current date, this notification is just removed from the list of notifications. 
      /// </summary>
      void Start();
      /// <summary>
      /// Stops the timer.
      /// </summary>
      void Stop();
   }
}
