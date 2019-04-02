namespace Aliyun.Serverless.Core
{
    using System;

    /// <summary>
    /// This attribute is required for serialization of input/output parameters of
    /// a AliFc function if your AliFc function uses types other than string or
    /// System.IO.Stream as input/output parameters.
    /// 
    /// This attribute can be applied to a method (serializer used for method input
    /// and output), or to an assembly (serializer used for all methods).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class FcSerializerAttribute : System.Attribute
    {
        /// <summary>
        /// Type of the serializer.
        /// The custom serializer must implement Aliyun.Serverless.IAliFcSerializer
        /// interface, or an exception will be thrown.
        /// </summary>
        public Type SerializerType { get; set; }

        /// <summary>
        /// Constructs attribute with a specific serializer type.
        /// </summary>
        /// <param name="serializerType"></param>
        public FcSerializerAttribute(Type serializerType)
        {
            this.SerializerType = serializerType;
        }

    }
}