using System;
using ClientsYchetik;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestClientsYchet
{
    [TestClass]
    public class TClientManager
    {
        [TestMethod]
        public void AddClient_ValidData_ReturnsEmptyString()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var manager = new ClientManager(mockRepo.Object);

            var validClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Денисюк Янина Николаевна",
                DateOfBirth = new DateTime(2006, 2, 18),
                Phone = "+79161234567",
                Email = "gospojavalera@mail.ru",
                DateOfReg = DateTime.Today
            };

            mockRepo.Setup(r => r.ExistsByEmailOrPhone(validClient.Email, validClient.Phone)).Returns(false);

            // Act
            var result = manager.AddClient(validClient);

            // Assert
            Assert.AreEqual(string.Empty, result, "При успешном добавлении должно возвращаться пустое сообщение");
            mockRepo.Verify(r => r.AddClient(validClient), Times.Once);
        }

        [TestMethod]
        public void AddClient_InvalidEmail_ReturnsErrorMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var manager = new ClientManager(mockRepo.Object);

            var invalidClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Денисюк Янина Николаевна",
                DateOfBirth = new DateTime(2006, 2, 18),
                Phone = "+79161234567",
                Email = "это не почта хихи",
                DateOfReg = DateTime.Today
            };

            // Act
            var result = manager.AddClient(invalidClient);

            // Assert
            Assert.AreEqual("Некорректный e-mail", result);
            mockRepo.Verify(r => r.AddClient(It.IsAny<Client>()), Times.Never);
        }

        [TestMethod]
        public void AddClient_EmptyFullName_ReturnsErrorMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var manager = new ClientManager(mockRepo.Object);

            var invalidClient = new Client
            {
                IdClientd = 1,
                FioClienta = "",
                DateOfBirth = new DateTime(2006, 2, 18),
                Phone = "+79161234567",
                Email = "gospojavalera@mail.ru",
                DateOfReg = DateTime.Today
            };

            // Act
            var result = manager.AddClient(invalidClient);

            // Assert
            Assert.AreEqual("ФИО клиента не может быть пустым", result);
            mockRepo.Verify(r => r.AddClient(It.IsAny<Client>()), Times.Never);
        }

        [TestMethod]
        public void AddClient_InvalidPhone_ReturnsErrorMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var manager = new ClientManager(mockRepo.Object);

            var invalidClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Денисюк Янина Николаевна",
                DateOfBirth = new DateTime(2006, 2, 18),
                Phone = "9161234567",
                Email = "gospojavalera@mail.ru",
                DateOfReg = DateTime.Today
            };

            // Act
            var result = manager.AddClient(invalidClient);

            // Assert
            Assert.AreEqual("Некорректный номер телефона", result);
            mockRepo.Verify(r => r.AddClient(It.IsAny<Client>()), Times.Never);
        }

        [TestMethod]
        public void AddClient_ExistingEmailOrPhone_ReturnsDuplicateError()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var manager = new ClientManager(mockRepo.Object);

            var duplicateClient = new Client
            {
                FioClienta = "Денисюк Янина Николаевна",
                DateOfBirth = new DateTime(2006, 2, 18),
                Phone = "+79161234567",
                Email = "gospojavalera@mail.ru",
                DateOfReg = DateTime.Today
            };

            mockRepo.Setup(r => r.ExistsByEmailOrPhone(duplicateClient.Email, duplicateClient.Phone)).Returns(true);

            // Act
            var result = manager.AddClient(duplicateClient);

            // Assert
            Assert.AreEqual("Клиент с такими контактными данными уже существует", result);
            mockRepo.Verify(r => r.AddClient(It.IsAny<Client>()), Times.Never);
        }
    }
}
