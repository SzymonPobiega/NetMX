using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using NetMX.Server;

namespace NetMX.Relation.Tests
{
   /// <summary>
   /// Summary description for RelationServiceTests
   /// </summary>
   [TestFixture]
   public class RelationServiceTests
   {
      private IMBeanServer _server;
      private RelationServiceMBean _relationService;
      private Parent _parent;
      private Child _child1;
      private Child _child2;

      [SetUp]
      public void Initialize()
      {
         _server = new MBeanServer();
         _relationService = new RelationService();
         _server.RegisterMBean(_relationService, ":type=RelationService");

         _parent = new Parent();
         _server.RegisterMBean(_parent, ":type=Parent");
         _child1 = new Child();
         _server.RegisterMBean(_child1, ":type=Child,id=1");
         _child2 = new Child();
         _server.RegisterMBean(_child2, ":type=Child,id=2");

         _relationService.CreateRelationType("Parenthood",
            new RoleInfo[] { 
               new RoleInfo("Parent", typeof(ParentMBean), true, true, 1, 1, "Parent"),
               new RoleInfo("Child", typeof(ChildMBean), true, true, 0, RoleInfo.RoleCardinalityInfinity, "Child") });
      }

      [TearDown]
      public void Cleanup()
      {
         _server.UnregisterMBean(":type=RelationService");
      }

