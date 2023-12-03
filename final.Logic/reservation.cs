using System;
using System.Collections.Generic;
using final.data;
using System.Text;

namespace final.logic
{

    /// A static class representing a reservation manager with various methods for room, customer, and reservation management.
    public static class ReservationManager
    {

        static List<(int number, string roomType)> rooms = new();
        static List<(string newCustomerName, string newCardNumber)> customers = new();
        static List<Tuple<string, DateTime, int, string, string>> reservations = new();
        static List<(string, decimal)> prices = new();
        public static void LoadData()
        {
            rooms = DataManager.ReadRooms();
            customers = DataManager.ReadCustomers();
            reservations = DataManager.ReadReservations();
            prices = DataManager.ReadRoomPrices();
        }

        public static void SaveData()
        {
            DataManager.WriteRooms(rooms);
            DataManager.WriteCustomers(customers);
            DataManager.WriteReservations(reservations);
            DataManager.WriteRoomPrices(prices);
        }
        public static void ClearData()
        {
            rooms.Clear();
            customers.Clear();
            reservations.Clear();
        }

        public static void AddNewRoom(int newRoomNumber, string newRoomType)
        {
            // Add the new room to the list
            rooms.Add((newRoomNumber, newRoomType));
        }
        public static bool RoomNumberExists(int roomNumber)
        {
            // Check if the room number already exists in the list of rooms
            foreach (var room in rooms)
            {
                if (room.number == roomNumber)
                {
                    return true;
                }
            }
            return false;
        }

        /// Adds a new customer with the specified name and card number.

        public static void AddNewCustomer(string newCustomerName, string newCardNumber)
        {
            // Add the new customer to the list
            customers.Add((newCustomerName, newCardNumber));

        }


        /// Adds a new reservation with the specified date, room number, and customer name.
        public static void AddNewReservation(DateTime checkInDate, DateTime checkOutDate, int newReservationRoomNumber, string newReservationCustomerName)
        {
            // Check if the room is available for reservation for all days
            for (DateTime date = checkInDate; date <= checkOutDate; date = date.AddDays(1))
            {
                if (!IsRoomAvailable(newReservationRoomNumber, date, reservations))
                {
                    // Throw an exception if the room is not available for any day
                    throw new InvalidOperationException($"Error: Room {newReservationRoomNumber} is not available on {date.ToShortDateString()}.");
                }
            }

            // Check if the customer exists
            if (CustomersContainName(customers, newReservationCustomerName))
            {
                // Generate payment confirmation and reservation number
                string paymentConfirmation = Guid.NewGuid().ToString().Substring(0, 30);
                string reservationNumber = Guid.NewGuid().ToString();

                // Add the new reservation for all days to the list
                for (DateTime date = checkInDate; date <= checkOutDate; date = date.AddDays(1))
                {
                    reservations.Add(new Tuple<string, DateTime, int, string, string>(
                        reservationNumber, date, newReservationRoomNumber, newReservationCustomerName, paymentConfirmation));
                }

                // Write the updated reservations list to the data manager
                DataManager.WriteReservations(reservations);
            }
            else
            {
                // Throw an exception if the customer is not found
                throw new InvalidOperationException($"Error: Customer '{newReservationCustomerName}' not found.");
            }
        }


        public static decimal CalculateDiscountedPrice(decimal originalPrice, string customerName)
        {
            const int DiscountThreshold = 5;
            const decimal DiscountPercentage = 0.1m; // 10% discount

            // Count the number of reservations for the customer
            int customerReservationsCount = reservations.Count(reservation => reservation.Item4 == customerName);

            // Check if the customer qualifies for a discount
            if (customerReservationsCount >= DiscountThreshold)
            {
                // Apply the discount
                return originalPrice * (1 - DiscountPercentage);
            }

            // No discount
            return originalPrice;
        }


        /// Searches for available rooms on a specific date.
        public static List<int> AvailableRoomSearch(DateTime searchDate)
        {


            // List to store reserved room numbers
            List<int> reservedRooms = new List<int>();

            // Iterate through reservations to find reserved rooms on the specified date
            foreach (var reservation in reservations)
            {
                if (reservation.Item2.Date == searchDate.Date)
                {
                    reservedRooms.Add(reservation.Item3);
                }
            }

            // List to store available room numbers
            List<int> availableRooms = new List<int>();

            // Iterate through rooms to find available rooms
            foreach (var room in rooms)
            {
                if (!reservedRooms.Contains(room.Item1))
                {
                    availableRooms.Add(room.Item1);
                }
            }

            return availableRooms;
        }


        /// Generates a report of reservations for a specific date.

