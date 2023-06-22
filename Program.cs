﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using Models;

// 1. Generation
Console.WriteLine(GC.GetTotalMemory(true));

var generator = new PersonGenerator();

List<Person> persons = generator.GeneratePersons(10000).ToList<Person>();
Console.WriteLine(GC.GetTotalMemory(true));

// 2. Serialization and 3. Write to file
var serializerOptions = new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
// List<string> serializablePersons = persons.Select(person => JsonSerializer.Serialize(person, serializerOptions)).ToList();
Console.WriteLine(GC.GetTotalMemory(true));

string fileName = "Persons.json";
using (FileStream createStream = File.Create(fileName))
{
    JsonSerializer.Serialize<List<Person>>(createStream, persons, serializerOptions);
}

// 4. Clear data in memory
// TODO: сделать это правильно
persons.Clear();
GC.Collect();

Console.WriteLine(GC.GetTotalMemory(true));

// 5. Read from file
using (FileStream openStream = File.OpenRead(fileName))
{
    persons = JsonSerializer.Deserialize<List<Person>>(openStream, serializerOptions);
}

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