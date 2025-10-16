namespace Zoo.Presentation.Console.UI
{
    public sealed class Menu
    {
        public void Show()
        {
            System.Console.WriteLine("1) Добавить животное (ветпроверка)");
            System.Console.WriteLine("2) Суммарный корм в сутки");
            System.Console.WriteLine("3) Контактный зоопарк");
            System.Console.WriteLine("4) Инвентарный список");
            System.Console.WriteLine("0) Выход");
        }
    }
}
