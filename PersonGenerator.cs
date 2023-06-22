using System.Runtime.Serialization;
using Models;

class PersonGenerator
{
    // Граничная дата для совершеннолетия
    private static readonly DateTime s_comingOfAge = DateTime.UtcNow.AddYears(-18);
    // МРОТ
    private const int _minimumWage = 17866;

    private readonly Random _random = new();

    private static readonly List<string> s_maleNames = new(){
        "Александр",
        "Максим",
        "Михаил",
        "Марк",
        "Иван",
        "Артем",
        "Лев",
        "Дмитрий",
        "Матвей",
        "Даниил"
    };

    private static readonly List<string> s_femaleNames = new(){
        "София",
        "Анна",
        "Мария",
        "Алиса",
        "Ева",
        "Виктория",
        "Полина",
        "Варвара",
        "Александра",
        "Анастасия"
    };

    private readonly List<string> s_surnames = new(){
        "Иванов",
        "Смирнов",
        "Кузнецов",
        "Попов",
        "Васильев",
        "Петров",
        "Соколов",
        "Михайлов",
        "Новиков",
        "Фёдоров"
    };

    public IEnumerable<Person> GeneratePersons(int count)
    {
        var people = new List<Person>();
        for (int i = 0; i < count; i++)
        {
            var gender = (Gender)_random.Next(2);

            var birthDate = GeneratePersonBirthDate();
            var newPerson = (new Person()
            {
                Id = _random.Next(),
                TransportId = Guid.NewGuid(),
                FirstName = GetRandomName(gender),
                LastName = GetRandomSurname(gender),
                SequenceId = i,
                CreditCardNumbers = GenerateCreditCards(),
                Age = DateTime.UtcNow.AddTicks(birthDate.Ticks * -1).Year, // А может уберем это поле из модели и будем считать опираясь на дату рождения?
                Phones = GeneratePhones(),
                BirthDate = birthDate.Ticks,
                Salary = _random.NextDouble() * _random.Next(_minimumWage, 200000), // Допустим зарплаты будут от МРОТ до 200 т.р.
                IsMarred = _random.Next(2) == 1, // 0 - not marred, 1 - marred. Заменим на IsMarried?
                Gender = gender
            });
            newPerson.Children = GenerateChildren(newPerson);
            people.Add(newPerson);
        }
        return people;
    }

    private Child[] GenerateChildren(Person parent)
    {
        int count = _random.Next(5); // Допустим не более 4 детей на человека
        var children = count > 0 ? new Child[count] : Array.Empty<Child>();
        for (int i = 0; i < count; i++)
        {
            var gender = (Gender)_random.Next(2);
            children[i] = new Child()
            {
                Id = _random.Next(),
                FirstName = GetRandomName(gender),
                LastName = GetChildSurname(parent, gender),
                BirthDate = GenerateChildBirthDate().Ticks,
                Gender = gender
            };
        }
        return children;
    }

    private DateTime GeneratePersonBirthDate()
    {
        // от 1970 до даты совершеннолетия
        return GenerateBirthDate(DateTime.UnixEpoch.Year, s_comingOfAge.Year);
    }

    private DateTime GenerateChildBirthDate()
    {
        // от 18 лет назад и до текущей даты
        return GenerateBirthDate(s_comingOfAge.Year, DateTime.UtcNow.Year);
    }

    private DateTime GenerateBirthDate(int beginYear, int endYear)
    {
        var year = _random.Next(beginYear, endYear + 1);
        // от 1 до 12 или месяц из граничной даты (совпадает с текущим месяцем)
        var month = _random.Next(1, (year == endYear ? DateTime.UtcNow.Month : 12) + 1);
        // от 1 до <дней в месяце> или до дня в граничной дате (совпадает с сегодняшним днем)
        var day = _random.Next(1, (year == endYear && month == DateTime.UtcNow.Month ? DateTime.UtcNow.Day : DateTime.DaysInMonth(year, month)) + 1);
        return new DateTime(year, month, day);
    }

    private string[] GenerateCreditCards()
    {
        int count = _random.Next(4); // Допустим у человека может быть не более 4 карт
        string[] cards = count > 0 ? new string[count] : Array.Empty<string>();

        for (int i = 0; i < count; i++)
        {
            cards[i] = String.Format("{0:D4} {1:D4} {2:D4} {3:D4}", _random.Next(10000), _random.Next(10000), _random.Next(10000), _random.Next(10000));
        }
        return cards;
    }

    private string[] GeneratePhones()
    {
        int count = _random.Next(5); // Допустим у человека может быть не более 4 телефонов
        string[] phones = count > 0 ? new string[count] : Array.Empty<string>();

        for (int i = 0; i < count; i++)
        {
            phones[i] = String.Format("+7 ({0:D3}) {1:D3}-{2:D2}-{3:D2}", _random.Next(1000), _random.Next(1000), _random.Next(100), _random.Next(100));
        }

        return phones;
    }

    private string GetRandomName(Gender gender)
    {
        return gender == Gender.Male ? s_maleNames[_random.Next(s_maleNames.Count)] : s_femaleNames[_random.Next(s_femaleNames.Count)];
    }

    private string GetRandomSurname(Gender gender)
    {
        return $"{s_surnames[_random.Next(s_surnames.Count)]}{GetSurnameSuffix(gender)}";
    }

    private static string GetChildSurname(Person parent, Gender gender)
    {
        return $"{parent.LastName}{GetSurnameSuffix(gender)}";
    }

    private static string GetSurnameSuffix(Gender gender)
    {
        return gender == Gender.Male ? "" : "а";
    }

}