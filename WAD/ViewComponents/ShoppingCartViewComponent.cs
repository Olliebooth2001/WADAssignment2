using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WAD.Models;

namespace WAD.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        const string SessionCart = "_Cart";

        public ShoppingCartViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<CartItem> cart = new List<CartItem>();
            if (HttpContext.Session.GetString(SessionCart) != null)
            {
                string serialJSON = HttpContext.Session.GetString(SessionCart);
                cart = JsonSerializer.Deserialize<List<CartItem>>(serialJSON);
            }
            return View(cart);
        }
    }



}