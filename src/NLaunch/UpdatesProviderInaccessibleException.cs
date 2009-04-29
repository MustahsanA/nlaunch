using System;
using System.Runtime.Serialization;

namespace NLaunch
{
	[Serializable]
	public class UpdatesProviderInaccessibleException : ApplicationException
	{
		public UpdatesProviderInaccessibleException()
		{
		}

		public UpdatesProviderInaccessibleException(string message) : base(message)
		{
		}

		public UpdatesProviderInaccessibleException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected UpdatesProviderInaccessibleException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}