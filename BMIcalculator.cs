using System;
using System.Globalization;
using System.Collections.Generic;

public class BMIcalculator
{
    double height;
    double weight;
    double BMI;
    const char yes = 't';
    const char no  = 'n';

    Dictionary<string, double> weightUnits = new Dictionary<string, double>()
    {
        {"Kg",     1.0   },
        {"Lbs",    0.4536},
        {"Stones", 6.35  },
        {"Grams",  0.001 }
    };

    
    Dictionary<string, double> heightUnits = new Dictionary<string, double>()
    {
        {"M",    1.0   },
        {"Yd",   0.9144},
        {"Inch", 0.0254},
        {"Cm",   0.01  }
    };

    Dictionary<char, string> heightUnitChoices = new Dictionary<char, string>()
    {
        {'a', "M"   },
        {'b', "Cm"  },
        {'c', "Yd"  },
        {'d', "Inch"}
    };

    Dictionary<char, string> weightUnitChoices = new Dictionary<char, string>()
    {
        {'a', "Kg"    },
        {'b', "Grams" },
        {'c', "Lbs"   },
        {'d', "Stones"}
    };

    
    double GetNumber(string communicate)
    {
        while (true)
        {
            Console.WriteLine(communicate);
            string input = Console.ReadLine().Replace(',', '.');
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Blad: nie wprowadzono danych. Sprobuj ponownie.");
                continue;
            }
            if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                return result;
            Console.WriteLine("Blad: Nieprawidlowe dane. Sprobuj ponownie.");
        }
    }

    char GetUnitChoice(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine().ToLower();
            if (!char.TryParse(input, out char choice) || choice < 'a' || choice > 'e')
            {
                Console.WriteLine("Wprowadzono nieprawidlowa jednostke miary. Wybierz poprawna jednostke.");
                continue;
            }
            return choice;
        }
    }

    void ReadHeight()
    {
        char unitChoice = GetUnitChoice("Wybierz jednostke miary wzrostu:\n'a' - metry \n'b' - centymetry, \n'c' - jardy \n'd' - cale, \n'e' - wlasne");
        double userHeight = GetNumber("Podaj swoj wzrost w wybranej jednostce: ");
        if (unitChoice == 'e')
        {
            double ratio = GetNumber("Podaj ile metrow ma twoja jednostka wysokosci: ");
            height = userHeight * ratio;
        }
        else
            height = userHeight * heightUnits[heightUnitChoices[unitChoice]];
    }

    void ReadWeight()
    {
        char unitChoice = GetUnitChoice(
            "Wybierz jednostke miary wagi: \n'a' - kilogramy, \n'b' - gramy, \n'c' - funty \n'd' - kamienie, \n'e' - wlasne");
        double userWeight = GetNumber("Podaj swoja wage (w wybranej jednostce): ");
        if (unitChoice == 'e')
        {
            double ratio = GetNumber("Podaj ile kilogramow ma twoja jednostka wagi: ");
            weight = userWeight * ratio;
        }
        else
            weight = userWeight * weightUnits[weightUnitChoices[unitChoice]];
    }

    void CalculateBMI()
    {
        BMI = Math.Round(weight / (height * height), 2);
    }

    void PrintBMIResult()
    {
        Console.WriteLine("Twoje BMI wynosi: " + BMI);
        switch (BMI)
        {
            case < 18.5:
                Console.WriteLine("Twoja kategoria BMI: Niedowaga.\n\n");        
                break;
            case >= 18.5 and < 25:
                Console.WriteLine("Twoja kategoria BMI: Waga prawidlowa.\n\n");
                break;
            case >= 25 and < 30:
                Console.WriteLine("Twoja kategoria BMI: Nadwaga.\n\n");
                break;
            case >= 30:
                Console.WriteLine("Twoja kategoria BMI: Otylosc.\n\n");
                break;
        }
    }

    bool AskToContinue()
    {
        Console.WriteLine("Czy chcesz podac nowe dane? 't' - tak, 'n' - nie: ");
        while (true)
        {
            if (!char.TryParse(Console.ReadLine(), out char choice))
            {
                Console.WriteLine("Nie wprowadzono znaku. Podaj znak ponownie.\n");
                continue;
            }
            if (choice == yes) return true;
            if (choice == no)  return false;
            Console.WriteLine("Nieprawidlowy wybor znaku. Podaj znak ponownie.\n");
        }
    }

    void Run()
    {
        Console.WriteLine("KALKULATOR BMI");
        do
        {
            height = 0;
            weight = 0;
            ReadHeight();
            ReadWeight();
            CalculateBMI();
            PrintBMIResult();
        }
        while (AskToContinue());
    }

    public static void Main(string[] args)
    {
        new BMIcalculator().Run();
    }
}
