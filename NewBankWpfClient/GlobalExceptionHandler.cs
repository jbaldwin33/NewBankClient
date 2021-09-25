using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace NewBankWpfClient
{
    public class GlobalExceptionHandler
    {
        public static void ProcessException(RpcException rpcException)
        {
            if (rpcException.Status.StatusCode == StatusCode.PermissionDenied)
            {
                //MessageBox.Show(new SessionInvalidLoggingOutTranslatable(), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void ProcessException(Exception exception)
        {
            throw exception;
        }
    }
}
