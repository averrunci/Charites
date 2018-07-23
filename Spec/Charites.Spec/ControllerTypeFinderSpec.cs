// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carna;
using NSubstitute;

namespace Charites.Windows.Mvc
{
    [Specification("ControllerTypeFinder Spec")]
    class ControllerTypeFinderSpec : FixtureSteppable
    {
        IControllerTypeFinder<TestElement> ControllerTypeFinder { get; }

        IElementKeyFinder<TestElement> ElementKeyFinder { get; } = Substitute.For<IElementKeyFinder<TestElement>>();
        IDataContextFinder<TestElement> DataContextFinder { get; } = Substitute.For<IDataContextFinder<TestElement>>();

        TestElement Element { get; } = new TestElement(null);
        IEnumerable<Type> ControllerTypes { get; set; }
        
        public ControllerTypeFinderSpec()
        {
            ControllerTypeFinder = new ControllerTypeFinderTss(ElementKeyFinder, DataContextFinder);
        }

        [Example("When the key is specified")]
        void Ex01()
        {
            ElementKeyFinder.FindKey(Element).Returns("TestElement1");
            DataContextFinder.Find(Element).Returns(new TestControllers.TestDataContext());

            When("the element that has the key is specified", () => ControllerTypes = ControllerTypeFinder.Find(Element));
            Then("the found controller types should be ones attributed by the same key, the same type, or no specified", () =>
                ControllerTypes.SequenceEqual(new []
                {
                    typeof(TestControllers.TestElement1Controller),
                    typeof(TestControllers.TestElementController),
                    typeof(TestControllers.ElementController),
                    typeof(TestControllers.ViewController)
                })
            );
        }

        [Example("When the key is not specified")]
        void Ex02()
        {
            ElementKeyFinder.FindKey(Element).Returns((string)null);
            DataContextFinder.Find(Element).Returns(null);

            When("the element that does not have the key is specified", () => ControllerTypes = ControllerTypeFinder.Find(Element));
            Then("the found controller types should be ones attributed by the same type or no specified", () =>
                ControllerTypes.SequenceEqual(new[]
                {
                    typeof(TestControllers.TestElementController),
                    typeof(TestControllers.ViewController)
                })
            );
        }

        [Example("When the key is not specified and the view type has the base type")]
        void Ex03()
        {
            ElementKeyFinder.FindKey(Element).Returns((string)null);
            DataContextFinder.Find(Element).Returns(null);

            When("the element that has the key and has the base type is specified", () => ControllerTypes = ControllerTypeFinder.Find(new DerivedTestElement(null)));
            Then("the found controller types should be ones attributed by the same type, the base type of the same type, or no specified", () =>
                ControllerTypes.SequenceEqual(new[]
                {
                    typeof(TestControllers.TestElementController),
                    typeof(TestControllers.DerivedTestElementController),
                    typeof(TestControllers.ElementController),
                    typeof(TestControllers.ViewController)
                })
            );
        }

        [Example("When the key is specified and the view type has the base type")]
        void Ex04()
        {
            ElementKeyFinder.FindKey(Arg.Any<DerivedTestElement>()).Returns("TestElement1");
            DataContextFinder.Find(Element).Returns(new TestControllers.TestDataContext());

            When("the element that has the key and has the base type is specified", () => ControllerTypes = ControllerTypeFinder.Find(new DerivedTestElement(null)));
            Then("the found controller types should be ones attributed by the same key, the same type, the base type of the same type or no specified", () =>
                ControllerTypes.SequenceEqual(new[]
                {
                    typeof(TestControllers.TestElement1Controller),
                    typeof(TestControllers.TestElementController),
                    typeof(TestControllers.DerivedTestElementController),
                    typeof(TestControllers.DerivedTestElement1Controller),
                    typeof(TestControllers.ElementController),
                    typeof(TestControllers.ViewController)
                })
            );
        }

        [Example("When the key that expresses a data context type is specified")]
        [Sample(Source = typeof(ControllerSpecifiedDataContextSampleDataSource))]
        void Ex05(object dataContext, Type[] expectedControllerTypes)
        {
            ElementKeyFinder.FindKey(Element).Returns((string)null);
            DataContextFinder.Find(Element).Returns(dataContext);

            When("the element that contains the data context", () => ControllerTypes = ControllerTypeFinder.Find(Element));
            Then("the found controller types should be ones attributes by the key that expresses a data context type, the same type, or no specified", () =>
                ControllerTypes.SequenceEqual(new[] { typeof(TestControllers.TestElementController), typeof(TestControllers.ViewController) }.Concat(expectedControllerTypes))
            );
        }

        class ControllerSpecifiedDataContextSampleDataSource : ISampleDataSource
        {
            IEnumerable ISampleDataSource.GetData()
            {
                yield return new
                {
                    Description = "When the key is the name of the data context type",
                    DataContext = new TestControllers.TestDataContext(),
                    ExpectedControllerTypes = new[] { typeof(TestControllers.TestDataContextController) }
                };
                yield return new
                {
                    Description = "When the key is the name of the data context base type",
                    DataContext = new TestControllers.DerivedBaseTestDataContext(),
                    ExpectedControllerTypes = new[] { typeof(TestControllers.BaseTestDataContextController) }
                };
                yield return new
                {
                    Description = "When the key is the full name of the data context type",
                    DataContext = new TestControllers.TestDataContextFullName(),
                    ExpectedControllerTypes = new[] { typeof(TestControllers.TestDataContextFullNameController) }
                };
                yield return new
                {
                    Description = "When the key is the full name of the data context base type",
                    DataContext = new TestControllers.DerivedBaseTestDataContextFullName(),
                    ExpectedControllerTypes = new[] { typeof(TestControllers.BaseTestDataContextFullNameController) }
                };
                yield return new
                {
                    Description = "When the key is the name of the data context type that is generic",
                    DataContext = new TestControllers.GenericTestDataContext<string>(),
                    ExpectedControllerTypes = new[] { typeof(TestControllers.GenericTestDataContextController) }
                };
                yield return new
                {
                    Description = "When the key is the full name of the data context type that is generic",
                    DataContext = new TestControllers.GenericTestDataContextFullName<string>(),
                    ExpectedControllerTypes = new[] { typeof(TestControllers.GenericTestDataContextFullNameController), typeof(TestControllers.GenericTestDataContextFullNameWithoutParametersController) }
                };
                yield return new
                {
                    Description = "When the key is the name of interface implemented by the data context",
                    DataContext = new TestControllers.InterfaceImplementedTestDataContext(),
                    ExpectedControllerTypes = new[] { typeof(TestControllers.InterfaceImplementedTestDataContextController) }
                };
                yield return new
                {
                    Description = "When the key is the full name of interface implemented by the data context",
                    DataContext = new TestControllers.InterfaceImplementedTestDataContextFullName(),
                    ExpectedControllerTypes = new[] { typeof(TestControllers.InterfaceImplementedTestDataContextFullNameController) }
                };
            }
        }
    }
}
