using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace final.Data
{
    public class DataManager
    {
        private string roomsFilePath = "Rooms.txt";
        private string customersFilePath = "Customers.txt";
        private string reservationsFilePath = "Reservations.txt";

        public List<(int roomNumber, string roomType)> ReadRooms()
        {
            List<(int roomNumber, string roomType)> roomList = new List<(int, string)>();

            if (File.Exists(roomsFilePath))
            {
                foreach (string line in File.ReadLines(roomsFilePath))
                {
                    string[] parts = line.Split(',');
                    int roomNumber = int.Parse(parts[0]);
                    string roomType = parts[1];
                    roomList.Add((roomNumber, roomType));
                }
            }

            return roomList;
        }
public List<(string customerName, string cardNumber)> ReadCustomers()
{
    List<(string customerName, string cardNumber)> customerList = new List<(string, string)>();

    if (File.Exists(customersFilePath))
    {
        foreach (string line in File.ReadLines(customersFilePath))
        {
            string[] parts = line.Split(',');

            // Check if the array has at least two elements before accessing them
            if (parts.Length >= 2)
            {
                string customerName = parts[0];
                string cardNumber = parts[1];
                customerList.Add((customerName, cardNumber));
            }
            else
            {
                Console.WriteLine($"Invalid data format in line: {line}");
            }
        }
    }

    return customerList;
}


        public List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> ReadReservations()
        {
            List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> reservationList = new List<(string, DateTime, int, string, string)>();

            if (File.Exists(reservationsFilePath))
            {
                foreach (string line in File.ReadLines(reservationsFilePath))
                {
                    string[] parts = line.Split(',');
                    string reservationNumber = parts[0];
                    DateTime date = DateTime.Parse(parts[1]);
                    int roomNumber = int.Parse(parts[2]);
                    string customerName = parts[3];
                    string paymentConfirmation = parts[4];
                    reservationList.Add((reservationNumber, date, roomNumber, customerName, paymentConfirmation));
                }
            }

            return reservationList;
        }

        public void WriteRooms(List<(int roomNumber, string roomType)> rooms)
        {
            using (StreamWriter writer = new StreamWriter(roomsFilePath))
            {
                foreach ((int roomNumber, string roomType) in rooms)
                {
                    writer.WriteLine($"{roomNumber},{roomType}");
                }
            }
        }

        public void WriteCustomers(List<(string customerName, string cardNumber)> customers)
        {
            using (StreamWriter writer = new StreamWriter(customersFilePath))
            {
                foreach ((string customerName, string cardNumber) in customers)
                {
                    writer.WriteLine($"{customerName},{cardNumber}");
                }
            }
        }

        public void WriteReservations(List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> reservations)
        {
            using (StreamWriter writer = new StreamWriter(reservationsFilePath))
            {
                foreach ((string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation) in reservations)
                {
                    writer.WriteLine($"{reservationNumber},{date},{roomNumber},{customerName},{paymentConfirmation}");
                }
            }
        }
                public void AddNewRoom(int newRoomNumber, string newRoomType)
        {
            List<(int roomNumber, string roomType)> rooms = ReadRooms();
            rooms.Add((newRoomNumber, newRoomType));
            WriteRooms(rooms);
        }

        public void AddNewCustomer(string newCustomerName, string newCardNumber)
        {
            List<(string customerName, string cardNumber)> customers = ReadCustomers();
            customers.Add((newCustomerName, newCardNumber));
            WriteCustomers(customers);
        }

        public void AddNewReservation(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)
        {
            List<(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)> reservations = ReadReservations();
            reservations.Add((reservationNumber, date, roomNumber, customerName, paymentConfirmation));
            WriteReservations(reservations);
        }
                public void SaveChanges()
        {
            try
            {
                WriteRooms(ReadRooms());
                WriteCustomers(ReadCustomers());
                WriteReservations(ReadReservations());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
            }
        }
    }
}
