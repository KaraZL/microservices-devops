using Microsoft.Data.SqlClient;
using Polly;
using Polly.Retry;
using RabbitMQ.Client.Exceptions;
using System;

namespace GeneralStore.API.Policies
{
    public class ClientPolicy
    {
        //AsyncRetryPolicy aussi
        public RetryPolicy MigrationRetryPolicy { get; set; }
        public RetryPolicy RabbitMQRetryPolicy { get; set; }

        public ClientPolicy()
        {
            MigrationRetryPolicy = Policy.Handle<SqlException>()
                .WaitAndRetry(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, retryCount, retryAttempt) =>
                    {
                        Console.WriteLine($"--GeneralStoreAPI : StoreDb Migration Retry Polly... [{retryCount}]");
                    });

            RabbitMQRetryPolicy = Policy.Handle<BrokerUnreachableException>()
                .WaitAndRetry(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, retryCount, retryAttempt) =>
                    {
                        Console.WriteLine($"--GeneralStoreAPI : RabbitMQ Retry Polly... [{retryCount}]");
                    });
        }
    }
}
