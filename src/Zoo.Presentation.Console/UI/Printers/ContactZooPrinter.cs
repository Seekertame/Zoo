namespace Zoo.Presentation.Console.UI.Printers
{
    public sealed class ContactZooPrinter
    {
        public void Print(IEnumerable<string> animals)
        {
            System.Console.WriteLine("== Контактный зоопарк ==");
            foreach (var a in animals)
            {
                System.Console.WriteLine(a);
            }
        }
    }
}
