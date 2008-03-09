using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NetMX;
using System.Collections.Generic;





namespace Controls
{
   public class MBeanEditView : MBeanDefaultView
   {
      private string _editedAttribute;

      public MBeanEditView(Control postbackController, ObjectName objectName, IMBeanServerConnection connection, string editedAttribute)
         : base(postbackController, objectName, connection)
      {
         _editedAttribute = editedAttribute;
      }
      protected override void AddAttributesValueCell(TableRow row, string name, string value)
      {
         if (name == _editedAttribute)
         {
            TableCell cell = new TableCell();
            cell.CssClass = "Attribute";
            TextBox box = new TextBox();
            box.ID = name;
            box.Text = value;
            cell.Controls.Add(box);
            row.Cells.Add(cell);
         }
         else
         {
            base.AddAttributesValueCell(row, name, value);
         }
      }
   }
   public class MBeanDefaultView : WebControl
   {
      protected Control _controller;
      protected IMBeanServerConnection _connection;
      protected ObjectName _objectName;

      public MBeanDefaultView(Control postbackController, ObjectName objectName, IMBeanServerConnection connection)
      {
         _controller = postbackController;
         _connection = connection;
         _objectName = objectName;
      }
      protected override void CreateChildControls()
      {
         base.CreateChildControls();
         Table attributes = new Table();
         attributes.ControlStyle.Width = Unit.Percentage(100);
         attributes.Rows.Add(CreateAttributesHeader());

         MBeanInfo info = _connection.GetMBeanInfo(_objectName);
         string[] attributeNames = new string[info.Attributes.Count];
         for (int i = 0; i < info.Attributes.Count; i++)
         {
            if (info.Attributes[i].Readable)
            {
               attributeNames[i] = info.Attributes[i].Name;
            }
         }
         IList<AttributeValue> valuesTmp = _connection.GetAttributes(_objectName, attributeNames);
         Dictionary<string, object> values = new Dictionary<string, object>();
         foreach (AttributeValue val in valuesTmp)
         {
            values[val.Name] = val.Value;
         }
         foreach (MBeanAttributeInfo attrInfo in info.Attributes)
         {
            object value = null;
            values.TryGetValue(attrInfo.Name, out value);
            attributes.Rows.Add(CreateAttributesRow(attrInfo, value));
         }
         this.Controls.Add(attributes);
      }
      private TableHeaderRow CreateAttributesHeader()
      {
         TableHeaderRow attrHeader = new TableHeaderRow();
         attrHeader.CssClass = "Attribute";
         AddAttributesHeaderCell(attrHeader, "Name", 20);
         AddAttributesHeaderCell(attrHeader, "Description", 30);
         AddAttributesHeaderCell(attrHeader, "Access", 10);
         AddAttributesHeaderCell(attrHeader, "Value", 25);
         AddAttributesHeaderCell(attrHeader, "Actions", 25);
         return attrHeader;
      }
      private void AddAttributesHeaderCell(TableHeaderRow row, string name, double percentSize)
      {
         TableHeaderCell cell = new TableHeaderCell();
         cell.CssClass = "Attribute";
         cell.ControlStyle.Width = Unit.Percentage(percentSize);
         cell.Text = name;
         row.Cells.Add(cell);
      }
      private TableRow CreateAttributesRow(MBeanAttributeInfo attrInfo, object value)
      {
         TableRow attrRow = new TableRow();
         AddAttributesCell(attrRow, attrInfo.Name);
         AddAttributesCell(attrRow, attrInfo.Description);
         AddAttributesCell(attrRow, string.Format("{0}{1}", attrInfo.Readable ? "R" : "", attrInfo.Writable ? "W" : ""));
         AddAttributesValueCell(attrRow, attrInfo.Name, value != null ? value.ToString() : "");
         AddAttributesActions(attrRow, attrInfo.Name);
         return attrRow;
      }

      protected virtual void AddAttributesActions(TableRow attrRow, string name)
      {
         TableCell actionsCell = new TableCell();
         HtmlInputButton editButton = new HtmlInputButton();
         editButton.Value = "Edit";
         editButton.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(_controller, "_ATTR_" + name);
         actionsCell.Controls.Add(editButton);
         attrRow.Cells.Add(actionsCell);
      }
      protected virtual void AddAttributesValueCell(TableRow row, string name, string value)
      {
         AddAttributesCell(row, value);
      }
      private void AddAttributesCell(TableRow row, string value)
      {
         TableCell cell = new TableCell();
         cell.CssClass = "Attribute";
         cell.Text = value;
         row.Cells.Add(cell);
      }
   }

   public class MBeanUI : CompositeControl, IPostBackEventHandler, IPostBackDataHandler
   {
      private string _mBeanServerProxyID;
      /// <summary>
      /// ID of MBeanServerProxy control
      /// </summary>
      public string MBeanServerProxyID
      {
         get { return _mBeanServerProxyID; }
         set { _mBeanServerProxyID = value; }
      }
      private ObjectName _objectName;
      /// <summary>
      /// ObjectName of displayed/edited MBean
      /// </summary>
      public string ObjectName
      {
         get { return _objectName.ToString(); }
         set { _objectName = new ObjectName(value); }
      }
      private MBeanServerProxy _proxy;
      private MBeanServerProxy Proxy
      {
         get
         {
            if (_proxy == null)
            {
               Control container = this.NamingContainer;
               while (container != null)
               {
                  _proxy = (MBeanServerProxy)container.FindControl(MBeanServerProxyID);
                  if (_proxy != null)
                  {
                     break;
                  }
                  else
                  {
                     container = container.NamingContainer;
                  }
               }
            }
            return _proxy;
         }
      }
      protected override void CreateChildControls()
      {
         base.CreateChildControls();
      }
      protected override void OnPreRender(EventArgs e)
      {
         base.OnPreRender(e);
         MBeanDefaultView defaultView = new MBeanDefaultView(this, _objectName, Proxy.ServerConnection);
         this.Controls.Add(defaultView);
      }

      #region IPostBackEventHandler Members
      public void RaisePostBackEvent(string eventArgument)
      {

      }
      #endregion

      #region IPostBackDataHandler Members
      public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
      {
         return true;
      }
      public void RaisePostDataChangedEvent()
      {
      }
      #endregion
   }
}
