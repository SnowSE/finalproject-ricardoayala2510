using System;
using System.Collections.Generic;
using final.data;
using final.logic;
using Xunit;

public class ReservationManagerTests
{
    [Fact]
    public void AddNewRoom_Should_AddRoomToList()
    {
        // Arrange
        (int roomNumber, string roomType) newRoom = (101, "Single");

        // Act
        ReservationManager.AddNewRoom(newRoom.roomNumber, newRoom.roomType);

        // Assert
        var rooms = DataManager.ReadRooms();
        Assert.Contains(newRoom, rooms);
    }

    [Fact]
    public void AddNewCustomer_Should_AddCustomerToList()
    {
        // Arrange
        string customerName = "John Doe";
        string cardNumber = "1234567890123456";

        // Act
        ReservationManager.AddNewCustomer(customerName, cardNumber);

        // Assert
        var customers = DataManager.ReadCustomers();
        Assert.Contains((customerName, cardNumber), customers);
    }

    [Fact]
    public void AddNewReservation_Should_AddReservation()
    {
        // Arrange
        DateTime checkInDate = DateTime.Now.Date;
        DateTime checkOutDate = DateTime.Now.Date.AddDays(2);
        int roomNumber = 101;
        string customerName = "John Doe";

        // Act
        ReservationManager.AddNewCustomer(customerName, "1234567890123456");
        ReservationManager.AddNewRoom(roomNumber, "Single");
        ReservationManager.AddNewReservation(checkInDate, checkOutDate, roomNumber, customerName);

        // Assert
        var reservations = DataManager.ReadReservations();
        Assert.NotEmpty(reservations);

        for (DateTime date = checkInDate; date <= checkOutDate; date = date.AddDays(1))
        {
            Assert.Contains(reservations, r => r.Item2.Date == date.Date && r.Item3 == roomNumber && r.Item4 == customerName);
        }
    }

    [Fact]
    public void AvailableRoomSearch_Should_ReturnAvailableRooms()
    {
        // Act
        var availableRooms = ReservationManager.AvailableRoomSearch(DateTime.Now.Date);

        // Assert
        Assert.NotNull(availableRooms);
    }

    [Fact]
    public void ReservationReport_Should_ReturnReservationsForDate()
    {
        DateTime date = DateTime.Now.Date;

        // Act
        var reservations = ReservationManager.ReservationReport(date);

        // Assert
        Assert.NotNull(reservations);
    }

    [Fact]
    public void CustomerReservationReport_Should_ReturnCustomerReservations()
    {
        // Arrange
        string customerName = "John Doe";

        // Act
        var customerReservations = ReservationManager.CustomerReservationReport(customerName);

        // Assert
        Assert.NotNull(customerReservations);
    }

