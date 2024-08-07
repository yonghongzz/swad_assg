using System.Globalization;

User user = new User("U001", "JohnDoe", "88888888");

Renter renter = new Renter(
    user:user,
    driverLicense: "D1234567",
    isPrime: true,
    currentBooking: null,
    bookingHistory: new List<Booking>(),
    homeAddress: "Blk 123, Some Street, City"
);


List<Vehicle> vehicleList = new List<Vehicle>();
List<(DateTime Start, DateTime End)> rangeList1 = new List<(DateTime, DateTime)>
        {
            (new DateTime(2024, 9, 2, 11, 0, 0), new DateTime(2024, 9, 5, 12, 0, 0)),
            (new DateTime(2024, 8, 10, 12, 0, 0), new DateTime(2024, 8, 21, 12, 0, 0))
        };

List<(DateTime Start, DateTime End)> rangeList2 = new List<(DateTime, DateTime)>
        {
            (new DateTime(2024, 9, 2, 11, 0, 0), new DateTime(2024, 9, 5, 12, 0, 0)),
            (new DateTime(2024, 8, 10, 12, 0, 0), new DateTime(2024, 8, 21, 12, 0, 0))
        };

List<(DateTime Start, DateTime End)> rangeList3 = new List<(DateTime, DateTime)>

        {
            (new DateTime(2024, 9, 2, 11, 0, 0), new DateTime(2024, 9, 5, 12, 0, 0)),
            (new DateTime(2024, 8, 10, 12, 0, 0), new DateTime(2024, 8, 21, 12, 0, 0))
        };


Vehicle vehicle1 = new Vehicle(
    make: "Toyota",
    model: "Camry",
    year: 2022,
    mileage: 15000.5,
    photo: "http://example.com/camry.jpg",
    vehicleId: "V12345",
    rentalRate: 4,
    notAvailableDateTime: rangeList1,
    bookings: new List<Booking>(),
    vehicleInsuranceCompany: "State Farm",
    isDamage: false
);

Vehicle vehicle2 = new Vehicle(
    make: "Honda",
    model: "Civic",
    year: 2021,
    mileage: 12000.0,
    photo: "http://example.com/civic.jpg",
    vehicleId: "V67890",
    rentalRate: 3,
    notAvailableDateTime: rangeList2,
    bookings: new List<Booking>(),
    vehicleInsuranceCompany: "Geico",
    isDamage: false
);

Vehicle vehicle3 = new Vehicle(
    make: "Ford",
    model: "Focus",
    year: 2020,
    mileage: 18000.2,
    photo: "http://example.com/focus.jpg",
    vehicleId: "V54321",
    rentalRate: 6,
    notAvailableDateTime: rangeList3,
    bookings: new List<Booking>(),
    vehicleInsuranceCompany: "Allstate",
    isDamage: false
);

vehicleList.Add(vehicle1);
vehicleList.Add(vehicle2);
vehicleList.Add(vehicle3);


