using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ToDoList.Models;
using System;

namespace ToDoList.Tests
{

    [TestClass]
    public class ItemTests : IDisposable
    {
        public void Dispose()
        {
            Item.DeleteAll();
        }
        public ItemTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
        }

        [TestMethod]
        public void Save_AssignsIdToObject_Id()
        {
          //Arrange
          Item testItem = new Item("Mow the lawn");

          //Act
          testItem.Save();
          Item savedItem = Item.GetAll()[0];

          int result = savedItem.GetId();
          int testId = testItem.GetId();

          //Assert
          Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsItemInDatabase_Item()
        {
          //Arrange
          Item testItem = new Item("Mow the lawn");
          testItem.Save();

          //Act
          Item foundItem = Item.Find(testItem.GetId());

          //Assert
          Assert.AreEqual(testItem, foundItem);
        }
    }
}
