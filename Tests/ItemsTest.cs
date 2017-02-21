using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace InventoryList
{
  public class InventoryListTest : IDisposable
  {
    public InventoryListTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Inventory_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arange,Act
      int result = Item.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      //Arange, Act
      Item firstItem = new Item("Mow the lawn");
      Item secondItem = new Item("Mow the lawn");

      //Assert
      Assert.Equal(firstItem, secondItem);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arange
      Item testItem = new Item("Mow the lawn");

      //Act
      testItem.Save();
      List<Item> result = Item.GetAll();
      List<Item> testList = new List<Item>{testItem};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignIdToObject()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn");

      //Act
      testItem.Save();
      Item savedItem = Item.GetAll()[0];

      int result = savedItem.GetId();
      int testId = testItem.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Save_AssignNameToObject()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn");

      //Act
      testItem.Save();
      Item savedItem = Item.GetAll()[0];

      string result = savedItem.GetName();
      string testName = testItem.GetName();

      //Assert
      Assert.Equal(testName, result);
    }

    [Fact]
    public void Test_Find_FindsItemInDatabase()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn");
      testItem.Save();

      //Act
      Item foundItem = Item.Find(testItem.GetId());

      //Assert
      Assert.Equal(testItem, foundItem);
    }

    [Fact]
    public void Test_Search_SearchItemInDatabase()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn");
      Item testItem2 = new Item("Clean the house");
      Item testItem3 = new Item("Wash the floor");
      testItem.Save();
      testItem2.Save();
      testItem3.Save();

      //Act
      Item foundItem = Item.Search("Clean the house");

      //Assert
      Assert.Equal(testItem2, foundItem);
    }

    [Fact]
    public void Test_SearchWrong_SearchItemInDatabase()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn");
      Item testItem2 = new Item("Clean the house");
      Item testItem3 = new Item("Wash the floor");
      testItem.Save();
      testItem2.Save();
      testItem3.Save();

      //Act
      Item foundItem = Item.Search("Clean the housefsdfasd");
      Item verifyItem = new Item(null);

      //Assert
      Assert.Equal(verifyItem, foundItem);
    }

    public void Dispose()
    {
      Item.DeleteAll();
    }
  }
}
