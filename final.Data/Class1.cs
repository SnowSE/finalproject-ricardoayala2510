using System;
using System.Collections.Generic;
using System.IO;

namespace final.data
{
    /// A static class representing a data manager with methods to read and write room, customer, and reservation data to files.
    public static class DataManager
    {
        // File paths for storing data
        private static string roomsFilePath = FindFile("Rooms.txt");
        private static string customersFilePath = FindFile("Customers.txt");
        private static string reservationsFilePath = FindFile("Reservations.txt");

        /// Finds a file in the current directory and its parent directories.
        /// <param name="fileName">The name of the file to find.</param>
        /// <returns>The full path to the file.</returns>
        public static string FindFile(string fileName)
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (true)
            {
                var testPath = Path.Combine(directory.FullName, fileName);
                if (File.Exists(testPath))
                {
                    return testPath;
                }

                if (directory.FullName == directory.Root.FullName)
                {
                    throw new FileNotFoundException($"Could not find {fileName} in the current directory or any parent directories.");
                }
                directory = directory.Parent;
            }
        }

        /// Reads room data from the 'Rooms.txt' file and returns a list of room tuples.
        public static List<Tuple<int, string>> ReadRooms()
        {
            List<Tuple<int, string>> roomList = new List<Tuple<int, string>>();

            if (File.Exists(roomsFilePath))
            {
                foreach (string line in File.ReadLines(roomsFilePath))
                {
                    string[] parts = line.Split(',');
                    int roomNumber = int.Parse(parts[0]);
                    string roomType = parts[1];
                    roomList.Add(new Tuple<int, string>(roomNumber, roomType));
                }
            }

            return roomList;
        }

        /// Reads customer data from the 'Customers.txt' file and returns a list of customer tuples.
        public static List<Tuple<string, string>> ReadCustomers()
        {
            List<Tuple<string, string>> customerList = new List<Tuple<string, string>>();

            if (File.Exists(customersFilePath))
            {
                foreach (string line in File.ReadLines(customersFilePath))
                {
                    string[] parts = line.Split(',');

                    if (parts.Length >= 2)
                    {
                        string customerName = parts[0];
                        string cardNumber = parts[1];
                        customerList.Add(new Tuple<string, string>(customerName, cardNumber));
                    }
                    else
                    {
                        Console.WriteLine($"Invalid data format in line: {line}");
                    }
                }
            }

            return customerList;
        }

        /// Reads reservation data from the 'Reservations.txt' file and returns a list of reservation tuples.
        public static List<Tuple<string, DateTime, int, string, string>> ReadReservations()
        {
            List<Tuple<string, DateTime, int, string, string>> reservationList = new List<Tuple<string, DateTime, int, string, string>>();

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
                    reservationList.Add(new Tuple<string, DateTime, int, string, string>(reservationNumber, date, roomNumber, customerName, paymentConfirmation));
                }
            }

            return reservationList;
        }

        /// Writes room data to the 'Rooms.txt' file.
        public static void WriteRooms(List<Tuple<int, string>> rooms)
        {
            using (StreamWriter writer = new StreamWriter(roomsFilePath))
            {
                foreach (var room in rooms)
                {
                    writer.WriteLine($"{room.Item1},{room.Item2}");
                }
            }
        }

        /// Writes customer data to the 'Customers.txt' file.
        public static void WriteCustomers(List<Tuple<string, string>> customers)
        {
            using (StreamWriter writer = new StreamWriter(customersFilePath))
            {
                foreach (var customer in customers)
                {
                    writer.WriteLine($"{customer.Item1},{customer.Item2}");
                }
            }
        }

        /// Writes reservation data to the 'Reservations.txt' file.
        public static void WriteReservations(List<Tuple<string, DateTime, int, string, string>> reservations)
        {
            using (StreamWriter writer = new StreamWriter(reservationsFilePath))
            {
                foreach (var reservation in reservations)
                {
                    writer.WriteLine($"{reservation.Item1},{reservation.Item2},{reservation.Item3},{reservation.Item4},{reservation.Item5}");
                }
            }
        }

        /// Adds a new room with the specified number and type.

        public static void AddNewRoom(int newRoomNumber, string newRoomType)
        {
            List<Tuple<int, string>> rooms = ReadRooms();
            rooms.Add(new Tuple<int, string>(newRoomNumber, newRoomType));
            WriteRooms(rooms);
        }

        /// Adds a new customer with the specified name and card number.
        public static void AddNewCustomer(string newCustomerName, string newCardNumber)
        {
            List<Tuple<string, string>> customers = ReadCustomers();
            customers.Add(new Tuple<string, string>(newCustomerName, newCardNumber));
            WriteCustomers(customers);
        }

        /// Adds a new reservation with specified details.
        public static void AddNewReservation(string reservationNumber, DateTime date, int roomNumber, string customerName, string paymentConfirmation)
        {
            List<Tuple<string, DateTime, int, string, string>> reservations = ReadReservations();
            reservations.Add(new Tuple<string, DateTime, int, string, string>(reservationNumber, date, roomNumber, customerName, paymentConfirmation));
            WriteReservations(reservations);
        }

        /// Saves changes by writing all data to their respective files.
        public static void SaveChanges()
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
public static void ChangePrice(string roomType, decimal newPrice)
{
    // Read existing rooms from the data manager
    List<Tuple<int, string>> rooms = ReadRooms();

    // Flag to check if the room type is found
    bool roomTypeFound = false;

    // Iterate through the rooms
    for (int i = 0; i < rooms.Count; i++)
    {
        // Check if the room type matches
        if (rooms[i].Item2 == roomType)
        {
            // Update the price for the matched room type
            rooms[i] = new Tuple<int, string>(rooms[i].Item1, roomType + "," + newPrice.ToString());
            roomTypeFound = true;
            break;
        }
    }

    // If the room type is not found, add a new entry with the specified price
    if (!roomTypeFound)
    {
        rooms.Add(new Tuple<int, string>(rooms.Count + 1, roomType + "," + newPrice.ToString()));
    }

    // Write the updated rooms list to the data manager
    WriteRooms(rooms);
}

    }
}
