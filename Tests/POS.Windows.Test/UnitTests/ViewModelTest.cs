

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace POS.Windows.Test.UnitTests
{
    [TestClass]
    public class ViewModelTest
    {
        [TestMethod]
        public void IsAbstractBaseClass()
        {
            Type t = typeof(ViewModel);
            Assert.IsTrue(t.IsAbstract);
        }

        [TestMethod]
        public void IsDataErrorInfo()
        {
            Assert.IsTrue(typeof(IDataErrorInfo).IsAssignableFrom(typeof(ViewModel)));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void IDataErrorInfo_ErrorProperty_IsNotSupported()
        {
            var viewModel = new StubViewModel();
            var value = viewModel.Error;
        }

        [TestMethod]
        public void indexerPropertyValidatesPropertyNameWithInvalidValue()
        {
            var viewModel = new StubViewModel();
            Assert.IsNotNull(viewModel["RequeredProperty"]);

        }

        [TestMethod]
        public void indexerPropertyValidatesPropertyNameWithValidValue()
        {
            var viewModel = new StubViewModel()
            {
                RequeredProperty = "Some Value"
            };
            Assert.IsNull(viewModel["RequeredProperty"]);

        }

        [TestMethod]
        public void IsObservableObject()
        {
            Assert.IsTrue(typeof(ViewModel).BaseType == typeof(ObservableObject));
        }
        class StubViewModel : ViewModel
        {
            [Required]
            public string RequeredProperty
            {
                get;
                set;
            }
        }
    }
}
