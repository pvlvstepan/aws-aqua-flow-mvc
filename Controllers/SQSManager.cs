using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;

public class SQSManager
{
    private readonly IAmazonSQS sqsClient;
    private readonly string queueUrl = "https://sqs.us-east-1.amazonaws.com/581620127017/WaterDeliveryQueue.fifo";  

    public SQSManager()
    {
        sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
    }

    public void SendMessageToQueue(string messageBody)
    {
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = messageBody
        };

        var sendMessageResponse = sqsClient.SendMessage(sendMessageRequest);
        Console.WriteLine($"Message sent with ID: {sendMessageResponse.MessageId}");
    }
}
