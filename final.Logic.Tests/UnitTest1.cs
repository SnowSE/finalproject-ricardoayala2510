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
        int roomNumber = 101;
        string roomType = "Single";

        // Act
        ReservationManager.AddNewRoom(roomNumber, roomType);

        // Assert
        var rooms = DataManager.ReadRooms();
        Assert.Contains(new Tuple<int, string>(roomNumber, roomType), rooms);
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
        Assert.Contains(new Tuple<string, string>(customerName, cardNumber), customers);
    }

    [Fact]
    public void AddNewReservation_Should_AddReservationToList()
    {
        // Arrange
        DateTime reservationDate = DateTime.Now.Date;
        int roomNumber = 101;
        string customerName = "John Doe";

        // Act
        ReservationManager.AddNewReservation(reservationDate, roomNumber, customerName);

        // Assert
        var reservations = DataManager.ReadReservations();
        bool found = false;
        foreach (var reservation in reservations)
        {
            if (reservation.Item3 == roomNumber && reservation.Item4 == customerName && reservation.Item2.Date == reservationDate)
            {
                found = true;
                break;
            }
        }

        Assert.True(found);
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
        var customers = new List<Tuple<string, string>> { Tuple.Create(customerName, "1234567890123456") };

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
        var customers = new List<Tuple<string, string>> { Tuple.Create("Jane Doe", "1234567890123456") };

        // Act
        var result = ReservationManager.CustomersContainName(customers, customerName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ChangeRoomPrice_Should_UpdateRoomPrice()
    {
        // Arrange
        string roomType = "Single";
        decimal newPrice = 150.00m;

        // Act
        DataManager.ChangePrice(roomType, newPrice);

        // Assert
        var rooms = DataManager.ReadRooms();
        bool found = false;
        foreach (var room in rooms)
        {
            if (room.Item2 == roomType)
            {
                found = true;
                Assert.Equal(newPrice, Convert.ToDecimal(room.Item2));
                break;
            }
        }

        Assert.True(found);
    }
}
