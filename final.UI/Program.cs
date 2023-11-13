using System;
using final.Data;
using final.Logic;
using System.Collections.Generic;
class Program
{
    static void Main()
    {
        var dataManager = new DataManager();
        var reservationManager = new ReservationManager(dataManager);

        while (true)
        {
            Console.WriteLine("1. Add New Room");
            Console.WriteLine("2. Add New Customer");
            Console.WriteLine("3. Add New Reservation");
            Console.WriteLine("4. Available Room Search");
            Console.WriteLine("5. Reservation Report");
            Console.WriteLine("6. Customer Reservation Report");
            Console.WriteLine("7. Exit");

            Console.Write("Enter your choice (1-7): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    reservationManager.ProcessAddNewRoom();
                    break;

                case "2":
                    reservationManager.ProcessAddNewCustomer();
                    break;

                case "3":
                    reservationManager.ProcessAddNewReservation();
                    break;

                case "4":
                    reservationManager.ProcessAvailableRoomSearch();
                    break;

                case "5":
                    reservationManager.ProcessReservationReport();
                    break;

                case "6":
                    reservationManager.ProcessCustomerReservationReport();
                    break;

                case "7":
                    dataManager.SaveChanges();
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 7.");
                    break;
            }

            Console.WriteLine();
        }
    }
}
