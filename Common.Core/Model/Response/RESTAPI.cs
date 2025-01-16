using Common.Core.Enum;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Common.Core.Model.Response
{
    [ExcludeFromCodeCoverage]
    public class Response
    {
        public string transactionId { get; private set; }
        public string message { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string stackTrace { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ResponseStatus Status { get; private set; }

        public Response Fail(string transactionId, string message)
        {
            this.transactionId = transactionId;
            this.message = message;
            Status = ResponseStatus.FAIL;

            return this;
        }

        public Response Fail(string transactionId, string message, string stackTrace)
        {
            this.transactionId = transactionId;
            this.message = message;
            this.stackTrace = stackTrace;
            Status = ResponseStatus.FAIL;

            return this;
        }

        public Response Success(string transactionId, string message)
        {
            this.transactionId = transactionId;
            this.message = message;
            Status = ResponseStatus.SUCCESS;

            return this;
        }

        public Response BadRequest(string transactionId, string message)
        {
            this.transactionId = transactionId;
            this.message = message;
            Status = ResponseStatus.BAD_REQUEST;

            return this;
        }
    }

    [ExcludeFromCodeCoverage]
    public class Response<T> where T : class
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TransactionId { get; set; }
        public string Message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Page { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? NextPage { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Row { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Total { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? NextIteration { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? HasNextIteration { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ResponseStatus Status { get; set; }

        public Response<T> Fail(string transactionId, string message, int page, int row, int total, T? data)
        {
            TransactionId = transactionId;
            Message = message;
            Page = page;
            Row = row;
            Total = total;
            Data = data;
            Status = ResponseStatus.FAIL;

            return this;
        }
        public Response<T> BadRequest(string transactionId, string message, T? data = default)
        {
            TransactionId = transactionId;
            Message = message;
            Data = data;
            Status = ResponseStatus.BAD_REQUEST;

            return this;
        }

        public Response<T> Fail(string transactionId, string message, int page, int nextPage, int row, int total, T? data)
        {
            TransactionId = transactionId;
            Message = message;
            Page = page;
            NextPage = nextPage;
            Row = row;
            Total = total;
            Data = data;
            Status = ResponseStatus.FAIL;

            return this;
        }

        public Response<T> Fail(string transactionId, string message, T? data = default)
        {
            TransactionId = transactionId;
            Message = message;
            Data = data;
            Status = ResponseStatus.FAIL;

            return this;
        }

        public Response<T> Fail(string message, int page, int row, int total, T? data)
        {
            Message = message;
            Page = page;
            Row = row;
            Total = total;
            Data = data;
            Status = ResponseStatus.FAIL;

            return this;
        }

        public Response<T> Fail(string message, T? data)
        {
            Message = message;
            Data = data;
            Status = ResponseStatus.FAIL;

            return this;
        }

        public Response<T> Success(string transactionId, string message, int page, int row, int total, T? data)
        {
            TransactionId = transactionId;
            Message = message;
            Page = page;
            Row = row;
            Total = total;
            Data = data;
            Status = ResponseStatus.SUCCESS;

            return this;
        }

        public Response<T> Success(string transactionId, string message, int page, int nextPage, int row, int total, T? data)
        {
            TransactionId = transactionId;
            Message = message;
            Page = page;
            NextPage = nextPage;
            Row = row;
            Total = total;
            Data = data;
            Status = ResponseStatus.SUCCESS;

            return this;
        }

        public Response<T> Success(string transactionId, string message, T? data = default)
        {
            TransactionId = transactionId;
            Message = message;
            Data = data;
            Status = ResponseStatus.SUCCESS;

            return this;
        }

        public Response<T> Success(string message, int page, int row, int total, T? data)
        {
            Message = message;
            Page = page;
            Row = row;
            Total = total;
            Data = data;
            Status = ResponseStatus.SUCCESS;

            return this;
        }

        public Response<T> Success(string transactionId, string message, int page, int row, int nextPage, int nextIteration, bool hasNextIteration, int total, T? data)
        {
            TransactionId = transactionId;
            Message = message;
            Page = page;
            Row = row;
            NextPage = nextPage;
            NextIteration = nextIteration;
            HasNextIteration = hasNextIteration;
            Row = row;
            Total = total;
            Data = data;
            Status = ResponseStatus.SUCCESS;

            return this;
        }
    }
}