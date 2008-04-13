#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
#endregion

namespace NetMX
{
	[Serializable]
	internal sealed class MBeanPermissionImpl
	{
		#region REGEX
		private static readonly Regex _namePattern = new Regex("(?<className>.+?)?#(?<memberName>.+?)?\\[(?<objectName>.+?)?\\]", RegexOptions.Compiled);
		#endregion

		#region MEMBERS
		private string _className;
		private string _memberName;
		private ObjectName _objectName;
		private MBeanPermissionAction _actions;
		#endregion

		#region PROPERTIES
		internal string ClassName
		{
			get { return _className; }
		}
		internal string MemberName
		{
			get { return _memberName; }
		}
		internal ObjectName ObjectName
		{
			get { return _objectName; }
		}
		internal MBeanPermissionAction Actions
		{
			get { return _actions; }
		}
		#endregion

		#region CONSTRUCTOR
		public MBeanPermissionImpl(string name, MBeanPermissionAction actions)
		{
			ParseName(name, out _className, out _memberName, out _objectName);
			_actions = actions;
		}
		public MBeanPermissionImpl(string className, string memberName, ObjectName objectName, MBeanPermissionAction actions)
		{
			_className = className;
			_memberName = memberName;
			_objectName = objectName;
			_actions = actions;
		}
		#endregion

		#region UTILITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength")]
		internal void VerifyAsNeeded()
		{
			if (_className == "" || _memberName == "" || (_objectName != null && _objectName.ToString() == ""))
			{
				throw new InvalidOperationException("This operation equires a 'needed' permission.");
			}
		}
		internal void VerifyAsHeld()
		{
			if (_className == null || _memberName == null || _objectName == null)
			{
				throw new InvalidOperationException("This operation equires a 'held' permission.");
			}
		}
		private static void ParseName(string name, out string className, out string memberName, out ObjectName objectName)
		{
			Match m = _namePattern.Match(name);
			if (m.Success)
			{
				Group classNameGroup = m.Groups["className"];
				Group memberNameGroup = m.Groups["memberName"];
				Group objectNameGroup = m.Groups["objectName"];
				if (classNameGroup.Success)
				{
					if (classNameGroup.Value != "-")
					{
						className = classNameGroup.Value;
					}
					else
					{
						className = null;
					}
				}
				else
				{
					className = "";
				}
				if (memberNameGroup.Success)
				{
					if (memberNameGroup.Value != "-")
					{
						memberName = memberNameGroup.Value;
					}
					else
					{
						memberName = null;
					}
				}
				else
				{
					memberName = "";
				}
				if (objectNameGroup.Success)
				{
					if (objectNameGroup.Value != "-")
					{
						objectName = new ObjectName(objectNameGroup.Value);
					}
					else
					{
						objectName = null;
					}
				}
				else
				{
					objectName = new ObjectName("");
				}
			}
			else
			{
				throw new ArgumentException("Invalid name format. Should be: className#member[objectName].", "name");
			}
		}
		private static string GenerateName(string className, string memberName, ObjectName objectName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}#{1}[{2}]",
				 className ?? "-", memberName ?? "-", objectName != null ? objectName.ToString() : "-");
		}
		#endregion

		#region OVERRIDDEN
		public override string ToString()
		{
			return GenerateName(_className, _memberName, _objectName);
		}
		public override bool Equals(object obj)
		{
			MBeanPermissionImpl other = obj as MBeanPermissionImpl;
			if (other == null)
			{
				return false;
			}
			return (
				 (this._className == null && other._className == null) ||
				 (this._className != null && other._className != null && this._className.Equals(other._className))) &&
				 (
				 (this._memberName == null && other._memberName == null) ||
				 (this._memberName != null && other._memberName != null && this._memberName.Equals(other._memberName))) &&
				 (
				 (this._objectName == null && other._objectName == null) ||
				 (this._objectName != null && other._objectName != null && this._objectName.Equals(other._objectName))) &&
				 this._actions == other._actions;
		}
		public override int GetHashCode()
		{
			int hashCode = _actions.GetHashCode();
			if (_className != null)
			{
				hashCode ^= _className.GetHashCode();
			}
			if (_memberName != null)
			{
				hashCode ^= _memberName.GetHashCode();
			}
			if (_objectName != null)
			{
				hashCode ^= _objectName.GetHashCode();
			}
			return hashCode;
		}
		#endregion

		#region IPermission Members
		public MBeanPermissionImpl Copy()
		{
			return new MBeanPermissionImpl(_className, _memberName, _objectName, _actions);
		}
		public MBeanPermissionImpl Intersect(MBeanPermissionImpl target)
		{
			VerifyAsNeeded();
			if (target == null)
			{
				return null;
			}
			if (this.IsSubsetOf(target))
			{
				return this.Copy();
			}
			else if (target.IsSubsetOf(this))
			{
				return target.Copy();
			}
			else
			{
				return null;
			}
		}
		public bool IsSubsetOf(MBeanPermissionImpl other)
		{
			VerifyAsNeeded();
			return
			(other._className == null || this._className == other._className) &&
			(other._memberName == null || this._memberName == other._memberName) &&
			((other._actions & this._actions) == this._actions) &&
			(other._objectName == null || (this._objectName != null && other._objectName.Apply(this._objectName)));
		}
		public MBeanPermissionImpl Union(MBeanPermissionImpl target)
		{
			VerifyAsNeeded();
			if (target == null)
			{
				return this.Copy();
			}
			if (this.IsSubsetOf(target))
			{
				return target.Copy();
			}
			else if (target.IsSubsetOf(this))
			{
				return this.Copy();
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region ISecurityEncodable Members
		public void FromXml(SecurityElement e)
		{
			_className = e.Attribute("className");
			_memberName = e.Attribute("memberName");
			string objectName = e.Attribute("objectName");
			_objectName = objectName != null ? new ObjectName(objectName) : null;
		}

		public SecurityElement ToXml()
		{
			SecurityElement el = new SecurityElement("IPermission");
			el.AddAttribute("class", typeof(MBeanPermission).AssemblyQualifiedName);
			el.AddAttribute("version", "1.0");
			if (_className != null)
			{
				el.AddAttribute("className", _className);
			}
			if (_memberName != null)
			{
				el.AddAttribute("memberName", _memberName);
			}
			if (_objectName != null)
			{
				el.AddAttribute("objectName", _objectName.ToString());
			}
			return el;
		}
		#endregion
	}
}
