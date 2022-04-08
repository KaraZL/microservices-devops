using Microsoft.Data.SqlClient;
using Polly;
using Polly.Retry;
using RabbitMQ.Client.Exceptions;
using System;

namespace Courses.API.Policies
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
                        Console.WriteLine($"--CourseAPI : CourseDb Migration Retry Polly... [{retryCount}]");
                    });

            RabbitMQRetryPolicy = Policy.Handle<BrokerUnreachableException>()
                .WaitAndRetry(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, retryCount, retryAttempt) =>
                    {
                        Console.WriteLine($"--CourseAPI : RabbitMQ Retry Polly... [{retryCount}]");
                    });
        }
    }
}
