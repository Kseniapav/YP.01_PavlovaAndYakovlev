using ClientsYchet;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Testing
{
    [TestClass]
    public class TClientEditor
    {
        [TestMethod]
        public void ValideClientData_ValidData_ReturnsSuccessMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var editor = new ClientManager(mockRepo.Object);

            var originalClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Громова Дарья Сергеевна",
                DateOfBirth = new DateTime(2006, 4, 29),
                Phone = "+79801234567",
                Email = "gromchik@mail.ru",
                DateOfReg = new DateTime(2023, 1, 15)
            };

            var updatedClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Громова Дарья Сергеевна",
                DateOfBirth = new DateTime(2006, 4, 29),
                Phone = "+79205678901",
                Email = "gromchik@mail.ru",
                DateOfReg = new DateTime(2023, 1, 15)
            };

            mockRepo.Setup(r => r.UpdateClient(updatedClient)).Returns(true);

            // Act
            string result = editor.ValideClientData(updatedClient);

            // Assert
            Assert.AreEqual("Данные клиента Громова Дарья Сергеевна успешно изменены", result);
            mockRepo.Verify(r => r.UpdateClient(updatedClient), Times.Once);
        }


        [TestMethod]
        [DataRow("", "+79205678901", "koroleva@mail.ru", "Громова Дарья Сергеевна", "Упс! Поле ФИО не должно быть пустым!")] // пустое ФИО
        [DataRow("1222", "+79205678901", "koroleva@mail.ru", "Громова Дарья Сергеевна", "Упс! Проверьте заполненность данных ФИО!")] // неправильное ФИО
        [DataRow("Громова Дарья Сергеевна", "", "koroleva@mail.ru", "Громова Дарья Сергеевна", "Упс! Номер телефона не должно быть пустым!")] // пустой телефон
        [DataRow("Громова Дарья Сергеевна", "+79205678", "koroleva@mail.ru", "Громова Дарья Сергеевна", "Упс! Неправильный номер телефона!")] // неверный формат телефона
        [DataRow("Громова Дарья Сергеевна", "+79205678901", "koroleva@mail", "Громова Дарья Сергеевна", "Упс! Неверный формат e-mail!")] // неверный email
        public void ValideClientData_InvalidData_ReturnsExpectedError(
        string fio,
        string phone,
        string email,
        string validNameForContext,
        string expectedMessage)
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var editor = new ClientManager(mockRepo.Object);

            var invalidClient = new Client
            {
                IdClientd = 1,
                FioClienta = fio,
                DateOfBirth = new DateTime(2006, 4, 29),
                Phone = phone,
                Email = email,
                DateOfReg = new DateTime(2023, 1, 15)
            };

            // Act
            string result = editor.ValideClientData(invalidClient);

            // Assert
            Assert.AreEqual(expectedMessage, result);
            mockRepo.Verify(r => r.UpdateClient(It.IsAny<Client>()), Times.Never);
        }

        [TestMethod]
        public void ValideClientData_FutureBirthDate_ReturnsErrorMessage()
        {
            // Arrange
            Mock<IClientRepository> mockRepo = new Mock<IClientRepository>();
            ClientManager editor = new ClientManager(mockRepo.Object);

            Client invalidClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Громова Дарья Сергеевна",
                DateOfBirth = DateTime.Now.AddYears(1),
                Phone = "+79205678901",
                Email = "koroleva@mail.ru",
                DateOfReg = new DateTime(2023, 1, 15)
            };

            // Act
            string result = editor.ValideClientData(invalidClient);

            // Assert
            Assert.AreEqual("Упс! Дата рождения не может быть из будущего!", result);
            mockRepo.Verify(r => r.UpdateClient(It.IsAny<Client>()), Times.Never);
        }

        [TestMethod]
        public void ValideClientData_RepositoryUpdateFails_ReturnsErrorMessage()
        {
            // Arrange
            Mock<IClientRepository> mockRepo = new Mock<IClientRepository>();
            ClientManager editor = new ClientManager(mockRepo.Object);

            Client validClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Громова Дарья Сергеевна",
                DateOfBirth = new DateTime(2006, 4, 29),
                Phone = "+79205678901",
                Email = "koroleva@mail.ru",
                DateOfReg = new DateTime(2023, 1, 15)
            };

            mockRepo.Setup(r => r.UpdateClient(validClient)).Returns(false);

            // Act
            string result = editor.ValideClientData(validClient);

            // Assert
            Assert.AreEqual("Упс! Ошибка изменения базы данных, обратитесь к администратору.", result);
            mockRepo.Verify(r => r.UpdateClient(validClient), Times.Once);
        }
        [TestMethod]
        public void GetClientById_ReturnsUpdatedClientData()
        {
            // Arrange
            Mock<IClientRepository> mockRepo = new Mock<IClientRepository>();
            ClientManager manager = new ClientManager(mockRepo.Object);

            Client updatedClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Громова Дарья Сергеевна",
                DateOfBirth = new DateTime(2006, 4, 29),
                Phone = "+79205678901",
                Email = "gromchik@mail.ru",
                DateOfReg = new DateTime(2023, 1, 15)
            };

            // Настройка репозитория: при запросе клиента возвращаем обновлённые данные
            mockRepo.Setup(r => r.GetClientById(1)).Returns(updatedClient);

            // Act
            Client clientFromDb = manager.GetClientById(1);

            // Assert
            Assert.IsNotNull(clientFromDb);
            Assert.AreEqual(updatedClient.FioClienta, clientFromDb.FioClienta);
            Assert.AreEqual(updatedClient.Phone, clientFromDb.Phone);
            Assert.AreEqual(updatedClient.Email, clientFromDb.Email);
            mockRepo.Verify(r => r.GetClientById(1), Times.Once);
        }
    }
}
