#region USING
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetMX.OpenMBean;

#endregion

namespace NetMX.WebUI.WebControls
{
   /// <summary>
   /// Base class for complex value controls like <see cref="TabularValueControl"/> or 
   /// <see cref="CompositeValueControl"/>. Provides facilities to stack those controls to make possible
   /// editing nested complex open types.
   /// </summary>
   internal abstract class ComplexValueControlBase : CompositeControl
   {
      #region Members
      private string _title;
      private OpenTypeIndex _index;
      private OpenTypeIndex _nestedIndex;
      private bool _footerButtonsState = true;
      private MBeanUIContext _ctx;

      private bool _editMode;
      /// <summary>
      /// Tests if this control in edut mode.
      /// </summary>
      protected bool EditMode
      {
         get { return _editMode; }
      }
      private object _rootValue;
      /// <summary>
      /// Gets the value which is edited by this control.
      /// </summary>
      protected object RootValue
      {
         get { return _rootValue; }
         set { _rootValue = value; }
      }
      private OpenType _rootType;
      /// <summary>
      /// Gets the <see cref="OpenType"/> of value edited by this control.
      /// </summary>
      protected OpenType RootType
      {
         get { return _rootType; }         
      }      
      /// <summary>
      /// Gets the <see cref="MBeanUIContext"/> instance by searching <see cref="MBeanUI"/> control up
      /// the control hierarchy.
      /// </summary>
      protected MBeanUIContext UIContext
      {
       get
       {
          if (_ctx == null)
          {
             _ctx = MBeanUIContext.GetInstance(this);
          }
          return _ctx;
       }
      }
      #endregion

      #region Controls
      private ComplexValueControlBase _nestedControl;

      //private TableRow _headerRow;
      //private TableRow _contentRow;      
      //private TableRow _footerRow;
      private Control _contentControl;
      private Panel _footerPanel;
      private Panel _nestedControlPanel;

      //private TableRow _nestedControlRow;
      //private TableCell _nestedControlCell;

      private Button _cancelButton;
      private Button _submitButton;
      #endregion

      #region Constructor & Factory
      /// <summary>
      /// A factory method for creating <see cref="ComplexValueControlBase"/> derived controls based on <see cref="OpenType"/>
      /// of the value being edited. It is also used only to create bottom control of the stack. Non-bottom 
      /// controls are created using <see cref="OpenTypeIndex.CreateControl"/>.
      /// </summary>
      /// <param name="editMode">Will the control allow to edit the value, or only to view it.</param>
      /// <param name="rootType"><see cref="OpenType"/> of value.</param>
      /// <param name="rootValue">Value to edit or view.</param>
      /// <param name="title">Title of the control.</param>
      /// <returns></returns>
      public static ComplexValueControlBase Create(bool editMode, OpenType rootType, object rootValue, string title)
      {
         return DelegatingOpenTypeVisitor<ComplexValueControlBase>.VisitOpenType(rootType, null, null,
            delegate(TabularType type)
            {
               return new TabularValueControl(editMode, type, (ITabularData)rootValue, null, title);
            },
            delegate(CompositeType type)
            {
               return new CompositeValueControl(editMode, type, (ICompositeData) rootValue, null, title);
            });
      }
      /// <summary>
      /// A method compelementary to <see cref="Create"/>. Recreates the control based only on ist <see cref="openTypeKind"/>.
      /// Used by parent control to recreate <see cref="ComplexValueControlBase"/> ofter postback. All the data provideed
      /// as arguments to <see cref="Create"/> is in this cased read from ControlState. 
      /// </summary>
      /// <param name="openTypeKind">A kind of open type the controls edits or views.</param>
      /// <returns></returns>
      public static ComplexValueControlBase Recreate(OpenTypeKind openTypeKind)
      {
         switch (openTypeKind)
         {
            case OpenTypeKind.TabularType:
               return new TabularValueControl();
            default:
               return new CompositeValueControl();
         }
      }
      /// <summary>
      /// Creates new <see cref="ComplexValueControlBase"/> instance which reads its data from ControlState. 
      /// Used by <see cref="Recreate"/> method.
      /// </summary>
      protected ComplexValueControlBase()
      {         
      }
      /// <summary>
      /// Creates new <see cref="ComplexValueControlBase"/> instance providing all the necessary date in 
      /// constructor arguments.
      /// </summary>
      /// <param name="editMode">Will the control allow to edit the value, or only to view it.</param>
      /// <param name="rootType"><see cref="OpenType"/> of value.</param>
      /// <param name="rootValue">Value to edit or view.</param>
      /// <param name="index">An index within the root value.</param>
      /// <param name="title">Title of the control (provided only if control is in the bottom of the stack).</param>
      protected ComplexValueControlBase(bool editMode, OpenType rootType, object rootValue, OpenTypeIndex index, string title)
      {
         _editMode = editMode;
         _rootValue = rootValue;
         _rootType = rootType;
         _index = index;
         _title = title;
      }
      #endregion