    [Fact]
    public void IsRoomAvailable_Should_ReturnTrueForAvailableRoom()
    {
        // Arrange
        DateTime date = DateTime.Now.Date;
        int roomNumber = 101;

        // Act
        var reservations = new List<Tuple<string, DateTime, int, string, string>>();
        var result = ReservationManager.IsRoomAvailable(roomNumber, date, reservations);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRoomAvailable_Should_ReturnFalseForBookedRoom()
    {
        // Arrange
        DateTime date = DateTime.Now.Date;
        int roomNumber = 101;
        string customerName = "John Doe";
        var reservations = new List<Tuple<string, DateTime, int, string, string>>
        {
            Tuple.Create(Guid.NewGuid().ToString(), date, roomNumber, customerName, Guid.NewGuid().ToString())
        };

        // Act
        var result = ReservationManager.IsRoomAvailable(roomNumber, date, reservations);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CustomersContainName_Should_ReturnTrueForExistingName()
    {
        // Arrange
        string customerName = "John Doe";
        var customers = new List<(string, string)> { (customerName, "1234567890123456") };

        // Act
        var result = ReservationManager.CustomersContainName(customers, customerName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CustomersContainName_Should_ReturnFalseForNonExistingName()
    {
        // Arrange
        string customerName = "John Doe";
        var customers = new List<(string, string)> { ("Jane Doe", "1234567890123456") };

        // Act
        var result = ReservationManager.CustomersContainName(customers, customerName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CalculateDiscountedPrice_Should_ApplyDiscountForFrequentTraveler()
    {
        // Arrange
        ReservationManager.LoadData();
        string customerName = "John Doe";
        decimal originalPrice = 100.0m;

        int i = 0;
        DateTime checkInDate = DateTime.Now.AddMonths(6).AddDays(i);
        DateTime checkOutDate = DateTime.Now.AddMonths(6).AddDays(i + 5);
        int roomNumber = 100;
        ReservationManager.AddNewReservation(checkInDate, checkOutDate, roomNumber, customerName);

        // Act
        decimal discountedPrice = ReservationManager.CalculateDiscountedPrice(originalPrice, customerName);

        // Assert
        decimal expectedDiscountedPrice = originalPrice * (1 - 0.1m);
        Assert.Equal(expectedDiscountedPrice, discountedPrice);
    }

    [Fact]
    public void ChangeRoomPrice_Should_UpdateRoomPrice()
    {
        // Arrange
        ReservationManager.LoadData();
        string roomType = "Single";
        decimal originalPrice = ReservationManager.GetRoomPrice(roomType);
        decimal newPrice = originalPrice + 10.00m; 

        // Act
        ReservationManager.ChangeRoomPrice(roomType, newPrice);

        // Assert
        decimal updatedPrice = ReservationManager.GetRoomPrice(roomType);
        Assert.Equal(newPrice, updatedPrice);
    }



    [Fact]
    public void RefundReservation_Should_RemoveReservationAndAddRefund()
    {
        // Arrange
        DateTime checkInDate = DateTime.Now.Date;
        DateTime checkOutDate = checkInDate.AddDays(1);
        int roomNumber = 211;
        string customerName = "John Doe";

        ReservationManager.AddNewCustomer(customerName, "1234567890123456");
        ReservationManager.AddNewRoom(roomNumber, "Double");
        ReservationManager.AddNewReservation(checkInDate, checkOutDate, roomNumber, customerName);
        var reservation = ReservationManager.ReservationReport(checkInDate).FirstOrDefault();

        // Act
        ReservationManager.RefundReservation(reservation.Item1);

        // Assert
        var refundedReservations = DataManager.ReadRefunds();
        Assert.Contains(reservation, refundedReservations);

        var remainingReservations = ReservationManager.ReservationReport(checkInDate);
        Assert.DoesNotContain(reservation, remainingReservations);
    }

    [Fact]
    public void CalculateUtilizationRate_Should_ReturnUtilizationRateForDate()
    {
        // Arrange
        DateTime reportDate = DateTime.Now.Date;

        // Act
        decimal utilizationRate = ReservationManager.CalculateUtilizationRate(reportDate);

        // Assert
        Assert.InRange(utilizationRate, 0, 100);
    }

    [Fact]
    public void CalculateUtilizationRateRange_Should_ReturnUtilizationRatesForDateRange()
    {
        // Arrange
        DateTime startDate = DateTime.Now.Date;
        DateTime endDate = startDate.AddDays(5);

        // Act
        List<decimal> utilizationRates = ReservationManager.CalculateUtilizationRateRange(startDate, endDate);

        // Assert
        Assert.NotNull(utilizationRates);
        Assert.Equal((endDate - startDate).Days + 1, utilizationRates.Count);

    }

    [Fact]
    public void GetRoomText_Should_ReturnRoomDetailsText()
    {
        // Act
        string roomDetailsText = ReservationManager.GetRoomText();

        // Assert
        Assert.NotNull(roomDetailsText);
        // Add more specific assertions based on your data
    }

 [Fact]
public void SaveData_Should_WriteDataToDataManager()
{
    // Act
    ReservationManager.SaveData();

    // Assert
    var savedRooms = DataManager.ReadRooms();
    var savedCustomers = DataManager.ReadCustomers();
    var savedReservations = DataManager.ReadReservations();
    var savedRoomPrices = DataManager.ReadRoomPrices();

    // Add assertions based on your data structure
    Assert.NotNull(savedRooms);
    Assert.NotNull(savedCustomers);
    Assert.NotNull(savedReservations);
    Assert.NotNull(savedRoomPrices); 
}


}
