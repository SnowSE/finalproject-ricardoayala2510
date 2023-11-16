using System;
using final.logic;

public static class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Add New Room");
            Console.WriteLine("2. Add New Customer");
            Console.WriteLine("3. Add New Reservation");
            Console.WriteLine("4. Available Room Search");
            Console.WriteLine("5. Reservation Report");
            Console.WriteLine("6. Customer Reservation Report");
            Console.WriteLine("7. Change Room Price");
            Console.WriteLine("8. Exit");

            Console.Write("Enter your choice: ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        ProcessAddNewRoom();
                        break;
                    case 2:
                        ProcessAddNewCustomer();
                        break;
                    case 3:
                        ProcessAddNewReservation();
                        break;
                    case 4:
                        ProcessAvailableRoomSearch();
                        break;
                    case 5:
                        ProcessReservationReport();
                        break;
                    case 6:
                        ProcessCustomerReservationReport();
                        break;
                    case 7:
                        ChangeRoomPrice();
                        break;
                    case 8:
                        Console.WriteLine("Exiting the program.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 8.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }

            Console.WriteLine();
        }
    }

    private static void ProcessAddNewRoom()
    {
        Console.Write("Enter the new room number: ");
        if (int.TryParse(Console.ReadLine(), out int newRoomNumber))
        {
            Console.Write("Enter the type of the new room (Single, Double, Suite): ");
            string newRoomType = Console.ReadLine();

            ReservationManager.AddNewRoom(newRoomNumber, newRoomType);
            Console.WriteLine($"Room {newRoomNumber} ({newRoomType}) added successfully.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid room number.");
        }
    }

    private static void ProcessAddNewCustomer()
    {
        Console.Write("Enter the new customer name: ");
        string newCustomerName = Console.ReadLine();

        Console.Write("Enter the new card number: ");
        string newCardNumber = Console.ReadLine();

        ReservationManager.AddNewCustomer(newCustomerName, newCardNumber);
        Console.WriteLine($"Customer {newCustomerName} added successfully.");
    }

    private static void ProcessAddNewReservation()
    {
        Console.Write("Enter the reservation date (MM/DD/YYYY): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime newReservationDate))
        {
            Console.Write("Enter the room number for the reservation: ");
            if (int.TryParse(Console.ReadLine(), out int newReservationRoomNumber))
            {
                Console.Write("Enter the customer name for the reservation: ");
                string newReservationCustomerName = Console.ReadLine();

                try
                {
                    ReservationManager.AddNewReservation(newReservationDate, newReservationRoomNumber, newReservationCustomerName);
                    Console.WriteLine("Reservation added successfully.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid room number.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid date.");
        }
    }

    private static void ProcessAvailableRoomSearch()
    {
        Console.Write("Enter the search date (MM/DD/YYYY): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
        {
            var availableRooms = ReservationManager.AvailableRoomSearch(searchDate);
            Console.WriteLine("Available Rooms:");
            foreach (var roomNumber in availableRooms)
            {
                Console.WriteLine($"Room {roomNumber}");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid date.");
        }
    }

    private static void ProcessReservationReport()
    {
        Console.Write("Enter the report date (MM/DD/YYYY): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime reportDate))
        {
            var reservations = ReservationManager.ReservationReport(reportDate);
            Console.WriteLine("Reservations for the specified date:");
            foreach (var reservation in reservations)
            {
                Console.WriteLine($"Reservation Number: {reservation.Item1}, Room: {reservation.Item3}, Customer: {reservation.Item4}");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid date.");
        }
    }

    private static void ProcessCustomerReservationReport()
    {
        Console.Write("Enter the customer name: ");
        string customerName = Console.ReadLine();

        var customerReservations = ReservationManager.CustomerReservationReport(customerName);
        Console.WriteLine($"Reservations for Customer {customerName}:");

        if (customerReservations.Count > 0)
        {
            foreach (var reservation in customerReservations)
            {
                Console.WriteLine($"Reservation Number: {reservation.Item1}, Room: {reservation.Item3}, Date: {reservation.Item2.ToShortDateString()}");
            }
        }
        else
        {
            Console.WriteLine("No reservations found for the specified customer.");
        }
    }
    private static void ChangeRoomPrice()
    {
        Console.Write("Enter the room type to change the price: ");
        string roomType = Console.ReadLine();

        Console.Write("Enter the new price: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
        {
            ReservationManager.ChangeRoomPrice(roomType, newPrice);
            Console.WriteLine($"Price for room type '{roomType}' changed to {newPrice:C}");
        }
        else
        {
            Console.WriteLine("Invalid price format. Please enter a valid decimal value.");
        }
    }
}