      #region AddRelation tests
      [Test]
      public void TestAddRelationSuccess()
      {
         RelationSupport relation = new RelationSupport("REL1", ":type=RelationService", "Parenthood",
               new Role("Parent", new ObjectName(":type=Parent")),
               new Role("Child", new ObjectName(":type=Child,id=1"), new ObjectName(":type=Child,id=2")));
         _server.RegisterMBean(relation, ":type=Parenthood,id=1");
         _relationService.AddRelation(":type=Parenthood,id=1");
         Assert.IsTrue(_relationService.HasRelation("REL1"));
         Assert.AreEqual("REL1", _relationService.IsRelation(":type=Parenthood,id=1"));
         Assert.AreEqual(new ObjectName(":type=Parenthood,id=1"), _relationService.IsRelationMBean("REL1"));
      }
      [Test]
      [ExpectedException(typeof(ArgumentNullException))]
      public void TestAddRelationFailureNull()
      {
         _relationService.AddRelation(null);
      }
      [Test]
      [ExpectedException(typeof(InvalidRelationIdException))]
      public void TestAddRelationFailuereInvalidId()
      {
         RelationSupport relation = new RelationSupport("REL1", ":type=RelationService", "Parenthood",
               new Role("Parent", new ObjectName(":type=Parent")),
               new Role("Child", new ObjectName(":type=Child,id=1"), new ObjectName(":type=Child,id=2")));
         _server.RegisterMBean(relation, ":type=Parenthood,id=1");
         _relationService.AddRelation(":type=Parenthood,id=1");
         RelationSupport relation2 = new RelationSupport("REL1", ":type=RelationService", "Parenthood2",
               new Role("Parent", new ObjectName(":type=Parent")),
               new Role("Child", new ObjectName(":type=Child,id=1"), new ObjectName(":type=Child,id=2")));
         _server.RegisterMBean(relation, ":type=Parenthood,id=2");
         _relationService.AddRelation(":type=Parenthood,id=2");
      }
      [Test]
      [ExpectedException(typeof(InvalidRelationServiceException))]
      public void TestAddRelationFailureInvalidRelationService()
      {
         RelationSupport relation = new RelationSupport("REL1", ":type=RelationService2", "Parenthood",
               new Role("Parent", new ObjectName(":type=Parent")),
               new Role("Child", new ObjectName(":type=Child,id=1"), new ObjectName(":type=Child,id=2")));
         _server.RegisterMBean(relation, ":type=Parenthood,id=1");
         _relationService.AddRelation(":type=Parenthood,id=1");
      }
      [Test]
      [ExpectedException(typeof(RelationTypeNotFoundException))]
      public void TestAddRelationFailureTypeNotFound()
      {
         RelationSupport relation = new RelationSupport("REL1", ":type=RelationService", "Childhood",
               new Role("Parent", new ObjectName(":type=Parent")),
               new Role("Child", new ObjectName(":type=Child,id=1"), new ObjectName(":type=Child,id=2")));
         _server.RegisterMBean(relation, ":type=Parenthood,id=1");
         _relationService.AddRelation(":type=Parenthood,id=1");
      }
      [Test]
      [ExpectedException(typeof(InvalidRoleValueException))]
      public void TestAddRelationFailureMinDegree()
      {
         RelationSupport relation = new RelationSupport("REL1", ":type=RelationService", "Parenthood",
               new Role("Parent"),
               new Role("Child", new ObjectName(":type=Child,id=1"), new ObjectName(":type=Child,id=2")));
         _server.RegisterMBean(relation, ":type=Parenthood,id=1");
         _relationService.AddRelation(":type=Parenthood,id=1");
      }
      [Test]
      [ExpectedException(typeof(InvalidRoleValueException))]
      public void TestAddRelationFailureMaxDegree()
      {
         RelationSupport relation = new RelationSupport("REL1", ":type=RelationService", "Parenthood",
               new Role("Parent", new ObjectName(":type=Parent"), new ObjectName(":type=Parent")),
               new Role("Child", new ObjectName(":type=Child,id=1"), new ObjectName(":type=Child,id=2")));
         _server.RegisterMBean(relation, ":type=Parenthood,id=1");
         _relationService.AddRelation(":type=Parenthood,id=1");
      }
      [Test]
      [ExpectedException(typeof(InvalidRoleValueException))]
      public void TestAddRelationFailureInvalidClass()
      {
         RelationSupport relation = new RelationSupport("REL1", ":type=RelationService", "Parenthood",
               new Role("Parent", new ObjectName(":type=Child,id=1")),
               new Role("Child", new ObjectName(":type=Child,id=2")));
         _server.RegisterMBean(relation, ":type=Parenthood,id=1");
         _relationService.AddRelation(":type=Parenthood,id=1");
      }
      [Test]
      [ExpectedException(typeof(InvalidRoleValueException))]
      public void TestAddRelationFailureNotRegistered()
      {
         RelationSupport relation = new RelationSupport("REL1", ":type=RelationService", "Parenthood",
               new Role("Parent", new ObjectName(":type=Child,id=3")),
               new Role("Child", new ObjectName(":type=Child,id=2")));
         _server.RegisterMBean(relation, ":type=Parenthood,id=1");
         _relationService.AddRelation(":type=Parenthood,id=1");
      }
      #endregion

