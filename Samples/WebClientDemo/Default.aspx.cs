using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetMX.WebUI.WebControls;

namespace WebClientDemo
{
   public partial class _Default : System.Web.UI.Page
   {
      protected void Page_Load(object sender, EventArgs e)
      {
      }

      protected void ShowDetails(object sender, EventArgs e)
      {
         string selectedName = beanList.SelectedValue;
         beanUI.ObjectName = selectedName;
         view.ActiveViewIndex = 1;
      }

      protected void HideDetails(object sender, EventArgs e)
      {
         beanUI.ObjectName = null;
         view.ActiveViewIndex = 0;
      }

      protected override void OnInit(EventArgs e)
      {
         base.OnInit(e);
         beanList.DataSource = proxy.ServerConnection.QueryNames(null, null);
         beanList.DataBind();
      }
   }
}
