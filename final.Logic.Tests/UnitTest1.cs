using System;
using System.Collections.Generic;
using final.logic;
using final.data;
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
        Assert.Contains(newRoom, rooms) ;
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
    public void AddNewReservation_Should_AddReservationToList()
    {
        // Arrange

         ReservationManager.ClearData();
         ReservationManager.AddNewCustomer("John Doe","12234455667");
         ReservationManager.AddNewRoom(101,"Single");
        DateTime reservationDate = DateTime.Now.Date;
        int roomNumber = 101;
        string customerName = "John Doe";

        // Act
        ReservationManager.AddNewReservation(reservationDate, roomNumber, customerName);
  
        // Assert
       var customerReservations = ReservationManager.CustomerReservationReport(customerName);
       Assert.Equal(1,customerReservations.Count);
      
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


}
