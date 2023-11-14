using System;
using final.Data;
using final.Logic;
using System.Collections.Generic;
    public class Program
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
                        ProcessAddNewRoom(reservationManager);
                        break;

                    case "2":
                        ProcessAddNewCustomer(reservationManager);
                        break;

                    case "3":
                        ProcessAddNewReservation(reservationManager);
                        break;

                    case "4":
                        ProcessAvailableRoomSearch(reservationManager);
                        break;

                    case "5":
                        ProcessReservationReport(reservationManager);
                        break;

                    case "6":
                        ProcessCustomerReservationReport(reservationManager);
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

        static void ProcessAddNewRoom(ReservationManager reservationManager)
        {
            Console.Write("Enter the new room number: ");
            if (int.TryParse(Console.ReadLine(), out int newRoomNumber))
            {
                Console.Write("Enter the type of the new room (Single, Double, Suite): ");
                string newRoomType = Console.ReadLine();

                reservationManager.AddNewRoom(newRoomNumber, newRoomType);
                Console.WriteLine($"Room {newRoomNumber} ({newRoomType}) added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid room number.");
            }
        }

        static void ProcessAddNewCustomer(ReservationManager reservationManager)
        {
            Console.Write("Enter the new customer's name: ");
            string newCustomerName = Console.ReadLine();

            Console.Write("Enter the new customer's card number: ");
            string newCardNumber = Console.ReadLine();

            reservationManager.AddNewCustomer(newCustomerName, newCardNumber);
            Console.WriteLine($"Customer {newCustomerName} added successfully.");
        }

        static void ProcessAddNewReservation(ReservationManager reservationManager)
        {
            Console.Write("Enter the date of the reservation (MM/DD/YYYY): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime newReservationDate))
            {
                Console.Write("Enter the room number for the reservation: ");
                if (int.TryParse(Console.ReadLine(), out int newReservationRoomNumber))
                {
                    Console.Write("Enter the customer's name for the reservation: ");
                    string newReservationCustomerName = Console.ReadLine();

                    reservationManager.AddNewReservation(newReservationDate, newReservationRoomNumber, newReservationCustomerName);
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

        static void ProcessAvailableRoomSearch(ReservationManager reservationManager)
        {
            Console.Write("Enter the date to search for available rooms (MM/DD/YYYY): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
            {
                var availableRooms = reservationManager.AvailableRoomSearch(searchDate);
                Console.WriteLine($"Available rooms on {searchDate.ToShortDateString()}: {string.Join(", ", availableRooms)}");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid date.");
            }
        }

        static void ProcessReservationReport(ReservationManager reservationManager)
        {
            Console.Write("Enter the date for the reservation report (MM/DD/YYYY): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime reportDate))
            {
                var reservationsForDate = reservationManager.ReservationReport(reportDate);
                Console.WriteLine($"Reservations on {reportDate.ToShortDateString()}:");
                PrintReservations(reservationsForDate);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid date.");
            }
        }

        static void ProcessCustomerReservationReport(ReservationManager reservationManager)
        {
            Console.Write("Enter the customer's name for the reservation report: ");
            string customerName = Console.ReadLine();

            var customerReservations = reservationManager.CustomerReservationReport(customerName);
            Console.WriteLine($"Future Reservations for {customerName}:");
            PrintReservations(customerReservations);
        }

        static void PrintReservations(List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> reservations)
        {
            foreach (var reservation in reservations)
            {
                Console.WriteLine($"Reservation Number: {reservation.reservationNumber}, Room: {reservation.roomNumber}, Customer: {reservation.customerName}");
            }
        }
        
    }
