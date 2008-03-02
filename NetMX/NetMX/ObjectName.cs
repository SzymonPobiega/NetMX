#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Collections;
using System.Globalization;
using System.Runtime.Serialization;
#endregion

namespace NetMX
{
	/// <summary>
	/// Represents the object name of an MBean, or a pattern that can match the names of several MBeans. 
	/// Instances of this class are immutable.
	/// </summary>
	[Serializable]
	public sealed class ObjectName : ISerializable
	{
		#region REGEX
		private static readonly Regex _objectNamePattern =
			 new Regex("^(?<domain>.*?):(?<properties>((((?<key>[^,=:*?]+)=(?<value>[^,=:*?\"]+|\"[^\"]+\"))|(?<aster>\\*))(,(((?<key>[^,=:*?]+)=(?<value>[^,=:*?\"]+|\"[^\"]+\"))|(?<aster>\\*)))*)?)$", RegexOptions.Compiled);
		#endregion

		#region MEMBERS
		private bool _isDomainPattern;
		private bool _isPropertyPattern;
		private string _domain;
		private string _originalPropertyList;
		private string _canonicalPropertyList;
		private Dictionary<string, string> _properties;
		private IKeyProertyCollection _propertyCollection;
		#endregion

		#region PROPERTIES
		/// <summary>
		/// Returns the domain part. 
		/// </summary>
		public string Domain
		{
			get { return _domain; }
		}
		/// <summary>
		/// Returns the canonical form of the name; that is, a string representation where the properties 
		/// are sorted in lexical order.
		/// </summary>
		/// <remarks>
		/// More precisely, the canonical form of the name is a String consisting of the domain part, a colon (:), 
		/// the canonical key property list, and a pattern indication.
		/// The canonical key property list is the same string as described for <see cref="CanonicalKeyPropertyListString"/>.        
		/// The pattern indication can be: 
		/// <list type="bullet">
		/// <item>empty for an ObjectName that is not a property pattern</item>
		/// <item>an asterisk for an ObjectName that is a property pattern with no keys</item>
		/// <item>a comma and an asterisk (,*) for an ObjectName that is a property pattern with at least one key</item>
		/// </list>        
		/// </remarks>
		public string CanonicalName
		{
			get { return string.Format("{0}:{1}", _domain, _canonicalPropertyList); }
		}
		/// <summary>
		/// Returns a string representation of the list of key properties, in which the key properties are 
		/// sorted in lexical order. This is used in lexicographic comparisons performed in order to select 
		/// MBeans based on their key property list. Lexical order is the order implied by 
		/// <see cref="String.CompareTo(String)"/>. 
		/// </summary>
		public string CanonicalKeyPropertyListString
		{
			get { return _canonicalPropertyList; }
		}
		/// <summary>
		/// Returns a string representation of the list of key properties specified at creation time. 
		/// If this <see cref="ObjectName"/> was constructed with the constructor <see cref="ObjectName(String)"/>, the key properties 
		/// in the returned String will be in the same order as in the argument to the constructor.
		/// </summary>
		public string KeyProperyListString
		{
			get { return _originalPropertyList; }
		}
		/// <summary>
		/// Obtains the value associated with a key in a key property.
		/// </summary>
		public IKeyProertyCollection KeyProperty
		{
			get { return _propertyCollection; }
		}
		/// <summary>
		/// Checks whether the object name is a pattern on the key properties. 
		/// </summary>
		public bool IsPropertyPattern
		{
			get { return _isPropertyPattern; }
		}
		/// <summary>
		/// Checks whether the object name is a pattern on the domain part.
		/// </summary>
		public bool IsDomainPattern
		{
			get { return _isDomainPattern; }
		}
		/// <summary>
		/// Checks whether the object name is a pattern. An object name is a pattern if its domain contains 
		/// a wildcard or if the object name is a property pattern. 
		/// </summary>
		public bool IsPattern
		{
			get { return _isPropertyPattern || _isDomainPattern; }
		}
		#endregion

