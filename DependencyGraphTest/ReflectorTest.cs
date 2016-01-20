﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyGraphTest
{
    using DependencyGraph;
    using NUnit.Framework;
    using System.Reflection;

    [TestFixture]
    public class ReflectorTest
    {
        private Assembly sampleCodeBaseAssembly; 
        private Reflector reflector;

        [SetUp]
        public void setUp()
        {
            sampleCodeBaseAssembly = Assembly.GetAssembly(typeof(SampleCodeBase.Foo));
            reflector = new Reflector(sampleCodeBaseAssembly);
        }

        [Test]
        public void GetDependencyGraph()
        {
            DirectedGraph<Type> graph = reflector.GetDependencyGraph();
            Assert.IsNotNull(graph);
            Assert.AreEqual(19, graph.Count());

            DirectedGraph<Type>.Edges edges = graph.EdgesFor(typeof(SampleCodeBase.IFoo));
            Assert.IsNotNull(edges);
            Assert.IsNotNull(edges.inBound);
            Assert.IsNotNull(edges.outBound);
            Assert.AreEqual(1, edges.inBound.Count());
            Assert.AreEqual(typeof(SampleCodeBase.ImplementsIFoo), edges.inBound.First());
            Assert.AreEqual(0, edges.outBound.Count());
        }

        [Test]
        public void GetDependentClasses()
        {
            IEnumerable<Type> dependentClasses = reflector.GetDependentClasses(typeof(SampleCodeBase.Foo));
            Assert.IsNotNull(dependentClasses);
            Assert.AreEqual(15, dependentClasses.Count());
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.InheritsFoo)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsCtorArg)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsTempVar)));

            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsPublicMethodArg)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsProtectedMethodArg)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsPrivateMethodArg)));

            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsPublicStaticMethodArg)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsProtectedStaticMethodArg)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsPrivateStaticMethodArg)));

            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsPublicInstanceVar)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsProtectedInstanceVar)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsPrivateInstanceVar)));
            
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsPublicStaticVar)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsProtectedStaticVar)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsPrivateStaticVar)));
        }

        [Test]
        public void GetDependentClassesFromConstructor()
        {
            IEnumerable<Type> dependentClasses = reflector.GetDependentClassesFromConstructor(typeof(SampleCodeBase.Foo));
            Assert.IsNotNull(dependentClasses);
            Assert.AreEqual(1, dependentClasses.Count());
            Assert.AreEqual(typeof(SampleCodeBase.AcceptsFooAsCtorArg), dependentClasses.First());
        }

        [Test]
        public void GetDependentClassesFromInheritance()
        {
            IEnumerable<Type> dependentClasses = reflector.GetDependentClassesFromInheritance(typeof(SampleCodeBase.Foo));
            Assert.IsNotNull(dependentClasses);
            Assert.AreEqual(1, dependentClasses.Count());
            Assert.AreEqual(typeof(SampleCodeBase.InheritsFoo), dependentClasses.First());
        }

        [Test]
        public void GetDependentClassesFromInterface()
        {
            IEnumerable<Type> dependentClasses = reflector.GetDependentClassesFromInterface(typeof(SampleCodeBase.IFoo));
            Assert.IsNotNull(dependentClasses);
            Assert.AreEqual(1, dependentClasses.Count());
            Assert.AreEqual(typeof(SampleCodeBase.ImplementsIFoo), dependentClasses.First());
        }

        [Test]
        public void GetDependentClassesFromMethods()
        {
            IEnumerable<Type> dependentClasses = reflector.GetDependentClassesFromMethods(typeof(SampleCodeBase.Foo));
            Assert.IsNotNull(dependentClasses);
            Assert.AreEqual(6, dependentClasses.Count());
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsPublicStaticMethodArg)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsProtectedStaticMethodArg)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsPrivateStaticMethodArg)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsPublicMethodArg)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsProtectedMethodArg)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.AcceptsFooAsPrivateMethodArg)));
        }

        [Test]
        public void GetDependentClassesFromMembers()
        {
            IEnumerable<Type> dependentClasses = reflector.GetDependentClassesFromMembers(typeof(SampleCodeBase.Foo));
            Assert.IsNotNull(dependentClasses);
            Assert.AreEqual(6, dependentClasses.Count());
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsPublicStaticVar)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsProtectedStaticVar)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsPrivateStaticVar)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsPublicInstanceVar)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsProtectedInstanceVar)));
            Assert.IsTrue(dependentClasses.Contains(typeof(SampleCodeBase.UsesFooAsPrivateInstanceVar)));
        }

        [Test]
        public void GetDependentClassesFromTemporaryVariable()
        {
            IEnumerable<Type> dependentClasses = reflector.GetDependentClassesFromTemporaryVariable(typeof(SampleCodeBase.Foo));
            Assert.IsNotNull(dependentClasses);
            Assert.AreEqual(1, dependentClasses.Count());
            Assert.AreEqual(typeof(SampleCodeBase.UsesFooAsTempVar), dependentClasses.First());
        }
    }
}
