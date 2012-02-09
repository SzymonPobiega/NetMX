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
using System.ComponentModel;
using NetMX.Relation;
using System.Collections.Generic;

namespace NetMX.WebUI.WebControls
{
   public class RelationRoleTableRow : TableRow
   {
      #region Members
      private RelationServiceMBean _relationService;
      private ObjectName _name;
      private string _relationId;
      private RoleInfo _roleInfo;
      private bool _hasValue;
      internal bool HasValue
      {
         get { return _hasValue; }
      }
      #endregion

      #region Controls                  
      #endregion

      internal RelationRoleTableRow(ObjectName name, string relationId, RoleInfo roleInfo, RelationServiceMBean relationService, string rowCssClass, CommandEventHandler navigateCommandHandler)
      {
         _name = name;
         _roleInfo = roleInfo;
         _relationId = relationId;
         _relationService = relationService;
         this.CssClass = rowCssClass;

         AddCell(roleInfo.Name, false);
         AddCell(roleInfo.Description, false);
         AddCell(string.Format("{0}{1}", roleInfo.Readable ? Resources.AttributeTableRow.ReadableSymbol : "", roleInfo.Writable ? Resources.AttributeTableRow.WritableSymbol : ""), true);
         AddValueCell(navigateCommandHandler);         
      }
      private void AddValueCell(CommandEventHandler navigateCommandHandler)
      {
         TableCell cell = new TableCell();
         cell.CssClass = this.CssClass;
         cell.HorizontalAlign = HorizontalAlign.Center;

         IList<ObjectName> names = _relationService.GetRole(_relationId, _roleInfo.Name);
         for (int i = 0; i < names.Count; i++)         
         {
            ObjectName value = names[i];
            if (value != _name)
            {
               LinkButton button = new LinkButton();
               button.Command += navigateCommandHandler;
               button.CommandArgument = value.CanonicalName;
               button.Text = value.CanonicalName;
               cell.Controls.Add(button);
               _hasValue = true;
               cell.Controls.Add(new LiteralControl("<br />"));               
            }            
         }
         if (cell.Controls.Count > 0)
         {
            cell.Controls.RemoveAt(cell.Controls.Count - 1);
         }
         this.Cells.Add(cell);
      }      
      private void AddCell(string value, bool center)
      {
         TableCell cell = new TableCell();
         cell.CssClass = this.CssClass;
         cell.Text = value;
         if (center)
         {
            cell.HorizontalAlign = HorizontalAlign.Center;
         }
         this.Cells.Add(cell);
      }

      #region Overridden      
      protected override void OnPreRender(EventArgs e)
      {
         base.OnPreRender(e);         
      }
      #endregion      
   }
}