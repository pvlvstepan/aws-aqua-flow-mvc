using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;

public class SNSManager
{
    private readonly IAmazonSimpleNotificationService snsClient;
    private readonly string topicArn = "arn:aws:sns:us-east-1:581620127017:AquaFlowNotifications";  

    public SNSManager()
    {
        snsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
    }

    public void PublishMessage(string message)
    {
        var publishRequest = new PublishRequest
        {
            TopicArn = topicArn,
            Message = message
        };

        snsClient.Publish(publishRequest);
        Console.WriteLine("Message published to SNS topic.");
    }
}
