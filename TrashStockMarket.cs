using System;

public class StockMarket
{
    static double money = 10;
    static int trashAmount = 0;
    static double tax = 0.1;
    static char zakup = 'z';
    static char sprzedaz = 's';
    static bool FinishConditionsNotMet()
    {
        return money < 50 && (money > 0 || trashAmount > 0);
    }
    public static void Main(string[] args)
    {
        Console.WriteLine("GIELDA SMIECI");

        while (FinishConditionsNotMet())
        {
            Console.WriteLine("\nTwoj stan konta: " + Math.Round(money, 2) + "$");
            Console.WriteLine("Ilosc posiadanych smieci: " + trashAmount);
            Console.WriteLine("Wybierz czy chcesz zakupic (z) czy sprzedac (s) smieci?");

            string inputAction = Console.ReadLine();
            if (string.IsNullOrEmpty(inputAction)) continue;
            char action = inputAction[0];

            if (action == zakup)
            {
                Random rnd = new Random();
                double price = Math.Round(1.5 + rnd.NextDouble(), 1);
                Console.WriteLine("Cena jednostkowa wynosi: " + price + "$");

                bool incorrectAmount = true;
                while (incorrectAmount)
                {
                    Console.WriteLine("Ile smieci chcesz zakupic?");
                    string inputAmount = Console.ReadLine();

                    if (!int.TryParse(inputAmount, out int amount))
                    {
                        Console.WriteLine("To nie jest liczba! Proba kosztowala cie podatek.");
                        money -= tax;
                        continue;
                    }
                    if (amount < 0)
                    {
                        Console.WriteLine("Nie mozna wprowadzac wartosci ujemnych. Proba kosztowala cie podatek.");
                        money -= tax;
                        continue;
                    }
                    double totalCost = price * amount;
                    incorrectAmount = false;

                    if (money >= totalCost)
                    {
                        money -= totalCost;
                        trashAmount += amount;

                        if (amount > 0)
                        {
                            Console.WriteLine($"Zakupiono {amount} szt. za {totalCost}$ + {tax}$ podatku.");
                        }
                        else
                        {
                            Console.WriteLine($"Nie zakupiono zadnych smieci. Zaplacono {tax}$ podatku.");
                        }      
                    }
                    else
                    {
                        Console.WriteLine("Za malo pieniedzy! Proba kosztowala Cie podatek.");
                    }
                    money -= tax; 
                }
            }
            else if (action == sprzedaz)
            {
                double sellPrice = 2.0;
                Console.WriteLine("Cena jednostkowa wynosi: " + sellPrice + "$");
                bool incorrectAmount = true;
                while (incorrectAmount)
                {
                    Console.WriteLine("Ile smieci chcesz sprzedac?");
                    string inputAmount = Console.ReadLine();

                    if (!int.TryParse(inputAmount, out int amount))
                    {
                        Console.WriteLine("To nie jest liczba! Proba kosztowala cie podatek.");
                        money -= tax;
                        continue; 
                    }

                    if (amount < 0)
                    {
                        Console.WriteLine("Nie mozna wprowadzac wartosci ujemnych. Proba kosztowala cie podatek.");
                        money -= tax;
                        continue;
                    }
                    
                    if (amount > trashAmount)
                    {
                        Console.WriteLine("Nie masz tylu smieci! Proba kosztowala cie podatek.");
                        money -= tax; 
                        continue;
                    }
                    else
                    {
                        double earned = sellPrice * amount;
                        money += earned;
                        trashAmount -= amount;
                        if(earned > 0)
                        {
                            Console.WriteLine("Zarobiles: " + (earned - tax) + "$.");
                        }
                        else
                        {
                            Console.WriteLine("Nie sprzedano zadnych smieci. Zaplacono" + tax + "$ podatku.");
                        }
                        incorrectAmount = false;
                    }
                    money -= tax;
                }
            }
            else
            {
                Console.WriteLine("Nieznana komenda! Wybierz 'z' lub 's'.");
            }
        }

        if (money >= 50)
        {
            Console.WriteLine("\nTwoj ostateczny stan konta: " + Math.Round(money, 2) + "$");
            Console.WriteLine("Wygrana! Jestes hegemonem smieci!!!");
        }
        else
        {
            Console.WriteLine("\nPrzegrana! Zbankrutowales/as.");
        }
    }
}
