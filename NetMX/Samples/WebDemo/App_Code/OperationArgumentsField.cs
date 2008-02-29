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

namespace Controls
{
	public class OperationArgumentsField : BoundField
	{		
		protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
		{
			PlaceHolder arguments = new PlaceHolder();
			arguments.DataBinding += new EventHandler(OnDataBindField);			
			cell.Controls.Add(arguments);
		}

		protected override void OnDataBindField(object sender, EventArgs e)
		{
			PlaceHolder control = (PlaceHolder)sender;			
			MBeanParameterInfo[] signature = (MBeanParameterInfo[])this.GetValue(control.NamingContainer);			
			for (int i = 0; i < signature.Length; i++)
			{
				MBeanParameterInfo info = signature[i];				
				control.Controls.Add(new LiteralControl(info.Name+"&nbsp;"));
				//HiddenField hiddenType = new HiddenField();
				//hiddenType.ID = info.Name + "#type";
				//hiddenType.Value = info.Type;
				//control.Controls.Add(hiddenType);
				TextBox valueBox = new TextBox();
				valueBox.ID = info.Name;
				control.Controls.Add(valueBox);
				if (i < signature.Length - 1)
				{
					control.Controls.Add(new LiteralControl("&nbsp;"));
				}
			}
		}
		public override void ExtractValuesFromCell(System.Collections.Specialized.IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
		{
			PlaceHolder holder = (PlaceHolder)cell.Controls[0];
			foreach (Control ctl in holder.Controls)
			{
				TextBox valueBox = ctl as TextBox;
				if (valueBox != null)
				{
					dictionary[valueBox.ID] = valueBox.Text;
				}
				//else
				//{
				//   HiddenField hiddenType = ctl as HiddenField;
				//   if (hiddenType != null)
				//   {
				//      dictionary[valueBox.ID] = hiddenType.Value;
				//   }
				//}
			}
		}

		protected override DataControlField CreateField()
		{
			return new OperationArgumentsField();
		}
	}
}
