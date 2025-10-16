# Zoo - ERP-система

Учебный проект по курсу «Конструирование ПО». Реализованы слои Domain / Application / Infrastructure / Presentation, внедрение зависимостей (DI), принципы SOLID, юнит-тесты и сбор покрытия.

---

## Структура проекта и что где лежит

```
Zoo/
├─ src/
│  ├─ Zoo.Domain/                 // Чистая предметная модель (без зависимостей наружу)
│  │  ├─ Abstractions/            // Маркеры домена: IAlive(FoodKgPerDay), IInventory(Number)
│  │  └─ Entities/
│  │     ├─ Animals/              // Animal → Herbivore/Predator → Rabbit/Monkey/Tiger/Wolf
│  │     └─ Things/               // Thing → Table/Computer
│  ├─ Zoo.Application/            // Use-cases + контракты инфраструктуры
│  │  ├─ Interfaces/              // IAnimalRepository, IThingRepository, IUnitOfWork, ...
│  │  ├─ UseCases/
│  │  │  ├─ Admission/            // Приём животного после ветпроверки
│  │  │  ├─ Reports/              // Суммарный корм, контактный зоопарк, инвентаризация
│  │  │  └─ Inventory/            // Сервис добавления вещей (проверяет уникальность номеров)
│  │  └─ Exceptions/              // InventoryConflictException
│  ├─ Zoo.Infrastructure/         // Технические реализации интерфейсов
│  │  ├─ Clinics/                 // RandomizedVeterinaryClinic : IVeterinaryClinic
│  │  ├─ Persistence/InMemory/    // InMemory*Repository, InMemoryUnitOfWork
│  │  ├─ Policies/                // InMemoryInventoryPolicy : IInventoryPolicy
│  │  └─ DI/                      // ServiceCollectionExtensions (регистрация зависимостей)
│  └─ Zoo.Presentation.Console/   // Консольный UI и composition root
│     ├─ CompositionRoot/         // Bootstrapper (склеивает DI)
│     └─ UI/                      // Menu, InputReader, принтеры, ConsoleApp (цикл)
└─ tests/
   └─ Zoo.Tests/                  // xUnit-тесты (Domain/Application)
```

Ключевые use-case’ы:

* **Приём животного**: `AdmissionService` → вызывает `IVeterinaryClinic` (решение Accept/Reject). Перед сохранением проверяет уникальность инв. номера через `IInventoryPolicy`.
* **Отчёты**: `ReportService` - суммарный корм, «контактный зоопарк» (травоядные с добротой > 5), общий инвентарный список (животные + вещи).
* **Добавление вещи**: `ThingService` - накрывает репозиторий, применяет политику уникальности и `UnitOfWork`.

---

## Где применены принципы SOLID (по делу, не «на словах»)

* **S - Single Responsibility**

  * `AdmissionService` делает только приём животных.
  * `ReportService` - только отчёты.
  * `ThingService` - только команда добавления вещи.
    Это позволяет менять логику приёма, не трогая отчёты и наоборот.

* **O - Open/Closed**
  Добавить новый вид животного (`Giraffe : Herbivore`) или вещь (`Cage : Thing`) можно без правок сервисов: они работают через абстракции (`IAlive`, `IInventory`, репозитории). Фильтры отчётов используют общие свойства.

* **L - Liskov Substitution**
  `Herbivore` и `Predator` подставимы вместо `Animal` (инварианты не нарушаются, интерфейсы не «ослабляются»). Сервисы не знают конкретный подтип - опираются на контракт `Animal`.

* **I - Interface Segregation**
  Узкие интерфейсы: `IAlive` (только потребление корма), `IInventory` (только инв. номер), отдельные `IAnimalRepository`, `IThingRepository`, `IVeterinaryClinic`, `IInventoryPolicy`. Никаких «толстых» ultimate-интерфейсов.

* **D - Dependency Inversion**
  Сервисы Application зависят от абстракций, а не реализаций: `IVeterinaryClinic`, `IInventoryPolicy`, `I*Repository`, `IUnitOfWork`. Реальные реализации (InMemory/Randomized) - в Infrastructure и подставляются через DI.

---

## Запуск программы

```bash
dotnet restore
dotnet build
dotnet run --project src/Zoo.Presentation.Console
```

Консольное меню:

1. Добавить животное (через ветклинику)
2. Посчитать суммарный корм (кг/сутки)
3. Показать «контактный зоопарк» (травоядные с добротой > 5)
4. Инвентарный список (животные + вещи)
0. Выход

> Чтобы кириллица отображалась корректно, в начале `Program.cs` добавили:
>
> ```csharp
> Console.OutputEncoding = Encoding.UTF8;
> Console.InputEncoding  = Encoding.UTF8;
> ```

---

## Покрытие тестами и что тестируется

### Итоговое покрытие

На момент последнего прогона: **Line ≈ 72,8%**, **Branch ≈ 87,5%** (> 60%).
Отчёт генерировался `coverlet.collector` + `reportgenerator`.

Сгенерировать у себя:

```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory TestResults
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:HtmlSummary
# открой coveragereport/index.html
```

### Что именно покрыто тестами

* **Domain**

  * Валидации инвариантов (`Animal`/`Thing`: пустые имена, невалидные номера, отриц. рацион; `Herbivore.Kindness` в диапазоне 0..10).
  * Логика `IsContactZooAllowed` и геттеры `Species` у конкретных животных.

* **Application**

  * `AdmissionService`: сценарии **Accept/Reject**; отдельный тест гарантирует, что политика уникальности вызывается только при Accept.
  * `ReportService`:

    * сумма корма;
    * «контактный зоопарк» (травоядные с добротой > 5, граничные случаи: ровно 5 — не проходит);
    * инвентаризация для комбинаций «только животные» / «только вещи» / «оба».
  * `ThingService`: успешный путь (добавляет и вызывает `SaveChangesAsync`).
  * `InMemoryInventoryPolicy`:

    * успех на свободном номере;
    * бросает `InventoryConflictException`, если номер уже занят животным или вещью.

* **Infrastructure**

  * `RandomizedVeterinaryClinic`: принимаем травоядных и хищников с рационом ≤ 7; отклоняем «прожорливых» хищников.

### Почему coverage-отчёты удалены из репозитория

HTML-отчёты (`coveragereport/`) и `TestResults/` - генерируемые артефакты, они «шумные», тяжёлые и меняются от запуска к запуску. Поэтому они добавлены в `.gitignore` и были удалены из репозитория.

Если нужно доказательство, то вот блок + скрин:
```xml
<?xml version="1.0" encoding="utf-8"?>
<coverage line-rate="0.7288" branch-rate="0.875" version="1.9" timestamp="1760631726" lines-covered="86" lines-valid="118" branches-covered="21" branches-valid="24">
  <sources>
    <source>C:\Users\timur\source\repos\Zoo\src\</source>
```
<img width="1920" height="1200" alt="image" src="https://github.com/user-attachments/assets/ffa1fa6c-f4f6-4dc6-80b5-aadf02bdfec3" />

---

## Примечания по требованиям задания

* Маркеры домена (`IAlive`, `IInventory`) реализованы и используются сущностями животных/вещей.
* Приём новых животных возможен только после решения ветклиники.
* Реализованы 3 отчёта: суммарный рацион, «контактный зоопарк», инвентаризация.
* Инвентарные номера уникальны в рамках всего зоопарка (и животные, и вещи) — это контролирует `IInventoryPolicy`.
* Архитектура слоистая + DI, принципы SOLID применены предметно.
* Тестовое покрытие ≥60% достигнуто.
