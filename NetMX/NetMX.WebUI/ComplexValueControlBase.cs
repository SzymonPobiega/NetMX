#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetMX.OpenMBean;

#endregion

namespace NetMX.WebUI.WebControls
{
   public abstract class ComplexValueControlBase : CompositeControl
   {
      #region Members
      private bool _editMode;
      protected bool EditMode
      {
         get { return _editMode; }
      }
      private object _rootValue;
      protected object RootValue
      {
         get { return _rootValue; }
      }
      private OpenType _rootType;
      protected OpenType RootType
      {
         get { return _rootType; }
      }

      private string _title;
      private OpenTypeIndex _index;
      private OpenTypeIndex _nestedIndex;
      private bool _footerButtonsState = true;
      private MBeanUIContext _ctx;
      protected MBeanUIContext Context
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

      private TableRow _headerRow;
      private TableRow _contentRow;      
      private TableRow _footerRow;
      private TableRow _nestedControlRow;
      private TableCell _nestedControlCell;

      private Button _cancelButton;
      private Button _submitButton;
      #endregion

      #region Constructor & Factory
      public static ComplexValueControlBase Create(bool editMode, OpenType rootType, object rootValue, string title)
      {
         return DelegatingOpenTypeVisitor<ComplexValueControlBase>.VisitOpenType(rootType, null, null,
            delegate(TabularType type)
            {
               return new TabularValueControl(editMode, type, (ITabularData)rootValue, null, title);
            },
               delegate(CompositeType type)
               {
                  return null;
               });
      }
      public static ComplexValueControlBase Recreate(OpenTypeKind openTypeKind)
      {
         switch (openTypeKind)
         {
            case OpenTypeKind.TabularType:
               return new TabularValueControl();
            default:
               throw new NotSupportedException("Open type not supported");
         }
      }
      protected ComplexValueControlBase()
      {         
      }
      protected ComplexValueControlBase(bool editMode, OpenType rootType, object rootValue, OpenTypeIndex index, string title)
      {
         _editMode = editMode;
         _rootValue = rootValue;
         _rootType = rootType;
         _index = index;
         _title = title;
      }
      #endregion

      protected void AddNestedControl(OpenTypeIndex nestedTypeIndex)
      {
         _nestedIndex = nestedTypeIndex;
         CreateAndWireUpNestedControl(RootValue);         
      }  
      protected void SetButtonsState(bool enabled)
      {
         _footerButtonsState = enabled;
      }

      #region Abstract
      protected abstract Control ContentControl { get; }
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
         _nestedControlRow.Visible = _nestedIndex != null;
         _contentRow.Visible = _footerRow.Visible = _nestedIndex == null;
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
            titleLabel.CssClass = Context.SectionTitleCssClass;
            Controls.Add(titleLabel);
         }
         else
         {
            CssClass = Context.ControlCssClass;
         }

         Table t = new Table();
         t.ControlStyle.Width = Unit.Percentage(100);
         Controls.Add(t);
         _headerRow = new TableRow();         
         TableCell headerCell = new TableCell();
         if (_index != null)
         {
            headerCell.Text = _index.Visualize();
         }
         _headerRow.Cells.Add(headerCell);
         t.Controls.Add(_headerRow);

         _contentRow = new TableRow();
         TableCell contentCell = new TableCell();
         contentCell.Controls.Add(ContentControl);
         _contentRow.Cells.Add(contentCell);
         t.Controls.Add(_contentRow);

         _nestedControlRow = new TableRow();
         _nestedControlCell = new TableCell();
         _nestedControlRow.Cells.Add(_nestedControlCell);
         t.Controls.Add(_nestedControlRow);

         _footerRow = new TableRow();
         TableCell footerCell = new TableCell();

         _cancelButton = new Button();
         _cancelButton.Text = "Cancel";
         _cancelButton.Click += HandleCancel;
         _cancelButton.CssClass = Context.ButtonCssClass;
         _cancelButton.EnableViewState = false;
         footerCell.Controls.Add(_cancelButton);

         _submitButton = new Button();
         _submitButton.Text = "Submit";
         _submitButton.Click += HandleSubmit;
         _submitButton.CssClass = Context.ButtonCssClass;
         _submitButton.EnableViewState = false;
         footerCell.Controls.Add(_submitButton);

         _footerRow.Cells.Add(footerCell);
         t.Controls.Add(_footerRow);

         if (_nestedIndex != null)
         {            
            CreateAndWireUpNestedControl(null);
         }
      }
      #endregion

      #region Events
      public event EventHandler<ValueAndIndexEventArgs> Submit;
      public event EventHandler Cancel;
      #endregion

      #region Event handlers
      private void HandleSubmit(object source, EventArgs e)
      {
         if (Submit != null)
         {
            ValueAndIndexEventArgs args = new ValueAndIndexEventArgs(ExtractValue(), _index);
            Submit(this, args);
         }
      }

      private void HandleCancel(object source, EventArgs e)
      {
         if (Cancel != null)
         {
            Cancel(this, e);
         }
      }
      private void HandleNestedSubmit(object source, ValueAndIndexEventArgs e)
      {
         e.Index.UpdateValue(_rootType, _rootValue, e.Value);
         _nestedControl = null;
         _nestedIndex = null;
      }

      private void HandleNestedCancel(object source, EventArgs e)
      {
         _nestedControl = null;
         _nestedIndex = null;
      }
      #endregion

      #region Utility
      private void CreateAndWireUpNestedControl(object rootValue)
      {
         _nestedControl = _nestedIndex.CreateControl(_editMode, _rootType, rootValue);
         _nestedControl.Cancel += HandleNestedCancel;
         _nestedControl.Submit += HandleNestedSubmit;
         _nestedControlCell.Controls.Add(_nestedControl);
      }
      #endregion
   }

   public sealed class ValueAndIndexEventArgs : EventArgs
   {
      private readonly object _value;
      public object Value
      {
         get { return _value; }
      }
      private readonly OpenTypeIndex _index;
      public OpenTypeIndex Index
      {
         get { return _index; }
      }

      public ValueAndIndexEventArgs(object value, OpenTypeIndex index)
      {
         _value = value;
         _index = index;
      }
   }
}
