namespace Zoo.Presentation.Console.UI.Printers
{
    public sealed class InventoryPrinter
    {
        public void Print(IEnumerable<string> lines)
        {
            System.Console.WriteLine("== Инвентаризация ==");
            foreach (var l in lines)
            {
                System.Console.WriteLine(l);
            }
        }
    }
}
