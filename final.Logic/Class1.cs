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

}
}