        public static List<Tuple<string, DateTime, int, string, string>> ReservationReport(DateTime reportDate)
        {
            // Read existing reservations from the data manager
            List<Tuple<string, DateTime, int, string, string>> reservationsForDate = new List<Tuple<string, DateTime, int, string, string>>();

            // Iterate through reservations to find reservations for the specified date
            foreach (var reservation in reservations)
            {
                if (reservation.Item2.Date == reportDate.Date)
                {
                    reservationsForDate.Add(reservation);
                }
            }

            return reservationsForDate;
        }


        /// Generates a report of reservations for a specific customer.
        public static List<Tuple<string, DateTime, int, string, string>> CustomerReservationReport(string customerName)
        {
            // Read existing reservations from the data manager
            // List to store reservations for the specified customer
            List<Tuple<string, DateTime, int, string, string>> customerReservations = new List<Tuple<string, DateTime, int, string, string>>();

            // Iterate through reservations to find reservations for the specified customer
            foreach (var reservation in reservations)
            {
                if (reservation.Item4 == customerName && reservation.Item2.Date >= DateTime.Now.Date)
                {
                    customerReservations.Add(reservation);
                }
            }

            return customerReservations;
        }


        /// Checks if a room is available on a specific date.

        public static bool IsRoomAvailable(int roomNumber, DateTime date, List<Tuple<string, DateTime, int, string, string>> reservations)
        {
            // Iterate through reservations to check if the room is available on the specified date
            foreach (var reservation in reservations)
            {
                if (reservation.Item3 == roomNumber && reservation.Item2.Date == date.Date)
                {
                    return false;
                }
            }
            return true;
        }

        /// Checks if the list of customers contains a customer with the specified name.
        public static bool CustomersContainName(List<(string, string)> customers, string name)
        {
            // Iterate through customers to check if a customer with the specified name exists
            foreach (var customer in customers)
            {
                if (customer.Item1 == name)
                {
                    return true;
                }
            }

            return false;
        }
        public static void ChangeRoomPrice(string roomType, decimal newPrice)
        {
            // Check if the room type exists in the prices list
            int index = -1;
            for (int i = 0; i < prices.Count; i++)
            {
                if (prices[i].Item1 == roomType)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                // Update the room price
                prices[index] = (roomType, newPrice);
            }
            else
            {
                // Throw an exception if the room type is not found
                throw new InvalidOperationException($"Error: Room type {roomType} not found.");
            }
        }

        public static decimal GetRoomPrice(string roomType)
        {
            // Check if the room number exists in the prices list
            int index = -1;
            for (int i = 0; i < prices.Count; i++)
            {
                if (prices[i].Item1 == roomType)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                // Return the room price
                return prices[index].Item2;
            }
            else
            {
                // Throw an exception if the room type is not found
                throw new InvalidOperationException($"Error: Room {roomType} not found.");
            }
        }
        public static void RefundReservation(string reservationNumber)
        {
            // Find the reservation with the specified reservation number
            Tuple<string, DateTime, int, string, string> reservationToRemove = null;

            foreach (var reservation in reservations)
            {
                if (reservation.Item1 == reservationNumber)
                {
                    reservationToRemove = reservation;
                    break;
                }
            }

            if (reservationToRemove != null)
            {
                // Remove the reservation from the list
                reservations.Remove(reservationToRemove);

                // Add the refund to the refunds file
                DataManager.WriteRefunds(reservationToRemove);
            }
            else
            {
                // Throw an exception if the reservation is not found
                throw new InvalidOperationException($"Error: Reservation {reservationNumber} not found.");
            }
        }

        public static decimal CalculateUtilizationRate(DateTime reportDate)
        {
            // Get the total number of rooms
            int totalRooms = rooms.Count;

            // Get the number of reserved rooms for the specified date
            int reservedRooms = 0;

            foreach (var reservation in reservations)
            {
                if (reservation.Item2.Date == reportDate.Date)
                {
                    reservedRooms++;
                }
            }

            // Calculate the utilization rate
            decimal utilizationRate = (decimal)reservedRooms / totalRooms * 100;

            return utilizationRate;
        }

        public static List<decimal> CalculateUtilizationRateRange(DateTime startDate, DateTime endDate)
        {
            List<decimal> utilizationRates = new List<decimal>();

            // Get the total number of rooms
            int totalRooms = rooms.Count;

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Get the number of reserved rooms for each day in the range
                int reservedRooms = 0;

                foreach (var reservation in reservations)
                {
                    if (reservation.Item2.Date == date.Date)
                    {
                        reservedRooms++;
                    }
                }

                // Calculate the utilization rate for each day
                decimal utilizationRate = (decimal)reservedRooms / totalRooms * 100;

                utilizationRates.Add(utilizationRate);
            }

            return utilizationRates;
        }
        public static string GetRoomText()
        {
            string roomDetailsText = "";

            foreach (var room in rooms)
            {
                roomDetailsText += $"{room.number},{room.roomType}\n";
            }

            return roomDetailsText;
        }


    }

}

