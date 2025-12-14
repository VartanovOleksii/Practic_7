using System.ComponentModel;
using System.Text.RegularExpressions;

public class Gpu
{
    //Приватні змінні
    private string _modelName;
    private int _gpuClock;
    private GPUArchitecture _architecture;
    private int _memorySize;
    private DateTime _releaseDate;
    private short _memoryBusWidth;
    private decimal _launchPrice;

    //Приватні статичні значення
    private static int _counter;
    private static decimal _discount;

    //Публічні дефолтні значення
    public const string DefName = "DefaultName";
    public const int DefClock = 1000;
    public const GPUArchitecture DefArchitecture = GPUArchitecture.Turing;
    public const int DefMemory = 1;
    public const short DefBus = 128;
    public const decimal DefPrice = 0.01m;


    //Публічні властивості
    public bool InBasket { get; private set; } = true;

    public string ModelName
    {
        get => _modelName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Назва моделі не може бути порожньою.");

            if (value.Length < 5 || value.Length > 40)
                throw new ArgumentException("Довжина назви моделі має бути від 5 до 40 символів.");

            if (!Regex.IsMatch(value, @"^[a-zA-Z0-9\s]+$"))
                throw new ArgumentException("Назва моделі може містити лише латинські літери та цифри.");

            _modelName = value;
        }
    }

    public int GpuClock
    {
        get => _gpuClock;
        set
        {
            if (value < 1000 || value > 4000)
                throw new ArgumentException("Частота GPU має бути в діапазоні 1000-4000 МГц.");
            _gpuClock = value;
        }
    }

    public GPUArchitecture Architecture
    {
        get => _architecture;
        set
        {
            if (!Enum.IsDefined(typeof(GPUArchitecture), value))
            {
                throw new InvalidEnumArgumentException("Архітектура не коректна.");
            }
            _architecture = value;
        }
    }

    public int MemorySize
    {
        get => _memorySize;
        set
        {
            if (value < 1 || value > 32)
                throw new ArgumentException("Об'єм пам'яті має бути в діапазоні 1–32 ГБ.");
            _memorySize = value;
        }
    }

    public DateTime ReleaseDate
    {
        get => _releaseDate;
        set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Дата випуску не може бути у майбутньому.");
            _releaseDate = value;
        }
    }

    public short MemoryBusWidth
    {
        get => _memoryBusWidth;
        set
        {
            if (value < 128 || value > 2048)
                throw new ArgumentException("Розрядність шини має бути в діапазоні 128-2048 біт.");
            _memoryBusWidth = value;
        }
    }

    public decimal LaunchPrice
    {
        get => _launchPrice;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Ціна на релізі має бути більше 0.");
            _launchPrice = value;
        }
    }


    //Публічні статичні властивості
    public static int Counter => _counter;

    public static decimal Discount
    {
        get { return _discount; }
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException("Знижка повинна бути в діапазоні від 0 до 100%.");
            }
            _discount = value;
        }
    }


    //Публічні методи
    public string PrintInfo()
    {
        string result = string.Empty;
        result += ($"Модель: {ModelName}\n");
        result += ($"GPU Clock: {GpuClock} МГц\n");
        result += ($"Архітектура: {Architecture}\n");
        result += ($"Пам'ять: {MemorySize} ГБ\n");
        result += ($"Розрядність шини: {MemoryBusWidth} біт\n");
        result += ($"Дата випуску: {ReleaseDate.ToShortDateString()}\n");
        result += ($"Ціна на релізі: {LaunchPrice} $\n");

        if (InBasket)
        {
            result += ("Відеокарта знаходиться в кошику\n");
        }
        else
        {
            result += ("Відеокарта не знаходиться в кошику\n");
        }

        return result;
    }

    public int YearsSinceRelease()
    {
        return DateTime.Now.Year - ReleaseDate.Year;
    }

    public int YearsSinceRelease(DateTime selectedDate)
    {
        if (selectedDate.Year - ReleaseDate.Year < 0)
            return -1;
        return selectedDate.Year - ReleaseDate.Year;
    }

    public string AddToBasket()
    {
        if (!InBasket)
        {
            InBasket = true;
            return "Відеокарта додана в кошик.";
        }
        else
        {
            return "Відеокарта вже знаходиться в кошику.";
        }
    }

    public string DeleteFromBasket()
    {
        if (InBasket)
        {
            InBasket = false;
            return "Відеокарта видалена з кошика.";
        }
        else
        {
            return "Відеокарти не було в кошику.";
        }
    }


    //Публічні статичні методи
    public static decimal PriceWithDiscount(decimal price)
    {
        return price * (1 - Discount);
    }
    public static void DecrementCounter()
    {
        if (_counter > 0)
        {
            _counter--;
        }
    }

    //Статичний конструктор
    static Gpu()
    {
        Discount = 0.15m;
    }


    //Конструктори
    public Gpu() : this(DefName, DefClock, DefArchitecture, DefMemory, DateTime.Parse("01.01.1999"), DefBus, DefPrice)
    {

    }

    public Gpu(string modelName, GPUArchitecture architecture, decimal launchPrice) : this(modelName, DefClock, architecture, DefMemory, DateTime.Parse("01.01.1999"), DefBus, launchPrice)
    {

    }

    public Gpu(string modelName, int gpuClock, GPUArchitecture architecture, int memorySize, DateTime releaseDate, short memoryBusWidth, decimal launchPrice)
    {
        ModelName = modelName;
        GpuClock = gpuClock;
        Architecture = architecture;
        MemorySize = memorySize;
        ReleaseDate = releaseDate;
        MemoryBusWidth = memoryBusWidth;
        LaunchPrice = launchPrice;

        _counter++;
    }


    //Parse та TryParce
    public override string ToString()
    {
        return $"{ModelName};{GpuClock};{Architecture};{MemorySize};{ReleaseDate.ToShortDateString()};{MemoryBusWidth};{LaunchPrice}";
    }

    public static Gpu Parse(string s)
    {
        if (string.IsNullOrEmpty(s))
            throw new ArgumentNullException(null, "Строка не може бути нулем або пустою.");

        string[] part = s.Split(';');

        if (part.Length != 7)
            throw new FormatException("Строка неправильного формату.");

        int clock;

        if (!int.TryParse(part[1], out clock))
            throw new ArgumentException("Значення частоти некоректне.");

        GPUArchitecture arch;

        if (!Enum.TryParse<GPUArchitecture>(part[2], true, out arch))
            throw new InvalidEnumArgumentException("Архітектура некоректна.");

        int size;

        if (!int.TryParse(part[3], out size))
            throw new ArgumentException("Значення розміру пам'яті некоректне.");

        DateTime release;

        if (!DateTime.TryParse(part[4], out release))
            throw new ArgumentException("Дата некоректна.");

        short width;

        if (!short.TryParse(part[5], out width))
            throw new ArgumentException("Значення розрядності пам'яті некоректне.");

        decimal price;

        if (!decimal.TryParse(part[6], out price))
            throw new ArgumentException("Значення ціни не коректне.");

        return new Gpu(part[0], clock, arch, size, release,width, price);
    }

    public static bool TryParse(string s, out Gpu gpu)
    {
        gpu = null;
        bool valid = false;

        try
        {
            gpu = Parse(s);
            valid = true;
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TryParse: {ex.Message}");
        }

        return valid;
    }
}