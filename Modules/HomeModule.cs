using Nancy;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InventoryList;

namespace InventoryListing
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/items"] = _ => {
        List<Item> allItems = Item.GetAll();
        return View["items.cshtml", allItems];
      };
      Get["/item/{id}"] = parameters => {
        Item eachItem = Item.Find(parameters.id);
        return View["item.cshtml", eachItem];
      };
      Post["/items"] = _ => {
        Item inputItem = new Item(Request.Form["item"]);
        inputItem.Save();
        List<Item> allItems = Item.GetAll();
        return View["items.cshtml", allItems];
      };
      Post["/items_cleared"] = _ => {
        Item.DeleteAll();
        return View["items_cleared.cshtml"];
      };
      Post["/search"] = _ => {
        Item searchedItem = Item.Search(Request.Form["search-item"]);
        return View["item.cshtml", searchedItem];
      };
    }
  }
}