		#region CONSTRUCTOR
		/// <summary>
		/// Construct an object name from the given string.
		/// </summary>
		/// <param name="name">A string representation of the object name.</param>
		/// <exception cref="NetMX.MalformedObjectNameException">The string passed as a parameter does not have 
		/// the right format. </exception>
		public ObjectName(string name)
		{
			ParseName(name, out _domain, out _originalPropertyList, out _isPropertyPattern, out _properties);
			SetDomainPattern();
			_propertyCollection = new KeyPropertyCollection(_properties);
			CreateCanonicalPropertyList();
		}
		/// <summary>
		/// Construct an object name with several key properties from a Dictionary.
		/// </summary>
		/// <param name="domain">The domain part of the object name.</param>
		/// <param name="properties">A hash table containing one or more key properties. The key of each entry 
		/// in the table is the key of a key property in the object name. The associated value in the table is 
		/// the associated value in the object name.</param>
		/// <exception cref="NetMX.MalformedObjectNameException">The domain contains an illegal character, or one of 
		/// the keys or values in table contains an illegal character, or one of the values in table does not 
		/// follow the rules for quoting.</exception>
		public ObjectName(string domain, Dictionary<string, string> properties)
		{
			_domain = domain;
			SetDomainPattern();
			_properties = properties;
			if (_properties.ContainsKey("*"))
			{
				_properties.Remove("*");
				_isPropertyPattern = true;
			}
			_propertyCollection = new KeyPropertyCollection(_properties);
			CreateCanonicalPropertyList();
			_originalPropertyList = _canonicalPropertyList;
		}
		/// <summary>
		/// Construct an object from serialized information.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ObjectName(SerializationInfo info, StreamingContext context)
			: this(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", info.GetString("domain"), info.GetString("properties")))
		{
		}
		#endregion

		#region UTILITY
		private void SetDomainPattern()
		{
			if (_domain.Contains("*") || _domain.Contains("?"))
			{
				_isDomainPattern = true;
			}
		}
		private void CreateCanonicalPropertyList()
		{
			List<string> keys = new List<string>(_properties.Keys);
			keys.Sort();
			List<string> keyValuePairs = new List<string>();
			foreach (string key in keys)
			{
				string value = _properties[key];
				keyValuePairs.Add(string.Format(CultureInfo.InvariantCulture, "{0}={1}", key, value));
			}
			if (_isPropertyPattern)
			{
				keyValuePairs.Add("*");
			}
			_canonicalPropertyList = string.Join(",", keyValuePairs.ToArray());
		}
		private static void ParseName(string name, out string domain, out string properties, out bool isPropertyPattern, out Dictionary<string, string> propertyDictionary)
		{
			Match m = _objectNamePattern.Match(name);
			if (m.Success)
			{
				domain = m.Groups["domain"].Value;
				properties = m.Groups["properties"].Value;
				Group asters = m.Groups["aster"];
				if (asters.Captures.Count > 1)
				{
					throw new MalformedObjectNameException(name);
				}
				isPropertyPattern = asters.Captures.Count == 1;
				propertyDictionary = new Dictionary<string, string>();
				CaptureCollection keys = m.Groups["key"].Captures;
				CaptureCollection values = m.Groups["value"].Captures;
				for (int i = 0; i < keys.Count; i++)
				{
					propertyDictionary[keys[i].Value] = values[i].Value;
				}
			}
			else
			{
				throw new MalformedObjectNameException(name);
			}
		}
		private static bool MatchDomain(string pattern, bool isDomainPattern, string target)
		{
			if (isDomainPattern)
			{
				Regex patternRegex = new Regex(pattern.Replace("?", ".").Replace("*", ".*"));
				return patternRegex.IsMatch(target);
			}
			else
			{
				return pattern == target;
			}
		}
		private static bool MatchProperties(Dictionary<string, string> pattern, bool isPropertyPattern, Dictionary<string, string> target)
		{
			foreach (string key in pattern.Keys)
			{
				string targetValue;
				if (target.TryGetValue(key, out targetValue))
				{
					if (targetValue == pattern[key])
					{
						continue;
					}
				}
				return false;
			}
			return isPropertyPattern || pattern.Count == target.Count;
		}
		#endregion

		#region OVERRIDDEN
		/// <summary>
		/// Returns a string representation of the object name. The format of this string is not specified, 
		/// but users can expect that two ObjectNames return the same string if and only if they are equal.
		/// </summary>
		/// <returns>a string representation of this object name.</returns>
		public override string ToString()
		{
			return string.Format("{0}:{1}", _domain, _originalPropertyList);
		}
		/// <summary>
		/// Compares the current object name with another object name. Two ObjectName instances are equal 
		/// if and only if their canonical forms are equal. The canonical form is the string described for 
		/// <see cref="CanonicalName"/>. 
		/// </summary>
		/// <param name="obj">The object name that the current object name is to be compared with.</param>
		/// <returns>True if <paramref name="object"/> is an ObjectName whose canonical form is equal to that 
		/// of this ObjectName.</returns>
		public override bool Equals(object obj)
		{
			ObjectName other = obj as ObjectName;
			if (other != null)
			{
				return this.CanonicalName.Equals(other.CanonicalName);
			}
			return false;
		}
		/// <summary>
		/// Returns a hash code for this object name.
		/// </summary>
		/// <returns>a hash code value for this object.</returns>
		public override int GetHashCode()
		{
			return this.CanonicalName.GetHashCode();
		}
		#endregion

		#region INTERFACE
		/// <summary>
		/// Test whether this ObjectName, which may be a pattern, matches another ObjectName. If name is a 
		/// pattern, the result is false. If this ObjectName is a pattern, the result is true if and only if 
		/// name matches the pattern. If neither this ObjectName nor name is a pattern, the result is true if 
		/// and only if the two ObjectNames are equal as described for the <see cref="Equals(Object)"/> method.
		/// </summary>
		/// <param name="name">The name of the MBean to compare to.</param>
		/// <returns>True if <paramref name="name"/> matches this ObjectName.</returns>
		public bool Apply(ObjectName name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.IsPattern)
			{
				return false;
			}
			else if (!this.IsPattern)
			{
				return this.Equals(name);
			}
			else
			{
				return MatchDomain(_domain, _isDomainPattern, name._domain) && MatchProperties(_properties, _isPropertyPattern, name._properties);
			}
		}
		#endregion

		#region UTILITY CLASS
		/// <summary>
		/// Provides access to key properties.
		/// </summary>
		public interface IKeyProertyCollection
		{
			/// <summary>
			/// Obtains the value associated with a key in a key property.
			/// </summary>
			/// <param name="key">key</param>
			/// <returns></returns>
			string this[string key] { get; }
		}
		private class KeyPropertyCollection : IKeyProertyCollection
		{
			private Dictionary<string, string> _internalDictionary;
			public KeyPropertyCollection(Dictionary<string, string> internalDictionary)
			{
				_internalDictionary = internalDictionary;
			}

			#region IKeyProertyCollection Members
			public string this[string key]
			{
				get
				{
					string value;
					if (_internalDictionary.TryGetValue(key, out value))
					{
						return value;
					}
					return null;
				}
			}
			#endregion
		}
		#endregion

		#region ISerializable Members
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("domain", _domain);
			info.AddValue("properties", _canonicalPropertyList);
		}
		#endregion
	}
}