      #region Subclass contract
      /// <summary>
      /// Adds nested control to this one (or in other words - adds another control to the stack) to edit some
      /// nested value (like row of the table or a composite value).
      /// </summary>
      /// <param name="nestedTypeIndex">Index (within current <see cref="RootValue"/> pointing to the nested 
      /// value to edit/view.</param>
      protected void AddNestedControl(OpenTypeIndex nestedTypeIndex)
      {
         _nestedIndex = nestedTypeIndex;
         CreateAndWireUpNestedControl(RootValue);         
      }  
      /// <summary>
      /// Sets the state of footer byttons (controling navigation) to enabled (true) / disabled (false).
      /// </summary>
      /// <param name="enabled"></param>
      protected void SetButtonsState(bool enabled)
      {
         _footerButtonsState = enabled;
      }
      /// <summary>
      /// Creates the content control instance - the control which is responsible for task of editing and presenting
      /// the valuel.
      /// </summary>
      protected abstract Control CreateContentControl();
      /// <summary>
      /// Returns the value to save the changes in the parent value under current index.
      /// </summary>
      /// <returns></returns>
      protected abstract object ExtractValue();
      #endregion

      #region Overridden
      protected override HtmlTextWriterTag TagKey
      {
         get
         {
            return HtmlTextWriterTag.Div;
         }
      }
      protected override void OnPreRender(EventArgs e)
      {
         base.OnPreRender(e);
         //_nestedControlRow.Visible = _nestedIndex != null;
         _nestedControlPanel.Visible = _nestedIndex != null;

         //_contentRow.Visible = _footerRow.Visible = _nestedIndex == null;
         _footerPanel.Visible = _nestedIndex == null;
         if (_contentControl != null)
         {
            _contentControl.Visible = _nestedIndex == null;
         }         

         _submitButton.Enabled = _cancelButton.Enabled = _footerButtonsState;         
      }
      protected override void OnInit(EventArgs e)
      {
         base.OnInit(e);
         Page.RegisterRequiresControlState(this);
      }
      protected override void LoadControlState(object savedState)
      {
         object[] state = (object[])savedState;
         base.LoadControlState(state[0]);
         if (_rootValue == null)
         {
            _editMode = (bool)state[1];
            _footerButtonsState = (bool)state[2];
            _rootType = (OpenType)state[3];
            _rootValue = state[4];
            _index = (OpenTypeIndex)state[5];
            _nestedIndex = (OpenTypeIndex)state[6];
            _title = (string) state[7];
         }
      }
      protected override object SaveControlState()
      {
         return new object[] { base.SaveControlState(), _editMode, _footerButtonsState, _rootType, _rootValue, _index, _nestedIndex, _title };
      }
      protected override void OnLoad(EventArgs e)
      {
         base.OnLoad(e);

         if (_title != null)
         {
            Label titleLabel = new Label();
            titleLabel.Text = _title;
            titleLabel.CssClass = UIContext.SectionTitleCssClass;
            Controls.Add(titleLabel);
         }
         else
         {
            CssClass = UIContext.ControlCssClass;
         }

         //Table t = new Table();
         //t.ControlStyle.Width = Unit.Percentage(100);
         //Controls.Add(t);
         //_headerRow = new TableRow();         
         //TableCell headerCell = new TableCell();
         //if (_index != null)
         //{
         //   headerCell.Text = _index.Visualize();
         //}
         //_headerRow.Cells.Add(headerCell);
         //t.Controls.Add(_headerRow);

         if (_index != null)
         {
            Label headerLabel = new Label();
            headerLabel.Text = _index.Visualize();
            Controls.Add(headerLabel);
         }         

         //_contentRow = new TableRow();
         //TableCell contentCell = new TableCell();
         //contentCell.Controls.Add(ContentControl);
         //_contentRow.Cells.Add(contentCell);
         //t.Controls.Add(_contentRow);

         //_nestedControlRow = new TableRow();
         //_nestedControlCell = new TableCell();
         //_nestedControlRow.Cells.Add(_nestedControlCell);
         _nestedControlPanel = new Panel();
         Controls.Add(_nestedControlPanel);
         //t.Controls.Add(_nestedControlRow);
         _contentControl = CreateContentControl();
         Controls.Add(_contentControl);

         //_footerRow = new TableRow();
         //TableCell footerCell = new TableCell();
         
         _footerPanel = new Panel();

         _cancelButton = new Button();
         _cancelButton.Text = "Cancel";
         _cancelButton.Click += OnCancel;
         _cancelButton.CssClass = UIContext.ButtonCssClass;
         _cancelButton.EnableViewState = false;
         //footerCell.Controls.Add(_cancelButton);
         _footerPanel.Controls.Add(_cancelButton);

         _submitButton = new Button();
         _submitButton.Text = "Submit";
         _submitButton.Click += OnSubmit;
         _submitButton.CssClass = UIContext.ButtonCssClass;
         _submitButton.EnableViewState = false;
         //footerCell.Controls.Add(_submitButton);
         _footerPanel.Controls.Add(_submitButton);

         //_footerRow.Cells.Add(footerCell);
         //t.Controls.Add(_footerRow);
         Controls.Add(_footerPanel);

         if (_nestedIndex != null)
         {            
            CreateAndWireUpNestedControl(null);
         }
      }
      #endregion

