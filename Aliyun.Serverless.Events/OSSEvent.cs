using System;
using Newtonsoft.Json;

namespace Aliyun.Serverless.Events
{
    public class OSSEvent
    {
        public class Bucket
        {

            public readonly string arn;
            public readonly string name;
            public readonly string ownerIdentity;
            public readonly string virtualBucket;

            [JsonConstructor]
            public Bucket(string arn,
                          string name,
                          string ownerIdentity,
                          string virtualBucket)
            {
                this.arn = arn;
                this.name = name;
                this.ownerIdentity = ownerIdentity;
                this.virtualBucket = virtualBucket;
            }
        }

        public class Object
        {
            public readonly long deltaSize;
            public readonly string eTag;
            public readonly string key;
            public readonly long size;

            [JsonConstructor]
            public Object(long deltaSize,
                          string eTag,
                          string key,
                          long size)
            {
                this.deltaSize = deltaSize;
                this.eTag = eTag;
                this.key = key;
                this.size = size;
            }
        }

        public class RequestParameters
        {

            public readonly string sourceIPAddress;

            [JsonConstructor]
            public RequestParameters(string sourceIPAddress)
            {
                this.sourceIPAddress = sourceIPAddress;
            }
        }

        public class ResponseElements
        {
            public readonly string requestId;

            [JsonConstructor]
            public ResponseElements(string requestId)
            {
                this.requestId = requestId;
            }
        }

        public class UserIdentity
        {

            public readonly string principalId;

            [JsonConstructor]
            public UserIdentity(string principalId)
            {
                this.principalId = principalId;
            }
        }

        public class Oss
        {
            public readonly Bucket bucket;

            [JsonProperty("object")]
            public readonly Object obj;
            public readonly string ossSchemaVersion;
            public readonly string ruleId;

            [JsonConstructor]
            public Oss(Bucket bucket,
                      Object obj,
                      string ossSchemaVersion,
                      string ruleId)
            {
                this.bucket = bucket;
                this.obj = obj;
                this.ossSchemaVersion = ossSchemaVersion;
                this.ruleId = ruleId;
            }
        }

        public class Event
        {
            public readonly string eventName;
            public readonly string eventSource;
            public readonly string eventTime;
            public readonly string eventVersion;
            public readonly Oss oss;
            public readonly string region;
            public readonly RequestParameters requestParameters;
            public readonly ResponseElements responseElements;
            public readonly UserIdentity userIdentity;

            [JsonConstructor]
            public Event(string eventName,
                         string eventSource,
                         string eventTime,
                         string eventVersion,
                         Oss oss,
                         string region,
                         RequestParameters requestParameters,
                         ResponseElements responseElements,
                         UserIdentity userIdentity)
            {
                this.eventName = eventName;
                this.eventSource = eventSource;
                this.eventTime = eventTime;
                this.eventVersion = eventVersion;
                this.oss = oss;
                this.region = region;
                this.requestParameters = requestParameters;
                this.responseElements = responseElements;
                this.userIdentity = userIdentity;
            }
        }

        public readonly Event[] events;

        [JsonConstructor]
        public OSSEvent(Event[] events)
        {
            this.events = events;
        }
    }
}
