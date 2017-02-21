using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace InventoryList
{
  public class Item
  {
    private int _id;
    private string _itemName;

    public Item(string ItemName, int Id = 0)
    {
      _id = Id;
      _itemName = ItemName;
    }

    //Getters and Setters
    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _itemName;
    }
    public void SetName(string Name)
    {
      _itemName = Name;
    }

    public override bool Equals(System.Object otherItems)
    {
      if(!(otherItems is Item))
      {
        return false;
      }
      else
      {
        Item newItem = (Item) otherItems;
        bool idEquality = (this.GetId() == newItem.GetId());
        bool descriptionEquality = (this.GetName() == newItem.GetName());
        return (idEquality && descriptionEquality);
      }
    }

    public static Item Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM items WHERE id = @ItemId;", conn);

      SqlParameter itemIdParameter = new SqlParameter();
      itemIdParameter.ParameterName = "@ItemId";
      itemIdParameter.Value = id.ToString();
      cmd.Parameters.Add(itemIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundItemId = 0;
      string foundItemName = null;

      while(rdr.Read())
      {
        foundItemId = rdr.GetInt32(0);
        foundItemName = rdr.GetString(1);
      }

      Item foundItem = new Item(foundItemName, foundItemId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return foundItem;
    }

    public static Item Search(string searchTerm)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM items WHERE name = @ItemName;", conn);

      SqlParameter itemNameParameter = new SqlParameter();
      itemNameParameter.ParameterName = "@ItemName";
      itemNameParameter.Value = searchTerm;
      cmd.Parameters.Add(itemNameParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundItemId = 0;
      string foundItemName = null;

      while(rdr.Read())
      {
        foundItemId = rdr.GetInt32(0);
        foundItemName = rdr.GetString(1);
      }

      Item foundItem = new Item(foundItemName,foundItemId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundItem;
    }


    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM items;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        Item newItem = new Item(itemDescription,itemId);
        allItems.Add(newItem);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return allItems;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO items (name) OUTPUT INSERTED.id VALUES (@ItemName)", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@ItemName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM items;", conn);
      cmd.ExecuteNonQuery();

      SqlCommand cmd2 = new SqlCommand("DBCC CHECKIDENT('items', RESEED, 0)");
      cmd2.ExecuteNonQuery();
    }

    // public static void ClearId()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("ALTER TABLE items DROP COLUMN id;", conn);
    //   cmd.ExecuteNonQuery();
    // }
    //
    // public static void AddId()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("ALTER TABLE items ADD id INT IDENTITY (1,1) BEFORE name;", conn);
    //   cmd.ExecuteNonQuery();
    // }
  }
}
