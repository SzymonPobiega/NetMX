//using System;
//using System.Linq;
//using System.Collections.Generic;

//namespace NetMX
//{
//   public static class ObjectMath
//   {
//      public static object Add(object left, object right)
//      {
         
//      }

//      public static object Sub(object left, object right)
//      {
//         throw new NotImplementedException();
//      }

//      public static object Mul(object left, object right)
//      {
//         throw new NotImplementedException();
//      }

//      public static object Div(object left, object right)
//      {
//         throw new NotImplementedException();
//      }

//      public static bool Eq(object left, object right)
//      {
//         throw new NotImplementedException();
//      }

//      public static bool Gt(object left, object right)
//      {
//         throw new NotImplementedException();
//      }

//      public static bool GtE(object left, object right)
//      {
//         throw new NotImplementedException();
//      }

//      public static bool Lt(object left, object right)
//      {
//         throw new NotImplementedException();
//      }

//      public static bool LtE(object left, object right)
//      {
         
//         throw new NotImplementedException();
//      }
//   }

//   public interface IMath
//   {
//      object Add(object left, object right);
//      object Sub(object left, object right);
//      object Mul(object left, object right);
//      object Div(object left, object right);

//      bool Eq(object left, object right);
//      bool Gt(object left, object right);
//      bool GtE(object left, object right);
//      bool Lt(object left, object right);
//      bool LtE(object left, object right);      
//   }

//   public abstract class MathImpl<T> : IMath
//   {
//      public object Add(object left, object right)
//      {
//         return DoAdd((T) left, (T) right);
//      }
//      protected abstract T DoAdd(T left, T right);

//      public object Sub(object left, object right)
//      {
//         return DoSub((T)left, (T)right);
//      }
//      protected abstract T DoSub(T left, T right);

//      public object Mul(object left, object right)
//      {
//         return DoMul((T)left, (T)right);
//      }
//      protected abstract T DoMul(T left, T right);

//      public object Div(object left, object right)
//      {
//         return DoDiv((T)left, (T)right);
//      }
//      protected abstract T DoDiv(T left, T right);

//      public bool Eq(object left, object right)
//      {
//         return DoEq((T)left, (T)right);
//      }
//      protected abstract bool DoEq(T left, T right);

//      public bool Gt(object left, object right)
//      {
//         return DoGt((T)left, (T)right);
//      }
//      protected abstract bool DoGt(T left, T right);

//      public bool GtE(object left, object right)
//      {
//         return DoGtE((T)left, (T)right);
//      }
//      protected abstract bool DoGtE(T left, T right);

//      public bool Lt(object left, object right)
//      {
//         return DoLt((T)left, (T)right);
//      }
//      protected abstract bool DoLt(T left, T right);

//      public bool LtE(object left, object right)
//      {
//         return DoLtE((T)left, (T)right);
//      }
//      protected abstract bool DoLtE(T left, T right);
//   }   


//   public class IntMath : MathImpl<Int64>
//   {
//      protected override int DoAdd(int left, int right)
//      {
//         return left + right;
//      }

//      protected override int DoSub(int left, int right)
//      {
//         return left - right;
//      }

//      protected override int DoMul(int left, int right)
//      {
//         return left * right;
//      }

//      protected override int DoDiv(int left, int right)
//      {
//         return left/right;
//      }

//      protected override bool DoEq(int left, int right)
//      {
//         return left == right;
//      }

//      protected override bool DoGt(int left, int right)
//      {
//         return left > right;
//      }

//      protected override bool DoGtE(int left, int right)
//      {
//         return left >= right;
//      }

//      protected override bool DoLt(int left, int right)
//      {
//         return left < right;
//      }

//      protected override bool DoLtE(int left, int right)
//      {
//         return left <= right;
//      }
//   }
//}