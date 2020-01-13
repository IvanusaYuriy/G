using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.Models;
using GameStore.WebUI.HtmlHelpers;

namespace GameStore.UnitTests
{
    
    [TestClass]
    public class CartTests
    {
        public void Can_Add_New_Lines()
        {

            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };

            // Створення корзини
            Cart cart = new Cart();

            // Дія
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Ствердження
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Game, game1);
            Assert.AreEqual(results[1].Game, game2);
        }
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };

            
            Cart cart = new Cart();

            // Дії
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Game.GameId).ToList();

            // Твердження
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 екземплярів добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }
        public void Can_Remove_Line()
        {

            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };
            Game game3 = new Game { GameId = 3, Name = "Game3" };


            Cart cart = new Cart();

            // Добавлення декількох game в корзину
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 4);
            cart.AddItem(game3, 2);
            cart.AddItem(game2, 1);

            // Дія
            cart.RemoveLine(game2);

            // Ствердження
            Assert.AreEqual(cart.Lines.Where(c => c.Game == game2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }
        public void Calculate_Cart_Total()
        {

            Game game1 = new Game { GameId = 1, Name = "Игра1", Price = 100 };
            Game game2 = new Game { GameId = 2, Name = "Игра2", Price = 55 };


            Cart cart = new Cart();

            // Дія
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            decimal result = cart.ComputeTotalValue();

            // Ствердження
            Assert.AreEqual(result, 655);
        }
        public void Can_Clear_Contents()
        {
            Game game1 = new Game { GameId = 1, Name = "Game1", Price = 100 };
            Game game2 = new Game { GameId = 2, Name = "Game2", Price = 55 };

            Cart cart = new Cart();

            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            cart.Clear();

            Assert.AreEqual(cart.Lines.Count(), 0);
        }
        public void Can_Add_To_Cart()
        {
            // Створення імітованого сховища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
        new Game {GameId = 1, Name = "Game1", Category = "Cat1"},
    }.AsQueryable());


            Cart cart = new Cart();


            CartController controller = new CartController(mock.Object, null);

            // Добавлення гри в корзину
            controller.AddToCart(cart, 1, null);

            // Ствердження
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Game.GameId, 1);
        }

        // Перенаправлення на сторінку корзини після добавлення гри
        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // Імітоване сховище
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
        new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
    }.AsQueryable());


            Cart cart = new Cart();


            CartController controller = new CartController(mock.Object,null);


            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Ствердження
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // перевіряєм URL
        [TestMethod]
        public void Can_View_Cart_Contents()
        {

            Cart cart = new Cart();


            CartController target = new CartController(null,null);

            // Виклид метода Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Ствердження
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
        public void Cannot_Checkout_Empty_Cart()
        {
            // Створення імітованого обробника замовлень
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Створення порожньої корзини
            Cart cart = new Cart();

            // Деталі доставки
            ShippingDetails shippingDetails = new ShippingDetails();

           
            CartController controller = new CartController(null, mock.Object);

            // Дії
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Твердження — перевірка, що замовлення не було передане оброблювачу
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Твердження — перевірка, що метод повернув стандартне представлення
            Assert.AreEqual("", result.ViewName);

            // Твердження - про передачу неправильного представлення
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Створення корзини з елементом
            Cart cart = new Cart();
            cart.AddItem(new Game(), 1);

            // Створення контроллера
            CartController controller = new CartController(null, mock.Object);

            // Добавлення помилок в модель
            controller.ModelState.AddModelError("error", "error");

            // Спроба переходу до оплати
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Твердження - перевірка, що замовлення не передається обробнику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Твердження - перевірка, що метод повернув стандартне представлення
            Assert.AreEqual("", result.ViewName);

            // Твердження - про передачу неправильного представлення
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
    }
}

