using System;
using System.Collections.Generic;
using final.Data;
using final.Logic;
using Xunit;

public class ReservationManagerTests
{
    [Fact]
    public void AddNewRoom_Should_AddRoomToList()
    {
        // Arrange
        var dataManager = new DataManager();
        var reservationManager = new ReservationManager(dataManager);

        // Act
        reservationManager.AddNewRoom(201, "Single");

        // Assert
        var rooms = dataManager.ReadRooms();
        Assert.Contains((201, "Single"), rooms);
    }

    [Fact]
    public void AddNewCustomer_Should_AddCustomerToList()
    {
        // Arrange
        var dataManager = new DataManager();
        var reservationManager = new ReservationManager(dataManager);

        // Act
        reservationManager.AddNewCustomer("John Doe", "1234567890123456");

        // Assert
        var customers = dataManager.ReadCustomers();
        Assert.Contains(("John Doe", "1234567890123456"), customers);
    }

    [Fact]
    public void AddNewReservation_Should_AddReservationToList()
    {
        // Arrange
        var dataManager = new DataManager();
        var reservationManager = new ReservationManager(dataManager);
        var reservationDate = DateTime.Now.Date;
        var roomNumber = 101;
        var customerName = "John Doe";

        // Act
        reservationManager.AddNewReservation(reservationDate, roomNumber, customerName);

        // Assert
        var reservations = dataManager.ReadReservations();
        Assert.Contains(reservations, r => r.roomNumber == roomNumber && r.customerName == customerName && r.date.Date == reservationDate);
    }

    [Fact]
    public void AvailableRoomSearch_Should_ReturnAvailableRooms()
    {
        // Arrange
        var dataManager = new DataManager();
        var reservationManager = new ReservationManager(dataManager);

        // Act
        var availableRooms = reservationManager.AvailableRoomSearch(DateTime.Now.Date);

        // Assert
        Assert.NotNull(availableRooms);
    }

    [Fact]
    public void ReservationReport_Should_ReturnReservationsForDate()
    {
        // Arrange
        var dataManager = new DataManager();
        var reservationManager = new ReservationManager(dataManager);
        var date = DateTime.Now.Date;

        // Act
        var reservations = reservationManager.ReservationReport(date);

        // Assert
        Assert.NotNull(reservations);
    }

    [Fact]
    public void CustomerReservationReport_Should_ReturnCustomerReservations()
    {
        // Arrange
        var dataManager = new DataManager();
        var reservationManager = new ReservationManager(dataManager);
        var customerName = "John Doe";

        // Act
        var customerReservations = reservationManager.CustomerReservationReport(customerName);

        // Assert
        Assert.NotNull(customerReservations);
    }

}