      #region AddRelationType tests
      [Test]
      public void TestAddRelationTypeSuccess()
      {
         RelationTypeSupport type = new RelationTypeSupport("Partnership",
            new RoleInfo("Partner1", typeof(ParentMBean), true, false, 0, 1, "Partner1"),
            new RoleInfo("Partner2", typeof(ParentMBean), false, true, 1, RoleInfo.RoleCardinalityInfinity, "Partner2"));
         _relationService.AddRelationType(type);
         Assert.AreEqual(RoleStatus.RoleOk, _relationService.CheckRoleReading("Partner1", "Partnership"));
         Assert.AreEqual(RoleStatus.RoleNotReadable, _relationService.CheckRoleReading("Partner2", "Partnership"));
         Assert.AreEqual(RoleStatus.RoleOk, _relationService.CheckRoleWriting("Partner2", "Partnership", false));
         Assert.AreEqual(RoleStatus.RoleNotWritable, _relationService.CheckRoleWriting("Partner1", "Partnership", false));
      }
      [Test]
      [ExpectedException(typeof(ArgumentNullException))]
      public void TestAddRelationTypeFailureNull()
      {
         _relationService.AddRelationType(null);
      }
      [Test]
      [ExpectedException(typeof(InvalidRelationTypeException), "Relation type with that name already registered.")]
      public void TestAddRelationTypeFailureInvalidRelationTypeName()
      {
         RelationTypeSupport type = new RelationTypeSupport("Parenthood",
            new RoleInfo("Partner1", typeof(ParentMBean), true, false, 0, 1, "Partner1"),
            new RoleInfo("Partner2", typeof(ParentMBean), false, true, 1, RoleInfo.RoleCardinalityInfinity, "Partner2"));
         _relationService.AddRelationType(type);
      }
      [Test]
      [ExpectedException(typeof(InvalidRelationTypeException), "Relation type contains no roles.")]
      public void TestAddRelationTypeFailureNoRoles()
      {
         RelationTypeSupport type = new RelationTypeSupport("Partnership");
         _relationService.AddRelationType(type);
      }
      [Test]
      [ExpectedException(typeof(InvalidRelationTypeException), "Relation type contains null RoleInfo.")]
      public void TestAddRelationTypeFailureNullRoleInfo()
      {
         RelationTypeSupport type = new RelationTypeSupport("Partnership",
            null,
            new RoleInfo("Partner2", typeof(ParentMBean), false, true, 1, RoleInfo.RoleCardinalityInfinity, "Partner2"));
         _relationService.AddRelationType(type);
      }
      [Test]
      [ExpectedException(typeof(InvalidRelationTypeException), "Relation type contains two roles with same name.")]
      public void TestAddRelationTypeFailureDuplicateRoleName()
      {
         RelationTypeSupport type = new RelationTypeSupport("Partnership",
            new RoleInfo("Partner1", typeof(ParentMBean), true, false, 0, 1, "Partner1"),
            new RoleInfo("Partner1", typeof(ParentMBean), false, true, 1, RoleInfo.RoleCardinalityInfinity, "Partner1"));
         _relationService.AddRelationType(type);
      }
      #endregion

      #region FindAssociatedMBeans tests
      [Test]
      public void TestFindAssociatedMBeansNoFilter()
      {
         PrepareForAssociationAndReferenceTests();
         IDictionary<ObjectName, IList<string>> res = _relationService.FindAssociatedMBeans(":type=Child,id=1", null, null);
         Assert.AreEqual(2, res.Count);
         Assert.IsTrue(res.ContainsKey(":type=Child,id=2"));
         Assert.IsTrue(res.ContainsKey(":type=Parent"));
         Assert.AreEqual(1, res[":type=Child,id=2"].Count);
         Assert.AreEqual("REL1", res[":type=Child,id=2"][0]);
         Assert.AreEqual(2, res[":type=Parent"].Count);
         Assert.IsTrue(res[":type=Parent"].Contains("REL1"));
         Assert.IsTrue(res[":type=Parent"].Contains("REL2"));         
      }
      [Test]
      public void TestFindAssociatedMBeansFilterByRelationType()
      {
         PrepareForAssociationAndReferenceTests();
         IDictionary<ObjectName, IList<string>> res = _relationService.FindAssociatedMBeans(":type=Parent", "Parenthood", null);
         Assert.AreEqual(2, res.Count);
         Assert.IsTrue(res.ContainsKey(":type=Child,id=1"));
         Assert.IsTrue(res.ContainsKey(":type=Child,id=2"));
         Assert.AreEqual(1, res[":type=Child,id=2"].Count);
         Assert.AreEqual("REL1", res[":type=Child,id=2"][0]);
         Assert.AreEqual(2, res[":type=Child,id=1"].Count);
         Assert.IsTrue(res[":type=Child,id=1"].Contains("REL1"));
         Assert.IsTrue(res[":type=Child,id=1"].Contains("REL2"));         
      }
      [Test]
      public void TestFindAssociatedMBeansFilterByRole()
      {
         PrepareForAssociationAndReferenceTests();
         IDictionary<ObjectName, IList<string>> res = _relationService.FindAssociatedMBeans(":type=Parent", null, "Partner1");
         Assert.AreEqual(1, res.Count);
         Assert.AreEqual("REL3", res[":type=Parent,id=2"][0]);
      }
      #endregion

