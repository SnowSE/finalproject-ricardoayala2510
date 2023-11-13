using System;
using System.Collections.Generic;
using final.Data;

namespace final.Logic
{
    public class ReservationManager
    {
        private readonly DataManager dataManager;

        public ReservationManager(DataManager dataManager)
        {
            this.dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
        }

        public void AddNewRoom(int newRoomNumber, string newRoomType)
        {
            List<(int roomNumber, string roomType)> rooms = dataManager.ReadRooms();
            rooms.Add((newRoomNumber, newRoomType));
            dataManager.WriteRooms(rooms);
            dataManager.SaveChanges();
        }

        public void AddNewCustomer(string newCustomerName, string newCardNumber)
        {
            List<(string customerName, string cardNumber)> customers = dataManager.ReadCustomers();
            customers.Add((newCustomerName, newCardNumber));
            dataManager.WriteCustomers(customers);
            dataManager.SaveChanges();
        }

        public void AddNewReservation(DateTime newReservationDate, int newReservationRoomNumber, string newReservationCustomerName)
        {
            List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> reservations = dataManager.ReadReservations();
            List<(int roomNumber, string roomType)> rooms = dataManager.ReadRooms();
            List<(string customerName, string cardNumber)> customers = dataManager.ReadCustomers();

            if (IsRoomAvailable(newReservationRoomNumber, newReservationDate, reservations))
            {
                if (customers.Any(c => c.customerName == newReservationCustomerName))
                {
                    string paymentConfirmation = Guid.NewGuid().ToString().Substring(0, 30);
                    string reservationNumber = Guid.NewGuid().ToString();

                    reservations.Add((reservationNumber, newReservationDate, newReservationRoomNumber, newReservationCustomerName, paymentConfirmation));
                    dataManager.WriteReservations(reservations);
                    dataManager.SaveChanges();
                }
                else
                {
                    Console.WriteLine($"Error: Customer '{newReservationCustomerName}' not found.");
                }
            }
            else
            {
                Console.WriteLine($"Error: Room {newReservationRoomNumber} is not available on {newReservationDate.ToShortDateString()}.");
            }
        }

        public List<int> AvailableRoomSearch(DateTime searchDate)
        {
            List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> reservations = dataManager.ReadReservations();
            List<(int roomNumber, string roomType)> rooms = dataManager.ReadRooms();

            List<int> reservedRooms = reservations
                .Where(r => r.date.Date == searchDate.Date)
                .Select(r => r.roomNumber)
                .ToList();

            List<int> availableRooms = rooms
                .Where(r => !reservedRooms.Contains(r.roomNumber))
                .Select(r => r.roomNumber)
                .ToList();

            return availableRooms;
        }

        public List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> ReservationReport(DateTime reportDate)
        {
            List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> reservations = dataManager.ReadReservations();

            var reservationsForDate = reservations
                .Where(r => r.date.Date == reportDate.Date)
                .ToList();

            return reservationsForDate;
        }

        public List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> CustomerReservationReport(string customerName)
        {
            List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> reservations = dataManager.ReadReservations();

            var customerReservations = reservations
                .Where(r => r.customerName == customerName && r.date.Date >= DateTime.Now.Date)
                .ToList();

            return customerReservations;
        }

        public bool IsRoomAvailable(int roomNumber, DateTime date, List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> reservations)
        {
            return !reservations.Any(r => r.roomNumber == roomNumber && r.date.Date == date.Date);
        }
                public void ProcessAddNewRoom()
        {
            while (true)
            {
                Console.Write("Enter the new room number: ");
                if (int.TryParse(Console.ReadLine(), out int newRoomNumber))
                {
                    Console.Write("Enter the type of the new room (Single, Double, Suite): ");
                    string newRoomType = Console.ReadLine();

                    AddNewRoom(newRoomNumber, newRoomType);
                    Console.WriteLine($"Room {newRoomNumber} ({newRoomType}) added successfully.");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid room number.");
                }
            }
        }

        public void ProcessAddNewCustomer()
        {
            while (true)
            {
                Console.Write("Enter the new customer's name: ");
                string newCustomerName = Console.ReadLine();

                Console.Write("Enter the new customer's card number: ");
                string newCardNumber = Console.ReadLine();

                AddNewCustomer(newCustomerName, newCardNumber);
                Console.WriteLine($"Customer {newCustomerName} added successfully.");
                break;
            }
        }

        public void ProcessAddNewReservation()
        {
            while (true)
            {
                Console.Write("Enter the date of the reservation (MM/DD/YYYY): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime newReservationDate))
                {
                    Console.Write("Enter the room number for the reservation: ");
                    if (int.TryParse(Console.ReadLine(), out int newReservationRoomNumber))
                    {
                        Console.Write("Enter the customer's name for the reservation: ");
                        string newReservationCustomerName = Console.ReadLine();

                        AddNewReservation(newReservationDate, newReservationRoomNumber, newReservationCustomerName);
                        break;
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
        }

        public void ProcessAvailableRoomSearch()
        {
            while (true)
            {
                Console.Write("Enter the date to search for available rooms (MM/DD/YYYY): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                {
                    var availableRooms = AvailableRoomSearch(searchDate);
                    Console.WriteLine($"Available rooms on {searchDate.ToShortDateString()}: {string.Join(", ", availableRooms)}");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid date.");
                }
            }
        }

        public void ProcessReservationReport()
        {
            while (true)
            {
                Console.Write("Enter the date for the reservation report (MM/DD/YYYY): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime reportDate))
                {
                    var reservationsForDate = ReservationReport(reportDate);
                    Console.WriteLine($"Reservations on {reportDate.ToShortDateString()}:");
                    PrintReservations(reservationsForDate);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid date.");
                }
            }
        }

        public void ProcessCustomerReservationReport()
        {
            while (true)
            {
                Console.Write("Enter the customer's name for the reservation report: ");
                string customerName = Console.ReadLine();

                var customerReservations = CustomerReservationReport(customerName);
                Console.WriteLine($"Future Reservations for {customerName}:");
                PrintReservations(customerReservations);
                break;
            }
        }

        public void PrintReservations(List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> reservations)
        {
            foreach (var reservation in reservations)
            {
                Console.WriteLine($"Reservation Number: {reservation.reservationNumber}, Room: {reservation.roomNumber}, Customer: {reservation.customerName}");
            }
    }

}
}