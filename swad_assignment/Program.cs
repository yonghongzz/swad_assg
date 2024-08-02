class Booking
{
    private string id;
    private DateTime startDateTime;
    private DateTime endDateTime;
    private DateOnly bookingDate;
    private int cost;
    private string status;
    private Location deliveryLocation;
    public string Id { get { return id; } set{ id = value; } }
    public DateTime StartDateTime { get { return startDateTime; } set { startDateTime = value; } }
    public DateTime EndDateTime { get { return endDateTime; } set { endDateTime = value; } }
    public DateOnly BookingDate { get { return bookingDate; } set { bookingDate = value; } }
    public int Cost { get { return cost; } set { cost = value; } }
    public string Status { get { return status; } set { status = value; } }
    public Location DeliveryLocation { get { return deliveryLocation; } set { deliveryLocation = value; } }

    public Booking() { }
    public Booking(string id, DateTime startDateTime, DateTime endDateTime, DateOnly bookingDate, int cost, string status)
    {
        Id = id;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        BookingDate = bookingDate;
        Cost = cost;
        Status = status;
    }

    public void SetLocation(Location location)
    {
        DeliveryLocation = location;
    }

}

class Location
{
    private string locationId;
    private string pickupAddress;
    private DateTime pickupDateTime;
    private string returnAddress;
    private DateTime returnDateTime;
    public string LocationId { get{ return locationId; } set{locationId = value; } }
    public string PickupAddress { get { return pickupAddress; } set { pickupAddress = value; } }
    public DateTime PickupDateTime { get {return returnDateTime; } set { returnDateTime = value; } }
    public string ReturnAddress { get {return returnAddress; } set {returnAddress = value; } }
    public DateTime ReturnDateTime { get { return ReturnDateTime; } set { returnDateTime = value; } }
    public Location() { }
    public Location(string id, string pickupAddress, DateTime pickupDateTime, string returnAddress, DateTime returnDateTime)
    {
        LocationId = id;
        PickupAddress = pickupAddress;
        PickupDateTime = pickupDateTime;
        ReturnAddress = returnAddress;
        ReturnDateTime = returnDateTime;
    }
}

class Renter
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
    public Renter(string driverLicense, bool isPrime, Booking currentBooking, List<Booking> bookingHistory, string homeAddress)
    {
        DriverLicense = driverLicense;
        IsPrime = isPrime;
        CurrentBooking = currentBooking;
        BookingHistory = bookingHistory;
        HomeAddress = homeAddress;
    }

    public void addBooking(Booking booking)
    {
        BookingHistory.Add(booking);
    }
}


