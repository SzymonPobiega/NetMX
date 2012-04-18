using System;
using NetMX;
using NetMX.Timer;

namespace TimersConsoleDemo
{
   class Program
   {
      static void Main(string[] args)
      {
         IMBeanServer server = MBeanServerFactory.CreateMBeanServer();
         Timer timer = new Timer();
         ObjectName name = new ObjectName("Timer:");
         server.RegisterMBean(timer, name);

         var timerBean = server.CreateDynamicProxy(name);
         timerBean.Start();
         server.AddNotificationListener(name, OnTimerEvent, null, null);         

         Console.WriteLine("******");
         timerBean.AddNotification2("Type1", "Message1", 4, DateTime.Now.AddSeconds(2), new TimeSpan(0, 0, 0, 1));
         //timerBean.AddNotification4("Type1", "Message1", 4, DateTime.Now.AddSeconds(2), new TimeSpan(0,0,0,1),3,true);
         timerBean.SendPastNotifications = true;
         bool exit = false;
         while (!exit)
         {
            ConsoleKeyInfo info = Console.ReadKey();
            switch (char.ToUpper(info.KeyChar))
            {
               case 'X':
                  exit = true;
                  break;
               case 'S':
                  if (timerBean.IsActive)
                  {
                     timerBean.Stop();
                  }
                  else
                  {
                     timerBean.Start();
                  }
                  break;
            }
         }               
      }
      static void OnTimerEvent(Notification notification, object handback)
      {
         TimerNotification timerNotif = (TimerNotification) notification;
         Console.WriteLine("Timer event {0}! Message: {1}, user provided data: {2}", timerNotif.Type, timerNotif.Message, notification.UserData);
      }
   }
}
