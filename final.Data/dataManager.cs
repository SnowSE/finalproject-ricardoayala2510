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
        private static string roomPricesFilePath = FindFile("RoomPrices.txt");
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
        public static List<(int, string)> ReadRooms()
        {
            List<(int, string)> roomList = new();

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

        /// Reads customer data from the 'Customers.txt' file and returns a list of customer tuples.
        public static List<(string, string)> ReadCustomers()
        {
            List<(string, string)> customerList = new ();

            if (File.Exists(customersFilePath))
            {
                foreach (string line in File.ReadLines(customersFilePath))
                {
                    string[] parts = line.Split(',');

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
        public static void WriteRooms(List<(int, string)> rooms)
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
        public static void WriteCustomers(List<(string, string)> customers)
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
public static List<(string, decimal)> ReadRoomPrices()
{
    List<(string, decimal)> roomPricesList = new List<(string, decimal)>();

    if (File.Exists(roomPricesFilePath))
    {
        foreach (string line in File.ReadLines(roomPricesFilePath))
        {
            string[] parts = line.Split(',');

            if (parts.Length >= 2 && decimal.TryParse(parts[1], out decimal price))
            {
                roomPricesList.Add((parts[0], price));
            }
            else
            {
                Console.WriteLine($"Invalid data format in line: {line}");
            }
        }
    }

    return roomPricesList;
}

public static void WriteRoomPrices(List<(string, decimal)> roomPrices)
{
    using (StreamWriter writer = new StreamWriter(roomPricesFilePath))
    {
        foreach (var roomPrice in roomPrices)
        {
            writer.WriteLine($"{roomPrice.Item1},{roomPrice.Item2}");
        }
    }
}
public static void WriteRefunds(List<Tuple<string, DateTime, int, string, string>> refunds, List<Tuple<string, DateTime, int, string, string>> reservations)
{
    using (StreamWriter writer = new StreamWriter(reservationsFilePath))
    {
        foreach (var reservation in reservations)
        {
            // Skip the refunded reservation while writing to the file
            if (!refunds.Contains(reservation))
            {
                writer.WriteLine($"{reservation.Item1},{reservation.Item2},{reservation.Item3},{reservation.Item4},{reservation.Item5}");
            }
        }
    }
}


    }
}
