using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace prjVegetable.Controllers
{

    public class BusinessOperationController : Controller
	{

        public IActionResult List()
		{
			return View();
		}

		//銷售概況
		public IActionResult business()
		{
			return View();
		}

		//交易紀錄
		public IActionResult transaction()
		{
			return View();
		}

		//銷售商品分類
		public IActionResult goods()
		{
			return View();
		}






    }



}
