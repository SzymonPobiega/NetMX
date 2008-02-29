#region Using
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using NetMX;
using NetMX.Remote;
#endregion

public sealed class MBeanAttribute
{
	public MBeanAttribute(MBeanAttributeInfo info, string value)
	{
		_name = info.Name;
		_description = info.Description;
		_type = info.Type;
		_readable = info.Readable;
		_writable = info.Writable;
		_value = value;
	}

	private string _name;
	public string Name
	{
		get { return _name; }
	}
	private string _description;
	public string Description
	{
		get { return _description; }
	}
	private string _type;
	public string Type
	{
		get { return _type; }
	}
	private bool _readable;
	public bool Readable
	{
		get { return _readable; }
	}
	private bool _writable;
	public bool Writable
	{
		get { return _writable; }
	}
	private string _value;
	public string Value
	{
		get { return _value; }
		set { _value = value; }
	}
	public string Access
	{
		get
		{
			return string.Format("{0}{1}", Readable ? "R" : "", Writable ? "W" : "");
		}
	}
}

