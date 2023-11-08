namespace RickData;

public class Class1
{
        enum RoomType
    {
        Single,
        Double,
        Suite
    }
            // Read data from files
        List<(int roomNumber, RoomType roomType)> rooms = ReadRooms("Rooms.txt");
        List<(Guid reservationNumber, DateOnly date, int roomNumber, string customerName, string paymentConfirmation)> reservations = ReadReservations("Reservations.txt");
        List<(string name, string cardNumber)> customers = ReadCustomers("Customers.txt");
        List<(RoomType roomType, decimal dailyRate)> roomPrices = ReadRoomPrices("RoomPrices.txt");
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

}