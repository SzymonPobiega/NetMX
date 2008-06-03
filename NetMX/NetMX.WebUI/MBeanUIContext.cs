using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace NetMX.WebUI.WebControls
{
   public class MBeanUIContext
   {
      private MBeanUI _ui;

      private MBeanUIContext(MBeanUI ui)
      {
         _ui = ui;
      }

      public static MBeanUIContext GetInstance(Control thisControl)
      {         
         Control current = thisControl;
         while (current != null)
         {
            MBeanUI ui = current as MBeanUI;
            if (ui != null)
            {
               return new MBeanUIContext(ui);
            }
            current = current.Parent;
         }
         throw new InvalidOperationException();
      }
      public string ControlCssClass
      {
         get { return _ui.CssClass; }  
      }
      public string ButtonCssClass
      {
         get { return _ui.ButtonCssClass; }  
      }
      /// <summary>
      /// Cell-spacing of all embedded tables.
      /// </summary>
      public int TableCellSpacing
      {
         get { return _ui.TableCellSpacing; }  
      }
      /// <summary>
      /// Cell-padding of all embedded tables.
      /// </summary>
      public int TableCellPadding
      {
         get { return _ui.TableCellPadding; }  
      }
      public string AttributeTableCssClass
      {
         get { return _ui.AttributeTableCssClass; }  
      }
      public string TabularDataTableCssClass
      {
         get { return _ui.TabularDataTableCssClass; }
      }
      public string OperationTableCssClass
      {
         get { return _ui.OperationTableCssClass; }
      }
      public string RelationTableCssClass
      {
         get { return _ui.RelationTableCssClass; }
      }
      public string GeneralInfoCssClass
      {
         get { return _ui.GeneralInfoCssClass; }
      }
      public string GeneralInfoNameCssClass
      {
         get { return _ui.GeneralInfoNameCssClass; }
      }
      public string GeneralInfoValueCssClass
      {
         get { return _ui.GeneralInfoValueCssClass; }
      }
      public string SectionTitleCssClass
      {
         get { return _ui.SectionTitleCssClass; }
      }
   }
}
