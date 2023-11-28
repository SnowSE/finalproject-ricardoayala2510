using System;
using final.logic;

public static class Program
{

    public static void Main()
    {
        ReservationManager.LoadData();
        Console.Clear();
        while (true)
        {
            Console.WriteLine("1. Add New Room");
            Console.WriteLine("2. Add New Customer");
            Console.WriteLine("3. Add New Reservation");
            Console.WriteLine("4. Available Room Search");
            Console.WriteLine("5. Reservation Report");
            Console.WriteLine("6. Customer Reservation Report");
            Console.WriteLine("7. Change Room price");
            Console.WriteLine("8. Refund Reservation");
            Console.WriteLine("9. See Room Types and Prices");
            Console.WriteLine("10. Utilization report for a day");
            Console.WriteLine("11. Utilization report for a date range");
            Console.WriteLine("12. Exit");

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
                        ProcessChangeRoomPrice();
                        break;
                    case 8:
                        ProcessRefundReservation();
                        break;
                    case 9:
                        PrintRoomPrices();
                        break;
                    case 10:
                        GenerateUtilizationReport();
                        break;
                    case 11:
                        GenerateUtilizationReportRange();
                        break;

                    case 12:
                        ReservationManager.SaveData();
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
        Console.Write("Enter the check-in date (MM/DD/YYYY): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime checkInDate))
        {
            Console.Write("Enter the check-out date (MM/DD/YYYY): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime checkOutDate))
            {
                Console.Write("Enter the room number for the reservation: ");
                if (int.TryParse(Console.ReadLine(), out int newReservationRoomNumber))
                {
                    Console.Write("Enter the customer name for the reservation: ");
                    string newReservationCustomerName = Console.ReadLine();

                    try
                    {
                        ReservationManager.AddNewReservation(checkInDate, checkOutDate, newReservationRoomNumber, newReservationCustomerName);

                        // Fetch room type and price from RoomPrices.txt
                        Console.Write("Enter the room type (Single, Double, Suite): ");
                        string roomType = Console.ReadLine();

                        decimal roomPrice = ReservationManager.GetRoomPrice(roomType);

                        // Calculate the discounted price
                        decimal discountedPrice = ReservationManager.CalculateDiscountedPrice(roomPrice, newReservationCustomerName);

                        Console.WriteLine($"Reservation added successfully. Total Price: {discountedPrice:C}");
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
                Console.WriteLine("Invalid input. Please enter a valid check-out date.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid check-in date.");
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
    private static void ProcessChangeRoomPrice()
    {
        Console.Write("Enter the room type to change the price (Single,Double,Suite): ");
        string roomType = Console.ReadLine();

        Console.Write("Enter the new room price: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
        {
            try
            {
                ReservationManager.ChangeRoomPrice(roomType, newPrice);
                Console.WriteLine($"Room {roomType} price changed to {newPrice:C} successfully.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid price.");
        }
    }
    private static void ProcessRefundReservation()
    {
        Console.Write("Enter the reservation number to refund: ");
        string reservationNumber = Console.ReadLine();

        try
        {
            ReservationManager.RefundReservation(reservationNumber);
            Console.WriteLine($"Reservation {reservationNumber} refunded successfully.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void PrintRoomPrices()
    {

        string roomPricesText = ReservationManager.GetRoomPricesText();
        Console.WriteLine(roomPricesText);

    }
    private static void GenerateUtilizationReport()
    {
        Console.Write("Enter the date for the Utilization Report (MM/DD/YYYY): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime reportDate))
        {
            decimal utilizationRate = ReservationManager.CalculateUtilizationRate(reportDate);
            Console.WriteLine($"Utilization Rate for {reportDate.ToShortDateString()}: {utilizationRate:F2}%");
        }
        else
        {
            Console.WriteLine("Invalid date format. Please enter a valid date.");
        }
    }

    private static void GenerateUtilizationReportRange()
    {
        Console.Write("Enter the start date for the Utilization Report (MM/DD/YYYY): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
        {
            Console.Write("Enter the end date for the Utilization Report (MM/DD/YYYY): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
            {
                List<decimal> utilizationRates = ReservationManager.CalculateUtilizationRateRange(startDate, endDate);

                Console.WriteLine($"Utilization Report for the Date Range ({startDate.ToShortDateString()} to {endDate.ToShortDateString()}):");
                for (int i = 0; i < utilizationRates.Count; i++)
                {
                    Console.WriteLine($"{startDate.AddDays(i).ToShortDateString()}: {utilizationRates[i]:F2}%");
                }
            }
            else
            {
                Console.WriteLine("Invalid end date format. Please enter a valid date.");
            }
        }
        else
        {
            Console.WriteLine("Invalid start date format. Please enter a valid date.");
        }
    }

}
