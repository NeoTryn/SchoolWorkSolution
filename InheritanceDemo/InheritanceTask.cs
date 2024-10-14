using System;
using System.Collections.Generic;
using System.Linq;

namespace InheritanceDemo.Application
{
    class Order
    {
        private PaymentProvider _paymentProvider;
        private List<Product> _products = new List<Product>();
        private decimal _invoiceAmount;
        public IReadOnlyList<Product> Products { get { return _products.AsReadOnly(); } }
        public decimal InvoiceAmount { get { return _invoiceAmount; } }   

        public Order(PaymentProvider paymentProvider)
        {
            _paymentProvider = paymentProvider;
            _invoiceAmount = paymentProvider.Limit - 100;
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public bool Checkout()
        {
            if (Products.Count.Equals(0))
            {
                return true;
            }
            else
            {  
                decimal amount = 0;
                foreach (Product p in Products)
                {
                    amount += p.Price;
                }
                _products.Clear();
                return _paymentProvider.Pay(amount);
            }
        }
    }


    abstract class PaymentProvider
    {
        public decimal Limit { get; }

        public PaymentProvider(decimal limit)
        {
            Limit = limit;
        }

        public virtual bool Pay(decimal amount)
        {
            if (amount > Limit)
            {
                    return false;
                }
                else
                {
                    return true;
                }
        }
    }

    class Product
    {
        public string Ean { get; }
        public string Name { get; }
        public decimal Price { get; }

        public Product(string ean, string name, decimal price)
        {
            Ean = ean;
            Name = name;
            Price = price;
        }
    }

    class CreditCard : PaymentProvider
    {
        public string Number { get; }

        public CreditCard(decimal limit, string number) : base(limit)
        {
            Number = number;
        }
    }

    class PrepaidCard : PaymentProvider
    {
        private decimal _credit;
        public decimal Credit { get { return _credit; } }

        public PrepaidCard(decimal limit, decimal credit) : base(limit)
        {
            _credit = credit;
        }

        public void Charge(decimal amount)
        {
            if (Pay(amount))
            {
                _credit -= amount;
            }
        }

        public override bool Pay(decimal amount)
        {
            if (amount > Credit)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
        class Program
    {
        private static int _testCount = 0;
        private static int _testsSucceeded = 0;

        private static void Main(string[] args)
        {
            Console.WriteLine("Teste Klassenimplementierung.");
            CheckAndWrite(() => typeof(Product).GetProperties().Any() && typeof(Product).GetProperties().All(p => p.CanWrite == false), "Alle Properties in Product sind read only");
            CheckAndWrite(() => typeof(Order).GetProperties().Any() && typeof(Order).GetProperties().All(p => p.CanWrite == false), "Alle Properties in Order sind read only");
            CheckAndWrite(() => typeof(PaymentProvider).GetProperties().Any() && typeof(PaymentProvider).GetProperties().All(p => p.CanWrite == false), "Alle Properties in PaymentProvider sind read only");
            CheckAndWrite(() => typeof(CreditCard).GetProperties().Any() && typeof(CreditCard).GetProperties().All(p => p.CanWrite == false), "Alle Properties in CreditCard sind read only");

            CheckAndWrite(() => typeof(PaymentProvider).GetConstructor(Type.EmptyTypes) is null, "Kein Defaultkonstruktor in PaymentProvider");
            CheckAndWrite(() => typeof(PaymentProvider).IsAbstract, "PaymentProvider ist abstrakt");
            CheckAndWrite(() => typeof(Order).GetProperty(nameof(Order.Products))?.PropertyType == typeof(IReadOnlyList<Product>), "Order.Products ist IReadOnlyList<Product>");
            CheckAndWrite(() => typeof(PrepaidCard).GetProperty(nameof(PrepaidCard.Credit))?.GetSetMethod() is null, "Credit kann nicht öffentlich gesetzt werden");

            Product p1 = new Product("1001", "Apple iPhone 13 Pro Max 1TB gold", 1800);
            Product p2 = new Product("1002", "Samsung Galaxy Z Fold 3 5G F926B/DS 512GB Phantom Black", 1600);

            {
                Console.WriteLine("Tests mit CreditCard als PaymentProdiver.");
                CreditCard creditCard = new CreditCard(limit: 3500, number: "123456789");
                CheckAndWrite(() => creditCard.Limit == 3500, "Limit ist 3500");
                Order order = new Order(paymentProvider: creditCard);
                order.AddProduct(p1);
                order.AddProduct(p2);
                CheckAndWrite(() => order.InvoiceAmount == 3400, "InvoiveAmount ist 3400");
                CheckAndWrite(() => order.Checkout() && order.Products.Count() == 0, "Checkout true und Produktliste leer");
            }
            {
                Console.WriteLine("Teste Limit.");
                CreditCard creditCard = new CreditCard(limit: 100, number: "123456789");
                Order order = new Order(paymentProvider: creditCard);
                order.AddProduct(p1);
                CheckAndWrite(() => !order.Checkout(), "Checkout false wenn amount > limit");
            }

            {
                Console.WriteLine("Tests mit PrepaidCard als PaymentProdiver.");
                PrepaidCard prepaidCard = new PrepaidCard(limit: 2000, credit: 1000);
                Order order = new Order(paymentProvider: prepaidCard);
                order.AddProduct(p1);
                CheckAndWrite(() => !order.Checkout(), "Checkout false wenn credit < amount");
                prepaidCard.Charge(900);
                CheckAndWrite(() => order.Checkout() && order.Products.Count() == 0, "Checkout true und Produktliste leer");
                CheckAndWrite(() => (prepaidCard.Credit == 100), "Credit = 100");
            }
            Console.WriteLine($"{_testsSucceeded} von {_testCount} Punkte erreicht.");
        }

        private static void CheckAndWrite(Func<bool> predicate, string message)
        {
            _testCount++;
            if (predicate())
            {
                Console.WriteLine($"   {_testCount} OK: {message}");
                _testsSucceeded++;
                return;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"   {_testCount} Nicht erfüllt: {message}");
            Console.ResetColor();
        }
    }   
}