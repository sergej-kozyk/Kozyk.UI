﻿using Microsoft.AspNetCore.Mvc;

namespace Kozyk.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            //var cart = HttpContext.Session.Get<Cart>("cart");
            return View(/*cart*/);
        }
    }
}

