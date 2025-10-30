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
            var result = editor.EditClient(updatedClient);

            // Assert
            Assert.AreEqual("Номер телефона клиента успешно изменен", result);
            mockRepo.Verify(r => r.UpdateClient(updatedClient), Times.Once);
        }

        [TestMethod]
        public void ValideClientData_EmptyFullName_ReturnsErrorMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var editor = new ClientManager(mockRepo.Object);

            var invalidClient = new Client
            {
                IdClientd = 1,
                FioClienta = "",
                DateOfBirth = new DateTime(2006, 4, 29),
                Phone = "+79205678901",
                Email = "koroleva@mail.ru",
                DateOfReg = new DateTime(2023, 1, 15)
            };

            // Act
            var result = editor.ValideClientData(invalidClient);

            // Assert
            Assert.AreEqual("Упс! Поле ФИО не должно быть пустым!", result);
            mockRepo.Verify(r => r.UpdateClient(It.IsAny<Client>()), Times.Never);
        }

        [TestMethod]
        public void ValideClientData_EmptyPhone_ReturnsErrorMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var editor = new ClientManager(mockRepo.Object);

            var invalidClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Громова Дарья Сергеевна",
                DateOfBirth = new DateTime(2006, 4, 29),
                Phone = "",
                Email = "koroleva@mail.ru",
                DateOfReg = new DateTime(2023, 1, 15)
            };

            // Act
            var result = editor.ValideClientData(invalidClient);

            // Assert
            Assert.AreEqual("Упс! Номер телефона не должно быть пустым!", result);
            mockRepo.Verify(r => r.UpdateClient(It.IsAny<Client>()), Times.Never);
        }

        [TestMethod]
        public void ValideClientData_InvalidFullNameFormat_ReturnsErrorMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var editor = new ClientManager(mockRepo.Object);

            var invalidClient = new Client
            {
                IdClientd = 1,
                FioClienta = "1222",
                DateOfBirth = new DateTime(2006, 4, 29),
                Phone = "+79205678901",
                Email = "koroleva@mail.ru",
                DateOfReg = new DateTime(2023, 1, 15)
            };

            // Act
            var result = editor.ValideClientData(invalidClient);

            // Assert
            Assert.AreEqual("Упс! Проверьте заполненность данных ФИО!", result);
            mockRepo.Verify(r => r.UpdateClient(It.IsAny<Client>()), Times.Never);
        }

        [TestMethod]
        public void ValideClientData_FutureBirthDate_ReturnsErrorMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var editor = new ClientManager(mockRepo.Object);

            var invalidClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Громова Дарья Сергеевна",
                DateOfBirth = DateTime.Now.AddYears(1),
                Phone = "+79205678901",
                Email = "koroleva@mail.ru",
                DateOfReg = new DateTime(2023, 1, 15)
            };

            // Act
            var result = editor.ValideClientData(invalidClient);

            // Assert
            Assert.AreEqual("Упс! Дата рождения не может быть из будущего!", result);
            mockRepo.Verify(r => r.UpdateClient(It.IsAny<Client>()), Times.Never);
        }

        [TestMethod]
        public void ValideClientData_InvalidPhoneFormat_ReturnsErrorMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var editor = new ClientManager(mockRepo.Object);

            var invalidClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Громова Дарья Сергеевна",
                DateOfBirth = new DateTime(2006, 4, 29),
                Phone = "+79205678",
                Email = "koroleva@mail.ru",
                DateOfReg = new DateTime(2023, 1, 15)
            };

            // Act
            var result = editor.ValideClientData(invalidClient);

            // Assert
            Assert.AreEqual("Упс! Неправильный номер телефона!", result);
            mockRepo.Verify(r => r.UpdateClient(It.IsAny<Client>()), Times.Never);
        }

        [TestMethod]
        public void ValideClientData_InvalidEmailFormat_ReturnsErrorMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var editor = new ClientManager(mockRepo.Object);

            var invalidClient = new Client
            {
                IdClientd = 1,
                FioClienta = "Громова Дарья Сергеевна",
                DateOfBirth = new DateTime(2006, 4, 29),
                Phone = "+79205678901",
                Email = "koroleva@mail",
                DateOfReg = new DateTime(2023, 1, 15)
            };

            // Act
            var result = editor.ValideClientData(invalidClient);

            // Assert
            Assert.AreEqual("Упс! Неверный формат e-mail!", result);
            mockRepo.Verify(r => r.UpdateClient(It.IsAny<Client>()), Times.Never);
        }

        [TestMethod]
        public void ValideClientData_RepositoryUpdateFails_ReturnsErrorMessage()
        {
            // Arrange
            var mockRepo = new Mock<IClientRepository>();
            var editor = new ClientManager(mockRepo.Object);

            var validClient = new Client
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
            var result = editor.ValideClientData(validClient);

            // Assert
            Assert.AreEqual("Упс! Ошибка изменения базы данных, обратитесь к администратору.", result);
            mockRepo.Verify(r => r.UpdateClient(validClient), Times.Once);
        }

    }
}
