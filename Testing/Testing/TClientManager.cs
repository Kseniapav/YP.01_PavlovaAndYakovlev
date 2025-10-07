using System;
using ClientsYchet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Testing
{
    [TestClass]
    public class TClientManager
    {
        [TestMethod]
        public void TestMethod1()
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


        [TestMethod]
        public void DeleteClient_ExistingClient_ReturnsEmptyString()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var manager = new ClientManager(mockRepo.Object);

            var email = "ivanov@mail.ru";
            var phone = "+79161234567";

            var existingClient = new Client { FioClienta = "Иванов Иван Иванович", Email = email, Phone = phone };

            mockRepo.Setup(r => r.FindByEmailOrPhone(email, phone)).Returns(existingClient);
            mockRepo.Setup(r => r.DeleteByEmailOrPhone(email, phone)).Returns(true);

            // Act
            var result = manager.DeleteClient(email, phone);

            // Assert
            Assert.AreEqual("Клиент успешно удалён", result);
            mockRepo.Verify(r => r.DeleteByEmailOrPhone(email, phone), Times.Once);
        }

        [TestMethod]
        public void DeleteClient_NonExistingClient_ReturnsErrorMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var manager = new ClientManager(mockRepo.Object);

            var email = "nonexist@mail.ru";
            var phone = "+79990001122";

            mockRepo.Setup(r => r.FindByEmailOrPhone(email, phone)).Returns((Client)null);

            // Act
            var result = manager.DeleteClient(email, phone);

            // Assert
            Assert.AreEqual("Клиент с указанными данными не найден", result);
            mockRepo.Verify(r => r.DeleteByEmailOrPhone(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void DeleteClient_RepositoryUnavailable_ReturnsError()
        {
            // Arrange
            var manager = new ClientManager(null);

            // Act
            var result = manager.DeleteClient("test@mail.ru", "+79990000000");

            // Assert
            Assert.AreEqual("Репозиторий недоступен", result);
        }
    }
    
}
