using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetMX.Remote.Jsr262
{
   public sealed class GetAttributesFragment
   {
      private static readonly Regex _validatorExpr =
         new Regex("(//:Property\\[@name=\"([^\"]+)\"\\]|)*//:Property\\[@name=\"([^\"]+)\"\\]", RegexOptions.Compiled);

      private static readonly Regex _parserExpr =
         new Regex("//:Property\\[@name=\"(?<name>[^\"]+)\"\\]", RegexOptions.Compiled);

      private const string _expressionPattern = "//:Property[@name=\"{0}\"]|";         

      private readonly string[] _names;

      /// <summary>
      /// Names of attributes to get.
      /// </summary>
      public string[] Names
      {
         get { return _names; }
      }

      public GetAttributesFragment(IEnumerable<string> names)
      {
         _names = names.ToArray();
      }


      public string GetExpression()
      {
         StringBuilder expression = new StringBuilder();
         foreach (string name in _names)
         {
            expression.AppendFormat(_expressionPattern, name);
         }
         expression.Remove(expression.Length - 1, 1);
         return expression.ToString();
      }

      public static GetAttributesFragment Parse(string fragmentTransferExpression)
      {
         if (!_validatorExpr.Match(fragmentTransferExpression).Success)
         {
            throw new Exception();
         }
         List<string> names = new List<string>();
         Match m = _parserExpr.Match(fragmentTransferExpression);         
         while (m.Success)
         {
            string name = m.Groups["name"].Value;
            names.Add(name);
            m = m.NextMatch();
         }
         return new GetAttributesFragment(names);
      }
   }
}
