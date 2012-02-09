#region USING
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

namespace NetMX.WebUI.WebControls
{
   public class TextValueEditControl : CompositeControl, INamingContainer, IValueEditControl
   {
      #region Controls
      private TextBox _input;
      private BaseCompareValidator _validator;
      #endregion

      #region PROPERTIES
      #endregion

      #region Constructor
      public TextValueEditControl()
      {
         _input = new TextBox();
         _input.ID = "input";         
      }
      public TextValueEditControl(string defualtValue)
         : this()
      {
         _input.Text = defualtValue;
      }
      public TextValueEditControl(ValidationDataType dataType, string name, string defaultValue, string minValue, string maxValue)
         : this()
      {
         _input.Text = defaultValue;
         if (string.IsNullOrEmpty(minValue) && string.IsNullOrEmpty(maxValue))
         {
            CompareValidator validator = new CompareValidator();
            validator.Operator = ValidationCompareOperator.DataTypeCheck;
            _validator = validator;
         }
         else
         {
            RangeValidator validator = new RangeValidator();            
            validator.MinimumValue = minValue;
            validator.MaximumValue = maxValue;
            _validator = validator;
         }
         _validator.ControlToValidate = "input";
         _validator.Text = "*";
         _validator.Type = dataType;
         _validator.ErrorMessage =
               string.Format(CultureInfo.CurrentCulture, "Invalid value for attribute/property {0}.", name);
      }
      #endregion

      protected override void CreateChildControls()
      {
         base.CreateChildControls();
         Controls.Add(_input);
         if (_validator != null)
         {
            Controls.Add(_validator);
         }
      }

      #region IValueEditControl Members
      string IValueEditControl.Value
      {
         get
         {
            return _input.Text;
         }
         set
         {
            _input.Text = value;
         }
      }
      string IValueEditControl.CssClass
      {
         get
         {
            return _input.CssClass;
         }
         set
         {
            _input.CssClass = value;
         }
      }
      bool IValueEditControl.EnableViewState
      {
         get
         {
            return this.EnableViewState;
         }
         set
         {
            this.EnableViewState = value;
         }
      }
      bool IValueEditControl.Visible
      {
         get
         {
            return this.Visible;
         }
         set
         {
            this.Visible = value;
         }
      }
      string IValueEditControl.ID
      {
         get
         {
            return this.ID;
         }
         set
         {
            this.ID = value;
         }
      }
      #endregion
   }
}
