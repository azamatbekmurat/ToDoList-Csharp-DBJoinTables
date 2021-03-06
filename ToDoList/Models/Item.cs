using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
    public class Item
    {
        private string _description;
        private int _id;
        // private int _categoryId;

        public Item(string description, int id = 0)
        {
            _description = description;
            // _categoryId = categoryId;
            _id = id;
        }

        public override bool Equals(System.Object otherItem)
        {
          if (!(otherItem is Item))
          {
            return false;
          }
          else
          {
             Item newItem = (Item) otherItem;
             bool idEquality = this.GetId() == newItem.GetId();
             bool descriptionEquality = this.GetDescription() == newItem.GetDescription();
            //  bool categoryEquality = this.GetCategoryId() == newItem.GetCategoryId();
             return (idEquality && descriptionEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetDescription().GetHashCode();
        }

        public string GetDescription()
        {
            return _description;
        }
        public int GetId()
        {
            return _id;
        }
        // public int GetCategoryId()
        // {
        //     return _categoryId;
        // }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO items (description) VALUES (@description);";

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@description";
            description.Value = this._description;
            cmd.Parameters.Add(description);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Item> GetAll()
        {
            List<Item> allItems = new List<Item> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM items;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int itemId = rdr.GetInt32(0);
              string itemDescription = rdr.GetString(1);
              Item newItem = new Item(itemDescription, itemId);
              allItems.Add(newItem);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allItems;
        }
        public static Item Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM items WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int itemId = 0;
            string itemDescription = "";
            // int itemCategoryId = 0;

            while(rdr.Read())
            {
              itemId = rdr.GetInt32(0);
              itemDescription = rdr.GetString(1);
              // itemCategoryId = rdr.GetInt32(2);
            }
            Item newItem = new Item(itemDescription, itemId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newItem;
        }

        public void AddCategory(Category newCategory)
        {
           MySqlConnection conn = DB.Connection();
           conn.Open();
           var cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"INSERT INTO categories_items (category_id, item_id) VALUES (@CategoryId, @ItemId);";

           MySqlParameter category_id = new MySqlParameter();
           category_id.ParameterName = "@CategoryId";
           category_id.Value = newCategory.GetId();
           cmd.Parameters.Add(category_id);

           MySqlParameter item_id = new MySqlParameter();
           item_id.ParameterName = "@ItemId";
           item_id.Value = _id;
           cmd.Parameters.Add(item_id);

           cmd.ExecuteNonQuery();
           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }
       }

      public List<Category> GetCategories()
      {
         MySqlConnection conn = DB.Connection();
         conn.Open();
         var cmd = conn.CreateCommand() as MySqlCommand;
         cmd.CommandText = @"SELECT categories.* FROM items
         JOIN categories_items ON (item_id = categories_items.item_id)
         JOIN categories ON (categories_items.category_id = categories.id)
         WHERE item_id = @ItemId;";

         MySqlParameter itemIdParameter = new MySqlParameter();
         itemIdParameter.ParameterName = "@ItemId";
         itemIdParameter.Value = _id;
         cmd.Parameters.Add(itemIdParameter);

         var rdr = cmd.ExecuteReader() as MySqlDataReader;

         List<Category> categories = new List<Category> {};
         while(rdr.Read())
         {
           int categoryId = rdr.GetInt32(0);
           string categoryName = rdr.GetString(1);
           Category newCategory = new Category(categoryName, categoryId);
           categories.Add(newCategory);
         }
         rdr.Dispose();

         conn.Close();
         if (conn != null)
         {
             conn.Dispose();
         }
         return categories;
      }

      public void UpdateDescription(string newDescription)
      {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";

          MySqlParameter searchId = new MySqlParameter();
          searchId.ParameterName = "@searchId";
          searchId.Value = _id;
          cmd.Parameters.Add(searchId);

          MySqlParameter description = new MySqlParameter();
          description.ParameterName = "@newDescription";
          description.Value = newDescription;
          cmd.Parameters.Add(description);

          cmd.ExecuteNonQuery();
          _description = newDescription;
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }

      }

      public static void DeleteAll()
      {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"DELETE FROM items;";
          cmd.ExecuteNonQuery();
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
      }

      public void Delete()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM items WHERE id = @ItemId; DELETE FROM categories_items WHERE item_id = @ItemId;";

        MySqlParameter itemIdParameter = new MySqlParameter();
        itemIdParameter.ParameterName = "@ItemId";
        itemIdParameter.Value = this.GetId();
        cmd.Parameters.Add(itemIdParameter);

        cmd.ExecuteNonQuery();
        if (conn != null)
        {
          conn.Close();
        }
    }
  }
}
