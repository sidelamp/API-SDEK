record Answer(Result result);

public struct Result
{
    public string? price { get; set; }
    public int deliveryPeriodMin { get; set; }
    public int deliveryPeriodMax { get; set; }
    public int tariffId { get; set; }
    public string? currency { get; set; }
    public double priceByCurrency { get; set; }
    public int percentVAT { get; set; }
    public Error errors { get; set; }
}

public struct Error
{
    public int code { get; set; }
    public string? text { get; set; }
}
