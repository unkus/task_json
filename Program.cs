using System.Diagnostics;
using System.Text.Json;
using Models;

var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Persons.json");
var serializerOptions = new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

Debug.WriteLine($"Использовано памяти перед генерацией: {GC.GetTotalMemory(false)}");

// 1. Generation
var generator = new PersonGenerator();
List<Person> persons = generator.GeneratePersons(10000).ToList<Person>();
Debug.WriteLine($"Использовано памяти при генерации: {GC.GetTotalMemory(false)}");

// 2. Serialization and 3. Write to file
using (FileStream createStream = File.Create(fileName))
{
    JsonSerializer.Serialize<List<Person>>(createStream, persons, serializerOptions);
}

// 4. Clear data in memory
var memIn = GC.GetTotalMemory(false);
Debug.WriteLine($"Использовано памяти при сериализации: {memIn}");

persons = null;
// а есть другой способ для управляемых объектов?
// такой очистки явно не достаточно или я неправильно получаю использованную память
GC.Collect();
GC.WaitForPendingFinalizers();

var memOut = GC.GetTotalMemory(false);
Debug.WriteLine($"Использовано памяти после очистки: {memOut}");
Debug.WriteLine($"Освобождено памяти: {memIn - memOut}");

// 5. Read from file
using (FileStream openStream = File.OpenRead(fileName))
{
    persons = JsonSerializer.Deserialize<List<Person>>(openStream, serializerOptions);
}
Debug.WriteLine($"Использовано памяти после десериализации: {GC.GetTotalMemory(true)}");

// 6. Representation
Console.WriteLine($"Персон: {persons?.Count}");

var creditCardNumber = persons?.Select(p => p.CreditCardNumbers?.Length).Sum();
Console.WriteLine($"Число кредитных карт: {creditCardNumber}");

var averageChildAge = persons?
        .Where(p => p.Children != null && p.Children.Length > 0)
        .Average(p => p.Children
            .Average(c => DateTime.UtcNow.Year - new DateTime(c.BirthDate).Year)
        );
Console.WriteLine($"Средний возраст детей: {averageChildAge}");

Debug.WriteLine($"Использовано памяти перед завершением: {GC.GetTotalMemory(true)}");