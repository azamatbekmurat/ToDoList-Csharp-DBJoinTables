using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
          return View();
      }
      [HttpGet("/success")]
      public ActionResult Success()
      {
          return View();
      }

    }
}
