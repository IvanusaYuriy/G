using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            // Створення імітованого сховища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Game1"},
                new Game { GameId = 2, Name = "Game2"},
                new Game { GameId = 3, Name = "Game3"},
                new Game { GameId = 4, Name = "Game4"},
                new Game { GameId = 5, Name = "Game5"}
            });

            // Створення контроллера
            AdminController controller = new AdminController(mock.Object);

            // Дії
            List<Game> result = ((IEnumerable<Game>)controller.Index().
                ViewData.Model).ToList();

            // Твердження
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Game1", result[0].Name);
            Assert.AreEqual("Game2", result[1].Name);
            Assert.AreEqual("Game3", result[2].Name);
        }
        public void Can_Edit_Game()
        {
            
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
    {
        new Game { GameId = 1, Name = "Game1"},
        new Game { GameId = 2, Name = "Game2"},
        new Game { GameId = 3, Name = "Game3"},
        new Game { GameId = 4, Name = "Game4"},
        new Game { GameId = 5, Name = "Game5"}
    });

            
            AdminController controller = new AdminController(mock.Object);

            
            Game game1 = controller.Edit(1).ViewData.Model as Game;
            Game game2 = controller.Edit(2).ViewData.Model as Game;
            Game game3 = controller.Edit(3).ViewData.Model as Game;

            
            Assert.AreEqual(1, game1.GameId);
            Assert.AreEqual(2, game2.GameId);
            Assert.AreEqual(3, game3.GameId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Game()
        {
            // Створення імітованого сховища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
    {
        new Game { GameId = 1, Name = "Game1"},
        new Game { GameId = 2, Name = "Game2"},
        new Game { GameId = 3, Name = "Game3"},
        new Game { GameId = 4, Name = "Game4"},
        new Game { GameId = 5, Name = "Game5"}
    });

            
            AdminController controller = new AdminController(mock.Object);

            // Дії
            Game result = controller.Edit(6).ViewData.Model as Game;

            // Assert
        }
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            
            Mock<IGameRepository> mock = new Mock<IGameRepository>();

            
            AdminController controller = new AdminController(mock.Object);

            // Створення об'єкта Game
            Game game = new Game { Name = "Test" };

            // Спроба зберегту гру
            ActionResult result = controller.Edit(game);

            // Твердження - перевірка того, що до сховища проводиться звернення
            mock.Verify(m => m.SaveGame(game));

            // Твердження - перевірка типу результату метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            
            Mock<IGameRepository> mock = new Mock<IGameRepository>();

            
            AdminController controller = new AdminController(mock.Object);

            
            Game game = new Game { Name = "Test" };

            // Добавлення помилки в стан моделі
            controller.ModelState.AddModelError("error", "error");

            // Спроба зберегту гру
            ActionResult result = controller.Edit(game);

            // Твердження - перевірка того, що до сховища не проводиться звернення 
            mock.Verify(m => m.SaveGame(It.IsAny<Game>()), Times.Never());

            // Тведження - перевірка типу результату метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        public void Can_Delete_Valid_Games()
        {
            
            Game game = new Game { GameId = 2, Name = "Игра2" };

            
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
    {
        new Game { GameId = 1, Name = "Game1"},
        new Game { GameId = 2, Name = "Game2"},
        new Game { GameId = 3, Name = "Game3"},
        new Game { GameId = 4, Name = "Game4"},
        new Game { GameId = 5, Name = "Game5"}
    });

            
            AdminController controller = new AdminController(mock.Object);

            // Видалення гри
            controller.Delete(game.GameId);

            // перевірка того, що метод видалення в сховивищі викликається для коректного об'єкта Game
            mock.Verify(m => m.DeleteGame(game.GameId));
        }

    }

}