using System;

abstract class Storage
{
    private string name;
    private string model;

    public Storage(string name, string model)
    {
        this.name = name;
        this.model = model;
    }

    public string Name { get { return name; } }
    public string Model { get { return model; } }

    public abstract double GetMemory();
    public abstract void CopyData(double dataSize);
    public abstract double GetFreeSpace();
    public abstract void DisplayInfo();
}

class Flash : Storage
{
    private double usbSpeed;
    private double memorySize;

    public Flash(string name, string model, double usbSpeed, double memorySize) : base(name, model)
    {
        this.usbSpeed = usbSpeed;
        this.memorySize = memorySize;
    }

    public double UsbSpeed { get { return usbSpeed; } }

    public override double GetMemory()
    {
        return memorySize;
    }

    public override void CopyData(double dataSize)
    {
        Console.WriteLine($"Копирование данных на Flash-память со скоростью {usbSpeed} Gb/s");
    }

    public override double GetFreeSpace()
    {
        //10% памяти на Flash оставляем свободным
        return memorySize * 0.1;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Flash-память: {Name}, Модель: {Model}, Объем: {memorySize} Gb, Скорость USB: {usbSpeed} Gb/s");
    }
}

class DVD : Storage
{
    private double readWriteSpeed;
    private string discType;

    public DVD(string name, string model, double readWriteSpeed, string discType) : base(name, model)
    {
        this.readWriteSpeed = readWriteSpeed;
        this.discType = discType;
    }

    public override double GetMemory()
    {
        if (discType == "односторонний")
            return 4.7;
        else if (discType == "двусторонний")
            return 9;
        else
            return 0;
    }

    public override void CopyData(double dataSize)
    {
        Console.WriteLine($"Запись данных на DVD-диск со скоростью {readWriteSpeed} Gb/s");
    }

    public override double GetFreeSpace()
    {
        // 5% памяти на DVD оставляем свободным
        return GetMemory() * 0.05;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"DVD-диск: {Name}, Модель: {Model}, Тип: {discType}, Скорость чтения/записи: {readWriteSpeed} Gb/s");
    }
}

class HDD : Storage
{
    private double usbSpeed;
    private int partitions;
    private double partitionSize;

    public HDD(string name, string model, double usbSpeed, int partitions, double partitionSize) : base(name, model)
    {
        this.usbSpeed = usbSpeed;
        this.partitions = partitions;
        this.partitionSize = partitionSize;
    }

    public double UsbSpeed { get { return usbSpeed; } }

    public override double GetMemory()
    {
        return partitions * partitionSize;
    }

    public override void CopyData(double dataSize)
    {
        Console.WriteLine($"Перенос данных на съемный HDD со скоростью {usbSpeed} Gb/s");
    }

    public override double GetFreeSpace()
    {
        //15% памяти на HDD оставляем свободным
        return GetMemory() * 0.15;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Съемный HDD: {Name}, Модель: {Model}, Количество разделов: {partitions}, Объем разделов: {partitionSize} Gb, Скорость USB: {usbSpeed} Gb/s");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создаем массив носителей информации
        Storage[] devices = new Storage[]
        {
            new Flash("FlashDrive1", "Kingston", 3.0, 64),
            new DVD("DVD-RW", "Sony", 2.0, "двусторонний"),
            new HDD("SeagateHDD", "Expansion", 2.0, 2, 500),
        };

        // 1. Расчет общего количества памяти всех устройств
        double totalMemory = 0;
        foreach (var device in devices)
        {
            totalMemory += device.GetMemory();
        }
        Console.WriteLine($"Общий объем памяти всех устройств: {totalMemory} Gb");

        // 2. Копирование информации на устройства
        double dataSize = 565; // Размер данных в Gb
        foreach (var device in devices)
        {
            device.CopyData(dataSize);
        }

        // 3. Расчет времени необходимого для копирования
        // (предположим, что скорость копирования зависит от USB-скорости)
        double usbSpeedSum = 0;
        int flashCount = 0;

        foreach (var device in devices)
        {
            if (device is Flash flash)
            {
                usbSpeedSum += flash.UsbSpeed;
                flashCount++;
            }
            else if (device is HDD hdd)
            {
                usbSpeedSum += hdd.UsbSpeed;
            }
        }

        // Проверка деления на ноль, чтобы избежать ошибки при отсутствии Flash-накопителей
        double averageUsbSpeed = flashCount > 0 ? usbSpeedSum / flashCount : 0;

        // 4. Расчет необходимого количества носителей информации
        double totalRequiredDevices = dataSize / totalMemory;
        Console.WriteLine($"Необходимое количество носителей информации: {Math.Ceiling(totalRequiredDevices)} устройств");

        // Вывод информации о каждом устройстве
        foreach (var device in devices)
        {
            device.DisplayInfo();
            Console.WriteLine();
        }
    }
}