      #region Events
      /// <summary>
      /// Rised when used clicks Submit button in the footer buttons area to save the cahnges done in the value.
      /// </summary>
      public event EventHandler<ValueAndIndexEventArgs> Submit;
      /// <summary>
      /// Rised when user clicks Cancel button in the footer buttons area to canel the changes.
      /// </summary>
      public event EventHandler Cancel;
      #endregion

      #region Event handlers
      protected virtual void OnSubmit(object source, EventArgs e)
      {
         if (Submit != null)
         {
            ValueAndIndexEventArgs args = new ValueAndIndexEventArgs(ExtractValue(), _index);
            Submit(this, args);
         }
      }

      protected virtual void OnCancel(object source, EventArgs e)
      {
         if (Cancel != null)
         {
            Cancel(this, e);
         }
      }
      protected virtual void OnNestedSubmit(object source, ValueAndIndexEventArgs e)
      {
         e.Index.UpdateValue(_rootType, ref _rootValue, e.Value);
         _nestedControl = null;
         _nestedIndex = null;
      }

      protected virtual void OnNestedCancel(object source, EventArgs e)
      {
         _nestedControl = null;
         _nestedIndex = null;
      }
      #endregion

      #region Utility
      private void CreateAndWireUpNestedControl(object rootValue)
      {
         _nestedControl = _nestedIndex.CreateControl(_editMode, _rootType, rootValue);
         _nestedControl.Cancel += OnNestedCancel;
         _nestedControl.Submit += OnNestedSubmit;
         //_nestedControlCell.Controls.Add(_nestedControl);
         _nestedControlPanel.Controls.Add(_nestedControl);
      }
      #endregion

      #region Field class base
      protected class ComplexValueBoundField : BoundField
      {
         private readonly OpenType _openTypeInfo;

         public OpenType OpenTypeInfo
         {
            get { return _openTypeInfo; }
         }

         public ComplexValueBoundField(OpenType openTypeInfo)
         {
            _openTypeInfo = openTypeInfo;
         }

