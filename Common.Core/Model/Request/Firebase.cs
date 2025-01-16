﻿namespace Common.Core.Model.Request;

[ExcludeFromCodeCoverage]
public class FirebaseRequest
{
    public int userId { get; set; }
    public string transactionId { get; set; }
    public string feURL { get; set; }
    public int status { get; set; }
    public string payload { get; set; }
    public string entity { get; set; }
    public string method { get; set; }
    public bool isRead { get; set; } = false;
}