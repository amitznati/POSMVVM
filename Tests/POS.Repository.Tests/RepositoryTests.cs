using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POS.Data.Repository;
using POS.Data;
using POSEntities.Entities;

namespace POS.Repository.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void GetAllEmployee()
        {
            var emps = UnitOfWork.get().Employees.GetAll();
            Assert.IsNotNull(emps);
        }

        [TestMethod]
        public void addNewEmployee()
        {
            using(UnitOfWork inst = UnitOfWork.get()){
            Employee emp = new Employee();
            emp.FirstName = "שני";
            emp.LastName = "זנטי2";
            emp.Password = "1010";
            inst.Peoples.Add(emp);
            int empnum = inst.Complete();
            Assert.IsNotNull( inst.Employees.Get(empnum));
            //UnitOfWork.get().
            }

        }
        [TestMethod]
        public void addNewProduct()
        {
            using(UnitOfWork inst = UnitOfWork.get())
            {
                Product p = new Product()
                {
                    deptId = 2,
                    groupId = 1,
                    prodName = "לחמניה",
                    salePrice = 1.5M,
                };
                inst.Products.Add(p);
                inst.Complete();

            }
        }
    }
}
