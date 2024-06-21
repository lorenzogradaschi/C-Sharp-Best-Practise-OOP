;using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Auto
{
    public string Brand { get; set; } // Brand of the car
    public string Model { get; set; }  // Model of the car
    public double Price { get; set; } // Price of the car

    // Constructor with parameters
    public Auto(string brand, string model, double price)
    {
        Brand = brand;
        Model = model;
        Price = price;
    }

    // Default constructor
    public Auto() {}

    // Override ToString method to print car details
    public override string ToString()
    {
        return $"Auto [Brand={Brand}, Model={Model}, Price={Price}]";
    }

    // Method to convert car details to CSV format
    public string ToCSV()
    {
        return $"{Brand};{Model};{Price}";
    }
}



public class CarArchive
{
    private readonly string fileName = "car_list.csv"; // File name for storing the car list

    // Method to write the list of cars to a CSV file
    public void WriteAll(List<Auto> list)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                foreach (var car in list)
                {
                    sw.WriteLine(car.ToCSV());
                }
                Console.WriteLine("File written successfully");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Error writing to file: " + e.Message);
        }
    }

    // Method to read the list of cars from a CSV file
    public List<Auto> ReadAll()
    {
        List<Auto> cars = new List<Auto>();
        try
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] cols = line.Split(';');
                    if (cols.Length != 3)
                    {
                        Console.WriteLine("Invalid row in CSV file: " + line);
                        continue;
                    }

                    if (double.TryParse(cols[2], out double price))
                    {
                        Auto car = new Auto(cols[0], cols[1], price);
                        cars.Add(car);
                    }
                    else
                    {
                        Console.WriteLine("Number format error for row: " + line);
                    }
                }
                Console.WriteLine("File read successfully");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Error reading file: " + e.Message);
        }

        return cars;
    }
}

public class CarList
{
    private List<Auto> cars; // List to store cars
    private CarArchive carArchive; // Archive object for file operations

    // Constructor
    public CarList()
    {
        cars = new List<Auto>();
        carArchive = new CarArchive();
    }

    // Method to add a new car to the list
    public void Add(Auto newCar)
    {
        cars.Add(newCar);
        Console.WriteLine("Added to list: " + newCar);
    }

    // Method to delete a car from the list
    public void Delete(Auto car)
    {
        var carToRemove = cars.FirstOrDefault(auto =>
            auto.Model.Equals(car.Model, StringComparison.OrdinalIgnoreCase) ||
            auto.Brand.Equals(car.Brand, StringComparison.OrdinalIgnoreCase));

        if (carToRemove != null)
        {
            cars.Remove(carToRemove);
            Console.WriteLine("Removed from list: " + carToRemove);
        }
        else
        {
            Console.WriteLine("Car not found");
        }
    }

    // Method to save the list to a file
    public string Save()
    {
        try
        {
            carArchive.WriteAll(cars);
            Console.WriteLine("List saved successfully");
            return "Car list saved successfully";
        }
        catch (IOException e)
        {
            Console.WriteLine("Error saving list: " + e.Message);
            return "Error saving the car list: " + e.Message;
        }
    }

    // Method to read the list and return it as a string
    public string Read()
    {
        var carList = new System.Text.StringBuilder("List of Cars:\n");
        foreach (var car in cars)
        {
            carList.Append(car).Append("\n");
        }
        Console.WriteLine("List read successfully from memory:\n" + carList);
        return carList.ToString();
    }

    // Method to load the list from a file
    public void LoadFromFile()
    {
        try
        {
            cars = carArchive.ReadAll();
            Console.WriteLine("List loaded from file:\n" + string.Join("\n", cars));
        }
        catch (IOException e)
        {
            Console.WriteLine("Error loading list from file: " + e.Message);
        }
    }

    // Method to get the list of cars
    public List<Auto> GetCars()
    {
        return cars;
    }

    // Method to get a list of car brands
    public static List<string> GetBrands(List<Auto> carList)
    {
        return carList.Select(car => car.Brand).ToList();
    }

    // Method to filter cars with a price less than a given value
    public static List<Auto> FilterCarsByPrice(List<Auto> carList, double maxPrice)
    {
        return carList.FindAll(car => car.Price < maxPrice);
    }

    // Method to sort cars by price
    public static List<Auto> SortCarsByPrice(List<Auto> carList)
    {
        carList.Sort((x, y) => x.Price.CompareTo(y.Price));
        return carList;
    }
}


public class CarNotFoundException : Exception
{
    public CarNotFoundException() : base("Car not available") {}
}

public class Program
{
    public static void Main(string[] args)
    {
        CarList myCarList = new CarList();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. Add car");
            Console.WriteLine("2. Delete car");
            Console.WriteLine("3. Show car list");
            Console.WriteLine("4. Save car list");
            Console.WriteLine("5. Load car list from file");
            Console.WriteLine("6. Show car price");
            Console.WriteLine("7. Exit");
            Console.Write("Choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddCar(myCarList);
                    break;
                case "2":
                    DeleteCar(myCarList);
                    break;
                case "3":
                    ShowCarList(myCarList);
                    break;
                case "4":
                    SaveCarList(myCarList);
                    break;
                case "5":
                    LoadCarList(myCarList);
                    break;
                case "6":
                    ShowCarPrice(myCarList);
                    break;
                case "7":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }

    // Method to add a car
    private static void AddCar(CarList carList)
    {
        Console.Write("Enter brand: ");
        string brand = Console.ReadLine();
        Console.Write("Enter model: ");
        string model = Console.ReadLine();
        Console.Write("Enter price: ");
        if (double.TryParse(Console.ReadLine(), out double price))
        {
            Auto newCar = new Auto(brand, model, price);
            carList.Add(newCar);
            Console.WriteLine("Car added successfully");
        }
        else
        {
            Console.WriteLine("Invalid price");
        }
    }

    // Method to delete a car
    private static void DeleteCar(CarList carList)
    {
        Console.Write("Enter brand: ");
        string brand = Console.ReadLine();
        Console.Write("Enter model: ");
        string model = Console.ReadLine();
        Auto carToDelete = new Auto(brand, model, 0); // Price is irrelevant for deletion
        carList.Delete(carToDelete);
    }

    // Method to show the car list
    private static void ShowCarList(CarList carList)
    {
        Console.WriteLine(carList.Read());
    }

    // Method to save the car list to a file
    private static void SaveCarList(CarList carList)
    {
        carList.Save();
    }

    // Method to load the car list from a file
    private static void LoadCarList(CarList carList)
    {
        carList.LoadFromFile();
    }

    // Method to show the price of a specific car
    private static void ShowCarPrice(CarList carList)
    {
        Console.Write("Enter brand: ");
        string brand = Console.ReadLine();
        Console.Write("Enter model: ");
        string model = Console.ReadLine();
        Auto car = carList.GetCars().FirstOrDefault(a => a.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase) && a.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
        if (car != null)
        {
            Console.WriteLine($"Car price: {car.Price}");
        }
        else
        {
            Console.WriteLine("Car not found or price not available.");
        }
    }
}