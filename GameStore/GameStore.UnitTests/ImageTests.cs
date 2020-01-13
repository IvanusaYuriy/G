using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace GameStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Створення об'єкту Game з даними img
            Game game = new Game
            {
                GameId = 2,
                Name = "Game2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Створення імітованого сховища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
                new Game {GameId = 1, Name = "Game1"},
                game,
                new Game {GameId = 3, Name = "Game3"}
            }.AsQueryable());

            // Створення контроллера
            GameController controller = new GameController(mock.Object);

            // виклид методу GetImage()
            ActionResult result = controller.GetImage(2);

            // Твердження
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(game.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
                new Game {GameId = 1, Name = "Игра1"},
                new Game {GameId = 2, Name = "Игра2"}
            }.AsQueryable());

            
            GameController controller = new GameController(mock.Object);

            // виклик методу GetImage()
            ActionResult result = controller.GetImage(10);

            // Твердження
            Assert.IsNull(result);
        }
    }
}