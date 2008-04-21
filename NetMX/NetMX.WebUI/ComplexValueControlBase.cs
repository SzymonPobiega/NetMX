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
      private OpenTypeIndex _index;
      #endregion

      #region Controls
      private ComplexValueControlBase _nestedControl;

      private TableRow _headerRow;
      private TableRow _contentRow;
      private TableRow _footerRow;
      #endregion

      #region Constructor & Factory
      public static ComplexValueControlBase Create(bool editMode, OpenType rootType, object rootValue)
      {
         return DelegatingOpenTypeVisitor<ComplexValueControlBase>.VisitOpenType(rootType, null, null,
            delegate(TabularType type)
            {
               return new TabularValueControl(editMode, type, (ITabularData)rootValue, null);
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
            case OpenTypeKind.TabularType :
               return new TabularValueControl();
            default :
               throw  new NotSupportedException("Open type not supported");
         }
      }
      protected ComplexValueControlBase()
      {
      }      
      protected ComplexValueControlBase(bool editMode, OpenType rootType, object rootValue, OpenTypeIndex index)         
      {
         _editMode = editMode;
         _rootValue = rootValue;
         _rootType = rootType;
         _index = index;
      }
      #endregion

      public void AddNestedControl(OpenTypeIndex nestedTypeIndex)
      {         
         _nestedControl = nestedTypeIndex.CreateControl(_editMode, _rootType, _rootValue);
         _nestedControl.Cancel += HandleNestedCancel;
         _nestedControl.Submit += HandleNestedSubmit;
      }

      #region Abstract
      protected abstract Control ContentControl { get; }
      protected abstract object ExtractValue();
      #endregion

      #region Overridden
      protected override void OnInit(EventArgs e)
      {         
         base.OnInit(e);
         Page.RegisterRequiresControlState(this);
      }
      protected override void LoadControlState(object savedState)
      {
         object[] state = (object[]) savedState;
         base.LoadControlState(state[0]);
         _editMode = (bool) state[1];
         _rootType = (OpenType) state[2];
         _rootValue = state[3];
         _index = (OpenTypeIndex) state[4];
         if (_index != null)
         {
            _nestedControl = _index.CreateControl(_editMode, _rootType, _rootValue);
         }
      }
      protected override object SaveControlState()
      {
         return new object[] {base.SaveControlState(), _editMode, _rootType, _rootValue, _index};         
      }
      protected override void OnLoad(EventArgs e)
      {
         base.OnLoad(e);      
         _headerRow = new TableRow();
         TableCell headerCell = new TableCell();
         _headerRow.Cells.Add(headerCell);
         Controls.Add(_headerRow);

         _contentRow = new TableRow();
         TableCell contentCell = new TableCell();
         contentCell.Controls.Add(ContentControl);
         _contentRow.Cells.Add(contentCell);
         Controls.Add(_contentRow);

         _footerRow = new TableRow();
         TableCell footerCell = new TableCell();

         Button cancelButton = new Button();
         cancelButton.Text = "Cancel";
         cancelButton.Click += HandleCancel;
         footerCell.Controls.Add(cancelButton);

         Button submitButton = new Button();
         submitButton.Text = "Submit";
         submitButton.Click += HandleSubmit;
         footerCell.Controls.Add(submitButton);

         _footerRow.Cells.Add(footerCell);
         Controls.Add(_footerRow);
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
      }

      private void HandleNestedCancel(object source, EventArgs e)
      {
         _nestedControl = null;
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
