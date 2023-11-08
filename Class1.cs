namespace RickLogic;

public class Class1
{
    enum RoomType
    {
        Single,
        Double,
        Suite
    }
        // Get room number from the user
    static int GetRoomNumber()
    {
        Console.Write("Enter room number: ");
        if (int.TryParse(Console.ReadLine(), out int roomNumber))
        {
            return roomNumber;
        }
        else
        {
            Console.WriteLine("Invalid room number. Please try again.");
            return GetRoomNumber();
        }
    }

    // Get room type from the user
    static RoomType GetRoomType()
    {
        Console.WriteLine("Select room type:");
        Console.WriteLine("1. Single");
        Console.WriteLine("2. Double");
        Console.WriteLine("3. Suite");

        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 3)
        {
            return (RoomType)(choice - 1);
        }
        else
        {
            Console.WriteLine("Invalid choice. Please try again.");
            return GetRoomType();
        }
    }

    // Make a reservation
    static void MakeReservation(List<(Guid reservationNumber, DateOnly date, int roomNumber, string customerName, string paymentConfirmation)> reservations, List<(int roomNumber, RoomType roomType)> rooms)
    {
        Console.Write("Enter the customer's name: ");
        string customerName = Console.ReadLine();

        Console.WriteLine("Choose a room from the available rooms:");
        foreach (var room in rooms)
        {
            Console.WriteLine($"{room.roomNumber} ({room.roomType})");
        }

        if (int.TryParse(Console.ReadLine(), out int selectedRoomNumber))
        {
            var room = rooms.Find(r => r.roomNumber == selectedRoomNumber);
            if (room.Equals(default((int roomNumber, RoomType roomType))))
            {
                Console.WriteLine("Room not found. Reservation failed.");
            }
            else
            {
                Guid newReservationNumber = Guid.NewGuid();
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                string paymentConfirmation = GenerateRandomString(30);
                reservations.Add((newReservationNumber, currentDate, selectedRoomNumber, customerName, paymentConfirmation));
                Console.WriteLine("Reservation successfully made!");
            }
        }
        else
        {
            Console.WriteLine("Invalid room number. Reservation failed.");
        }
    }
     // Generate a random string
    static string GenerateRandomString(int length)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    // Add a new customer
    static (string name, string cardNumber) AddCustomer()
    {
        Console.WriteLine("Enter customer information:");

        Console.Write("Name: ");
        string name = Console.ReadLine();

        Console.Write("Card Number: ");
        string cardNumber = Console.ReadLine();

        return (name, cardNumber);
    }
    // Update room prices
    static void UpdateRoomPrices(List<(RoomType roomType, decimal dailyRate)> roomPrices)
    {
        Console.WriteLine("Choose a room type to update the price:");
        foreach (RoomType type in Enum.GetValues(typeof(RoomType)))
        {
            Console.WriteLine($"{(int)type}: {type}");
        }

        if (Enum.TryParse(Console.ReadLine(), out RoomType selectedType))
        {
            var roomPrice = roomPrices.Find(price => price.roomType == selectedType);
            if (roomPrice.Equals(default((RoomType roomType, decimal dailyRate))))
            {
                Console.WriteLine("Room type not found.");
            }
            else
            {
                Console.Write("Enter new daily rate: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal newRate))
                {
                    roomPrices[roomPrices.FindIndex(price => price.roomType == selectedType)] = (selectedType, newRate);
                    Console.WriteLine("Room price updated successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid daily rate. Price update failed.");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid room type. Price update failed.");
        }
    }
}