void displayMenu()
{
    Vehicle chosenVehicle;
    while (true)
    {
        Console.WriteLine("Rent Car");
        for (int i = 0; i < vehicleList.Count; i++)
        {
            Vehicle vehicle = vehicleList[i];
            Console.WriteLine($"{i + 1}: Make:{vehicle.Make,-7} Model:{vehicle.Model,-7} Year:{vehicle.Year,-7} MileAge:{vehicle.Mileage,-7} RentalRate:${vehicle.RentalRate,-2} per hour");
        }

        Console.Write("Pick a vehicle to rent: ");
        int option = Convert.ToInt32(Console.ReadLine());
        if(option <= vehicleList.Count)
        {
            chosenVehicle = vehicleList[option - 1];
            break;
        }
        else
        {
            Console.WriteLine("Invalid option. Please try again.");
        }
    }

    Console.WriteLine($"Car Chosen: {chosenVehicle.Make} {chosenVehicle.Model}");
    DateTime start;
    DateTime end;

    while (true)
    {
        Console.Write("Enter start date time in the format of yyyy-MM-dd h:mm tt (e.g., 2024-08-20 12:30 PM): ");
        string startDateTimeInput = Console.ReadLine();
        // Try parsing the input
        if (DateTime.TryParseExact(startDateTimeInput, "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out start))
        {
            if(start < DateTime.Now)
            {
                Console.WriteLine("Start date time cannot be before current date time!");
            }
            else
            {
                // Parsing succeeded
                Console.WriteLine($"Start DateTime: {start}");
                break;
            }
        }
        else
        {
            // Parsing failed
            Console.WriteLine("Invalid date time format. Please use yyyy-MM-dd h:mm tt (e.g., 2024-08-20 12:30 PM).");
        }
    }

    while (true)
    {

        Console.Write("Enter end date time in the format of yyyy-MM-dd h:mm tt (e.g., 2024-08-20 12:30 PM): ");
        string endDateTimeInput = Console.ReadLine();

        // Try parsing the input
        if (DateTime.TryParseExact(endDateTimeInput, "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out end))
        {
            if(end < start)
            {
                Console.WriteLine("End date time cannot be before start date time!");
            }
            else
            {
                // Parsing succeeded
                Console.WriteLine($"End DateTime: {end}");
                break;
            }

        }
        else
        {
            // Parsing failed
            Console.WriteLine("Invalid date time format. Please use yyyy-MM-dd h:mm tt (e.g., 2024-08-20 12:30 PM).");
        }
    }


    if (chosenVehicle.CheckAvailability(start, end))
    {
        string deliveryLocation = null;
        string returnLocation = null;

        string deliveryType = SelectDeliveryType();
        if (deliveryType == "Delivery")
        {
            Console.Write("Enter delivery location: ");
            deliveryLocation = Console.ReadLine();
        }
        string returnType = SelectReturnType();
        if (returnType == "I-Car pick up")
        {

            Console.Write("Enter return location: ");
            returnLocation = Console.ReadLine();

        }
        int i = 1;
        DateTime bookingDateTime = DateTime.Now;
        TimeSpan duration = end - start;
        decimal cost = chosenVehicle.RentalRate * (decimal)duration.TotalHours;
        Booking booking = new Booking(i, start, end, bookingDateTime, cost, "Pending",false,false, renter, chosenVehicle);
        Location location = new Location(i, deliveryLocation, returnLocation);
        booking.SetLocation(location);
        Console.WriteLine();

        displayRentalInformation(booking);
        DateTime paymentDateTime = DateTime.Now;
        Payment payment = new Payment(i, paymentDateTime, "Credit Card", booking.Cost);
        bool success = payment.MakePayment(booking.Cost);
        if (success)
        {
            displayMakePaymentDone();
            booking.Status = "Booked";

            booking.FinalizeBooking(renter, chosenVehicle);
            chosenVehicle.UpdateNotAvailableDateTime(start, end);
            Console.WriteLine();
            i += 1;
        }
    }
    else
    {
        Console.WriteLine("Car is not available on the chosen date time!");
    }
}

void displayRentalInformation(Booking booking)
{
    Console.WriteLine("Rental information");
    Console.WriteLine(booking.ToString());
}

void displayMakePaymentDone()
{
    Console.WriteLine("Make payment done");
}
string SelectDeliveryType()
{
    while (true)
    {
        Console.WriteLine("Delivery type");
        Console.WriteLine("1. Delivery");
        Console.WriteLine("2. Pick up");
        Console.Write("Your option: ");
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                return "Delivery";
            case "2":
                return "Pick up";
            default:
                Console.WriteLine("Invalid option. Please enter 1 or 2.");
                break;
        }
    }
}

string SelectReturnType()
{
    while (true)
    {
        Console.WriteLine("Return type");
        Console.WriteLine("1. Return by own");
        Console.WriteLine("2. I-Car pick up");
        Console.Write("Your option: ");
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                return "Return by own";
            case "2":
                return "I-Car pick up";
            default:
                Console.WriteLine("Invalid option. Please enter 1 or 2.");
                break;
        }
    }
}

while (true)
{
    displayMenu();
}

class Booking
{
    private int id;
    private DateTime startDateTime;
    private DateTime endDateTime;
    private DateTime bookingDateTime;
    private decimal cost;
    private string status;
    private bool isAuthorized;
    private bool isReturnedLate;
    private Location location;
    private Renter renter;
    private Vehicle vehicle;
    public int Id { get { return id; } set { id = value; } }
    public DateTime StartDateTime { get { return startDateTime; } set { startDateTime = value; } }
    public DateTime EndDateTime { get { return endDateTime; } set { endDateTime = value; } }
    public DateTime BookingDate { get { return bookingDateTime; } set { bookingDateTime = value; } }
    public decimal Cost { get { return cost; } set { cost = value; } }
    public string Status { get { return status; } set { status = value; } }
    public bool IsAuthorized { get { return isAuthorized; } set { isAuthorized = value; } }
    public bool IsReturnedLate { get { return isReturnedLate; } set { isReturnedLate = value; } }
    public Location Location { get { return location; } set { location = value; } }
    public Renter Renter { get { return renter; } set { renter = value; } }
    public Vehicle Vehicle { get { return vehicle; } set { vehicle = value; } }

    public Booking() { }
    public Booking(int id, DateTime startDateTime, DateTime endDateTime, DateTime bookingDate, decimal cost, string status,bool isAuthorized, bool isReturnedLate,Renter renter,Vehicle vehicle)
    {
        Id = id;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        BookingDate = bookingDate;
        Cost = cost;
        Status = status;
        IsAuthorized = isAuthorized;
        IsReturnedLate = isReturnedLate;
        Renter = renter;
        Vehicle = vehicle;
        Location = new Location();

    }

    public void SetLocation(Location location)
    {
        Location = location;
    }

    public void FinalizeBooking(Renter renter,Vehicle vehicle)
    {
        renter.SetCurrentBooking(this);
        renter.AddBooking(this);
        vehicle.AddBooking(this);
    }

    public override string ToString()
    {
        return $"Start date time: {StartDateTime}\nEnd date time: {EndDateTime}\nCost: ${Cost}\nDelivery Details\n{Location.ToString()}";
    }

}

class Location
{
    private int locationId;
    private string deliveryAddress;
    private string returnAddress;
    public int LocationId { get { return locationId; } set { locationId = value; } }
    public string DeliveryAddress { get { return deliveryAddress; } set { deliveryAddress = value; } }
    public string ReturnAddress { get { return returnAddress; } set { returnAddress = value; } }
    public Location() { }
    public Location(int id, string deliveryAddress,string returnAddress)
    {
        LocationId = id;
        DeliveryAddress = deliveryAddress;
        ReturnAddress = returnAddress;
    }

    public override string ToString()
    {
        return $"Delivery address: {DeliveryAddress}\nReturn address: {ReturnAddress}";
    }
}


class User
{
    private string id;
    private string userName;
    private string contactNumber;
    public string Id { get { return id; } set { id = value; } }
    public string UserName { get { return userName; } set { userName = value; } }
    public string ContactNumber { get { return contactNumber; } set { contactNumber = value; } }

    public User() { }
    public User(string id, string userName, string number)
    {
        Id = id;
        UserName = userName;
        ContactNumber = number;
    }
}

class Renter : User
{
    private string driverLicense;
    private bool isPrime;
    private Booking currentBooking;
    private List<Booking> bookingHistory;
    private string homeAddress;
    public string DriverLicense { get { return driverLicense; } set { driverLicense = value; } }
    public bool IsPrime { get { return isPrime; } set { isPrime = value; } }
    public Booking CurrentBooking { get { return currentBooking; } set { currentBooking = value; } }
    public List<Booking> BookingHistory { get { return bookingHistory; } set { bookingHistory = value; } }
    public string HomeAddress { get { return homeAddress; } set { homeAddress = value; } }
    public Renter() { }
    public Renter(User user,string driverLicense, bool isPrime, Booking currentBooking, List<Booking> bookingHistory, string homeAddress)
    {
        Id = user.Id;
        UserName = user.UserName;
        ContactNumber = user.ContactNumber;
        DriverLicense = driverLicense;
        IsPrime = isPrime;
        CurrentBooking = currentBooking;
        BookingHistory = bookingHistory;
        HomeAddress = homeAddress;
    }

    public void AddBooking(Booking booking)
    {
        BookingHistory.Add(booking);
    }

    public void SetCurrentBooking(Booking booking)
    {
        CurrentBooking = booking;   
    }
}

class Vehicle
{
    private string make;
    private string model;
    private int year;
    private double mileage;
    private string photo; 
    private string vehicleId;
    private decimal rentalRate;
    private List<(DateTime start,DateTime end)> notAvailableDateTime;
    private List<Booking> bookings;
    private string vehicleInsuranceCompany;
    private bool isDamage;

    public string Make { get { return make; } set { make = value; } }
    public string Model { get { return model; } set { model = value; } }
    public int Year { get { return year; } set { year = value; } }
    public double Mileage { get { return mileage; } set { mileage = value; } }
    public string Photo { get { return photo; } set { photo = value; } }
    public string VehicleId { get { return vehicleId; } set { vehicleId = value; } }
    public decimal RentalRate { get { return rentalRate; } set { rentalRate = value; } }
    public List<(DateTime start,DateTime end)> NotAvailableDateTime { get { return notAvailableDateTime; } set { notAvailableDateTime = value; } }
    public List<Booking> Bookings { get { return bookings; } set { bookings = value; } }
    public string VehicleInsuranceCompany { get { return vehicleInsuranceCompany; } set { vehicleInsuranceCompany = value; } }
    public bool IsDamage { get { return isDamage; } set { isDamage = value; } }

    public Vehicle() { }

    public Vehicle(string make, string model, int year, double mileage, string photo, string vehicleId, decimal rentalRate, List<(DateTime, DateTime)> notAvailableDateTime,List<Booking> bookings, string vehicleInsuranceCompany,bool isDamage)
    {
        Make = make;
        Model = model;
        Year = year;
        Mileage = mileage;
        Photo = photo;
        VehicleId = vehicleId;
        RentalRate = rentalRate;
        NotAvailableDateTime = notAvailableDateTime;
        Bookings = bookings;
        VehicleInsuranceCompany = vehicleInsuranceCompany;
        IsDamage = isDamage;
    }

    public void AddBooking(Booking booking)
    {
        Bookings.Add(booking);
    }
    public bool CheckAvailability(DateTime start,DateTime end)
    {
        foreach (var range in NotAvailableDateTime)
        {
            if (start < range.end && end > range.start)
            {
                return false; 
            }
        }
        return true; 
    }

    public void UpdateNotAvailableDateTime(DateTime start,DateTime end)
    {
        NotAvailableDateTime.Add((start, end));
    }
}

class Payment
{
    private int paymentId;
    private DateTime paymentDateTime;
    private string paymentMethod;
    private decimal paymentCost;
    public int PaymentId { get { return paymentId; } set { paymentId = value; } }
    public DateTime PaymentDateTime { get { return paymentDateTime; } set { paymentDateTime = value; } }
    public string PaymentMethod { get { return paymentMethod; } set {paymentMethod = value; } }
    public decimal PaymentCost { get { return paymentCost; } set { paymentCost = value; } }

    public Payment() { }
    public Payment(int paymentId, DateTime paymentDateTime, string paymentMethod, decimal paymentCost)
    {
        PaymentId = paymentId;
        PaymentDateTime = paymentDateTime;
        PaymentMethod = paymentMethod;
        PaymentCost = paymentCost;
    }

    public bool MakePayment(decimal cost)
    {
        Console.WriteLine($"Payment of ${cost} done.");
        return true;
    }
}


