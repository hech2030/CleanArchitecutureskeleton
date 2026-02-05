using Integrations.ModernService.Domain.Entities;
using Integrations.ModernService.Domain.Entities.Common.Options;
using Integrations.ModernService.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Integrations.ModernService.Infrastructure.RabbitMq;

public class RabbitMqListener : BackgroundService
{
    private readonly RabbitMqOptions _options;
    private IConnection? _connection;
    private IModel? _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitMqListener> _logger;

    public RabbitMqListener(IOptions<RabbitMqOptions> options,
        IServiceProvider serviceProvider,
        ILogger<RabbitMqListener> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _options = options.Value;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            //Port = PORT HERE <OPTIONAL>
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _options.QueueName, durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                var message = JsonSerializer.Deserialize<LinkedInEventMessage>(messageJson);

                if (message == null || message.Message == null || string.IsNullOrWhiteSpace(message.Message.Data))
                {
                    _logger.LogError("Failed to deserialize or validate LinkedInEventMessage.");
                    return;
                }

                // Example: parse Data to int if needed
                if (!int.TryParse(message.Message.Data, out var requisitionId))
                {
                    _logger.LogError("Invalid Data field in LinkedInEventMessage.Message.");
                    return;
                }

                // Process the valid message
                using (var scope = _serviceProvider.CreateScope())
                {
                    var linkedInIntegrationService = scope.ServiceProvider.GetRequiredService<ILinkedInIntegrationService>();
                    var isRscEnabled = await linkedInIntegrationService.IsIntegrationEnabled(message.OrgId, CancellationToken.None);
                    if (isRscEnabled)
                    {
                        // call linkedIn API to create the requisition (to check with the legacy code)
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing RabbitMQ message.");
            }
            _logger.LogError("LinkedIn integration is not enabled. Message will not be processed.");
            // GET system setting from database with setting name as "linkedin.integrations.recruiter-system-connect.enabled"
            // If the settings is enabled , call 
            // GET system setting from database with setting name as "linkedin.integrations.apply-connect.enabled"
        };

        _channel.BasicConsume(queue: _options.QueueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}