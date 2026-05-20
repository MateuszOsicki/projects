using System;
using System.Collections.Generic;

class Gobblet
{
    public int Size {get;}
    public int Player {get;}

    public Gobblet(int size, int player)
    {
        Size = size;
        Player = player;
    }

    public override string ToString()
    {
        string letter = Player == 1? "B" : "O"; //blue, orange
        string size = Size == 1 ? "1" : Size == 2 ? "2" : "3";
        return letter + size;
    }
}
class Gobblets
{
    static Stack<Gobblet>[,] board = new Stack<Gobblet>[3, 3];
    static List<Gobblet>[] hands = new List<Gobblet>[2];
    static int currentPlayer = 1;

    static void Main()
    {
        InitializeGame();

        Console.WriteLine("GOBBLETY");
        Console.WriteLine("Gracz 1 = Niebieski (B1 B2 B3)");
        Console.WriteLine("Gracz 2 = Pomaranczowy (O1 O2 O3)");
        Console.WriteLine("Liczba przy literze = rozmiar pionka");
        Console.WriteLine("1 = maly, 2 = sredni, 3 = duzy");
        Console.WriteLine("Nacisnij ENTER aby zaczac...)");
        Console.ReadLine(); 

        while(true)
        {
            DrawBoard();
            Console.WriteLine($"\n-- Tura Gracza {currentPlayer} " + $"({(currentPlayer == 1 ? "Niebieski" : "Pomaranczowy")}) --");

            MakeMove();

            int winner = checkIfWin();
            if(winner > 0)
            {
                DrawBoard();
                Console.WriteLine($"GRACZ {winner} WYGRYWA!!!\nNacisnij ENTER aby zakonczyc...");
                return;
            }
            currentPlayer = (currentPlayer == 1) ? 2 : 1;
        }
    }

    static void InitializeGame()
    {
        for(int r = 0; r < 3; r++)
        {
            for(int c = 0; c < 3; c++)
            {
                board[r, c] = new Stack<Gobblet>();
            }
        }

        hands[0] = new List<Gobblet>();
        hands[1] = new List<Gobblet>();

        for(int player = 1; player <= 2; player++)
        {
            for(int size = 1; size <= 3; size++)
            {
                hands[player - 1].Add(new Gobblet(size, player));
                hands[player - 1].Add(new Gobblet(size, player));
            }
        }
    }

    static void DrawBoard()
    {
        Console.Clear();
        Console.WriteLine("              GOBBLETY");
        Console.WriteLine("         kol1   kol2   kol3  ");
        Console.WriteLine("       +------+------+------+");

        for(int r = 0; r < 3; r++)
        {
            Console.Write($"rzad {r + 1} |");
            for(int c = 0; c < 3; c++)
            {
                if(board[r, c].Count > 0)
                {
                    Gobblet top = board[r, c].Peek();
                    Console.Write($"  {top}  |");
                }
                else
                {
                    Console.Write("  --  |");
                }
            }
            Console.WriteLine();
            Console.WriteLine("       +------+------+------+");
        }

        Console.WriteLine("\nGracz 1 (Niebieski) w rece: ");
        DisplayHand(0);

        Console.WriteLine("\nGracz 2 (Pomaranczowy) w rece: ");
        DisplayHand(1);
    }

    static void DisplayHand(int playerIndex)
    {
        if(hands[playerIndex].Count == 0)
        {
            Console.WriteLine("(brak pionkow)");
            return;
        }
        foreach (Gobblet g in hands[playerIndex])
        {
            Console.Write($"{g} ");
        }
        Console.WriteLine();
    }

    static void MakeMove()
    {
        Console.WriteLine("Co chcesz zrobic?\n[1] Postawic nowego pionka z reki\n[2] Przesunac pionka z planszy");

        Console.WriteLine("Twoj wybor: ");
        string choice = Console.ReadLine()?.Trim() ?? "";

        if(choice == "1")
        {
            PutFromHand();
        }
        else if(choice == "2")
        {
            MoveFromBoard();
        }
        else
        {
            Console.WriteLine("Blad: wpisz 1 lub 2. Sprobuj ponownie.");
            MakeMove();
        }
    }

