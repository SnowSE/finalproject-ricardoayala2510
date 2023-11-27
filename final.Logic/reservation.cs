using System;
using System.Collections.Generic;
using final.data;

namespace final.logic
{

    /// A static class representing a reservation manager with various methods for room, customer, and reservation management.
    public static class ReservationManager
    {


        static List<(int number, string roomType)> rooms = new();
        static List<(string newRoomNumber, string newRoomType)> customers = new();
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


        /// Adds a new customer with the specified name and card number.

        public static void AddNewCustomer(string newCustomerName, string newCardNumber)
        {
            // Add the new customer to the list
            customers.Add((newCustomerName, newCardNumber));

        }


        /// Adds a new reservation with the specified date, room number, and customer name.

        public static void AddNewReservation(DateTime newReservationDate, int newReservationRoomNumber, string newReservationCustomerName)
        {
            // Read existing reservations, rooms, and customers from the data manager

            // Check if the room is available for reservation
            if (IsRoomAvailable(newReservationRoomNumber, newReservationDate, reservations))
            {
                // Check if the customer exists
                if (CustomersContainName(customers, newReservationCustomerName))
                {
                    // Generate payment confirmation and reservation number
                    string paymentConfirmation = Guid.NewGuid().ToString().Substring(0, 30);
                    string reservationNumber = Guid.NewGuid().ToString();

                    // Add the new reservation to the list
                    reservations.Add(new Tuple<string, DateTime, int, string, string>(
                        reservationNumber, newReservationDate, newReservationRoomNumber, newReservationCustomerName, paymentConfirmation));

                    // Write the updated reservations list to the data manager

                }
                else
                {
                    // Throw an exception if the customer is not found
                    throw new InvalidOperationException($"Error: Customer '{newReservationCustomerName}' not found.");
                }
            }
            else
            {
                // Throw an exception if the room is not available
                throw new InvalidOperationException($"Error: Room {newReservationRoomNumber} is not available on {newReservationDate.ToShortDateString()}.");
            }
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
        // Update the room price
        prices[index] = (roomType, newPrice);
    }
    else
    {
        // Throw an exception if the room number is not found
        throw new InvalidOperationException($"Error: Room {roomType} not found.");
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
        // Throw an exception if the room number is not found
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
            }
            else
            {
                // Throw an exception if the reservation is not found
                throw new InvalidOperationException($"Error: Reservation {reservationNumber} not found.");
            }
        }

    }

}

