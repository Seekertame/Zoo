namespace Zoo.Presentation.Console.UI.Printers
{
    public sealed class TotalFoodPrinter
    {
        public void Print(int kgPerDay) =>
            System.Console.WriteLine($"Всего корма в сутки: {kgPerDay} кг");
    }
}
