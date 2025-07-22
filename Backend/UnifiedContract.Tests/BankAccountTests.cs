using System;
using UnifiedContract.Domain.Entities.HR;
using Xunit;

namespace UnifiedContract.Tests
{
    public class BankAccountTests
    {
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var bankName = "Test Bank";
            var accountNumber = "123456789";
            var iban = "SA12345678901234567890";
            var swiftCode = "TESTSWIFT";
            var branchName = "Main Branch";
            var branchCode = "001";
            var isPrimary = true;

            // Act
            var account = new BankAccount(employeeId, bankName, accountNumber, iban, swiftCode, branchName, branchCode, isPrimary);

            // Assert
            Assert.Equal(employeeId, account.EmployeeId);
            Assert.Equal(bankName, account.BankName);
            Assert.Equal(accountNumber, account.AccountNumber);
            Assert.Equal(iban, account.IBAN);
            Assert.Equal(swiftCode, account.SwiftCode);
            Assert.Equal(branchName, account.BranchName);
            Assert.Equal(branchCode, account.BranchCode);
            Assert.True(account.IsPrimary);
        }

        [Fact]
        public void UpdateDetails_UpdatesPropertiesCorrectly()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var account = new BankAccount(employeeId, "Bank1", "111", "IBAN1", "SWIFT1", "Branch1", "B001", false);

            // Act
            account.UpdateDetails(bankName: "Bank2", accountNumber: "222", iban: "IBAN2", swiftCode: "SWIFT2", branchName: "Branch2", branchCode: "B002");

            // Assert
            Assert.Equal("Bank2", account.BankName);
            Assert.Equal("222", account.AccountNumber);
            Assert.Equal("IBAN2", account.IBAN);
            Assert.Equal("SWIFT2", account.SwiftCode);
            Assert.Equal("Branch2", account.BranchName);
            Assert.Equal("B002", account.BranchCode);
        }

        [Fact]
        public void SetAsPrimary_And_UnsetAsPrimary_WorkCorrectly()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var account = new BankAccount(employeeId, "Bank", "123", isPrimary: false);

            // Act
            account.SetAsPrimary();
            Assert.True(account.IsPrimary);

            account.UnsetAsPrimary();
            Assert.False(account.IsPrimary);
        }

        [Fact]
        public void Constructor_ThrowsException_WhenBankNameIsEmpty()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            // Act & Assert
            Assert.Throws<UnifiedContract.Domain.Exceptions.BusinessRuleException>(() =>
                new BankAccount(employeeId, "", "123")
            );
        }

        [Fact]
        public void Constructor_ThrowsException_WhenAccountNumberIsEmpty()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            // Act & Assert
            Assert.Throws<UnifiedContract.Domain.Exceptions.BusinessRuleException>(() =>
                new BankAccount(employeeId, "Bank", "")
            );
        }
    }
} 