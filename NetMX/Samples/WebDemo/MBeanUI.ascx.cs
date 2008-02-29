#region Using
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NetMX;
using NetMX.Remote;
using Controls;
using System.ComponentModel;
using System.Collections.Generic;
#endregion

public partial class MBeanUI : System.Web.UI.UserControl
{
	#region PROPERTIES
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
				_proxy = (MBeanServerProxy)NamingContainer.FindControl(MBeanServerProxyID);
			}
			return _proxy;
		}
	}
	private string _mBeanDescription;
	/// <summary>
	/// Description of this MBean
	/// </summary>
	public string MBeanDescription
	{
		get { return _mBeanDescription; }
	}
	private string _mBeanType;
	/// <summary>
	/// Type of this MBean
	/// </summary>
	public string MBeanType
	{
		get { return _mBeanType; }
	}
	#endregion

	#region OVERRIDDEN
	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		attributes.DataBind();
	}
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		operations2.DataBind();

	}
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		CreateAttributesColumns();
		CreateOperationsColumns();
	}
	#endregion

	#region EVENT HANDLERS
	protected void Page_Load(object sender, EventArgs e)
	{
		attributes.RowDataBound += new GridViewRowEventHandler(attributes_RowDataBound);
		Proxy.GetDescriptionAndClassName(_objectName, out _mBeanDescription, out _mBeanType);
	}

	private void attributes_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			MBeanAttribute attribute = (MBeanAttribute)e.Row.DataItem;
			if (!attribute.Writable)
			{
				e.Row.Cells[4].Controls.Clear();
			}
		}
	}

	private void attributes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		attributes.EditIndex = -1;
	}

	private void attributes_RowEditing(object sender, GridViewEditEventArgs e)
	{
		attributes.EditIndex = e.NewEditIndex;
	}	
	#endregion

	#region UTILITY
	private void CreateAttributesColumns()
	{
		BoundField bf = new BoundField();
		bf.DataField = "Name";
		bf.HeaderText = "Name";
		bf.ReadOnly = true;
		attributes.Columns.Add(bf);

		bf = new BoundField();
		bf.DataField = "Description";
		bf.HeaderText = "Description";
		bf.ReadOnly = true;
		attributes.Columns.Add(bf);

		bf = new BoundField();
		bf.DataField = "Access";
		bf.HeaderText = "Access";
		bf.ReadOnly = true;
		attributes.Columns.Add(bf);

		bf = new BoundField();
		bf.DataField = "Value";
		bf.HeaderText = "Value";
		bf.NullDisplayText = "(not readable)";
		attributes.Columns.Add(bf);

		CommandField cf = new CommandField();
		cf.ShowEditButton = true;
		cf.ButtonType = ButtonType.Button;
		attributes.Columns.Add(cf);
	}
	private void CreateOperationsColumns()
	{
		BoundField bf = new BoundField();
		bf.DataField = "Name";
		bf.HeaderText = "Name";
		bf.ReadOnly = true;
		operations2.Columns.Add(bf);

		bf = new BoundField();
		bf.DataField = "Description";
		bf.HeaderText = "Description";
		bf.ReadOnly = true;
		operations2.Columns.Add(bf);

		bf = new BoundField();
		bf.DataField = "Impact";
		bf.HeaderText = "Impact";
		bf.ReadOnly = true;
		operations2.Columns.Add(bf);

		OperationArgumentsField oaf = new OperationArgumentsField();
		oaf.DataField = "Signature";
		operations2.Columns.Add(oaf);

		ButtonField btnField = new ButtonField();
		btnField.ButtonType = ButtonType.Button;
		btnField.CommandName = "update";
		btnField.Text = "Invoke";
		operations2.Columns.Add(btnField);
	}
	#endregion
}
