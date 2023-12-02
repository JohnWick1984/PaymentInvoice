using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
class PaymentBill : ISerializable
{
    private decimal paymentPerDay;
    private int numberOfDays;
    private decimal penaltyPerDay;
    private int daysDelayed;

    public decimal PaymentPerDay
    {
        get { return paymentPerDay; }
        set { paymentPerDay = value; }
    }

    public int NumberOfDays
    {
        get { return numberOfDays; }
        set { numberOfDays = value; }
    }

    public decimal PenaltyPerDay
    {
        get { return penaltyPerDay; }
        set { penaltyPerDay = value; }
    }

    public int DaysDelayed
    {
        get { return daysDelayed; }
        set { daysDelayed = value; }
    }

    public decimal AmountToPayWithoutPenalty
    {
        get { return paymentPerDay * numberOfDays; }
    }

    public decimal Penalty
    {
        get { return penaltyPerDay * daysDelayed; }
    }

    public decimal TotalAmountToPay
    {
        get { return AmountToPayWithoutPenalty + Penalty; }
    }

    public PaymentBill(decimal paymentPerDay, int numberOfDays, decimal penaltyPerDay, int daysDelayed)
    {
        this.paymentPerDay = paymentPerDay;
        this.numberOfDays = numberOfDays;
        this.penaltyPerDay = penaltyPerDay;
        this.daysDelayed = daysDelayed;
    }

    // Реализация интерфейса ISerializable для контроля сериализации
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("PaymentPerDay", paymentPerDay);
        info.AddValue("NumberOfDays", numberOfDays);
        info.AddValue("PenaltyPerDay", penaltyPerDay);
        info.AddValue("DaysDelayed", daysDelayed);
    }

    // Конструктор для десериализации
    protected PaymentBill(SerializationInfo info, StreamingContext context)
    {
        paymentPerDay = info.GetDecimal("PaymentPerDay");
        numberOfDays = info.GetInt32("NumberOfDays");
        penaltyPerDay = info.GetDecimal("PenaltyPerDay");
        daysDelayed = info.GetInt32("DaysDelayed");
    }

    // Сериализация объекта в файл
    public void Serialize(string fileName)
    {
        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
        }
    }

    // Десериализация объекта из файла
    public static PaymentBill Deserialize(string fileName)
    {
        using (FileStream stream = new FileStream(fileName, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (PaymentBill)formatter.Deserialize(stream);
        }
    }
}

class Program
{
    static void Main()
    {
        // Пример использования класса
        PaymentBill paymentBill = new PaymentBill(50, 10, 5, 2);

        // Сериализация объекта
        paymentBill.Serialize("paymentBill.dat");

        // Десериализация объекта
        PaymentBill deserializedPaymentBill = PaymentBill.Deserialize("paymentBill.dat");

        // Вывод результатов
        Console.WriteLine("Amount to pay without penalty: " + deserializedPaymentBill.AmountToPayWithoutPenalty);
        Console.WriteLine("Penalty: " + deserializedPaymentBill.Penalty);
        Console.WriteLine("Total amount to pay: " + deserializedPaymentBill.TotalAmountToPay);
    }
}