         protected override void OnDataBindField(object sender, EventArgs e)
         {
            Control control = (Control)sender;
            Control namingContainer = control.NamingContainer;
            object dataValue = GetValue(namingContainer);
            bool encode = (SupportsHtmlEncode && HtmlEncode) && (control is TableCell);
            string str = FormatDataValue(dataValue, encode);
            if (control is TableCell)
            {
               if (str.Length == 0)
               {
                  str = "&nbsp;";
               }
               ((TableCell)control).Text = str;
            }
            else
            {
               IValueEditControl valueControl = (IValueEditControl)control;             
               if (ApplyFormatInEditMode)
               {
                  valueControl.Value = str;
               }
               else if (dataValue != null)
               {
                  valueControl.Value = dataValue.ToString();
               }               
            }
         }


         
         protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
         {
            Control child = null;
            Control control2 = null;
            if ((((rowState & DataControlRowState.Edit) != DataControlRowState.Normal) && !ReadOnly) || ((rowState & DataControlRowState.Insert) != DataControlRowState.Normal))
            {
               Control box = (Control) ValueEditControlFactory.CreateValueEditControl(_openTypeInfo);
               //box.ToolTip = HeaderText;
               child = box;
               if ((DataField.Length != 0) && ((rowState & DataControlRowState.Edit) != DataControlRowState.Normal))
               {
                  control2 = box;
               }
            }
            else if (DataField.Length != 0)
            {
               control2 = cell;
            }
            if (child != null)
            {
               cell.Controls.Add(child);
            }
            if (control2 != null && Visible)
            {
               control2.DataBinding += OnDataBindField;
            }
         }
         protected override object GetValue(Control controlContainer)
         {
            ICompositeData compositeData = (ICompositeData)DataBinder.GetDataItem(controlContainer);
            return compositeData[DataField];
         }
         public override void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
         {
            string dataField = DataField;
            string text = null;
            if (((rowState & DataControlRowState.Insert) == DataControlRowState.Normal) || InsertVisible)
            {
               if (cell.Controls.Count > 0)
               {
                  Control control = cell.Controls[0];
                  IValueEditControl box = control as IValueEditControl;
                  if (box != null)
                  {
                     text = box.Value;
                  }
               }
               else
               {
                  string cellText = cell.Text;
                  if (cellText == "&nbsp;")
                  {
                     text = string.Empty;
                  }
                  else if (SupportsHtmlEncode && HtmlEncode)
                  {
                     text = HttpUtility.HtmlDecode(cellText);
                  }
                  else
                  {
                     text = cellText;
                  }
               }
               if (text != null)
               {
                  if (text.Length == 0 && ConvertEmptyStringToNull)
                  {
                     text = null;
                  }
                  if (text == NullDisplayText && NullDisplayText.Length > 0)
                  {
                     text = null;
                  }
                  if (dictionary.Contains(dataField))
                  {
                     dictionary[dataField] = text;
                  }
                  else
                  {
                     dictionary.Add(dataField, text);
                  }
               }
            }
         }
      }
      #endregion
   }

   /// <summary>
   /// Encapsulates arguments of <see cref="ComplexValueControlBase.Submit"/> event: the modified value and its
   /// index within parent value.
   /// </summary>
   internal sealed class ValueAndIndexEventArgs : EventArgs
   {
      private readonly object _value;
      /// <summary>
      /// The modified value.
      /// </summary>
      public object Value
      {
         get { return _value; }
      }
      private readonly OpenTypeIndex _index;
      /// <summary>
      /// Index within parent value in which current value should be placed.
      /// </summary>
      public OpenTypeIndex Index
      {
         get { return _index; }
      }

      /// <summary>
      /// Creates new <see cref="ValueAndIndexEventArgs"/> instance.
      /// </summary>
      /// <param name="value">The modified value.</param>
      /// <param name="index">Index within parent value in which current value should be placed.</param>
      internal ValueAndIndexEventArgs(object value, OpenTypeIndex index)
      {
         _value = value;
         _index = index;
      }
   }
}
