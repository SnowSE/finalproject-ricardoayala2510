class Program
{

    enum RoomType
    {
        Single,
        Double,
        Suite
    }
    static void Main()
    {
        // Read data from files
        List<(int roomNumber, RoomType roomType)> rooms = ReadRooms("Rooms.txt");
        List<(Guid reservationNumber, DateOnly date, int roomNumber, string customerName, string paymentConfirmation)> reservations = ReadReservations("Reservations.txt");
        List<(string name, string cardNumber)> customers = ReadCustomers("Customers.txt");
        List<(RoomType roomType, decimal dailyRate)> roomPrices = ReadRoomPrices("RoomPrices.txt");

        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Hotel Management System");
            Console.WriteLine("1. Add a new room");
            Console.WriteLine("2. Make a reservation");
            Console.WriteLine("3. Add a new customer");
            Console.WriteLine("4. Update room prices");
            Console.WriteLine("5. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    rooms.Add((GetRoomNumber(), GetRoomType()));
                    Console.WriteLine("Room added successfully!");
                    break;
                case "2":
                    MakeReservation(reservations, rooms);
                    break;

                case "3":
                    customers.Add(AddCustomer());
                    Console.WriteLine("Customer added successfully!");
                    break;
                case "4":
                    UpdateRoomPrices(roomPrices);
                    Console.WriteLine("Room prices updated successfully!");
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Write lists back to original files
        WriteRooms("Rooms.txt", rooms);
        WriteReservations("Reservations.txt", reservations);
        WriteCustomers("Customers.txt", customers);
        WriteRoomPrices("RoomPrices.txt", roomPrices);
    }

    // ... (rest of the code)

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

    // Read Rooms.txt
    static List<(int roomNumber, RoomType roomType)> ReadRooms(string fileName)
    {
        List<(int roomNumber, RoomType roomType)> rooms = new List<(int roomNumber, RoomType roomType)>();
        string[] lines = File.ReadAllLines(fileName);

        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            int roomNumber = int.Parse(parts[0]);
            RoomType roomType = Enum.Parse<RoomType>(parts[1]);
            rooms.Add((roomNumber, roomType));
        }
        return rooms;
    }

    // Read Reservations.txt
    static List<(Guid reservationNumber, DateOnly date, int roomNumber, string customerName, string paymentConfirmation)> ReadReservations(string fileName)
    {
        List<(Guid reservationNumber, DateOnly date, int roomNumber, string customerName, string paymentConfirmation)> reservations = new List<(Guid reservationNumber, DateOnly date, int roomNumber, string customerName, string paymentConfirmation)>();
        string[] lines = File.ReadAllLines(fileName);

        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            Guid reservationNumber = Guid.Parse(parts[0]);
            DateOnly date = DateOnly.FromDateTime(DateTime.Parse(parts[1]));
            int roomNumber = int.Parse(parts[2]);
            string customerName = parts[3];
            string paymentConfirmation = parts[4];
            reservations.Add((reservationNumber, date, roomNumber, customerName, paymentConfirmation));
        }
        return reservations;
    }

    // Read Customers.txt
    static List<(string name, string cardNumber)> ReadCustomers(string fileName)
    {
        List<(string name, string cardNumber)> customers = new List<(string name, string cardNumber)>();
        string[] lines = File.ReadAllLines(fileName);

        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            string name = parts[0];
            string cardNumber = parts[1];
            customers.Add((name, cardNumber));
        }
        return customers;
    }
    // Read RoomPrices.txt
    static List<(RoomType roomType, decimal dailyRate)> ReadRoomPrices(string fileName)
    {
        List<(RoomType roomType, decimal dailyRate)> roomPrices = new List<(RoomType roomType, decimal dailyRate)>();
        string[] lines = File.ReadAllLines(fileName);

        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            RoomType roomType = Enum.Parse<RoomType>(parts[0]);
            decimal dailyRate = decimal.Parse(parts[1]);
            roomPrices.Add((roomType, dailyRate));
        }
        return roomPrices;
    }
    // Write Rooms.txt
    static void WriteRooms(string fileName, List<(int roomNumber, RoomType roomType)> rooms)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var room in rooms)
            {
                writer.WriteLine($"{room.roomNumber},{room.roomType}");
            }
        }
    }

    // Write Reservations.txt
    static void WriteReservations(string fileName, List<(Guid reservationNumber, DateOnly date, int roomNumber, string customerName, string paymentConfirmation)> reservations)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var reservation in reservations)
            {
                writer.WriteLine($"{reservation.reservationNumber},{reservation.date:MM/dd/yyyy},{reservation.roomNumber},{reservation.customerName},{reservation.paymentConfirmation}");
            }
        }
    }
    // Write Customers.txt
    static void WriteCustomers(string fileName, List<(string name, string cardNumber)> customers)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var customer in customers)
            {
                writer.WriteLine($"{customer.name},{customer.cardNumber}");
            }
        }
    }

    // Write RoomPrices.txt
    static void WriteRoomPrices(string fileName, List<(RoomType roomType, decimal dailyRate)> roomPrices)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var price in roomPrices)
            {
                writer.WriteLine($"{price.roomType},{price.dailyRate}");
            }
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