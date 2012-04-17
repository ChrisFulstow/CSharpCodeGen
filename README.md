#CSharpCodeGen
## Generates C# object and collection initializer code

### Example

For the purposes of a simple example, consider a simple Vehicle class defined as:

```c#
class Vehicle
{
	public string Manufacturer { get; set; }
	public string Model { get; set; }
	public int YearOfManufacture { get; set; }
	public double EngineDisplacement { get; set; }
	public bool IsTaxed { get; set; }
	public IEnumerable<Passenger> Passengers { get; set; }
	public Dictionary<string, decimal> OptionalExtraPrices { get; set; }
	public IEnumerable<string> PreviousOwners { get; set; }
}
```
Now we can build up and instantiate a new Vehicle object with some test values:

```c#
Vehicle vehicle = new Vehicle();
vehicle.Manufacturer = "Mini";
vehicle.Model = "Cooper";
vehicle.YearOfManufacture = 2012;
vehicle.EngineDisplacement = 1.598;
vehicle.IsTaxed = true;

Passenger driver = new Passenger();
driver.Name = "Bob";
driver.Age = 28;

Passenger passenger = new Passenger();
passenger.Name = "Alice";
passenger.Age = 25;

Passenger[] passengers = new Passenger[2];
passengers[0] = driver;
passengers[1] = passenger;

vehicle.Passengers = passengers;

vehicle.OptionalExtraPrices = new Dictionary<string, decimal>();
vehicle.OptionalExtraPrices["Sunroof"] = 399;
vehicle.OptionalExtraPrices["Sports exhaust"] = 699;
vehicle.OptionalExtraPrices["Leather seats"] = 499;

List<string> previousOwners = new List<string>();
previousOwners.Add("Carol");
previousOwners.Add("Dave");
vehicle.PreviousOwners = previousOwners;

string objectInitCode = vehicle.InitializationCode();
```

Calling the `InitializationCode` extension method generates the C# object initialisation code for the object.
The string value of the `objectInitCode` variable is now:

	var vehicle = new Vehicle
	{
		Manufacturer = "Mini",
		Model = "Cooper",
		YearOfManufacture = 2012,
		EngineDisplacement = 1.598D,
		IsTaxed = true,
		Passengers = new Passenger[]
		{
			new Passenger
			{
				Name = "Bob",
				Age = 28
			},
			new Passenger
			{
				Name = "Alice",
				Age = 25
			}
		},
		OptionalExtraPrices = new Dictionary<String,Decimal>
		{
			{ "Sunroof", 399M },
			{ "Sports exhaust", 699M },
			{ "Leather seats", 499M }
		},
		PreviousOwners = new List<String> { "Carol", "Dave" }
	};

##License
CSharpCodeGen is open source under the [The MIT License (MIT)](http://www.opensource.org/licenses/mit-license.php)