      #region FindReferencingRelations test
      [Test]
      public void TestFindReferencingRelationsNoFilter()
      {
         PrepareForAssociationAndReferenceTests();
         IDictionary<string, IList<string>> res = _relationService.FindReferencingRelations(":type=Parent",  null, null);
         Assert.AreEqual(3, res.Count);
         Assert.IsTrue(res.ContainsKey("REL1"));
         Assert.IsTrue(res.ContainsKey("REL2"));
         Assert.IsTrue(res.ContainsKey("REL3"));
         Assert.AreEqual(1, res["REL1"].Count);
         Assert.AreEqual("Parent", res["REL1"][0]);
         Assert.AreEqual(1, res["REL2"].Count);
         Assert.AreEqual("Parent", res["REL2"][0]);
         Assert.AreEqual(1, res["REL3"].Count);         
         Assert.IsTrue(res["REL3"].Contains("Partner1"));
      }
      [Test]
      public void TestFindReferencingRelationsFilterByRelationType()
      {
         PrepareForAssociationAndReferenceTests();
         IDictionary<string, IList<string>> res = _relationService.FindReferencingRelations(":type=Parent", "Parenthood", null);
         Assert.AreEqual(2, res.Count);
         Assert.IsTrue(res.ContainsKey("REL1"));
         Assert.IsTrue(res.ContainsKey("REL2"));
         Assert.AreEqual(1, res["REL1"].Count);
         Assert.AreEqual("Parent", res["REL1"][0]);
         Assert.AreEqual(1, res["REL2"].Count);
         Assert.AreEqual("Parent", res["REL2"][0]);
      }
      [Test]
      public void TestFindReferencingRelationsFilterByRole()
      {
         PrepareForAssociationAndReferenceTests();
         IDictionary<string, IList<string>> res = _relationService.FindReferencingRelations(":type=Parent", null, "Partner1");
         Assert.AreEqual(1, res.Count);
         Assert.IsTrue(res.ContainsKey("REL3"));
         Assert.AreEqual(1, res["REL3"].Count);
         Assert.IsTrue(res["REL3"].Contains("Partner1"));
      }
      #endregion

      #region Utility
      private void PrepareForAssociationAndReferenceTests()
      {
         _server.RegisterMBean(new Parent(), ":type=Parent,id=2");
         _relationService.CreateRelationType("Partnership",
            new RoleInfo[] { 
               new RoleInfo("Partner1", typeof(ParentMBean), true, false, 1, 1, "Partner1"),
               new RoleInfo("Partner2", typeof(ParentMBean), false, true, 0, RoleInfo.RoleCardinalityInfinity, "Partner2") });

         _relationService.CreateRelation("REL1", "Parenthood", new Role[]
         {
            new Role("Parent", new ObjectName(":type=Parent")),
               new Role("Child", new ObjectName(":type=Child,id=1"), new ObjectName(":type=Child,id=2"))
         });
         _relationService.CreateRelation("REL2", "Parenthood", new Role[]
         {
            new Role("Parent", new ObjectName(":type=Parent")),
               new Role("Child", new ObjectName(":type=Child,id=1"))
         });
         _relationService.CreateRelation("REL3", "Partnership", new Role[]
         {
            new Role("Partner1", new ObjectName(":type=Parent")),
               new Role("Partner2", new ObjectName(":type=Parent,id=2"))
         });
      }
      #endregion

      #region MBeans
      public interface ParentMBean
      {
      }
      public class Parent : ParentMBean
      {
      }
      public interface ChildMBean
      {
      }
      public class Child : ChildMBean
      {
      }
      #endregion
   }
}
