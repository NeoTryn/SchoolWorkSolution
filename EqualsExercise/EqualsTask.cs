using System;

record PhoneRecord(long Vorwahl, long Telefonummer);

class PhoneNr : IComparable<PhoneNr>, IComparable, IEquatable<PhoneNr>
{
    public PhoneRecord Phone { get; }

    public PhoneNr(long vorwahl, long telefonummer)
    {
        Phone = new PhoneRecord(vorwahl, telefonummer);
    }

    public int CompareTo(PhoneNr? other) {
        if (Phone.Vorwahl == other?.Phone.Vorwahl)
        {
            return Phone.Telefonummer.CompareTo(other.Phone.Telefonummer);
        }
        else
        {
            return Phone.Vorwahl.CompareTo(other?.Phone.Vorwahl);
        }
    }
    public int CompareTo(object? obj) => CompareTo(obj as PhoneNr);

    public override bool Equals(object? obj) => this.Equals(obj as PhoneNr);
    public bool Equals(PhoneNr? other) => Phone.Equals(other?.Phone);

    public override string ToString() => $"0{Phone.Vorwahl}/{Phone.Telefonummer}";

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator >(PhoneNr lhs, PhoneNr rhs) => lhs.CompareTo(rhs) > 0;
    public static bool operator <(PhoneNr lhs, PhoneNr rhs) => lhs.CompareTo(rhs) < 0;
}


class Program
{
    static void Main(string[] args)
    {
        // HTL Wien V
        PhoneNr nr1 = new PhoneNr(01, 54615);
        // BMBWF
        PhoneNr nr2 = new PhoneNr(01, 53120);
        // Handynummer
        PhoneNr nr3 = new PhoneNr(0699, 99999999);
        // HTL Wien V
        PhoneNr nr4 = new PhoneNr(01, 54615);

        Console.WriteLine($"nr1 ist ident mit nr2?:           {nr1.Equals(nr2)}");
        Console.WriteLine($"nr1 ist ident mit nr4?:           {nr1.Equals(nr4)}");
        Console.WriteLine($"nr1 ist ident mit (object) nr4?:  {nr1.Equals((object)nr4)}");
        Console.WriteLine($"nr3 ist ident mit null?:          {nr3.Equals(null)}");
        Console.WriteLine($"nr3 ist größer als n4?:           {nr3.CompareTo(nr4) > 0}");
        Console.WriteLine($"nr3 ist größer als n4?:           {nr3 > nr4}");

        Console.WriteLine($"EHEHEH: {nr4 is object}");

        Console.WriteLine($"Hash von nr1:           {nr1.GetHashCode()}");
        Console.WriteLine($"Hash von nr4:           {nr4.GetHashCode()}");

        List<PhoneNr> numbers = new List<PhoneNr>() { nr1, nr2, nr3, nr4 };
        numbers.Sort();
        foreach (PhoneNr n in numbers)
        {
            Console.WriteLine(n);
        }
    }
}
