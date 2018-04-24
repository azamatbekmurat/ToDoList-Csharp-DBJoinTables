using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System;

namespace ToDoList.Tests
{
  [TestClass]
  public class ItemTest : IDisposable
  {
    [TestMethod]
    public void GetDescription_ReturnsDescription_String()
    {
      //Arrange
      string description = "Walk the dog.";
      Item newItem = new Item(description);
      newItem.Save(); //there is no Save() method on project directory since Category class was added.

      //Act
      string result = newItem.GetDescription();

      //Assert
      Assert.AreEqual(description, result);
    }

    //to clear Item before starting next test case
    public void Dispose()
    {
      Item.ClearAll();
    }

    [TestMethod]
    public void GetAll_ReturnsItems_ItemList()
    {
      //Arrange
      string description01 = "Walk the dog";
      string description02 = "Wash the dishes";
      Item newItem1 = new Item(description01);
      newItem1.Save(); //there is no Save() method on project directory since Category class was added.
      Item newItem2 = new Item(description02);
      newItem2.Save(); //there is no Save() method on project directory since Category class was added.
      List<Item> newList = new List<Item> { newItem1, newItem2 };

      //Act
      List<Item> result = Item.GetAll();

      // foreach (Item thisItem in result)
      // {
      //   Console.WriteLine("Output: " + thisItem.GetDescription());
      // }

      //Assert
      CollectionAssert.AreEqual(newList, result);
    }
  }
}