    static void PutFromHand()
    {
        int listIndex = currentPlayer - 1;
        var hand = hands[listIndex];

        if(hand.Count == 0)
        {
            Console.WriteLine("Blad: nie masz wiecej pionkow w rece. Musisz ruszyc pionkiem z planszy.");
            MoveFromBoard();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Twoje pionki w rece: ");
        for(int i = 0; i < hand.Count; i++)
        {
            Console.WriteLine($"  [{i + 1}]   {hand[i]}   (romiar {hand[i].Size})");
        }

        int pawnNumber = AskForAmount("Ktory pionek chcesz wybrac (podaj numer): ", 1, hand.Count);

        Console.WriteLine();
        int row = AskForAmount("Na ktory RZAD postawic pionka? (1-3)", 1, 3);
        int column = AskForAmount("Na ktora KOLUMNE postawic pionka? (1-3)", 1, 3);

        Gobblet choosen = hand[pawnNumber - 1];

        if(!CanPut(choosen, row - 1, column - 1))
        {
            return;
        }

        hand.Remove(choosen);
        board[row - 1, column - 1].Push(choosen);

        Console.WriteLine($"Postawiono {choosen} na [{row}, {column}].");
    }

    static void MoveFromBoard()
    {
        Console.WriteLine("\nSkad zabrac pionka?");
        int sourceRow = AskForAmount("Rzad zrodlowy (1-3): ", 1, 3);
        int sourceColumn = AskForAmount("Kolumna zrodlowa (1-3): ", 1, 3);

        var source = board[sourceRow - 1, sourceColumn - 1];

        if(source.Count == 0)
        {
            Console.WriteLine("To pole jest puste - sprobuj ponownie.");
            MoveFromBoard();
            return;
        }

        Gobblet topPawn = source.Peek();

        if(topPawn.Player != currentPlayer)
        {
            Console.WriteLine("Blad: wierzch tego pola nalezy do przeciwnika - sprobuj ponownie.");
            MoveFromBoard();
            return;
        }

        Console.WriteLine("\nDokad przeniesc?");
        int destinationRow = AskForAmount("Rzad docelowy (1-3): ", 1, 3);
        int destinationColumn = AskForAmount("Kolumna docelowa (1-3): ", 1, 3);

        if(sourceRow == destinationRow && sourceColumn == destinationColumn)
        {
            Console.WriteLine("Podane zrodlo i cel to to samo pole - sprobuj ponownie.");
            MoveFromBoard();
            return;
        }

        if(!CanPut(topPawn, destinationRow - 1, destinationColumn - 1))
        {
            return;
        }

        source.Pop();
        board[destinationRow - 1, destinationColumn - 1].Push(topPawn);

        Console.WriteLine($"Przeniesiono {topPawn} from [{sourceRow}, {sourceColumn}] na [{destinationRow}, {destinationColumn}]");
    }
    static bool CanPut(Gobblet pawn, int r, int c)
    {
        var stack = board[r, c];

        if(stack.Count == 0)
        {
            return true;
        }

        Gobblet onTop = stack.Peek();

        if(pawn.Size > onTop.Size)
        {
            return true;
        }

        Console.WriteLine($"Blad: nie mozesz tu postawic {pawn}!. Na polu stoi {onTop} (rozmiar {onTop.Size}). Twoj pionek musi byc wiekszy.");

        return false;
    }

    static int checkIfWin()
    {
        for(int r = 0; r < 3; r++)
        {
            int w = CheckThree(r, 0,  r, 1,  r, 2);
            if (w > 0)
            {
                return w;
            }
        }

        for(int c = 0; c < 3; c++)
        {
            int w = CheckThree(0, c,  1, c,  2, c);
            if (w > 0)
            {
                return w;
            }
        }

        int diag1 = CheckThree(0, 0,  1, 1,  2, 2);
        if (diag1 > 0)
        {
            return diag1;
        }

        int diag2 = CheckThree(0, 2,  1, 1,  2, 0);
        if (diag2 > 0)
        {
            return diag2;
        }

        return 0;
    }

    static int CheckThree(int r1, int c1,  int r2, int c2,  int r3, int c3)
    {
        var b1 = board[r1, c1];
        var b2 = board[r2, c2];
        var b3 = board[r3, c3];

        if(b1.Count == 0 || b2.Count == 0 || b3.Count == 0)
        {
            return 0;
        }

        int p1 = b1.Peek().Player;
        int p2 = b2.Peek().Player;
        int p3 = b3.Peek().Player;

        if(p1 == p2 && p2 == p3)
        {
            return p1;
        }

        return 0;
    }

    static int AskForAmount(string comunicate, int min, int max)
    {
        while(true)
        {
            Console.WriteLine(comunicate);
            string input = Console.ReadLine()?.Trim() ?? "";

            if(int.TryParse(input, out int number) && number >= min && number <= max)
            {
                return number;
            }

            Console.WriteLine($"Blad: wpisz liczbe od {min} do {max}.");
        }
    }
}
